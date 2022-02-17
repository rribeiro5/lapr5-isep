import { ValueObject } from "../core/domain/ValueObject";
import { Result } from "../core/logic/Result";
import { Guard } from "../core/logic/Guard";

interface PostTextProps {
    value: string;
}

const MIN_SIZE = 1
const MAX_SIZE=10_000

export class PostText extends ValueObject<PostTextProps>{
       
    
    get value(): string {
        return this.props.value;
    }

    private constructor(props: PostTextProps) {
        super(props);
    }

    public static create(text: string): Result<PostText> {
        let guardResult = Guard.againstNullOrUndefined(text, 'text');
        
        if(guardResult.succeeded){
            const len=text.length;
            guardResult = Guard.inRange(len,MIN_SIZE,MAX_SIZE, 'text length');
        }       
        if (!guardResult.succeeded) {
            return Result.fail<PostText>(guardResult.message);
        } else {
            return Result.ok<PostText>(new PostText({ value: text }))
        }
    }

    toString(){
        return String(this.props.value)
    }
}