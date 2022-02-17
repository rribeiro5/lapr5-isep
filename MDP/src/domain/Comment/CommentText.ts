
import { ValueObject } from "../../core/domain/ValueObject";
import { Result } from "../../core/logic/Result";
import { Guard } from "../../core/logic/Guard";

interface CommentTextProps {
    value: string;
}

export class CommentText extends ValueObject<CommentTextProps>{


    get value(): string {
        return this.props.value;
    }

    private constructor(props: CommentTextProps) {
        super(props);
    }

    public static create(text: string): Result<CommentText> {
        let guardResult = Guard.againstEmptyOrNullOrUndefined(text, 'text');

        if (guardResult.succeeded) {

            return Result.ok<CommentText>(new CommentText({ value: text }))
        }else{
            return Result.fail<CommentText>(guardResult.message);
        }
    }

    toString() {
        return String(this.props.value)
    }
}
