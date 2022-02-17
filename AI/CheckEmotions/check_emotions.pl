check_emotions("Distressed", Value) :- Value > 0.5, !, fail.
check_emotions("Fearful", Value) :- Value > 0.5, !, fail.
check_emotions("Disappointed", Value) :- Value > 0.5, !, fail.
check_emotions("Remorseful", Value) :- Value > 0.5, !, fail.
check_emotions("Angry", Value) :- Value > 0.5, !, fail.
check_emotions(_, _).
