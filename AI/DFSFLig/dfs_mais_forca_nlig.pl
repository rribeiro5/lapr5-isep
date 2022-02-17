
:-dynamic melhor_sol_maxforca_nlig1/2.
:-dynamic counter_maxforca_nlig1/1.

all_dfs_max_forca_nlig1(IdOrigem,IdCaminho,N,LCam):-get_time(T1),
    findall(Cam,dfs_maxforca_nlig1(IdOrigem,IdCaminho,N,Cam,_),LCam),
    length(LCam,NLCam),
    get_time(T2),
    write(NLCam),write(' solucoes encontradas em '),
    T is T2-T1,write(T),write(' segundos'),nl,
    write('Lista de Caminhos possiveis: '),write(LCam),nl,nl.

dfs_maxforca_nlig1(Orig,Dest,N,Cam,F):-dfs2_maxforca_nlig1(Orig,Dest,N,0,[Orig],Cam,F).
dfs2_maxforca_nlig1(Dest,Dest,_,_,LA,Cam,0):-!,reverse(LA,Cam).
dfs2_maxforca_nlig1(Act,Dest,N,A,LA,Cam,F):- A < N, no(ActId,Act,_,_,_),((ligacao(ActId,NextActId,F1,_,_,_),FR is F1);(ligacao(NextActId,ActId,_,F2,_,_), FR is F2)),
    no(NextActId,NextAct,_,_,_),\+ member(NextAct,LA), A1 is A + 1,dfs2_maxforca_nlig1(NextAct,Dest,N,A1,[NextAct|LA],Cam,FA), F is FA + FR.

caminho_maxforca_nlig1(Orig,Dest,N,LCaminho_minlig,F):-
	(melhor_caminho_maxforca_nlig1(Orig,Dest,N);true),
	retract(melhor_sol_maxforca_nlig1(LCaminho_minlig,F)).

testar_caminho_maxforca_nlig1(Orig,Dest,N,LCaminho_minlig,F):-
		get_time(Ti),
		(melhor_caminho_maxforca_nlig1(Orig,Dest,N);true),
		retract(melhor_sol_maxforca_nlig1(LCaminho_minlig,F)),
		retract(counter_maxforca_nlig1(C)), %%%
		write('N solucoes:'),write(C),nl, %%%
		get_time(Tf),
		T is Tf-Ti,
		write('Tempo de geracao da solucao:'),write(T),nl.

melhor_caminho_maxforca_nlig1(Orig,Dest,N):-
		retractall(melhor_sol_maxforca_nlig1(_,_)),
		asserta(melhor_sol_maxforca_nlig1(_,0)),
		asserta(counter_maxforca_nlig1(0)), %%%
		dfs_maxforca_nlig1(Orig,Dest,N,LCaminho,F),
		atualiza_melhor_maxforca_nlig1(LCaminho,F),
		fail.

atualiza_melhor_maxforca_nlig1(LCaminho,F):-
		retract(counter_maxforca_nlig1(C)), C1 is C + 1, %%%
		asserta(counter_maxforca_nlig1(C1)), %%%
		melhor_sol_maxforca_nlig1(_,N),
		F>N,retract(melhor_sol_maxforca_nlig1(_,_)),
		asserta(melhor_sol_maxforca_nlig1(LCaminho,F)).
