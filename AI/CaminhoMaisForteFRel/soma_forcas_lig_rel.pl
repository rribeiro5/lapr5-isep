soma_forcas_lig_rel(FLig,FRel,Res) :- FRel < -200 , !, soma_forcas_lig_rel(FLig, -200, Res).
soma_forcas_lig_rel(FLig,FRel,Res) :- FRel > 200 , !, soma_forcas_lig_rel(FLig, 200, Res).
soma_forcas_lig_rel(FLig,FRel,Res) :- Res is float((FLig * 0.5) + (((FRel + 200) // 4) * 0.5)).