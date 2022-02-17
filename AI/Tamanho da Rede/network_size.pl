
:-dynamic utilizadorVisitado/1.

tamanho_rede(Orig,Nivel,L, Total):-
    retractall(utilizadorVisitado(_)),
    findall(Orig,dfs(Orig,Nivel),_),
    findall(User,utilizadorVisitado(User),L),
    retractall(utilizadorVisitado(_)),
    length(L,Total).


dfs(Orig,Nivel):-
    dfs2(Orig,Nivel,[Orig]).


dfs2(_,0,_):-!.


dfs2(Act,Nivel,LA):-
    Nivel > 0,
    (ligacao(Act,X,_,_,_,_);ligacao(X,Act,_,_,_,_)),
    \+utilizadorVisitado(X),
    \+ member(X,LA),
    Nivel1 is Nivel-1,
    asserta(utilizadorVisitado(X)),
    dfs2(X,Nivel1,[X|LA]).
