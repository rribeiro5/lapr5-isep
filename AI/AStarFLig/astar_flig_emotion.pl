:- ['../CheckEmotions/check_emotions.pl'].

:-dynamic cam_aStar_fligacao_emotion/2.

aStar_fligacao_emotion(Orig,Dest,N,Cam,Custo):-
    aStar_fligacao2_emotion(Dest,[(_,0,[Orig])],N,Cam,Custo).

aStar_fligacao2_emotion(Dest,[(_,Custo,[Dest|T])|_],_,Cam,Custo):-
    reverse([Dest|T],Cam).

aStar_fligacao2_emotion(Dest,[(_,Ca,LA)|Outros],N,Cam,Custo):-
    LA=[Act|_],
    no(ActId,Act,_,_,_),
    max_flig_astar_emotion(ActId,Dest,LA,MFLig),
    findall((CEX,CaX,[X|LA]),
        (Dest\==Act,
        length(LA,Tamanho),
        Tamanho=<N,
        ligacao(ActId,XId,F1,_,_,_),
        no(XId,X,_,Emotion,EValue),
        CustoX is F1,
        (X == Dest; check_emotions(Emotion,EValue)),
        \+ member(X,LA),
        CaX is CustoX + Ca,
        N1 is N - Tamanho,
        estimativa_fligacao_emotion(MFLig,N1,EstX),
        CEX is CaX + EstX), Novos),
    append(Outros,Novos,Todos),
    sort(0,@>=,Todos,TodosOrd),
    aStar_fligacao2_emotion(Dest,TodosOrd,N,Cam,Custo).

max_flig_astar_emotion(No,Dest,LA,MFLig):-
    findall(FLig,
        (Dest\==No,
        ligacao(No,XId,FLig,_,_,_),
        no(XId,X,_,_,_),
        \+ member(X,LA))
        ,Forcas),
    append([1],Forcas,Todos),
    sort(0,@>=,Todos,TodosOrd),
    TodosOrd=[MFLig|_].

estimativa_fligacao_emotion(MFLig,Rest,Estimativa):- Estimativa is MFLig * Rest.

caminho_aStar_flig_emotion(Orig,Dest,N,C,F):-
    (melhor_caminho_aStar_flig_emotion(Orig,Dest,N);true),
    retract(cam_aStar_fligacao_emotion(C,F)).

testar_caminho_aStar_flig_emotion(Orig,Dest,N,C,F):-
    get_time(Ti),
    (melhor_caminho_aStar_flig_emotion(Orig,Dest,N);true),
    retract(cam_aStar_fligacao_emotion(C,F)),
    get_time(Tf),
    T is Tf-Ti,
    write('Tempo de geracao da solucao:'),write(T),nl.

melhor_caminho_aStar_flig_emotion(Orig,Dest,N):-
    asserta(cam_aStar_fligacao_emotion(_,-1)),
    aStar_fligacao_emotion(Orig,Dest,N,C,F),
    atualiza_melhor_caminho_aStar_flig_emotion(C,F),
    fail.

atualiza_melhor_caminho_aStar_flig_emotion(C,F):-
    cam_aStar_fligacao_emotion(_,N),
    F > N,
    retract(cam_aStar_fligacao_emotion(_,_)),
    asserta(cam_aStar_fligacao_emotion(C,F)).
