
/* Efetuar esses cálculos para as Emoções Alegria e Angústia tendo como base a diferença entre Likes e Dislikes 

Invocar aumento quando likes > dislikes mas talvez seja melhor quando alguem da like e dislike

Aumento emocao : Nova emocao = emocao antiga + (1-emocao antiga) * (minimo(valor,valor)/valor saturacao) 

Dimunir emocao : novao emocao = antiga emocao * (1- (minimo(valor,valor)/valor saturacao) )
Para as Emoções Esperança, Medo, Alívio e Deceção considere o facto de ser sugerido um ou mais utilizadores para um grupo do utilizador, por exemplo o utilizador uA tem esperança que uB, uC e uD sejam incluídos numa sugestão de grupo de utilizadores e tem medo que uE e uF sejam incluídos nesse mesmo
grupo

*/

valorSaturacao(200).
valorDefaultEmocao(0.5).

% Apenas Para ser utilizado em LAPR , nao serve para efeitos de ALGAV
obter_emocao_ativa_diferenca_likes(UtilizadoId,DiferencaLikes,EmocaoAtiva):-
    no(UtilizadoId,_,_,EmocaoAtual,_),
    mudar_emocao_diferenca_likes(UtilizadoId,DiferencaLikes,[Joyful,Distressed]),
    ((EmocaoAtual=="Joyful",EmocaoAtiva is Joyful)
    ;(EmocaoAtual=="Distressed",EmocaoAtiva is Distressed)).


% WE GOT LONDON IN THE TRACK

mudar_emocao_diferenca_likes(UtilizadoId,DiferencaLikes,[Joyful,Distressed]):-
    no(UtilizadoId,_,_,EmocaoAtual,ValorEmocao),
    ((mudar_alegria(EmocaoAtual,ValorEmocao,DiferencaLikes,Joyful), Distressed is float(1 - Joyful));
    (mudar_angustia(EmocaoAtual,ValorEmocao,DiferencaLikes,Distressed)), Joyful is float(1 - Distressed)).

mudar_emocao_diferenca_likes(_,_,[Joyful,Distressed]):-
    valorDefaultEmocao(ValorDefault),
    Joyful is ValorDefault,
    Distressed is ValorDefault.

mudar_alegria("Joyful",ValorEmocao,DiferencaLikes,Resultado):-
    (DiferencaLikes>=0,aumentar_emocao_likes(ValorEmocao,DiferencaLikes,Resultado))
    ;
    (DiferencaLikes<0,DiferencaLikesAbs is abs(DiferencaLikes),diminuir_emocao_likes(ValorEmocao,DiferencaLikesAbs,Resultado)).

mudar_angustia("Distressed",ValorEmocao,DiferencaLikes,Resultado):-
    (DiferencaLikes<0,DiferencaLikesAbs is abs(DiferencaLikes),aumentar_emocao_likes(ValorEmocao,DiferencaLikesAbs,Resultado))
    ;
    (DiferencaLikes>=0,diminuir_emocao_likes(ValorEmocao,DiferencaLikes,Resultado)).

aumentar_emocao_likes(AntigaEmocao,Valor,NovaEmocao):-
    valorSaturacao(ValorSaturacao),
    ValorMin is min(Valor,ValorSaturacao), 
    NovaEmocao is float( AntigaEmocao + (1 - AntigaEmocao) * (ValorMin / ValorSaturacao)).

diminuir_emocao_likes(AntigaEmocao,Valor,NovaEmocao):-
    valorSaturacao(ValorSaturacao),
    ValorMin is min(Valor,ValorSaturacao), 
    NovaEmocao is float(  AntigaEmocao * (1 - (ValorMin / ValorSaturacao))).



% Eles foram meter beat de drill em kpop 😇 https://www.youtube.com/watch?v=uR8Mrt1IpXg 

/* 
    Sugestao de grupos 
*/


obter_emocao_ativa_utilizadores_sugeridos(UtilizadoId,ListaSugeridos,ListaEsperanca,ListaMedo,EmocaoAtiva):-
    no(UtilizadoId,_,_,EmocaoAtual,_),
    mudar_emocao_utilizadores_sugeridos(UtilizadoId,ListaSugeridos,ListaEsperanca,ListaMedo,[Hopeful,Fearful,Relieve,Disappointed]),
    ((EmocaoAtual=="Hopeful",EmocaoAtiva is Hopeful)
    ;(EmocaoAtual=="Fearful",EmocaoAtiva is Fearful)
    ;(EmocaoAtual=="Relieve",EmocaoAtiva is Relieve)
    ;(EmocaoAtual=="Disappointed",EmocaoAtiva is Disappointed)).


