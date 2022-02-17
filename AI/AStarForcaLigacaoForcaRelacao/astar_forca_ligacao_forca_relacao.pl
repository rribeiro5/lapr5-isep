:-dynamic cam_aStar_fLigacao_fRelacao/2.

aStar_forca_ligacao_forca_relacao(Orig,Dest,NCamadas,Cam,Custo):-
    aStar_forca_ligacao_forca_relacao2(Dest,[(_,0,[Orig])],NCamadas,Cam,Custo).

aStar_forca_ligacao_forca_relacao2(Dest,[(_,Custo,[Dest|T])|_],_,Cam,Custo):-
    reverse([Dest|T],Cam).

aStar_forca_ligacao_forca_relacao2(Dest,[(_,CAtual,ListaA)|Outros],NCamadas,Cam,Custo):-
    ListaA=[AtualEmail|_],
    no(AtualId,AtualEmail,_,_,_),
    max_flig_frel_astar(AtualId,Dest,ListaA,MFLigRel),
    findall((CEstimado,CaX,[FriendEmail|ListaA]),
        (Dest\==AtualEmail,
        length(ListaA,Tamanho),
        Tamanho=<NCamadas,
        ligacao(AtualId,FriendId,FL1,_,FR1,_), 
        no(FriendId,FriendEmail,_,_,_),
        \+ member(FriendEmail,ListaA),
        soma_forcas_lig_rel_a_star(FL1,FR1,Soma),
        CaX is CAtual + Soma ,
        CamadasRestantes is NCamadas - Tamanho,
        estimativa(MFLigRel,CamadasRestantes,EstX),
        CEstimado is CaX + EstX), Novos),
    append(Outros,Novos,Todos),
    sort(0,@>=,Todos,TodosOrd),
    aStar_forca_ligacao_forca_relacao2(Dest,TodosOrd,NCamadas,Cam,Custo).

max_flig_frel_astar(No,Dest,LA,MFLigRel):-
    findall(SomaForcaRelacao,
        (Dest\==No,
            ligacao(No,FriendId,FL1,_,FR1,_), 
            no(FriendId,FriendEmail,_,_,_),
        \+ member(FriendEmail,LA),
        soma_forcas_lig_rel_a_star(FL1,FR1,SomaForcaRelacao)),
    Forcas),
    append([0],Forcas,Todos),
    sort(0,@>=,Todos,TodosOrd),
    TodosOrd=[MFLigRel|_].


estimativa(MFLigRel,CamadasRestantes,Estimativa):- 
    Estimativa is MFLigRel * CamadasRestantes.


soma_forcas_lig_rel_a_star(FLig,FRel,Res) :- 
    ((FRel>200,FRelacao1 is 200);
        (FRel < -200 , FRelacao1 is -200);FRelacao1 is FRel),
        !,
    Res is float((FLig / 2) + ((FRelacao1 + 200) / 8)).


caminho_AStar_fLigacao_fRelacao(Orig,Dest,NCamadas,Caminho,Custo):-
    (melhor_caminho_AStar_fLigacao_fRelacao(Orig,Dest,NCamadas);true),
retract(cam_aStar_fLigacao_fRelacao(Caminho,Custo)).    


testar_caminho_AStar_fLigacao_fRelacao(Orig,Dest,NCamadas,Caminho,Custo):-
    get_time(Ti),
        (melhor_caminho_AStar_fLigacao_fRelacao(Orig,Dest,NCamadas);true),  
        retract(cam_aStar_fLigacao_fRelacao(Caminho,Custo)), 
        get_time(Tf),
        T is Tf-Ti,
        write('Tempo de geracao da solucao:'),write(T),nl.


melhor_caminho_AStar_fLigacao_fRelacao(Orig,Dest,NCamadas):-
    asserta(cam_aStar_fLigacao_fRelacao(_,-9999.9)),
    aStar_forca_ligacao_forca_relacao(Orig,Dest,NCamadas,Caminho,Custo),
    atualizar_melhor_caminho_AStar_fLigacao_fRelacao(Caminho,Custo),
    fail.

atualizar_melhor_caminho_AStar_fLigacao_fRelacao(Caminho,Custo):-
    cam_aStar_fLigacao_fRelacao(_,Old),
    Custo>Old,
    retract(cam_aStar_fLigacao_fRelacao(_,_)),
    asserta(cam_aStar_fLigacao_fRelacao(Caminho,Custo)).
