import { Mapper } from "../core/infra/Mapper";
import { Document, Model } from 'mongoose';
import { UniqueEntityID } from "../core/domain/UniqueEntityID";
import Reaction  from "../domain/Reaction/Reaction";
import IReactionDTO from "../dto/IReactionDTO";
import { IPostPersistence } from "../dataschema/IPostPersistence";
import {Comment} from "../domain/Comment/Comment";
import ICommentDTO from "../dto/ICommentDTO";
import ReactionMap from "./ReactionMap";
import ICreatingCommentResponseDTO from "../dto/ICreatingCommentResponseDTO";

export default class CommentMap extends Mapper<Comment> {

  public static toDTO(comment: Comment, postId:string): ICommentDTO | ICreatingCommentResponseDTO{
    const id = (typeof comment.id.toValue !== 'function') 
      ? comment.id : comment.id.toValue()
    return {
      id: id,
      postId: postId,
      userId : comment.userId.toString(),
      text: comment.text.props.value,
      reactions: comment.reactions.map(reaction=>ReactionMap.toDTO(reaction)),
      creationDateTime:comment.creationDateTime.value
    } as ICommentDTO | ICreatingCommentResponseDTO;
  }


  public static toPersistence(comment: Comment): any{
    return {
      domainId: comment.id.toValue(),
      userId : comment.userId.toString(),
      text: comment.text.value,
      reactions: comment.reactions.map(x=>ReactionMap.toDTO(x)),
      creationDateTime: comment.creationDateTime.value
    }
  }

}
