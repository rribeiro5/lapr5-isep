import { AggregateRoot } from "../core/domain/AggregateRoot";
import { UniqueEntityID } from "../core/domain/UniqueEntityID";

import { Result } from "../core/logic/Result";
import IPostDTO from "../dto/IPostDTO";
import IReactionDTO from "../dto/IReactionDTO";
import { PostId } from "./postId";
import { PostText } from "./postText";
import { Tag } from "./tag";
import { UnixDateTime } from "./unixDateTime";
import { UserId } from "./userId";
import Reaction from "./Reaction/Reaction";
import { Document, Model } from 'mongoose';
import { IPostPersistence } from "../dataschema/IPostPersistence";
import {Comment} from "./Comment/Comment";
import ICommentDTO from "../dto/ICommentDTO";
import ICreatingCommentDTO from "../dto/ICreatingCommentDTO";


interface PostProps {
    userId: UserId
    text: PostText
    tags: Tag[]
    reactions: Reaction[]
    comments: Comment[]
    creationDateTime:UnixDateTime
}


export class Post extends AggregateRoot<PostProps>{
    get id(): UniqueEntityID {
        return this._id;
    }

    get userId(): UserId {
        return this.props.userId;
    }

    // TODO Dar fix nisto porque da loop infinito
    get postId(): PostId {
        return new PostId(this.postId.toValue());
    }

    get text(): PostText {
        return this.props.text;
    }

    get tags(): Tag[] {
        return this.props.tags;
    }

    get reactions(): Reaction[] {
        return this.props.reactions;
    }

    get comments(): Comment[] {
      return this.props.comments;
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

    public getCommentFromUser (_userID: string): Comment {
        for(let index in this.props.comments) {
            let comment = this.props.comments[index]
            if(comment.userId.id === _userID) {
                return comment
            }
        }
        return null;
    }

    public getCommentWithId(_commentId: string) : Comment {
        //let commentId = new UniqueEntityID(_commentId)
        for(let index in this.comments) {
            let comment = this.comments[index]

            if(comment.id.toString() === _commentId) {
                return comment
            }
        }
        return null;
    }


    get creationDateTime(): UnixDateTime{
        return this.props.creationDateTime;
    }

    private constructor(props: PostProps, id?: UniqueEntityID) {
        super(props, id);
    }

    public static create(postDTO: IPostDTO|any, id?: UniqueEntityID): Result<Post> {
        const userId = UserId.create(postDTO.userId);
        const tagsDto = postDTO.tags;

        if (userId.isFailure) {
            return Result.fail<Post>('Must provide a user id')
        }
        const text = PostText.create(postDTO.text);

        if (text.isFailure) {
            return Result.fail<Post>('Must provide a post with minimum 1 character')
        }
        let tags : Tag[];
        if (tagsDto === null || tagsDto===undefined){
            tags=[]
        }else{
            tags= tagsDto.map(value => Tag.create(value).getValue())
        }
        const creationDateTime = (postDTO.creationDateTime === null || postDTO.creationDateTime===undefined)
            ?UnixDateTime.create(new Date().getTime())
            : UnixDateTime.create(postDTO.creationDateTime);

        let _reactions : Reaction[]
        _reactions = []


        if(postDTO.reactions!==undefined){
            const length = postDTO.reactions.length
            for(let index = 0; index<length; index++){
                const currentReaction = postDTO.reactions[index]

                let newReactionProps = {
                    userId: currentReaction.userId,
                    reaction : currentReaction.typeReaction
                } as IReactionDTO
                let newReaction = Reaction.create(newReactionProps)

                if (newReaction.isFailure) {
                    return Result.fail<Post>('Invalid Reactions on Post')
                }

                _reactions.push(newReaction.getValue())
            }
        }

        let _comments : Comment[]
        _comments = []


        if(postDTO.comments!==undefined){
            const length = postDTO.comments.length
            for(let index = 0; index<length; index++){
            const currentComment = postDTO.comments[index]
            let newCommentProps = {
                userId: currentComment.userId,
                text: currentComment.text,
                reactions: currentComment.reactions
            } as ICreatingCommentDTO

            let newComment = Comment.create(newCommentProps,currentComment.domainId)

            if (newComment.isFailure) {
                return Result.fail<Post>('Invalid Comments on Post')
            }

            _comments.push(newComment.getValue())
            }
        }


        const post = new Post({
            userId: userId.getValue(),
            text:text.getValue(),
            tags:tags,
            reactions:_reactions,
            comments: _comments,
            creationDateTime: creationDateTime.getValue()
        }, id);
        return Result.ok<Post>(post)
    }
}
