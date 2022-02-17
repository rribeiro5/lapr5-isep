import { Request, Response, NextFunction } from 'express';
import { Inject, Service } from 'typedi';
import config from "../../config";

import IReactionController from './IControllers/IReactionController';

import IReactionService from '../services/IServices/IReactionService';
import ICreatingReactionDTO from '../dto/ICreatingReactionDTO';
import ICreatingReactionResponseDTO from "../dto/ICreatingReactionResponseDTO";

import { Result } from "../core/logic/Result";
import { ParamsDictionary } from 'express-serve-static-core';
import { ParsedQs } from 'qs';


@Service()
export default class ReactionController implements IReactionController { 
    
    constructor(
        @Inject(config.services.reaction.name) private reactionServiceInstance: IReactionService
    ){}


    async createReactionPost(req: Request<ParamsDictionary, any, any, ParsedQs, Record<string, any>>, res: Response<any, Record<string, any>>, next: NextFunction) {
        try {
            
            const ReactionOrError = await this.reactionServiceInstance.createReactionPost(req.body as ICreatingReactionDTO) as Result<ICreatingReactionResponseDTO>;
            
            if(ReactionOrError.isFailure)
                return res.status(400).send(ReactionOrError.error)

            res.status(201);
            return res.json(ReactionOrError.getValue());

        }catch(e){
            return next(e)
        }    
    }


    async createReactionCommentary(req: Request<ParamsDictionary, any, any, ParsedQs, Record<string, any>>, res: Response<any, Record<string, any>>, next: NextFunction) {
        try {
            
            const ReactionOrError = await this.reactionServiceInstance.createReactionCommentary(req.body as ICreatingReactionDTO) as Result<ICreatingReactionResponseDTO>;
            
            if(ReactionOrError.isFailure)
                return res.status(400).send(ReactionOrError.error)

            res.status(201);
            return res.json(ReactionOrError.getValue());

        }catch(e){
            return next(e)
        }
    }
    
    }