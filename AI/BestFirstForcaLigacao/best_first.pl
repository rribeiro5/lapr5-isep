%:-[baseConhecimento].

bestfs1(Orig,Dest,NMaximoLigacoes,Cam,Custo):-
	((bestfs12(Dest,[[Orig]],NMaximoLigacoes,Cam,Custo)
	);(Cam=[],Custo is -1)).

bestfs12(Dest,[[Dest|T]|_],0,Cam,Custo):-
	reverse([Dest|T],Cam),
	calcula_custo(Cam,Custo).

bestfs12(Dest,[[Dest|T]|_],_,Cam,Custo):-
	reverse([Dest|T],Cam),
	calcula_custo(Cam,Custo).

bestfs12(Dest,[[Dest|_]|LLA2],NMaximoLigacoes,Cam,Custo):-
	!,
	NMaximoLigacoes1 is NMaximoLigacoes-1,
	bestfs12(Dest,LLA2,NMaximoLigacoes1,Cam,Custo).

bestfs12(Dest,LLA,NMaximoLigacoes,Cam,Custo):-
	member1(LA,LLA,LLA1),
	LA=[Act|_],
	no(ActId,Act,_,_,_),

	((Act==Dest,!,bestfs12(Dest,[LA|LLA1],NMaximoLigacoes,Cam,Custo))
	 ;
	 (
	  findall((CX,[X|LA]),
	  (
	  no(XId,X,_,_,_),
	  ligacao(ActId,XId,CX,_,_,_),
	  \+member(X,LA)
	  ),Novos),
	  Novos\==[],
	  sort(0,@>=,Novos,NovosOrd),
	  %write('NovosOrd='),write(NovosOrd),nl,
	  retira_custos(NovosOrd,NovosOrd1),
	  append(NovosOrd1,LLA1,LLA2),
	  %write('LLA2='),write(LLA2),nl,
	  NMaximoLigacoes>0,
	  NMaximoLigacoes1 is NMaximoLigacoes -1,
	  bestfs12(Dest,LLA2,NMaximoLigacoes1,Cam,Custo)
	 )).

member1(LA,[LA|LAA],LAA).
member1(LA,[_|LAA],LAA1):-member1(LA,LAA,LAA1).


retira_custos([],[]).
retira_custos([(_,LA)|L],[LA|L1]):-retira_custos(L,L1).

calcula_custo([Act,X],C):-!,
                    no(ActId,Act,_,_,_),
                    no(XId,X,_,_,_),
                    ligacao(ActId,XId,C,_,_,_).
calcula_custo([Act,X|L],S):-calcula_custo([X|L],S1),
                    no(ActId,Act,_,_,_),
                    no(XId,X,_,_,_),
					ligacao(ActId,XId,C,_,_,_),S is S1+C.
