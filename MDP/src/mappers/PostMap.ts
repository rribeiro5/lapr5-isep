import { Mapper } from "../core/infra/Mapper";
import { Document, Model } from 'mongoose';
import { UniqueEntityID } from "../core/domain/UniqueEntityID";
import { Post } from "../domain/post";
import IPostDTO from "../dto/IPostDTO";
import { IPostPersistence } from "../dataschema/IPostPersistence";
import ReactionMap from "./ReactionMap";
import CommentMap from "./CommentMap";

export class PostMap extends Mapper<Post> {

    public static toDTO(post: Post): IPostDTO {
        return {
            id: post.id.toValue(),
            userId: post.userId.toString(),
            text: post.text.toString(),
            tags: post.tags.map(tag=>tag.toString()),
            reactions: post.reactions.map(reaction=>ReactionMap.toDTO(reaction)),
            comments: post.comments.map(comment => CommentMap.toDTO(comment, post.id.toValue() as string)),
            creationDateTime:post.creationDateTime.value
        } as IPostDTO;
    }

    public static toDomain(post: any | Model<IPostPersistence & Document>): Post {
        const postOrError = Post.create(
            post,
            new UniqueEntityID(post.domainId)
        );

        postOrError.isFailure ? console.log(postOrError.error) : '';

        return postOrError.isSuccess ? postOrError.getValue() : null;
    }

    public static toPersistence(post: Post): any {
        return {
            domainId: post.id.toValue(),
            userId: post.userId.toString(),
            text: post.text.toString(),
            reactions: post.reactions.map(reaction=>ReactionMap.toPersistence(reaction)),
            comments: post.comments.map(comment=>CommentMap.toPersistence(comment)),
            tags: post.tags.map(tag => tag.toString()),
            creationDateTime: post.creationDateTime.value
        }
    }
}
