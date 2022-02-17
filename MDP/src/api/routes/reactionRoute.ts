import { Router } from 'express';
import { celebrate, Joi } from 'celebrate';

import { Container } from 'typedi';
import IReactionController from '../../controllers/IControllers/IReactionController';

import config from "../../../config";

const route = Router();

export default (app: Router) => {
    app.use('/reactions', route);

    const ctrl = Container.get(config.controllers.reaction.name) as IReactionController;
    
    route.post('/posts',
        celebrate({
            body: Joi.object({
                publicationId:Joi.string().required(),
                userId: Joi.string().required(),
                reaction:Joi.string().required(),
            })
        }),
        (req, res, next) => ctrl.createReactionPost(req, res, next));

    route.post('/comments',
        celebrate({
            body: Joi.object({
                publicationId:Joi.string().required(),
                userId: Joi.string().required(),
                reaction:Joi.string().required(),
            })
        }),
        (req, res, next) => ctrl.createReactionCommentary(req, res, next));    

        
};