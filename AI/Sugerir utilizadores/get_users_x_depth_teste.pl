%para efeitos de teste, centro da lista para id=1
get_connections_up_to_level_2(L):-
	L=[1,11,12,13,14,51,61,21,22,23,24].

get_connections_up_to_level_3(L):-
	L=[1,11,12,13,14,51,61,21,22,23,24,31,32,33,34,200].

get_connections_up_to_level_4(L):-
	L=[1,11,12,13,14,51,61,21,22,23,24,31,32,33,34,41,42,43,44,200].


%temporary predicate to get users up to X depth
get_users_x_depth(X,L):-
	X==2->get_connections_up_to_level_2(L);
	X==3->get_connections_up_to_level_3(L);
	get_connections_up_to_level_4(L).