import { ValueObject } from "../core/domain/ValueObject";
import { Guard } from "../core/logic/Guard";
import { Result } from "../core/logic/Result";
import { PostText } from "./postText";


interface TagProps{
    value: string
}

export class Tag extends ValueObject<TagProps>{
    get value(): string {
        return this.props.value;
    }

    private constructor(props: TagProps) {
        super(props);
    }

    public static create(tag: string): Result<Tag> {
        let guardResult = Guard.againstNullOrUndefined(tag, 'tag');

        if (!guardResult.succeeded) {
            return Result.fail<PostText>(guardResult.message);
        } else {
            return Result.ok<PostText>(new Tag({ value: tag }))
        }
    }

    toString(){
        return String(this.props.value)
    }
}