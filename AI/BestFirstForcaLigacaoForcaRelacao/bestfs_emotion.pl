%:-[bc].

:- ['../CheckEmotions/check_emotions.pl'].

limMaxFr_emotion(200).
limMinFr_emotion(-200).


bestfs_fl_fr_emotion(Orig,Dest,NMaximoLigacoes,Cam,Custo):-
	no(OrigId,Orig,_,_,_),
	no(DestId,Dest,_,_,_),
	((bestfs_fl_fr2_emotion(DestId,[[OrigId]],NMaximoLigacoes,CamIds,Custo),!,id_to_email_emotion(CamIds,Cam)
	);(Cam=[],Custo is -1,!)).


id_to_email_emotion([],[]):-!.

id_to_email_emotion([Id|Ids],[Email|Cam]):-
	no(Id,Email,_,_,_),
	id_to_email_emotion(Ids,Cam).

bestfs_fl_fr2_emotion(Dest,[[Dest|T]|_],0,Cam,Custo):- 
	reverse([Dest|T],Cam),
	calcula_custo_flfr_emotion(Cam,Custo).



bestfs_fl_fr2_emotion(Dest,[[Dest|T]|_],_,Cam,Custo):- 
	reverse([Dest|T],Cam),
	calcula_custo_flfr_emotion(Cam,Custo).


bestfs_fl_fr2_emotion(Dest,[[Dest|_]|LLA2],NMaximoLigacoes,Cam,Custo):- 
	!,
	NMaximoLigacoes1 is NMaximoLigacoes-1,
	bestfs_fl_fr2_emotion(Dest,LLA2,NMaximoLigacoes1,Cam,Custo).
	

bestfs_fl_fr2_emotion(Dest,LLA,NMaximoLigacoes,Cam,Custo):-
	member1_flfr_emotion(LA,LLA,LLA1),
	LA=[Act|_],
	
	((Act==Dest,!,bestfs_fl_fr2_emotion(Dest,[LA|LLA1],NMaximoLigacoes,Cam,Custo))
	 ;
	 (
	  findall((CX,[X|LA]),
	  	(
	  		ligacao(Act,X,Fl,_,Fr,_),
			no(X,_,_,Emotion,EValue),
	  		calcularForcaFinal_emotion(Fl,Fr,CX),
			(X == Dest; check_emotions(Emotion,EValue)),
	  		\+member(X,LA)
	  	),Novos),
	  Novos\==[],
	  sort(0,@>=,Novos,NovosOrd),
	  %write('NovosOrd='),write(NovosOrd),nl,
	  retira_custos_flfr_emotion(NovosOrd,NovosOrd1),
	  append(NovosOrd1,LLA1,LLA2),
	  %write('LLA2='),write(LLA2),nl,
	  NMaximoLigacoes>0,
	  NMaximoLigacoes1 is NMaximoLigacoes-1,
	  bestfs_fl_fr2_emotion(Dest,LLA2,NMaximoLigacoes1,Cam,Custo)
	 )).


member1_flfr_emotion(LA,[LA|LAA],LAA).


member1_flfr_emotion(LA,[_|LAA],LAA1):-
	member1_flfr_emotion(LA,LAA,LAA1).


retira_custos_flfr_emotion([],[]).

retira_custos_flfr_emotion([(_,LA)|L],[LA|L1]):-
	retira_custos_flfr_emotion(L,L1).


calcula_custo_flfr_emotion([Act,X],C):-
	!,ligacao(Act,X,Fl,_,Fr,_),
	calcularForcaFinal_emotion(Fl,Fr,C).


calcula_custo_flfr_emotion([Act,X|L],S):-
	calcula_custo_flfr_emotion([X|L],S1), 
	ligacao(Act,X,Fl,_,Fr,_),
	calcularForcaFinal_emotion(Fl,Fr,C),
	S is S1+C.


checkLimitesForca_emotion(FRel,FRel1):-
	limMaxFr_emotion(LimMaxFr),
	limMinFr_emotion(LimMinFr),
	(FRel>LimMaxFr->FRel1 is LimMaxFr;
	FRel<LimMinFr->FRel1 is LimMinFr;
	FRel1 is FRel).


calcularForcaFinal_emotion(ForcaLig,ForcaRel,Forca):-
	checkLimitesForca_emotion(ForcaRel,ForcaRel1),
	limMaxFr_emotion(LimMaxFr),
	Forca is ForcaLig>>1 + (ForcaRel1+LimMaxFr)>>3.
