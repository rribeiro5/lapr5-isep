import { Router } from 'express';
import { celebrate, Joi } from 'celebrate';

import { Container } from 'typedi';
import IPostController from '../../controllers/IControllers/IPostController';

import config from "../../../config";

const route = Router();

export default (app: Router) => {
  app.use('/posts', route);

  const ctrl = Container.get(config.controllers.post.name) as IPostController;

  route.post('/comments',
    celebrate({
      body: Joi.object({
        postId:Joi.string().required(),
        userId: Joi.string().required(),
        text:Joi.string().required(),
        reactions:Joi.array()
      })
    }),
    (req, res, next) => ctrl.createCommentPost(req, res, next));
};
