import { Repo } from "../../core/infra/Repo";
import { Post } from "../../domain/post";
import { UserId } from "../../domain/userId";
import  Reaction  from "../../domain/Reaction/Reaction";
import ICreatingReactionResponseDTO from "../../dto/ICreatingReactionResponseDTO"
import {Comment} from "../../domain/Comment/Comment";
import ICreatingCommentResponseDTO from "../../dto/ICreatingCommentResponseDTO";

export default interface IRoleRepo extends Repo<Post> {
    save(role: Post): Promise<Post>;
    findById(id:string): Promise<Post>;
    findAllByUserId(userId: UserId): Promise<Post[]>;
    updateReactionToPost(reaction : Reaction,postId:string): Promise<ICreatingReactionResponseDTO>;
    updateReactionToComment(reaction : Reaction,commentId:string): Promise<ICreatingReactionResponseDTO>;
    updateCommentToPost(comment : Comment,postId:string): Promise<Comment>;
    
    //findByDomainId(roleId: RoleId | string): Promise<Post>;

    //findByIds (rolesIds: RoleId[]): Promise<Role[]>;
    //saveCollection (roles: Role[]): Promise<Role[]>;
    //removeByRoleIds (roles: RoleId[]): Promise<any>
}
