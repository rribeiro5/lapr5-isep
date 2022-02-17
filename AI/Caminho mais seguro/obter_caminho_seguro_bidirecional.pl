:-dynamic cam_mais_seguro_bidirecional/2.

dfs_seguro_bidirecional(Orig,Dest,MinLigacao,Cam,ForcaSomatorio):-dfs_seguro_bidirecional2(Orig,Dest,MinLigacao,[Orig],Cam,0,ForcaSomatorio).
dfs_seguro_bidirecional2(Dest,Dest,_,LA,Cam,ForcaResultante,ForcaResultante):-!,reverse(LA,Cam).
dfs_seguro_bidirecional2(Act,Dest,MinLigacao,LA,Cam,ForcaSomatorio,ForcaCaminho):-no(ActId,Act,_,_,_),
    ((ligacao(ActId,NextActId,ForcaLigacao1,ForcaLigacao2,_,_));(ligacao(NextActId,ActId,ForcaLigacao1,ForcaLigacao2,_,_))),
    TempSomatorio is ForcaLigacao1+ForcaLigacao2,
    TempSomatorio>=MinLigacao,
    ForcaSomatorio1 is ForcaSomatorio + TempSomatorio,
    no(NextActId,NextAct,_,_,_),\+ member(NextAct,LA),dfs_seguro_bidirecional2(NextAct,Dest,MinLigacao,[NextAct|LA],Cam,ForcaSomatorio1,ForcaCaminho).


caminho_seguro_bidirecional(Orig,Dest,MinLigacao,LCaminho_maior_Forca,ForcaLigacaoMaior):-
        (melhor_caminho_seguro_bidirecional(Orig,Dest,MinLigacao);true),  
        retract(cam_mais_seguro_bidirecional(LCaminho_maior_Forca,ForcaLigacaoMaior)).


testar_caminho_seguro_bidirecional(Orig,Dest,MinLigacao,LCaminho_maior_Forca,ForcaLigacaoMaior):-
        get_time(Ti),
        (melhor_caminho_seguro_bidirecional(Orig,Dest,MinLigacao);true),  
        retract(cam_mais_seguro_bidirecional(LCaminho_maior_Forca,ForcaLigacaoMaior)), 
        get_time(Tf),
        T is Tf-Ti,
        write('Tempo de geracao da solucao:'),write(T),nl.

melhor_caminho_seguro_bidirecional(Orig,Dest,MinLigacao):-
        asserta(cam_mais_seguro_bidirecional(_,-10000)), 
        dfs_seguro_bidirecional(Orig,Dest,MinLigacao,LCaminho,ForcaSomatorio),
        atualiza_caminho_seguro(LCaminho,ForcaSomatorio),
        fail.

atualiza_caminho_seguro(LCaminho,New):-
        cam_mais_seguro_bidirecional(_,Old), 
        New>Old,retract(cam_mais_seguro_bidirecional(_,_)),
        asserta(cam_mais_seguro_bidirecional(LCaminho,New)). 


% Base de Conhecimento



