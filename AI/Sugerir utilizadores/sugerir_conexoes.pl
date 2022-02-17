:-['../Sugerir Grupos/bc'].
%:-[sinonimos].
:-['../Sugerir Grupos/sinonimos'].
:-['dfs_suggested_users.pl'].
:-['get_users_x_depth_teste.pl'].
:-['utils.pl'].
:-['../Tamanho da Rede/network_size.pl'].

:-dynamic tagsUserEmSinonimo/2.


/*
* X:max depth level
* Lusers: List of users up to X depth
* LsuggestedUsers: List of suggested Users
*/

get_suggested_users(X,UserId,LsuggestedUsers):-
	tamanho_rede(UserId,X,Lusers,_),
	all_connected_users_with_n_common_tags(1,UserId,Lusers,LconnectedUsers),
	prepare_all_dfs(UserId,LconnectedUsers,LsuggestedUsers),
	retractall(tagsUserEmSinonimo(_,_)).


prepare_all_dfs(UserId,LconnectedUsers,Lusers):-
	tagsUserEmSinonimo(UserId,Ltags),!,
	call_all_dfs(UserId,LconnectedUsers,Ltags,Lusers).


all_connected_users_with_n_common_tags(N,UserId,Lusers,LconnectedUsers):-
	get_list_without_user_and_friends(UserId,Lusers,LusersWithoutFriends),
	no(UserId,_,LuserTags1,_,_), %get user tags
	change_to_synonims(LuserTags1,LuserTags),	
	assertz(tagsUserEmSinonimo(UserId,LuserTags)),
	all_users_in_list_with_x_common_tags(N, LusersWithoutFriends,LuserTags,LconnectedUsers).


get_list_without_user_and_friends(UserId,Lusers,LusersWithoutFriends):-
	get_user_friends(UserId,Lfriends),
	subtract(Lusers,[UserId|Lfriends],LusersWithoutFriends).



%all users with x common tags
all_users_in_list_with_x_common_tags(_,[],_,[]):-!.


all_users_in_list_with_x_common_tags(X,[H|Lusers],Ltags,[H|L]):-
	user_with_x_common_tags(X,H,Ltags),!,
	all_users_in_list_with_x_common_tags(X,Lusers,Ltags,L).


all_users_in_list_with_x_common_tags(X,[_|Lusers],Ltags,L):-
	all_users_in_list_with_x_common_tags(X,Lusers,Ltags,L).


user_with_x_common_tags(X,UserId,Ltags):-
	%bucar lista de tags do user
	no(UserId,_,LuserTags1,_,_),
	change_to_synonims(LuserTags1,LuserTags),
	intersection(LuserTags,Ltags,LI),
	length(LI,Size),
	Size >= X,
	assertz(tagsUserEmSinonimo(UserId,LuserTags)).

	
%all user friends
get_user_friends(UserId,LF):-
	findall(FriendId,user_friend(UserId,FriendId),LF).	
	

user_friend(UserId,FriendId):-
	ligacao(UserId,FriendId,_,_,_,_);ligacao(FriendId,UserId,_,_,_,_).

