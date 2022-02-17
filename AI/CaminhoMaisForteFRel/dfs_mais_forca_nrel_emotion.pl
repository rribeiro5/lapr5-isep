:- ['soma_forcas_lig_rel.pl'].
:- ['../CheckEmotions/check_emotions.pl'].

:-dynamic melhor_sol_maxforca_nrel_emotion/2.
:-dynamic counter_maxforca_nrel_emotion/1.

all_dfs_max_forca_nrel_emotion(IdOrigem,IdCaminho,N,LCam):-get_time(T1),
    findall(Cam,dfs_maxforca_nrel_emotion(IdOrigem,IdCaminho,N,Cam,_),LCam),
    length(LCam,NLCam),
    get_time(T2),
    write(NLCam),write(' solucoes encontradas em '),
    T is T2-T1,write(T),write(' segundos'),nl,
    write('Lista de Caminhos possiveis: '),write(LCam),nl,nl.

dfs_maxforca_nrel_emotion(Orig,Dest,N,Cam,F):-dfs2_maxforca_nrel_emotion(Orig,Dest,N,0,[Orig],Cam,F).
dfs2_maxforca_nrel_emotion(Dest,Dest,_,_,LA,Cam,0.0):-!,reverse(LA,Cam).
dfs2_maxforca_nrel_emotion(Act,Dest,N,A,LA,Cam,F):- A < N, no(ActId,Act,_,_,_),((ligacao(ActId,NextActId,F1,_,FR1,_), soma_forcas_lig_rel(F1,FR1,FR));(ligacao(NextActId,ActId,_,F2,_,FR2), soma_forcas_lig_rel(F2,FR2,FR))),
    no(NextActId,NextAct,_,Emotion,EValue),((NextAct==Dest);check_emotions(Emotion,EValue)), \+ member(NextAct,LA), A1 is A + 1,dfs2_maxforca_nrel_emotion(NextAct,Dest,N,A1,[NextAct|LA],Cam,FA), F is FA + FR.

caminho_maxforca_nrel_emotion(Orig,Dest,N,LCaminho_minlig,F):-
	(melhor_caminho_maxforca_nrel_emotion(Orig,Dest,N);true),
	retract(melhor_sol_maxforca_nrel_emotion(LCaminho_minlig,F)).

testar_caminho_maxforca_nrel_emotion(Orig,Dest,N,LCaminho_minlig,F):-
		get_time(Ti),
		(melhor_caminho_maxforca_nrel_emotion(Orig,Dest,N);true),
		retract(melhor_sol_maxforca_nrel_emotion(LCaminho_minlig,F)),
		retract(counter_maxforca_nrel_emotion(C)), %%%
		write('N solucoes:'),write(C),nl, %%%
		get_time(Tf),
		T is Tf-Ti,
		write('Tempo de geracao da solucao:'),write(T),nl.

melhor_caminho_maxforca_nrel_emotion(Orig,Dest,N):-
		retractall(melhor_sol_maxforca_nrel_emotion(_,_)),
		asserta(melhor_sol_maxforca_nrel_emotion(_,0.0)),
		asserta(counter_maxforca_nrel_emotion(0)), %%%
		dfs_maxforca_nrel_emotion(Orig,Dest,N,LCaminho,F),
		atualiza_melhor_maxforca_nrel_emotion(LCaminho,F),
		fail.

atualiza_melhor_maxforca_nrel_emotion(LCaminho,F):-
		retract(counter_maxforca_nrel_emotion(C)), C1 is C + 1, %%%
		asserta(counter_maxforca_nrel_emotion(C1)), %%%
		melhor_sol_maxforca_nrel_emotion(_,N),
		F>N,retract(melhor_sol_maxforca_nrel_emotion(_,_)),
		asserta(melhor_sol_maxforca_nrel_emotion(LCaminho,F)).
