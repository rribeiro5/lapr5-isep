:-[bc].
:-[utils].
:-['../Sugerir Utilizadores/utils.pl'].
:-[tag_combos].

%list index value, list of tags, User Count
:-dynamic indexTagListUserCount/3. 
%list index value, UserId
:-dynamic indexUser/2. 



suggest_group(UserId,LTagsObrigatoriasNaoTraduzidas,NTagsComum,NMinimoUsers,Group):-	
	NTagsComum>=0,
	NMinimoUsers>=0,
	translate_LTagsObrigatorias(LTagsObrigatoriasNaoTraduzidas,LTagsObrigatorias,NumTagsObrigatorias),!,
	((NTagsComum>=NumTagsObrigatorias,
	get_users_and_tag_lists(UserId,LTagsObrigatorias,NumTagsObrigatorias,UsersTags),
	get_combinations_from_tags_lists(UsersTags,LTagsObrigatorias,NumTagsObrigatorias,NTagsComum,LCombTags),
	find_biggest_group_if_exists(LCombTags,UsersTags,NTagsComum,NMinimoUsers,Group),
	!);Group=[]).


translate_LTagsObrigatorias(LTagsObrigatoriasNaoTraduzidas,LTagsObrigatorias,NumTagsObrigatorias):-
	change_to_synonims(LTagsObrigatoriasNaoTraduzidas,LTagsObrigatorias1),
	remove_duplicates(LTagsObrigatorias1,LTagsObrigatorias),
	length(LTagsObrigatorias,NumTagsObrigatorias).


get_combinations_from_tags_lists(UsersTags,LTagsObrigatorias,NumTagsObrigatorias,NTagsComum,LCombTags):-
	extract_tags(UsersTags,LTags),
	flatten_unique(LTagsObrigatorias,LTags,LTagsFlatten),
	get_tag_combos(NTagsComum,LTagsFlatten,LTagsObrigatorias,NumTagsObrigatorias,LCombTags).


get_users_and_tag_lists(UserId,LTagsObrigatorias,NumTagsObrigatorias,UsersTags):-
	findall((X,LTagsTraduzidas),
		(
			no(X,_,LTags,_,_),
			X\==UserId,
			change_to_synonims(LTags,LTagsTraduzidas),
			intersect_with_len(LTagsTraduzidas,LTagsObrigatorias,_,Len),
			Len>=NumTagsObrigatorias
		),
		UsersTags).


find_biggest_group_if_exists(LCombTags,UsersTags,NTagsComum,NMinimoUsers,Group):-
	init_index_tag_list_user_count_map(LCombTags),
	compare_users_with_combos(UsersTags,NTagsComum),
	((find_biggest_group(Index,Count),
		Count>=NMinimoUsers,
		get_group(Index,Group)
		);!,clear(),Group=[]
	),clear().


find_biggest_group(Index,Count):-
	indexTagListUserCount(Index,_,Count),
	\+ (indexTagListUserCount(_,_,Count1),Count1>Count).



clear():-
	retractall(indexTagListUserCount(_,_,_)),
	retractall(indexUser(_,_)).


get_group(Index,Group):-
	findall(User,indexUser(Index,User),Group).


compare_users_with_combos([],_):-!.


compare_users_with_combos([(User,Tags)|T],Minimum):-
	compare_user_list_with_combos((User,Tags),Minimum),
	compare_users_with_combos(T,Minimum).


compare_user_list_with_combos((User,Tags),Minimum):-
	findall(_,
		(
			indexTagListUserCount(Index,TagComb,Count),
			intersect_with_len(Tags,TagComb,_,Len),
			Len>=Minimum,
			retract(indexTagListUserCount(Index,_,_)),
			NewCount is Count +1,
			asserta(indexTagListUserCount(Index,TagComb,NewCount)),
			asserta(indexUser(Index,User))
			),_).


extract_tags([],[]):-!.


extract_tags([(_,Tags)|T],[TagsTraduzidas|LTags]):-
	change_to_synonims(Tags,TagsTraduzidas),
	extract_tags(T,LTags).


get_tag_combos(NTagsComum,LTags,LTagsObrigatorias,NumTagsObrigatorias,LCombTags):-
	NTagsComum1 is NTagsComum-NumTagsObrigatorias,
	all_combos(NTagsComum1,LTags,LTagsObrigatorias,LCombTags).



init_index_tag_list_user_count_map(LLTags):-	
	init_index_tag_list_user_count_map1(_,LLTags).	


init_index_tag_list_user_count_map1(0,[]):-!.


init_index_tag_list_user_count_map1(Index,[LTags|LLTags]):-
	init_index_tag_list_user_count_map1(Index1,LLTags),
	asserta(indexTagListUserCount(Index1,LTags,0)),
	Index is Index1+1.	
