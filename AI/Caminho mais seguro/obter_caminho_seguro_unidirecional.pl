:-dynamic cam_mais_seguro_unidirecional/2.

dfs_seguro_unidirecional(Orig,Dest,MinLigacao,Cam,ForcaSomatorio):-dfs_seguro_unidirecional2(Orig,Dest,MinLigacao,[Orig],Cam,0,ForcaSomatorio).
dfs_seguro_unidirecional2(Dest,Dest,_,LA,Cam,ForcaResultante,ForcaResultante):-!,reverse(LA,Cam).
dfs_seguro_unidirecional2(Act,Dest,MinLigacao,LA,Cam,ForcaSomatorio,ForcaCaminho):-no(ActId,Act,_,_,_),
    ((ligacao(ActId,NextActId,ForcaLigacaoAtual,_,_,_));(ligacao(NextActId,ActId,_,ForcaLigacaoAtual,_,_))),
    ForcaLigacaoAtual>=MinLigacao,
    ForcaSomatorio1 is ForcaSomatorio + ForcaLigacaoAtual,
    no(NextActId,NextAct,_,_,_),\+ member(NextAct,LA),dfs_seguro_unidirecional2(NextAct,Dest,MinLigacao,[NextAct|LA],Cam,ForcaSomatorio1,ForcaCaminho).


caminho_seguro_unidirecional(Orig,Dest,MinLigacao,LCaminho_maior_Forca,ForcaLigacaoMaior):-
        (melhor_caminho_seguro_unidirecional(Orig,Dest,MinLigacao);true),  
retract(cam_mais_seguro_unidirecional(LCaminho_maior_Forca,ForcaLigacaoMaior)).

testar_caminho_seguro_unidirecional(Orig,Dest,MinLigacao,LCaminho_maior_Forca,ForcaLigacaoMaior):-
        get_time(Ti),
        (melhor_caminho_seguro_unidirecional(Orig,Dest,MinLigacao);true),  
        retract(cam_mais_seguro_unidirecional(LCaminho_maior_Forca,ForcaLigacaoMaior)), 
        get_time(Tf),
        T is Tf-Ti,
        write('Tempo de geracao da solucao:'),write(T),nl.

melhor_caminho_seguro_unidirecional(Orig,Dest,MinLigacao):-
        asserta(cam_mais_seguro_unidirecional(_,-10000)), 
        dfs_seguro_unidirecional(Orig,Dest,MinLigacao,LCaminho,ForcaSomatorio),
        atualiza_caminho_seguro1(LCaminho,ForcaSomatorio),
        fail.

atualiza_caminho_seguro1(LCaminho,New):-
        cam_mais_seguro_unidirecional(_,Old), 
        New>Old,retract(cam_mais_seguro_unidirecional(_,_)),
        asserta(cam_mais_seguro_unidirecional(LCaminho,New)). 

% Base de Conhecimento
