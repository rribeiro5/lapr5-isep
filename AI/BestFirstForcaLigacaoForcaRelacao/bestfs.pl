%:-[bc].

limMaxFr(200).
limMinFr(-200).


bestfs_fl_fr(Orig,Dest,NMaximoLigacoes,Cam,Custo):-
	no(OrigId,Orig,_,_,_),
	no(DestId,Dest,_,_,_),
	NMaximoLigacoes>=0,
	((bestfs_fl_fr2(DestId,[[OrigId]],NMaximoLigacoes,CamIds,Custo),
		!,id_to_email(CamIds,Cam)
	);(Cam=[],Custo is -1,!)).


id_to_email([],[]):-!.

id_to_email([Id|Ids],[Email|Cam]):-
	no(Id,Email,_,_,_),
	id_to_email(Ids,Cam).

bestfs_fl_fr2(Dest,[[Dest|T]|_],0,Cam,Custo):- 
	reverse([Dest|T],Cam),
	calcula_custo_flfr(Cam,Custo).



bestfs_fl_fr2(Dest,[[Dest|T]|_],_,Cam,Custo):- 
	reverse([Dest|T],Cam),
	calcula_custo_flfr(Cam,Custo).


bestfs_fl_fr2(Dest,[[Dest|_]|LLA2],NMaximoLigacoes,Cam,Custo):- 
	!,
	NMaximoLigacoes1 is NMaximoLigacoes-1,
	bestfs_fl_fr2(Dest,LLA2,NMaximoLigacoes1,Cam,Custo).
	

bestfs_fl_fr2(Dest,LLA,NMaximoLigacoes,Cam,Custo):-
	member1_flfr(LA,LLA,LLA1),
	LA=[Act|_],
	
	((Act==Dest,!,bestfs_fl_fr2(Dest,[LA|LLA1],NMaximoLigacoes,Cam,Custo))
	 ;
	 (
	  findall((CX,[X|LA]),
	  	(
	  		ligacao(Act,X,Fl,_,Fr,_),
	  		calcularForcaFinal(Fl,Fr,CX),
	  		\+member(X,LA)
	  	),Novos),
	  Novos\==[],
	  sort(0,@>=,Novos,NovosOrd),
	  %write('NovosOrd='),write(NovosOrd),nl,
	  retira_custos_flfr(NovosOrd,NovosOrd1),
	  append(NovosOrd1,LLA1,LLA2),
	  %write('LLA2='),write(LLA2),nl,
	  NMaximoLigacoes>0,
	  NMaximoLigacoes1 is NMaximoLigacoes-1,
	  bestfs_fl_fr2(Dest,LLA2,NMaximoLigacoes1,Cam,Custo)
	 )).


member1_flfr(LA,[LA|LAA],LAA).


member1_flfr(LA,[_|LAA],LAA1):-
	member1_flfr(LA,LAA,LAA1).


retira_custos_flfr([],[]).

retira_custos_flfr([(_,LA)|L],[LA|L1]):-
	retira_custos_flfr(L,L1).


calcula_custo_flfr([Act,X],C):-
	!,ligacao(Act,X,Fl,_,Fr,_),
	calcularForcaFinal(Fl,Fr,C).


calcula_custo_flfr([Act,X|L],S):-
	calcula_custo_flfr([X|L],S1), 
	ligacao(Act,X,Fl,_,Fr,_),
	calcularForcaFinal(Fl,Fr,C),
	S is S1+C.


checkLimitesForca(FRel,FRel1):-
	limMaxFr(LimMaxFr),
	limMinFr(LimMinFr),
	(FRel>LimMaxFr->FRel1 is LimMaxFr;
	FRel<LimMinFr->FRel1 is LimMinFr;
	FRel1 is FRel).


calcularForcaFinal(ForcaLig,ForcaRel,Forca):-
	checkLimitesForca(ForcaRel,ForcaRel1),
	limMaxFr(LimMaxFr),
	Forca is ForcaLig>>1 + (ForcaRel1+LimMaxFr)>>3.
