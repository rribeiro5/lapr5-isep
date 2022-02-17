%:-[baseConhecimento].
:- ['../CheckEmotions/check_emotions.pl'].

bestfs_emotion(Orig,Dest,NMaximoLigacoes,Cam,Custo):-
	((bestfs_emotion2(Dest,[[Orig]],NMaximoLigacoes,Cam,Custo)
	);(Cam=[],Custo is -1)).

bestfs_emotion2(Dest,[[Dest|T]|_],0,Cam,Custo):-
	reverse([Dest|T],Cam),
	calcula_custo_emotion(Cam,Custo).

bestfs_emotion2(Dest,[[Dest|T]|_],_,Cam,Custo):-
	reverse([Dest|T],Cam),
	calcula_custo_emotion(Cam,Custo).

bestfs_emotion2(Dest,[[Dest|_]|LLA2],NMaximoLigacoes,Cam,Custo):-
	!,
	NMaximoLigacoes1 is NMaximoLigacoes-1,
	bestfs_emotion2(Dest,LLA2,NMaximoLigacoes1,Cam,Custo).

bestfs_emotion2(Dest,LLA,NMaximoLigacoes,Cam,Custo):-
	member_emotion(LA,LLA,LLA1),
	LA=[Act|_],
	no(ActId,Act,_,_,_),

	((Act==Dest,!,bestfs_emotion2(Dest,[LA|LLA1],NMaximoLigacoes,Cam,Custo))
	 ;
	 (
	  findall((CX,[X|LA]),
	  (
	  no(XId,X,_,Emotion,EValue),
	  ligacao(ActId,XId,CX,_,_,_),
	  (X == Dest; check_emotions(Emotion,EValue)),
	  \+member(X,LA)
	  ),Novos),
	  Novos\==[],
	  sort(0,@>=,Novos,NovosOrd),
	  %write('NovosOrd='),write(NovosOrd),nl,
	  retira_custos_emotion(NovosOrd,NovosOrd1),
	  append(NovosOrd1,LLA1,LLA2),
	  %write('LLA2='),write(LLA2),nl,
	  NMaximoLigacoes>0,
	  NMaximoLigacoes1 is NMaximoLigacoes -1,
	  bestfs_emotion2(Dest,LLA2,NMaximoLigacoes1,Cam,Custo)
	 )).

member_emotion(LA,[LA|LAA],LAA).
member_emotion(LA,[_|LAA],LAA1):-member_emotion(LA,LAA,LAA1).


retira_custos_emotion([],[]).
retira_custos_emotion([(_,LA)|L],[LA|L1]):-retira_custos_emotion(L,L1).

calcula_custo_emotion([Act,X],C):-!,
                    no(ActId,Act,_,_,_),
                    no(XId,X,_,_,_),
                    ligacao(ActId,XId,C,_,_,_).
calcula_custo_emotion([Act,X|L],S):-calcula_custo_emotion([X|L],S1),
                    no(ActId,Act,_,_,_),
                    no(XId,X,_,_,_),
					ligacao(ActId,XId,C,_,_,_),S is S1+C.
