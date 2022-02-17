import { Request, Response, NextFunction } from 'express';
import { Inject, Service } from 'typedi';
import config from "../../config";

import { BaseController } from '../core/infra/BaseController';
import IPostController from "./IControllers/IPostController";
import IPostService from '../services/IServices/IPostService';
import IPostDTO from '../dto/IPostDTO';

import { Result } from "../core/logic/Result";
import ICreatingCommentResponseDTO from "../dto/ICreatingCommentResponseDTO";
import ICreatingCommentDTO from "../dto/ICreatingCommentDTO";
import { rest } from 'lodash';

@Service()
export default class PostController implements IPostController { //todo dar extends de BaseController?
    constructor(
        @Inject(config.services.post.name) private postServiceInstance: IPostService
    ){}

    async createPost(req: Request, res: Response, next: NextFunction) {
        try {
            const PostOrError = await this.postServiceInstance.createPost(req.body as IPostDTO) as Result<IPostDTO>;

            if (PostOrError.isFailure) {
                return res.status(402).send();
            }

            const PostDTO = PostOrError.getValue();
            res.status(201);
            return res.json(PostDTO);
        }
        catch (e) {
            return next(e);
        }
    }

    async getPostById(req: Request, res: Response, next: NextFunction) {
        try {
            const postId = req.params.postId
            const postOrError = await this.postServiceInstance.getPostById(postId) as Result<IPostDTO>

            if (postOrError.isFailure) {
                return res.status(400).send();
            }

            const post = postOrError.getValue();
            res.status(200);
            return res.json(post);
        } catch (e) {
            return next(e);
        }
    }

    async feedPosts(req: Request, res: Response, next: NextFunction) {
        try {
            const userId = req.params.userId
            const feedOrError = await this.postServiceInstance.feedPosts(userId) as Result<IPostDTO[]>

            if (feedOrError.isFailure) {
                return res.status(400).send();
            }

            const feedPosts = feedOrError.getValue();
            res.status(200);
            return  res.json(feedPosts);
        } catch(e) {
            return next(e);
        }
    }

    async createCommentPost(req: Request, res: Response, next: NextFunction) {
        try {
            
            const commentOrError = await this.postServiceInstance.createCommentPost(req.body as ICreatingCommentDTO) as Result<ICreatingCommentResponseDTO>;
            if(commentOrError.isFailure)
                return res.status(400).send(commentOrError.error)
            
            res.status(201)   
            return res.json(commentOrError.getValue());
        }catch(e){
            return next(e)
        }
    }

}
