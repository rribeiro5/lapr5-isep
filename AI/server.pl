:-module(startServer,
    [startServer/1
    ]).

% Bibliotecas HTTP
:- use_module(library(http/thread_httpd)).
:- use_module(library(http/http_dispatch)).
:- use_module(library(http/http_parameters)).
:- use_module(library(http/http_unix_daemon)).
:- use_module(library(http/http_open)).
:- use_module(library(http/http_cors)).
:- use_module(library(http/http_client)).
:- use_module(library(date)).
:- use_module(library(random)).

% Bibliotecas JSON
:- use_module(library(http/json_convert)).
:- use_module(library(http/http_json)).
:- use_module(library(http/json)).

suggestionDepth(3).


%% Nossos modules

:-ensure_loaded("./Caminho mais seguro/obter_caminho_seguro_bidirecional.pl").
:-ensure_loaded("./Caminho mais seguro/obter_caminho_seguro_unidirecional.pl").
:-ensure_loaded("./Caminho mais Curto/shortest_path.pl").
:-ensure_loaded("./Sugerir utilizadores/sugerir_conexoes.pl").
:-ensure_loaded("./Caminho mais forte/dfs_mais_forca1.pl").
:-ensure_loaded("./Caminho mais forte/dfs_mais_forca2.pl").
:-ensure_loaded("./AStarFLig/astar_flig.pl").
:-ensure_loaded("./CaminhoMaisForteFRel/dfs_mais_forca_nrel.pl").
:-ensure_loaded("./DFSFLig/dfs_mais_forca_nlig.pl").
:-ensure_loaded("./AStarForcaLigacaoForcaRelacao/astar_forca_ligacao_forca_relacao.pl").
:-ensure_loaded("./BestFirstForcaLigacaoForcaRelacao/bestfs.pl").
:-ensure_loaded("./BestFirstForcaLigacao/best_first.pl").
:-ensure_loaded("./Sugerir Grupos/sugestao_grupo.pl").
:-ensure_loaded("./NovosValoresEstadoEmocional/NovosValoresEstadoEmocional.pl").
:-ensure_loaded("./CaminhoMaisForteFRel/dfs_mais_forca_nrel_emotion.pl").
:-ensure_loaded("./AStarForcaLigacaoForcaRelacao/astar_forca_ligacao_forca_relacao_emotion.pl").
:-ensure_loaded("./BestFirstForcaLigacao/best_first_emotion.pl").
:-ensure_loaded("./DFSFLig/dfs_mais_forca_nlig_emotion.pl").
:-ensure_loaded("./BestFirstForcaLigacaoForcaRelacao/bestfs_emotion.pl").
:-ensure_loaded("./AStarFLig/astar_flig_emotion.pl").

% Objectos Json a receber no prolog
:- json_object caminho_seguro_user_json(caminho:list(string),forcaResultante:integer).
:- json_object caminho_mais_curto_user_json(caminho:list(string)).
:- json_object users_sugeridos_json(ids:list(string)).
:- json_object caminho_mais_forte_json(caminho:list(string),forcaResultante:integer).
:- json_object caminho_astar_fligacao(caminho:list(string),forcaResultante:integer).
:- json_object caminho_mais_forte_frel_json(caminho:list(string),forcaResultante:float).
:- json_object caminho_bestfs_flig_frel_json(caminho:list(string),forcaResultante:float).
:- json_object caminho_bestfs_flig_json(caminho:list(string),forcaResultante:integer).
:- json_object sugestao_grupo(grupo:list(string)).
:- json_object responseUpdateEmotionalStateLikesDto(userId:string,differenceLikes:integer,newEmotionValue:float).
:- json_object responseUpdateEmotionalStateGroupSugestions(userId:string,suggestedUsers:list(string),hopeUsers:list(string),scareUsers:list(string),newEmotionValue:float).


%% Base Conhecimento principal

:- dynamic no/5.
:- dynamic ligacao/6.



% Gerir servidor
startServer(Port) :-						
    http_server(http_dispatch, [port(Port)]),
    asserta(port(Port)).

stopServer:-
    retract(port(Port)),
    http_stop_server(Port,_).


%Cors
:- set_setting(http:cors, [*]).



%Porta default para o server
%default_server_port(4000).
default_server_port(80).

%url da API


%obter_users_url("http://localhost:5000/api/users").
%obter_ligacoes_url("http://localhost:5000/api/Connections/bidirecional").
obter_users_url("https://lapr5g50.azurewebsites.net/api/users").
obter_ligacoes_url("https://lapr5g50.azurewebsites.net/api/Connections/bidirecional").

adicionarClientes():-
    obterUsersRegistados(Data),
    parse_users(Data).

%% Obter users Registados (Data nao convertida)

