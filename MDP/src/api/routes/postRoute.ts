import { Router } from 'express';
import { celebrate, Joi } from 'celebrate';

import { Container } from 'typedi';
import IPostController from '../../controllers/IControllers/IPostController';

import config from "../../../config";

const route = Router();

export default (app: Router) => {
    app.use('/posts', route);

    const ctrl = Container.get(config.controllers.post.name) as IPostController;

    route.post('',
        celebrate({
            body: Joi.object({
                userId:Joi.string().required(),
                text: Joi.string().required(),
                tags:Joi.array().items(Joi.string()),
            })
        }),
        (req, res, next) => ctrl.createPost(req, res, next));

    route.get('/:postId',
        celebrate({
            params: Joi.object({
                postId: Joi.string().required(),
            })
        }),
        (req, res, next) => ctrl.getPostById(req, res, next));
    
    route.get('/feed/:userId',
        celebrate({
            params: Joi.object({
                userId: Joi.string().required(),
            })
        }),
        (req, res, next) => ctrl.feedPosts(req, res, next));
};