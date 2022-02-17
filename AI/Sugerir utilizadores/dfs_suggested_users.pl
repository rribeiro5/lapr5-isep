call_all_dfs(_,[],_,[]):-!.


call_all_dfs(Orig,[Dest|Lusers],Ltags,[Dest|L]):-
	dfs_suggestions(Orig,Dest,Ltags),!,
	call_all_dfs(Orig,Lusers,Ltags,L).

	
call_all_dfs(Orig,[_|Lusers],Ltags,L):-
	!,call_all_dfs(Orig,Lusers,Ltags,L).



%dfs
dfs_suggestions(Orig,Dest,LTagsOrig):-
	%verificar que o no destino possui a tag
	no(Dest,_,Ltags2,_,_),
	change_to_synonims(Ltags2,Ltags1),
	intersect_with_len(LTagsOrig,Ltags1,Ltags,Len),
	Len>0,
	dfs_suggestions2(Orig,Dest,[Orig],Ltags),!.


dfs_suggestions2(Dest,Dest,_,_).

	
dfs_suggestions2(Act,Dest,LA,LcommonTags):-
	user_friend(Act,X),
	\+ member(X,LA),	
	no(Act,_,Ltags1,_,_),
	change_to_synonims(Ltags1,LtagsAct),
	intersect_with_len(LcommonTags,LtagsAct,Ltags,Len),
	Len>0,
	dfs_suggestions2(X,Dest,[X|LA],Ltags).