
all_dfs(Nome1,Nome2,LCam):-
    findall(Cam,dfs(Nome1,Nome2,Cam),LCam).

dfs(Orig,Dest,Cam):-dfs2(Orig,Dest,[Orig],Cam).

dfs2(Dest,Dest,LA,Cam):-!,reverse(LA,Cam).

dfs2(Act,Dest,LA,Cam):-
    no(NAct,Act,_,_,_),
    (ligacao(NAct,NX,_,_,_,_);ligacao(NX,NAct,_,_,_,_)),
    no(NX,X,_,_,_),
    \+ member(X,LA),
    dfs2(X,Dest,[X|LA],Cam).
