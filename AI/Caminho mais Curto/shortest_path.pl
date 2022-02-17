:-dynamic melhor_sol_minlig/2.


plan_minlig(Orig,Dest,LCaminho_minlig):-
	        (melhor_caminho_minlig(Orig,Dest);true),!,
		retract(melhor_sol_minlig(LCaminho_minlig,_)).

melhor_caminho_minlig(Orig,Dest):-
		asserta(melhor_sol_minlig(_,10000)),
		dfs(Orig,Dest,LCaminho),
		atualiza_melhor_minlig(LCaminho),
		fail.

atualiza_melhor_minlig(LCaminho):-
		melhor_sol_minlig(_,N),
		length(LCaminho,C),
		C<N,retract(melhor_sol_minlig(_,_)),
		asserta(melhor_sol_minlig(LCaminho,C)).


dfs(Orig,Dest,Cam):-dfs2(Orig,Dest,[Orig],Cam).

dfs2(Dest,Dest,LA,Cam):-reverse(LA,Cam).

dfs2(Act,Dest,LA,Cam):-
		no(ActId,Act,_,_,_),
		(ligacao(ActId,NActId,_,_,_,_);
		ligacao(NActId,ActId,_,_,_,_)),
		no(NActId,NAct,_,_,_),\+ member(NAct,LA),
		dfs2(NAct,Dest,[NAct|LA],Cam).







