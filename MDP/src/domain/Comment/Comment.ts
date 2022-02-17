import { UniqueEntityID } from "../../core/domain/UniqueEntityID";
import { Entity } from "../../core/domain/Entity";

import { Result } from "../../core/logic/Result";
import ICommentDTO from "../../dto/ICommentDTO";
import { CommentId } from "./CommentId";
import  Reaction from "../Reaction/Reaction";
import { CommentText } from "./CommentText";
import { UserId } from "../userId";
import IReactionDTO from "../../dto/IReactionDTO";
import {UnixDateTime} from "../unixDateTime";
import ICreatingCommentDTO from "../../dto/ICreatingCommentDTO";


interface CommentProps {
    userId: UserId
    text: CommentText
    reactions: Reaction[]
    creationDateTime:UnixDateTime
}


export class Comment extends Entity<CommentProps>{
    get id(): UniqueEntityID {
        return this._id;
    }

    get userId(): UserId {
        return this.props.userId;
    }

    get commentId(): CommentId {
        return new CommentId(this.commentId.toValue());
    }

    get text(): CommentText {
        return this.props.text;
    }

    get reactions(): Reaction[] {
      return this.props.reactions;
    }

    get creationDateTime(): UnixDateTime{
      return this.props.creationDateTime;
    }

    public getReactionFromUser (_userID: string): Reaction {
      for(let index in this.props.reactions) {
          let reaction = this.props.reactions[index]
          if(reaction.userId.id === _userID) {
              return reaction
          }
      }
      return null;
  }

    private constructor(props: CommentProps, id?: UniqueEntityID) {
        super(props, id);
    }

    public static create(commentDTO: ICreatingCommentDTO, id?: UniqueEntityID): Result<Comment> {
        const userId = UserId.create(commentDTO.userId);
        const reactionsDto = commentDTO.reactions;

        if (userId.isFailure) {
            return Result.fail<Comment>('Must provide a user id')
        }
        const text = CommentText.create(commentDTO.text);

        if (text.isFailure) {
            return Result.fail<Comment>('Must provide a comment with minimum 1 character')
        }

        let reactions: Reaction[];
        reactions = []
      if (commentDTO.reactions !== undefined && commentDTO.reactions !==null) {
          const length = commentDTO.reactions.length
          for(let index = 0; index<length; index++){
            const currentReaction : any = commentDTO.reactions[index]

            let newReactionProps = {
              userId: currentReaction.userId,
              reaction : currentReaction.typeReaction
            } as IReactionDTO

            let newReaction = Reaction.create(newReactionProps)

            if (newReaction.isFailure) {
              return Result.fail<Comment>('Invalid Reactions on Comment')
            }
            reactions.push(newReaction.getValue())
          }
        }
        const creationDateTime = UnixDateTime.create(new Date().getTime());
        const comment = new Comment({
          userId: userId.getValue(),
          text: text.getValue(),
          reactions: reactions,
          creationDateTime: creationDateTime.getValue()
        }, id);
        return Result.ok<Comment>(comment)
    }
}