mudar_emocao_utilizadores_sugeridos(UtilizadoId,ListaSugeridos,ListaEsperanca,ListaMedo,[Hopeful,Fearful,Relieve,Disappointed]):-
    no(UtilizadoId,_,_,EmocaoAtual,ValorEmocao),
    length(ListaSugeridos,NSugeridos),
    intersection(ListaEsperanca,ListaSugeridos,ResultadoE),
    length(ResultadoE,NUsersEsperanca),
    intersection(ListaMedo,ListaSugeridos,ResultadoM),
    length(ResultadoM,NUsersMedo),
    NIndesejadosNEntrou is NSugeridos - NUsersMedo,
    NDesejadosNEntrou is NSugeridos - NUsersEsperanca,
    valorDefaultEmocao(ValorDefault),
    
    (
        (((mudar_esperanca(EmocaoAtual,ValorEmocao,NSugeridos,NUsersEsperanca,NUsersMedo,[Hopeful,Fearful]));
        (mudar_medo(EmocaoAtual,ValorEmocao,NSugeridos,NUsersEsperanca,NUsersMedo,[Hopeful,Fearful]))),
        mudar_dececao("Disappointed",ValorDefault,NSugeridos,NDesejadosNEntrou,NIndesejadosNEntrou,[Relieve,Disappointed]))
        ;
        (((mudar_alivio(EmocaoAtual,ValorEmocao,NSugeridos,NDesejadosNEntrou,NIndesejadosNEntrou,[Relieve,Disappointed]));
        (mudar_dececao(EmocaoAtual,ValorEmocao,NSugeridos,NDesejadosNEntrou,NIndesejadosNEntrou,[Relieve,Disappointed]))),
        mudar_esperanca("Hopeful",ValorDefault,NSugeridos,NUsersEsperanca,NUsersMedo,[Hopeful,Fearful]))
    ).

mudar_emocao_utilizadores_sugeridos(_,_,_,_,[ValorDefault,ValorDefault,ValorDefault,ValorDefault]):-
    valorDefaultEmocao(ValorDefault).


mudar_esperanca("Hopeful",AntigaEsperanca,_,NUEsperanca,NUEsperanca,[AntigaEsperanca,Fearful]):-
    Fearful is 1 - AntigaEsperanca.

mudar_esperanca("Hopeful",AntigaEsperanca,NSugeridos,NUEsperanca,NUsersMedo,[Hopeful,Fearful]):-
    ((NUEsperanca>NUsersMedo, Dif is NUEsperanca - NUsersMedo,!,aumentar_emocao(AntigaEsperanca,NSugeridos,Dif,Hopeful));
    (Dif is NUsersMedo -  NUEsperanca , ! ,diminuir_emocao(AntigaEsperanca,NSugeridos,Dif,Hopeful))),
    Fearful is float(1-Hopeful).

mudar_medo("Fearful",AntigoMedo,_,NUMedo,NUMedo,[Hopeful,AntigoMedo]):-
    Hopeful is float(1-AntigoMedo).

mudar_medo("Fearful",AntigoMedo,NSugeridos,NUEsperanca,NUsersMedo,[Hopeful,Fearful]):-
    ((NUEsperanca>NUsersMedo, Dif is NUEsperanca - NUsersMedo,!,diminuir_emocao(AntigoMedo,NSugeridos,Dif,Fearful));
    (Dif is NUsersMedo -  NUEsperanca, ! ,aumentar_emocao(AntigoMedo,NSugeridos,Dif,Fearful))),
    Hopeful is float(1 - Fearful).

mudar_alivio("Relieve",AntigoAlivio,_,NUMedo,NUMedo,[AntigoAlivio,Disappointed]):-
    Disappointed is float(1 - AntigoAlivio).

mudar_alivio("Relieve",AntigoAlivio,NSugeridos,NDesejadosNEntrou,NIndesejadosNEntrou,[Relieve,Disappointed]):-
    ((NIndesejadosNEntrou>NDesejadosNEntrou, Dif is NIndesejadosNEntrou - NDesejadosNEntrou,!,
        aumentar_emocao(AntigoAlivio,NSugeridos,Dif,Relieve));
    (Dif is NDesejadosNEntrou -  NIndesejadosNEntrou , ! ,diminuir_emocao(AntigoAlivio,NSugeridos,Dif,Relieve))),
    Disappointed is float(1-Relieve).

mudar_dececao("Disappointed",Antigadececao,_,NUMedo,NUMedo,[Relieve,Antigadececao]):-
    Relieve is float(1-Antigadececao).

mudar_dececao("Disappointed",Antigadececao,NSugeridos,NDesejadosNEntrou,NIndesejadosNEntrou,[Relieve,Disappointed]):-
    ((NIndesejadosNEntrou>NDesejadosNEntrou, Dif is NIndesejadosNEntrou - NDesejadosNEntrou,!,diminuir_emocao(Antigadececao,NSugeridos,Dif,Disappointed));
    (Dif is NDesejadosNEntrou -  NIndesejadosNEntrou , ! ,aumentar_emocao(Antigadececao,NSugeridos,Dif,Disappointed))),
    Relieve is float(1-Disappointed).
    

aumentar_emocao(AntigoValor,N,Users,NovoValor):-
    NovoValor is float(  AntigoValor + (1 - AntigoValor) * (Users / N)).

diminuir_emocao(AntigoValor,N,Users,NovoValor):-
    NovoValor is float( AntigoValor * (1 -  (Users / N))).


%no("1234","2@gmail.com",["tag1", "tag2"],"Disappointed",0.3).

%mudar_emocao_diferenca_likes("1234",100,Result).
%mudar_emocao_utilizadores_sugeridos("1234",["456","555","666","777"],["456"],[],NovaEmocao).
