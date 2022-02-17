%:-[bc_teste].
:-[sinonimos].
:-['utils.pl'].

%X: number of common tags
%Lusers: List of users with X common tags

common_users_all_tags(X,Lusers):-
	get_all_tags(Stags),
	all_users_with_x_common_tags(X,Stags,Lusers).


all_users_with_x_common_tags(X,LTags,Lusers):-
	all_combos(X,LTags,LcombXTags1),
	change_to_synonims_lists(LcombXTags1,LcombXTags),
	setof(UserId,find_user_with_x_common_tags(X,LcombXTags,UserId),Lusers).


find_user_with_x_common_tags(_,[],_):-fail.


find_user_with_x_common_tags(X,[LcombX|LcombXTags],UserId):-
	no(UserId,_,Ltags1,_,_),
	change_to_synonims(Ltags1,Ltags),
	intersect_with_len(Ltags,LcombX,_,Len),
	((Len>=X,true);
	find_user_with_x_common_tags(X,LcombXTags,UserId)).
	

change_to_synonims_lists([],[]).


change_to_synonims_lists([Comb1|LcombXTags1],[Comb2|LcombXTags2]):-
	change_to_synonims(Comb1,Comb2),!,
	change_to_synonims_lists(LcombXTags1,LcombXTags2).


change_to_synonims_lists([Comb1|LcombXTags1],[Comb1|LcombXTags2]):-
	change_to_synonims_lists(LcombXTags1,LcombXTags2).
	
	

get_all_tags(Stags):-
	findall(Tags,(no(_,_,Tags,_,_)),Ltags1),
	flatten(Ltags1,Ltags2),
	list_to_set(Ltags2,Stags2),
	remove_synonims(Stags2,Stags).


remove_synonims([],[]):-!.


remove_synonims([Tag1|Stags1],[Synonim|Stags2]):-
	sinonimo(Synonim,Tag1),!,
	remove_synonims(Stags1,Stags2).
	

remove_synonims([Tag1|Stags1],[Tag1|Stags2]):-
	remove_synonims(Stags1,Stags2).
	

	
all_combos(X,LTags,LcombXTags):-
	findall(L,combo(X,LTags,L),LcombXTags).


combo(0,_,[]):-!.


combo(X,[Tag|L],[Tag|T]):-
	X1 is X-1,
	combo(X1,L,T).


combo(X,[_|L],T):-
	combo(X,L,T).