import { Request, Response, NextFunction } from 'express';

export default interface IPostController {
    createPost(req: Request, res: Response, next: NextFunction);
    getPostById(req: Request, res: Response, next: NextFunction);
    feedPosts(req: Request, res: Response, next: NextFunction);
    createCommentPost(req: Request, res: Response, next: NextFunction);
}
