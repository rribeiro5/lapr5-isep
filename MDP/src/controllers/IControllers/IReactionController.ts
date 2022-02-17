import { Request, Response, NextFunction } from 'express';

export default interface IReactionController {
    createReactionPost(req: Request, res: Response, next: NextFunction);
    createReactionCommentary(req: Request, res: Response, next: NextFunction);
}