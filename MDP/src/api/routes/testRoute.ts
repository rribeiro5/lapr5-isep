import { Router } from 'express';
import { celebrate, Joi } from 'celebrate';

import { Container } from 'typedi';

import config from "../../../config";
import TestController from '../../controllers/testController';
import PostRepo from '../../repos/postRepo';

const route = Router();

export default (app: Router) => {
    app.use('/test', route);
    const repo = Container.get(config.repos.post.name) as PostRepo
    const ctrl = new TestController(repo);

    route.delete('/comments/:postId/:commentId',
        celebrate({
            params: Joi.object({
                postId: Joi.string().required(),
                commentId: Joi.string().required(),
            })
        }),
        (req, res, next) => ctrl.deleteComment(req, res, next));

    route.delete('/posts/:postId',
        celebrate({
            params: Joi.object({
                postId: Joi.string().required(),
            })
        }),
        (req, res, next) => ctrl.deletePost(req, res, next));
};