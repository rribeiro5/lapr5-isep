import { Mapper } from "../core/infra/Mapper";
import { Document, Model } from 'mongoose';
import { UniqueEntityID } from "../core/domain/UniqueEntityID";
import Reaction  from "../domain/Reaction/Reaction";
import IReactionDTO from "../dto/IReactionDTO";
import { IPostPersistence } from "../dataschema/IPostPersistence";

export default class ReactionMap extends Mapper<Reaction> {

    public static toDTO(reaction: Reaction): IReactionDTO{
        return {
            userId : reaction.userId.toString(),
            reaction : reaction.reactionType.toString()
        } as IReactionDTO;
    }

    public static toPersistence(reaction: Reaction): any{
        return {
            userId : reaction.userId.toString(),
            typeReaction : reaction.reactionType.toString(),
            createdAt: reaction.creationTime
        }
    }

    // talvez adicionar metodo de toDomain
    
}