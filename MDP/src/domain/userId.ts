
import { Entity } from "../core/domain/Entity";
import { UniqueEntityID } from "../core/domain/UniqueEntityID";
import { ValueObject } from "../core/domain/ValueObject";
import { Guard } from "../core/logic/Guard";
import { Result } from "../core/logic/Result";

interface UserIdProps{
  id: string
}

export class UserId extends ValueObject<UserIdProps> {

  get id (): string {
    return this.props.id;
  }

  private constructor(props: UserIdProps) {
      super(props)
  }
  public static create(id: string): Result<UserId> {
    let guardResult = Guard.againstEmptyOrNullOrUndefined(id, 'id');

    if (!guardResult.succeeded) {
      return Result.fail<UserId>(guardResult.message);
    } else {
      return Result.ok<UserId>(new UserId({ id: id }))
    }
  }

  toString(){
    return String(this.props.id);
  }
}