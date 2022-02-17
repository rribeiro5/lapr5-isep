import { Service, Inject } from 'typedi';
import config from "../../config";
import { Result } from "../core/logic/Result";
import IReactionDTO from '../dto/IReactionDTO';
import ICreatingReactionDTO from "../dto/ICreatingReactionDTO"
import ICreatingReactionResponseDTO from "../dto/ICreatingReactionResponseDTO"
 
import Reaction from "../domain/Reaction/Reaction"
import IReactionService from './IServices/IReactionService';

import { PostMap } from '../mappers/PostMap';
import IPostRepo from './IRepos/IPostRepo';



@Service()
export default class ReactionService implements IReactionService {
    
    constructor(
        @Inject(config.repos.post.name) private postRepo: IPostRepo
    ) { }

    public async createReactionPost(reactionDTO:ICreatingReactionDTO): Promise<Result<ICreatingReactionResponseDTO>> {
   
        const reactionOrError = await Reaction.create(reactionDTO as IReactionDTO )

        if(reactionOrError.isFailure){
            return Result.fail<ICreatingReactionResponseDTO>(reactionOrError.errorValue())
        }
        
        const addReactionToPost = await this.postRepo.updateReactionToPost(reactionOrError.getValue(),reactionDTO.publicationId)

        if(addReactionToPost===null)
            return Result.fail<ICreatingReactionResponseDTO>("Post not found")

        return Result.ok<ICreatingReactionResponseDTO>(addReactionToPost)
    }
    

    public async createReactionCommentary(reactionDTO:ICreatingReactionDTO): Promise<Result<ICreatingReactionResponseDTO>> {
        
        const reactionOrError = await Reaction.create(reactionDTO as IReactionDTO )

        if(reactionOrError.isFailure){
            return Result.fail<ICreatingReactionResponseDTO>(reactionOrError.errorValue())
        }
        
        const addReactionToPost = await this.postRepo.updateReactionToComment(reactionOrError.getValue(),reactionDTO.publicationId)

        if(addReactionToPost===null)
            return Result.fail<ICreatingReactionResponseDTO>("Comment not found")

        return Result.ok<ICreatingReactionResponseDTO>(addReactionToPost)

    }
    
}