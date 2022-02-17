
:-dynamic melhor_sol_maxforca1/2.
:-dynamic counter_maxforca1/1.

all_dfs_max_forca1(IdOrigem,IdCaminho,LCam):-get_time(T1),
    findall(Cam,dfs_maxforca1(IdOrigem,IdCaminho,Cam,_),LCam),
    length(LCam,NLCam),
    get_time(T2),
    write(NLCam),write(' solucoes encontradas em '),
    T is T2-T1,write(T),write(' segundos'),nl,
    write('Lista de Caminhos possiveis: '),write(LCam),nl,nl.

dfs_maxforca1(Orig,Dest,Cam,F):-dfs2_maxforca1(Orig,Dest,[Orig],Cam,F).
dfs2_maxforca1(Dest,Dest,LA,Cam,0):-!,reverse(LA,Cam).
dfs2_maxforca1(Act,Dest,LA,Cam,F):-no(ActId,Act,_,_,_),((ligacao(ActId,NextActId,F1,_,_,_),FR is F1);(ligacao(NextActId,ActId,_,F2,_,_), FR is F2)),
    no(NextActId,NextAct,_,_,_),\+ member(NextAct,LA),dfs2_maxforca1(NextAct,Dest,[NextAct|LA],Cam,FA), F is FA + FR.

caminho_maxforca1(Orig,Dest,LCaminho_minlig,F):-
	(melhor_caminho_maxforca1(Orig,Dest);true),
	retract(melhor_sol_maxforca1(LCaminho_minlig,F)).

testar_caminho_maxforca1(Orig,Dest,LCaminho_minlig,F):-
		get_time(Ti),
		(melhor_caminho_maxforca1(Orig,Dest);true),
		retract(melhor_sol_maxforca1(LCaminho_minlig,F)),
		retract(counter_maxforca1(C)), %%%
		write('N solucoes:'),write(C),nl, %%%
		get_time(Tf),
		T is Tf-Ti,
		write('Tempo de geracao da solucao:'),write(T),nl.

melhor_caminho_maxforca1(Orig,Dest):-
		retractall(melhor_sol_maxforca1(_,_)),
		asserta(melhor_sol_maxforca1(_,0)),
		asserta(counter_maxforca1(0)), %%%
		dfs_maxforca1(Orig,Dest,LCaminho,F),
		atualiza_melhor_maxforca1(LCaminho,F),
		fail.

atualiza_melhor_maxforca1(LCaminho,F):-
		retract(counter_maxforca1(C)), C1 is C + 1, %%%
		asserta(counter_maxforca1(C1)), %%%
		melhor_sol_maxforca1(_,N),
		F>N,retract(melhor_sol_maxforca1(_,_)),
		asserta(melhor_sol_maxforca1(LCaminho,F)).
