import { ValueObject } from "../../core/domain/ValueObject";
import { Result } from "../../core/logic/Result";
import { Guard } from "../../core/logic/Guard";


enum TypesReaction {
    Like="LIKE",
    Dislike="DISLIKE"
}

const TypesReactionList: {
    key: string;
    value: string;
  }[] = Object.entries(TypesReaction)
    .map(([key, value]) => ({ key, value }));


interface ReactionTypeProps {
    value: string;
}

export default class ReactionType extends ValueObject<ReactionTypeProps>{

    
    get value (): string {
        return this.props.value;
    }

    toString(){
        return String(this.props.value)
    }

    private constructor (props:ReactionTypeProps){
        super(props)
    }

    public static create(reaction: string) :Result<ReactionType> {
        let verifyNotNull = Guard.againstNullOrUndefined(reaction,"type of reaction")

        if(!verifyNotNull.succeeded)
            return Result.fail<ReactionType>("Type of Reaction cannot be null")

        let verifyValidReaction = Guard.isOneOf(reaction,TypesReactionList,"Type of reaction")
        
        if(!verifyValidReaction.succeeded)
            return Result.fail<ReactionType>("Invalid Type of Reaction")

        let newProps = { value: reaction }

        let reactionType = new ReactionType(newProps)
        return Result.ok<ReactionType>(reactionType)     

    }


}    