obterUsersRegistados(Data):-
    obter_users_url(URL),
    setup_call_cleanup(
        http_open(URL, In, [ cert_verify_hook(cert_accept_any)]),
        json_read_dict(In, Data),
        close(In)
).    

%% Convertes Json de todos Users em Lista

parse_users([]).
parse_users([H|List]):-
    asserta(no(H.get(id),H.get(email),H.get(interestTags),H.get(emotionalState),H.get(emotionalValue))),
    parse_users(List).


%% Colocar as ligacoes da api dinamicamente no prolog
adicionarLigacoes():-
    obterLigacoes(Data),
    parse_ligacoes(Data).


obterLigacoes(Data):-
    obter_ligacoes_url(URL),
    setup_call_cleanup(
        http_open(URL, In, [ cert_verify_hook(cert_accept_any)]),
        json_read_dict(In, Data),
        close(In)
).   

parse_ligacoes([]).
parse_ligacoes([H|List]):-
    asserta(ligacao(H.get(oUser),H.get(dUser),H.get(connectionStrengthOUser),H.get(connectionStrengthDUser),H.get(relationshipStrengthOUser),H.get(relationshipStrengthDUser))),
    parse_ligacoes(List).


% Remover dados do sistema

removerBaseConhecimento():-
        retractall(no(_,_,_,_,_)),
        retractall(ligacao(_,_,_,_,_,_)).
        

% Handlers 
:- http_handler('/api/caminhoSeguroUnidirecional', obter_caminho_seguro_unidirecional_handler, []).
:- http_handler('/api/caminhoSeguroBidirecional', obter_caminho_seguro_bidirecional_handler, []).
:- http_handler('/api/caminhoMaisCurto', obter_caminho_mais_curto_handler, []).
:- http_handler('/api/suggested',get_users_by_connects_and_tags,[]).
:- http_handler('/api/caminhoMaisForcaUnidirecional', obter_caminho_mais_forte1, []).
:- http_handler('/api/caminhoMaisForcaBidirecional', obter_caminho_mais_forte2, []).
:- http_handler('/api/caminhoAStarFLig', obter_caminho_astar_fligacao, []).
:- http_handler('/api/caminhoDFSFRelacao', obter_caminho_mais_forte_frel, []).
:- http_handler('/api/caminhoDFSFLigacao', obter_caminho_mais_forte_flig, []).
:- http_handler('/api/caminhoAStarFLigFRel', obter_caminho_astar_fligacao_frelacao_handler, []).
:- http_handler('/api/caminhoBestfsFLigFRel', obter_caminho_bestfs_fligacao_frelacao_handler, []).
:- http_handler('/api/updateEmotionLikesDislikes', atualizar_emocao_likes_dislikes_handler, []).
:- http_handler('/api/updateEmotionGroupSuggestion', atualizar_emocao_sugestao_grupos_handler, []).
:- http_handler('/api/caminhoBestfsFLig', obter_caminho_bestfs_fligacao_handler, []).
:- http_handler('/api/suggestGroup', sugerir_grupo_handler, []).
:- http_handler('/api/caminhoBestfsFLigEmocao', obter_caminho_bestfs_fligacao_emotion_handler, []).
:- http_handler('/api/caminhoAStarFLigFRelEmocao', obter_caminho_astar_fligacao_frelacao_emotion_handler, []).
:- http_handler('/api/caminhoDFSFLigFRelEmocao', obter_caminho_mais_forte_flig_frel_emotion_handler, []).
:- http_handler('/api/caminhoDFSFLigacaoEmocao', obter_caminho_mais_forte_flig_emotion, []).
:- http_handler('/api/caminhoBestfsFLigFRelEmocao', obter_caminho_bestfs_fligacao_frelacao_emotion_handler, []).
:- http_handler('/api/caminhoAStarFLigEmocao', obter_caminho_astar_fligacao_emotion, []).

obter_caminho_seguro_unidirecional_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [origId(Orig,[string]),
         destId(Dest,[string]),
         minLigacao(MinLigacao,[integer,between(0,100)])] ),


    caminho_seguro_unidirecional(Orig,Dest,MinLigacao,LCaminho,Forca),
    Reply=caminho_seguro_user_json(LCaminho,Forca),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


obter_caminho_seguro_bidirecional_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [origId(Orig,[string]),
         destId(Dest,[string]),
         minLigacao(MinLigacao,[integer,between(0,100)])] ),

    caminho_seguro_bidirecional(Orig,Dest,MinLigacao,LCaminho,Forca),
    Reply=caminho_seguro_user_json(LCaminho,Forca),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


obter_caminho_mais_curto_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [origId(Orig,[string]),
         destId(Dest,[string])]),

    plan_minlig(Orig,Dest,LCaminho),
    Reply=caminho_mais_curto_user_json(LCaminho),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).



