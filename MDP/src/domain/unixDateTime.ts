import { ValueObject } from "../core/domain/ValueObject";
import { Result } from "../core/logic/Result";
import { Guard } from "../core/logic/Guard";

interface UnixDateTimeProps {
    value: number;
}


export class UnixDateTime extends ValueObject<UnixDateTimeProps>{


    get value(): number {
        return this.props.value;
    }

    private constructor(props: UnixDateTimeProps) {
        super(props);
    }

    public static create(dateTime: number): Result<UnixDateTime> {
        let guardResult = Guard.againstNullOrUndefined(dateTime, 'dateTime');

        if (!guardResult.succeeded) {
            return Result.fail<UnixDateTime>(guardResult.message);
        } 
        else {
            return Result.ok<UnixDateTime>(new UnixDateTime({ value: dateTime }))
        }
    }

    toString() {
        return String(this.props.value)
    }
}