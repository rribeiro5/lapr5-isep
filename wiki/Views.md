	## Contents
- [Views](#views)
	- [Introduction](#introduction)
	
	- [Nível 1](#markdown-header-nivel-1)
	  - [Vista Lógica](#N1-VL)
    - [Vista de Processos](#vista-de-processos)
	      - [SSD UC03](#SSD-UC03)
	      - [SSD UC04](#SSD-UC04)
	      - [SSD UC05](#SSD-US05)
	      - [SSD UC06](#SSD-UC06)
	      - [SSD UC07](#SSD-UC07)
	      - [SSD UC09](#SSD-US09)
	      - [SSD UC10](#SSD-UC10)
	      - [SSD UC11](#SSD-UC11)
	      - [SSD UC12](#SSD-UC12)
	      - [SSD UC13](#SSD-UC13)
	      - [SSD UC14](#SSD-UC14)
	      - [SSD UC16](#SSD-UC16)
	      - [SSD UC18](#SSD-UC18)
	      - [SSD UC20](#SSD-UC20)
	      - [SSD UC21](#SSD-UC21)
	      - [SSD UC22a](#SSD-UC22a)
	      - [SSD UC22b](#SSD-UC22b)
	      - [SSD UC24](#SSD-UC24)
	      - [SSD UC27](#SSD-UC27)
	      - [SSD UC33](#SSD-UC33)
	      - [SSD UC35](#SSD-UC35)
	  
  - [Nível 2](#markdown-header-nivel-2)
    - [Vista Lógica](#N2-VL)
	  - [Vista de Processos](#vista-de-processos-1)
	      - [SSD UC03](#SSD-UC03-(N2))
	      - [SSD UC04](#SSD-UC04-(N2))
	      - [SSD UC05](#SSD-UC05-(N2))
	      - [SSD UC06](#SSD-UC06-(N2))
	      - [SSD UC07](#SSD-UC07-(N2))
	      - [SSD UC09](#SSD-UC09-(N2))
	      - [SSD UC10](#SSD-UC10-(N2))
	      - [SSD UC11](#SSD-UC11-(N2))
	      - [SSD UC12](#SSD-UC12-(N2))
	      - [SSD UC13](#SSD-UC13-(N2))
	      - [SSD UC14](#SSD-UC14-(N2))
	      - [SSD UC16](#SSD-UC16-(N2))
	      - [SSD UC18](#SSD-UC18-(N2))
	      - [SSD UC20](#SSD-UC20-(N2))
	      - [SSD UC21](#SSD-UC21-(N2))
	      - [SSD UC22a](#SSD-UC22a-(N2))
	      - [SSD UC22b](#SSD-UC22b-(N2))
	      - [SSD UC24](#SSD-UC24-(N2))
	      - [SSD UC27](#SSD-UC27-(N2))
	      - [SSD UC33](#SSD-UC33-(N2))
	      - [SSD UC35](#SSD-UC35-(N2))
	      - [(outros SSD arquiteturalmente relevantes)](#outros-ssd-arquiteturalmente-relevantes-1)
	  - [Vista de Implementação](#N2-VI)
	  - [Vista Física](#vista-física)
    
  - [Nível 3 (Social Network Master Data)](#nível-3-(Social Network Master Data))
  
    - [Vista Lógica](#vista-lógica-2)
  
	  - [Vista de Processos](#vista-de-processos-2)
	      - [SD UC03](#SD-UC03)
	      - [SD UC04](#SD-UC04)
	      - [SD UC05](#sd-uc05)
	      - [SD UC06](#SD-UC06)
	      - [SD UC07](#SD-UC07)
	      - [SD UC09](#sd-uc09)
	      - [SD UC10](#SD-UC10)
	      - [SD UC11](#SD-UC11)
	      - [SD UC12](#SD-UC12)
	      - [SD UC13](#SD-UC13)
	      - [SD UC14](#SD-UC14)
	      - [SD UC16](#SD-UC16)
	      - [SD UC20](#SD-UC20)
	      - [SD UC21](#SD-UC21)
	      - [SD UC22a](#SD-UC22a)
	      - [SSD UC22b](#SD-UC22b)
	      - [SSD UC24](#SD-UC24)
	      - [SSD UC27](#SD-UC27)
	      - [SD UC33](#SD-UC33)
	      - [SD UC35](#SD-UC35)
    
	- [Nível 3 (Presentation)](#nível-3-Presentation)
	  - [Vista Lógica](#vista-logica)
	  - [Vista de Implementação](#vista-de-implementacao)
	  - [Vista de Processos](#vista-de-Processos)
	      - [SD UC05](#sd-uc05)
	      - [SD UC09](#sd-uc09)
	      - [SD UC13](#sd-uc13)
	      - [SD UC14](#sd-uc14)
	      - [SD UC18](#SD-uc18)

	- [Nível 3 (Master Data Post)](#nível-3-Master-Data-Post)
	  - [Vista Lógica](#vista-logica)
	  - [Vista de Implementação](#vista-de-implementacao)
	  - [Vista de Processos](#vista-de-Processos)
	      - [SD UC13](#SD-uc13)
	      - [SD UC14](#SD-UC14)
	  
	    


# Views

## Introduction
Será adotada a combinação de dois modelos de representação arquitetural: C4 e 4+1.

O Modelo de Vistas 4+1 [[Krutchen-1995]](References.md#Kruchten-1995) propõe a descrição do sistema através de vistas complementares permitindo assim analisar separadamente os requisitos dos vários stakeholders do software, tais como utilizadores, administradores de sistemas, project managers, arquitetos e programadores. As vistas são deste modo definidas da seguinte forma:

- Vista lógica: relativa aos aspetos do software visando responder aos desafios do negócio;
- Vista de processos: relativa ao fluxo de processos ou interações no sistema;
- Vista de desenvolvimento: relativa à organização do software no seu ambiente de desenvolvimento;
- Vista física: relativa ao mapeamento dos vários componentes do software em hardware, i.e. onde é executado o software;
- Vista de cenários: relativa à associação de processos de negócio com atores capazes de os espoletar.

O Modelo C4 [[Brown-2020]](References.md#Brown-2020)[[C4-2020]](References.md#C4-2020) defende a descrição do software através de quatro níveis de abstração: sistema, contentor, componente e código. Cada nível adota uma granularidade mais fina que o nível que o antecede, dando assim acesso a mais detalhe de uma parte mais pequena do sistema. Estes níveis podem ser equiparáveis a mapas, e.g. a vista de sistema corresponde ao globo, a vista de contentor corresponde ao mapa de cada continente, a vista de componentes ao mapa de cada país e a vista de código ao mapa de estradas e bairros de cada cidade.
Diferentes níveis permitem contar histórias diferentes a audiências distintas.

Os níveis encontram-se definidos da seguinte forma:
- Nível 1: Descrição (enquadramento) do sistema como um todo;
- Nível 2: Descrição de contentores do sistema;
- Nível 3: Descrição de componentes dos contentores;
- Nível 4: Descrição do código ou partes mais pequenas dos componentes (e como tal, não será abordado neste DAS/SAD).

Pode-se dizer que estes dois modelos se expandem ao longo de eixos distintos, sendo que o Modelo C4 apresenta o sistema com diferentes níveis de detalhe e o Modelo de Vista 4+1 apresenta o sistema de diferentes perspetivas. Ao combinar os dois modelos torna-se possível representar o sistema de diversas perspetivas, cada uma com vários níveis de detalhe.

Para modelar/representar visualmente, tanto o que foi implementado como as ideias e alternativas consideradas, recorre-se à Unified Modeling Language (UML) [[UML-2020]](References.md#UML-2020) [[UMLDiagrams-2020]](References.md#UMLDiagrams-2020).

## Nível 1
### Vista Lógica

![N1-VL](diagramas/nivel1/N1-VL.png)

### Vista de Processos

#### SSD UC03
![UC3-N1](diagramas/nivel1/USs/N1-UC3.svg)

#### SSD UC04
![UC4-N1](diagramas/nivel1/USs/UC04-VP.svg)

#### SSD UC05

![US2-VP](diagramas/nivel1/USs/US05-VP.svg)

#### SSD UC06

![UC6-VP](diagramas/nivel1/USs/UC06-VP.svg)

#### SSD UC07
![UC7-N1](diagramas/nivel1/USs/N1-UC7.svg)

#### SSD UC08

![US8-VP](diagramas/nivel1/USs/US08-VP.svg)

#### SSD UC09
![US2-VP](diagramas/nivel1/USs/UC09-VP.svg)

#### SSD UC10
![UC10-VP](diagramas/nivel1/USs/UC10-VP.svg)

#### SSD UC11
![UC33-N1](diagramas/nivel1/USs/N1-US11-VP.svg)
#### SSD UC12
![UC12-VP](diagramas/nivel1/USs/UC12-VP.svg)

#### SSD UC13

![UC13-VP](diagramas/nivel1/USs/UC13-VP.svg)

#### SSD UC14

![UC14-VP](diagramas/nivel1/USs/UC14-VP.svg)

### SSD Reagir a comentário / Post 

![UCReagirComentario-VP](diagramas/nivel1/USs/N1-UCReagirComentarioPost-VP.svg)

#### SSD UC16

![UC12-VP](diagramas/nivel1/USs/UC16-VP.svg)

#### SSD UC17
![UC17-N1](diagramas/nivel1/USs/N1-UC17.svg)

#### SSD UC18

![UC18-N1](diagramas/nivel1/USs/UC18-VP.svg)

#### SSD UC19

![UC19-N1](diagramas/nivel1/USs/N1-US19-VP.svg)

#### SSD UC20

![UC20-N1](diagramas/nivel1/USs/UC20-VP.svg)

#### SSD UC21

![UC20-N1](diagramas/nivel1/USs/UC21-VP.svg)

#### SSD UC22a

![UC22a-N1](diagramas/nivel1/USs/UC22a-VP.svg)

#### SSD UC22b

![UC22a-N1](diagramas/nivel1/USs/UC22b-VP.svg)

#### SSD UC23

![UC23-N1](diagramas/nivel1/USs/UC23-VP.svg)

#### SSD UC24

![UC24-N1](diagramas/nivel1/USs/UC24-VP.svg)

#### SSD UC27

![UC27-N1](diagramas/nivel1/USs/UC27-VP.svg)

#### SSD UC33

![UC33-N1](diagramas/nivel1/USs/N1-UC33.svg)

#### SSD UC35
![UC35-N1](diagramas/nivel1/USs/N1-US35-VP.svg)

#### SSD Feed de posts
![Feed-N1](diagramas/nivel1/USs/N1-Feed.svg)

## Nível 2
### Vista Lógica

![N2-VL](diagramas/nivel2/N2-VL.svg)



### Vista de Processos

#### SSD UC03 (N2)
![UC3-N2](diagramas/nivel2/USs/N2-UC3.svg)

#### SSD UC04 (N2)
![UC4-N2](diagramas/nivel2/USs/UC04-VP.svg)


#### SSD US05 (N2)

![US2-VP](diagramas/nivel2/USs/US05-VP.svg)

#### SSD UC06 (N2)

![UC6-VP](diagramas/nivel2/USs/UC06-VP.svg)

#### SSD UC07 (N2)
![UC7-N2](diagramas/nivel2/USs/N2-UC7.svg)

### SSD UC08 (N2)
![US08-VP](diagramas/nivel2/USs/US08-VP.svg)

#### SSD UC09 (N2)

![US2-VP](diagramas/nivel2/USs/UC09-VP.svg)

#### SSD UC10 (N2)

![UC10-VP](diagramas/nivel2/USs/UC10-VP.svg)

#### SSD UC11 (N2)

![UC11-VP](diagramas/nivel2/USs/N2-US11-VP.svg)

#### SSD UC12 (N2)

![UC12-VP](diagramas/nivel2/USs/UC12-VP.svg)

#### SSD UC13 (N2)

![UC13-VP](diagramas/nivel2/USs/UC13-VP.svg)


#### SSD UC14 (N2)

![UC14-VP](diagramas/nivel2/USs/UC14-VP.svg)

### SSD Reagir a comentário / Post (N2)

![UCReagirComentario-VP](diagramas/nivel2/USs/N2-UCReagirComentarioPost-VP.svg)


#### SSD UC16 (N2)

![UC12-VP](diagramas/nivel2/USs/UC16-VP.svg)

#### SSD UC17 (N2)

![UC17-N2](diagramas/nivel2/USs/N2-UC17.svg)

#### SSD UC18 (N2)

![UC17-N2](diagramas/nivel2/USs/UC18-VP.svg)

#### SSD UC19 (N2)

![UC19-N2](diagramas/nivel2/USs/N2-US19-VP.svg)

#### SSD UC20 (N2)

![UC20-N2](diagramas/nivel2/USs/UC20-VP.svg)

#### SSD UC21 (N2)

![UC20-N2](diagramas/nivel2/USs/UC21-VP.svg)

#### SSD UC22a (N2)

![UC22a-N2](diagramas/nivel2/USs/UC22a-VP.svg)

#### SSD UC22b (N2)

![UC22a-N2](diagramas/nivel2/USs/UC22b-VP.svg)

#### SSD UC23 (N2)

![UC23-N2](diagramas/nivel2/USs/UC23-VP.svg)

#### SSD UC24 (N2)

![UC24-N2](diagramas/nivel2/USs/UC24-VP.svg)

#### SSD UC27 (N2)

![UC27-N2](diagramas/nivel2/USs/UC27-VP.svg)

#### SSD UC33 (N2)
![UC33-N2](diagramas/nivel2/USs/N2-UC33.svg)

#### SSD UC35 (N2)
![UC35-N2](diagramas/nivel2/USs/N2-US35-VP.svg)

#### SSD Feed de posts (N2)
![Feed-N2](diagramas/nivel2/USs/N2-Feed.svg)

### Vista de Implementação
![N2-VI](diagramas/nivel2/N2-VI.svg)


### Vista Física

Uma proposta muito simplificada. 
![N2-VL](diagramas/nivel2/N2-VF.svg)

De facto, deve-se ter em consideração os requisitos não funcionais ["Physical Contraints"](Background.md#Physical_Constraints).



## Nível 3 (Social Network Master Data)
### Vista Lógica

Baseada numa arquitetura por camadas concêntricas (Onion):
![N3-VL](diagramas/nivel3/SNMD/N3-VL.svg)



### Vista de Processos

#### SD UC03
![UC3-N3](diagramas/nivel3/SNMD/USs/N3-UC3.svg)

#### SD UC04
![UC4-N3](diagramas/nivel3/SNMD/USs/UC04-VP.svg)


#### SD US05

![US5-VP](diagramas/nivel3/SNMD/USs/US05-VP.svg)

#### SD UC06
![UC6-VP](diagramas/nivel3/SNMD/USs/UC06-VP.svg)

#### SD UC07
![UC7-N3](diagramas/nivel3/SNMD/USs/N3-UC7.svg)

#### SD UC08
![US08-VP](diagramas/nivel3/SNMD/USs/US08-VP.svg)

#### SD UC09
![US5-VP](diagramas/nivel3/SNMD/USs/US09-VP.svg)

#### SD UC10
![UC10-VP](diagramas/nivel3/SNMD/USs/UC10-VP.svg)

#### SD UC11
![UC11-VP](diagramas/nivel3/SNMD/USs/N3-US11-VP.svg)
#### SD UC12
![UC12-VP](diagramas/nivel3/SNMD/USs/UC12-VP.svg)

#### SD UC13

![UC13-VP](diagramas/nivel3/SNMD/USs/UC13-VP.svg)

#### SD UC14

![UC14-VP](diagramas/nivel3/SNMD/USs/UC14-VP.svg)

### SD Reagir a comentário / Post 

![UCReagirComentario-VP](diagramas/nivel3/SNMD/USs/N3-UCReagirComentarioPost-SNMD-VP.svg)

#### SD UC16

![UC12-VP](diagramas/nivel3/SNMD/USs/UC16-VP.svg)

#### SD UC20

![UC20-N3](diagramas/nivel3/SNMD/USs/UC20-VP.svg)

#### SD UC21

![UC20-N3](diagramas/nivel3/SNMD/USs/UC21-VP.svg)

#### SD UC22a

![UC22a-N3](diagramas/nivel3/SNMD/USs/UC22a-VP.svg)

#### SD UC22b

![UC22b-N3](diagramas/nivel3/SNMD/USs/UC22b-VP.svg)

#### SD UC23

![UC23-N3](diagramas/nivel3/SNMD/USs/UC23-VP.svg)

#### SD UC24

![UC24-N3](diagramas/nivel3/SNMD/USs/UC24-VP.svg)

#### SD UC27

![UC27-N3](diagramas/nivel3/SNMD/USs/UC27-VP.svg)


#### SD UC33
![UC33-N3](diagramas/nivel3/SNMD/USs/N3-UC33.svg)

#### SD UC35
![UC35-N3](diagramas/nivel3/SNMD/USs/UC35-VP.svg)

#### SD Feed de posts
![Feed-N3](diagramas/nivel3/SNMD/USs/N3-Feed-MDRS.svg)

### Vista de Implementação
![N3-VI](diagramas/nivel3/SNMD/N3-VI.svg)



### Vista Física

Por agora, não existe necessidade de ser representada.



## Nível 3 (Presentation)

### Vista Lógica

![N3-UI-VL](diagramas/nivel3/UI/N3-UI-VL.svg)

### Vista de Implementação

![N3-UI-VL](diagramas/nivel3/UI/N3-UI-VI.svg)



## Vista de Processos

#### UC03
![N3-UC03-VP](diagramas/nivel3/UI/UCs/N3-UC3-SPA.svg)

#### UC04

![N3-UC04-VP](diagramas/nivel3/UI/UCs/N3-UC4-SPA.svg)

#### UC05

![N3-UC05-VP](diagramas/nivel3/UI/UCs/UC05-VP.svg)

#### UC07

![N3-UC07-VP](diagramas/nivel3/UI/UCs/N3-UC7-SPA.svg)

#### UC06

![N3-UC06-VP](diagramas/nivel3/UI/UCs/UC06-VP.svg)

#### UC08

![N3-UC08-VP](diagramas/nivel3/UI/UCs/N3-US08-SPA-VP.svg)

#### UC09

![N3-UC09-VP](diagramas/nivel3/UI/UCs/UC09-VP.svg)

#### UC10

![N3-UC10-VP](diagramas/nivel3/UI/UCs/UC10-VP.svg)

#### UC11

![N3-UC11-VP](diagramas/nivel3/UI/UCs/N3-UC11-SPA-VP.svg)

#### UC12

![N3-UC12-VP](diagramas/nivel3/UI/UCs/UC12-VP.svg)

#### UC13

![N3-UC13-VP](diagramas/nivel3/UI/UCs/UC13-VP.svg)

#### UC14

![N3-UC14-VP](diagramas/nivel3/UI/UCs/UC14-VP.svg)

#### UC16

![N3-UC16-VP](diagramas/nivel3/UI/UCs/N3-UC16-SPA-VP.svg)

#### UC18

![N3-UC09-VP](diagramas/nivel3/UI/UCs/UC18-VP.svg)

#### UC19

![N3-UC19-VP](diagramas/nivel3/UI/UCs/N3-US19-VP.svg)

#### UC20

![N3-UC20-SPA-VP](diagramas/nivel3/UI/UCs/N3-UC20-SPA-VP.svg)

#### UC21

![N3-UC21-SPA-VP](diagramas/nivel3/UI/UCs/N3-UC21-SPA-VP.svg)

#### UC22a - Leader board - dimensão de rede

![N3-UC33-VP](diagramas/nivel3/UI/UCs/N3-UC22a-SPA-VP.svg)

#### UC22b

![N3-UC22b-VP](diagramas/nivel3/UI/UCs/N3-UC22b-SPA.svg)

#### UC23

![N3-UC23-VP](diagramas/nivel3/UI/UCs/N3-UC23-SPA.svg)

#### UC24

![N3-UC24-VP](diagramas/nivel3/UI/UCs/N3-UC24-SPA.svg)

#### UC25
![N3-UC25-SPA-VP](diagramas/nivel3/UI/UCs/N3-UC25-SPA-VP.svg)

#### UC26

![N3-UC26-SPA-VP](diagramas/nivel3/UI/UCs/N3-UC26-SPA-VP.svg)

#### UC27

![N3-UC27-VP](diagramas/nivel3/UI/UCs/N3-UC27-SPA.svg)

#### UC28

![N3-UC28-VP](diagramas/nivel3/UI/UCs/N3-UC28-SPA.svg)

#### UC33

![N3-UC33-VP](diagramas/nivel3/UI/UCs/N3-UC33-SPA.svg)

#### UC35

![N3-UC35-VP](diagramas/nivel3/UI/UCs/N3-US35-SPA-VP.svg)

#### Feed de posts

![N3-Feed-VP](diagramas/nivel3/UI/UCs/N3-Feed-SPA.svg)

### SD Reagir a comentário / Post (N3)

![UCReagirComentario-VP](diagramas/nivel3/UI/UCs/N3-UCN3-ReagirComentarioPost-SPA-VP.svg)


## Nível 3 (Master Data Posts)
### Vista Lógica

Baseada numa arquitetura por camadas concêntricas (Onion):
![N3-VL](diagramas/nivel3/MDP/N3-MDP-VL.svg)

### Vista de Implementação
![N3-VI](diagramas/nivel3/MDP/N3-MDP-VI.svg)

### Vista de Processos

#### SD UC13

![UC13-VP](diagramas/nivel3/MDP/UCs/UC13-VP.svg)

#### SD UC14

![UC14-VP](diagramas/nivel3/MDP/UCs/UC14-VP.svg)

### SD Reagir a comentário / Post 

![UCReagirComentario-VP](diagramas/nivel3/MDP/UCs/N3-UCReagirComentarioPost-MDP-VP.svg)

#### SD Feed de posts

![N3-Feed-VP](diagramas/nivel3/MDP/UCs/N3-Feed-MDP.svg)