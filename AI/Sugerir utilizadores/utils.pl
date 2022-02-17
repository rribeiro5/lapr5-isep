%intersect two lists and return the size of the result
intersect_with_len([],_,[],0):-!.


intersect_with_len([T|Ltags1],Ltags2,[T|Ltags3],X):-
	member(T,Ltags2),
	intersect_with_len(Ltags1,Ltags2,Ltags3,X1),!,
	X is X1+1.

	
intersect_with_len([_|Ltags1],Ltags2,Ltags3,X):-
	intersect_with_len(Ltags1,Ltags2,Ltags3,X).
	