get_users_by_connects_and_tags(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [id(Id,[string])]),
    suggestionDepth(Depth),
    get_suggested_users(Depth,Id,LUsers),
    %LUsers=["1","2"],
    Reply=users_sugeridos_json(LUsers),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_mais_forte1(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string])] ),
    
    caminho_maxforca1(Orig,Dest,L,F),
    Reply=caminho_mais_forte_json(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_mais_forte2(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string])] ),
    
    caminho_maxforca2(Orig,Dest,L,F),
    Reply=caminho_mais_forte_json(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_astar_fligacao(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_aStar_flig(Orig,Dest,N,L,F),
    Reply=caminho_astar_fligacao(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


obter_caminho_astar_fligacao_frelacao_handler(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_AStar_fLigacao_fRelacao(Orig,Dest,N,L,F),
    Reply=caminho_mais_forte_frel_json(L,F), 
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


obter_caminho_mais_forte_frel(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_maxforca_nrel1(Orig,Dest,N,L,F),
    Reply=caminho_mais_forte_frel_json(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_mais_forte_flig(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_maxforca_nlig1(Orig,Dest,N,L,F),
    Reply=caminho_mais_forte_json(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_bestfs_fligacao_frelacao_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    bestfs_fl_fr(Orig,Dest,N,C,F),
    Reply=caminho_astar_fligacao(C,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_bestfs_fligacao_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
        dest(Dest,[string]),
        maxLigacoes(N,[integer])] ),
    bestfs1(Orig,Dest,N,C,F),
    Reply=caminho_bestfs_flig_json(C,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


sugerir_grupo_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_read_json(Request,DictIn,[json_object(dict)]),
    suggest_group(DictIn.get(userId),DictIn.get(lTagsObrigatorias),DictIn.get(nTagsComum),DictIn.get(nMinimoUsers),Group),
    Reply=sugestao_grupo(Group),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


atualizar_emocao_likes_dislikes_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_read_json(Request,DictIn,[json_object(dict)]),
    obter_emocao_ativa_diferenca_likes(DictIn.get(userId),DictIn.get(differenceLikes),NovaEmocao),
    Reply=responseUpdateEmotionalStateLikesDto(DictIn.get(userId),DictIn.get(differenceLikes),NovaEmocao),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).


atualizar_emocao_sugestao_grupos_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_read_json(Request,DictIn,[json_object(dict)]),
    obter_emocao_ativa_utilizadores_sugeridos(DictIn.get(userId),DictIn.get(suggestedUsers),DictIn.get(hopeUsers),DictIn.get(scareUsers),NovaEmocao),
    Reply=responseUpdateEmotionalStateGroupSugestions(DictIn.get(userId),DictIn.get(suggestedUsers),DictIn.get(hopeUsers),DictIn.get(scareUsers),NovaEmocao),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_mais_forte_flig_frel_emotion_handler(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_maxforca_nrel_emotion(Orig,Dest,N,L,F),
    Reply=caminho_mais_forte_frel_json(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_astar_fligacao_frelacao_emotion_handler(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_AStar_fLigacao_fRelacao_emocao(Orig,Dest,N,L,F),
    Reply=caminho_mais_forte_frel_json(L,F), 
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_bestfs_fligacao_emotion_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
        dest(Dest,[string]),
        maxLigacoes(N,[integer])] ),
    bestfs_emotion(Orig,Dest,N,C,F),
    Reply=caminho_bestfs_flig_json(C,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_mais_forte_flig_emotion(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_maxforca_nlig_emotion(Orig,Dest,N,L,F),
    Reply=caminho_mais_forte_json(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_bestfs_fligacao_frelacao_emotion_handler(Request):-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    bestfs_fl_fr_emotion(Orig,Dest,N,C,F),
    Reply=caminho_astar_fligacao(C,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

obter_caminho_astar_fligacao_emotion(Request) :-
    cors_enable,
    removerBaseConhecimento(),
    carregar_dados_sistema(),
    http_parameters(Request,
        [orig(Orig,[string]),
         dest(Dest,[string]),
         maxLigacoes(N,[integer])] ),
    caminho_aStar_flig_emotion(Orig,Dest,N,L,F),
    Reply=caminho_astar_fligacao(L,F),
    prolog_to_json(Reply, JSONObject),
    reply_json(JSONObject,[json_object]).

%Métodos que carregam os dados para sistema
carregar_dados_sistema():-
    adicionarClientes(),
    adicionarLigacoes().


inicializar_server:-
    default_server_port(Port),
    startServer(Port),!,
    carregar_dados_sistema(),!.



%:-inicializar_server.

