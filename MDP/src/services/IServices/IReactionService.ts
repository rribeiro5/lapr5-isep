import { Result } from "../../core/logic/Result";
import ICreatingReactionDTO from "../../dto/ICreatingReactionDTO"
import ICreatingReactionResponseDTO from "../../dto/ICreatingReactionResponseDTO"
 
export default interface IReactionService{
    createReactionPost(reactionDTO:ICreatingReactionDTO): Promise<Result<ICreatingReactionResponseDTO>>
    createReactionCommentary(reactionDTO:ICreatingReactionDTO): Promise<Result<ICreatingReactionResponseDTO>>
}