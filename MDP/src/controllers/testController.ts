import { Request, Response, NextFunction } from "express";
import { emitWarning } from "process";
import { Inject } from "typedi";
import config from "../../config";
import PostRepo from "../repos/postRepo";
import IPostRepo from "../services/IRepos/IPostRepo";

export default class TestController{
    postRepo: PostRepo;

    constructor( postRepo:PostRepo){
        this.postRepo = postRepo
    }

    public async deletePost(req: Request, res: Response, next: NextFunction) {
        await this.postRepo.deletePost(req.params.postId);
        res.status(204);
        return res.json("post deleted");
    }

    public async deleteComment(req: Request, res: Response, next: NextFunction){
        await this.postRepo.deleteComment(req.params.postId, req.params.commentId);
        res.status(204);
        return res.json("comment deleted");
    }
}