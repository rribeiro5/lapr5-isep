
import { UniqueEntityID } from "../../core/domain/UniqueEntityID";
import { Entity } from "../../core/domain/Entity";

import { Result } from "../../core/logic/Result";
import { ReactionId } from "./ReactionId";

import { UserId } from "./../userId";
import ReactionType from "./ReactionType";
import IReactionDTO from "../../dto/IReactionDTO";

interface ReactionProps {
    userId: UserId,
    reaction: ReactionType,
    _creationTime: Date
}

export default class Reaction extends Entity<ReactionProps>{

    public readonly _creationTime : Date

    get id ():UniqueEntityID {
        return this._id
    }

    get reactionId(): ReactionId{
        return new ReactionId(this._id.toValue())
    }

    get reactionType() : ReactionType{
        return this.props.reaction
    }

    get userId(): UserId {
        return this.props.userId
    }

    get creationTime(): Date {
        return this.props._creationTime
    }



    private constructor (props: ReactionProps, id?: UniqueEntityID) {
        super(props, id);
    }

    public static create(reactionDTO:IReactionDTO,id?: UniqueEntityID):Result<Reaction>{
        const userID = UserId.create(reactionDTO.userId);
        
        if(userID.isFailure)
            return Result.fail<Reaction>('Invalid User Identifier')
        
        const reactionType : Result<ReactionType> = ReactionType.create(reactionDTO.reaction);

        const creationTime = new Date()

        if(reactionType.isFailure){
            return Result.fail<Reaction>(reactionType.error)
        }

        const props : ReactionProps = {
            userId: userID.getValue(),
            reaction : reactionType.getValue(),
            _creationTime : creationTime
        }

        const reaction = new Reaction(props)

        return Result.ok<Reaction>(reaction)
    }

}