import { Service, Inject } from 'typedi';


import { Document, FilterQuery, Model } from 'mongoose';
import { IPostPersistence } from '../dataschema/IPostPersistence';

import ICreatingReactionResponseDTO from "../dto/ICreatingReactionResponseDTO"

import IPostRepo from '../services/IRepos/IPostRepo';
import { Post } from '../domain/post';
import { PostId } from '../domain/postId';
import { PostMap } from '../mappers/PostMap';
import Reaction from '../domain/Reaction/Reaction';
import ReactionMap from '../mappers/ReactionMap'
import { UserId } from '../domain/userId';
import {Comment} from "../domain/Comment/Comment";
import CommentMap from "../mappers/CommentMap";

@Service()
export default class PostRepo implements IPostRepo {
    private models: any;

    constructor(
        @Inject('postSchema') private postSchema: Model<IPostPersistence & Document>,
    ) { }

    private createBaseQuery(): any {
        return {
            where: {},
        }
    }

    public async exists(post: Post): Promise<boolean> {

        const idX = post.id instanceof PostId ? (<PostId>post.id).toValue() : post.id;

        const query = { domainId: idX };
        const postDocument = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>);

        return !!postDocument === true;
    }



    public async findById(id: PostId| string): Promise<Post> {
        const idX = id instanceof PostId ? (<PostId>id).toValue() : id;
        const query = {domainId:idX}
        const postRecord = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>)
        return postRecord !==null ? PostMap.toDomain(postRecord) : null
    }

    public async findAllByUserId(userId: UserId): Promise<Post[]> {
        const userIdX = userId.id
        const query = { userId: userIdX }
        const feedRecord = await this.postSchema.find(query as FilterQuery<IPostPersistence & Document>, null, { sort: { _id: -1 }})
        return feedRecord !== null
            ? feedRecord.map((postRecord) => PostMap.toDomain(postRecord))
            : null
    }

    public async updateReactionToPost(newReaction : Reaction,postId:string): Promise<ICreatingReactionResponseDTO>{
        const query = {domainId:postId}
        const postRecord = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>)
        if(postRecord === null)
            return null;

        const tempPost = PostMap.toDomain(postRecord)
        const existingReaction = tempPost.getReactionFromUser(newReaction.userId.id)
        const reactionToPersistance = ReactionMap.toPersistence(newReaction);

        let incremented: boolean = false;

        if(existingReaction===null){
            await this.postSchema.updateOne(query, {$push : {
                reactions: reactionToPersistance
            }});

            if(newReaction.props.reaction.toString()==="LIKE")
                incremented = true
        }else{
            if(existingReaction.reactionType.toString() === newReaction.reactionType.toString()){
                // remover se ja deu like ou dislike
                await this.postSchema.updateOne(query, { $pull : {
                    reactions: {userId: newReaction.userId.id }
                }});

                if(newReaction.reactionType.toString() == "DISLIKE")
                    incremented = true;
            }else{

                await this.postSchema.updateOne(query, { $pull : {
                    reactions: {userId: newReaction.userId.id }
                }});

                await this.postSchema.updateOne(query, {$push : {
                    reactions: reactionToPersistance
                }});

                if(newReaction.props.reaction.toString()==="LIKE")
                incremented = true
            }
        }

        const updatedRecord = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>)

        const responseDto = {
            publicationId: postId,
            publicationUserId: updatedRecord.userId.toString(),
            userId: newReaction.props.userId.toString(),
            incrementRelation: incremented,
            reaction: newReaction.props.reaction.toString()
        }

        return responseDto;
    }

    public async updateReactionToComment(newReaction : Reaction,commentId:string): Promise<ICreatingReactionResponseDTO>{
        const query = {'comments.domainId':commentId}
        const postRecord = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>)

        if(postRecord === null)
            return null;

        const tempPost = PostMap.toDomain(postRecord)
        const comment: Comment = tempPost.getCommentWithId(commentId);

        if(comment === null)
            return null;

        const existingReaction = comment.getReactionFromUser(newReaction.userId.id)
        const reactionToPersistance = ReactionMap.toPersistence(newReaction);

        let incremented: boolean = false;

        if(existingReaction===null){


            await this.postSchema.updateOne(
                query
                ,
                {
                $push: {
                    'comments.$.reactions': reactionToPersistance }
                }
            );

            if(newReaction.props.reaction.toString()==="LIKE")
                incremented = true

            }else{
            if(existingReaction.reactionType.toString() === newReaction.reactionType.toString()){


                await this.postSchema.updateOne(
                    query
                    ,
                    {
                    $pull: {
                        'comments.$.reactions': {userId: newReaction.userId.id } }
                    }
                );

                if(newReaction.reactionType.toString() == "DISLIKE")
                    incremented = true;


            }else{

                await this.postSchema.updateOne(
                    query
                    ,
                    {
                    $pull: {
                        'comments.$.reactions': {userId: newReaction.userId.id } }
                    }
                );

                await this.postSchema.updateOne(
                    query
                    ,
                    {
                    $push: {
                        'comments.$.reactions': reactionToPersistance }
                    }
                );

                if(newReaction.props.reaction.toString()==="LIKE")
                incremented = true
            }
        }



        const responseDto = {
            publicationId: commentId,
            publicationUserId: comment.userId.toString(),
            userId: newReaction.props.userId.toString(),
            incrementRelation: incremented,
            reaction: newReaction.props.reaction.toString()
        }

        return responseDto;
    }



    public async save(post: Post): Promise<Post> {
        const query = { domainId: post.id.toValue()};

        const postDocument = await this.postSchema.findOne(query);

        try {
            if (postDocument === null) {
                const rawpost: any = PostMap.toPersistence(post);
                const postCreated = await this.postSchema.create(rawpost);

                return PostMap.toDomain(postCreated);
            } else {
                postDocument.userId = post.userId.toString();
                postDocument.text = post.text.toString();
                postDocument.tags= post.tags.map(tag=>tag.toString());
                await postDocument.save();
                return post;
            }
        } catch (err) {
            throw err;
        }
    }

  public async updateCommentToPost(comment: Comment, postId: string): Promise<Comment> {
    const query = {domainId:postId}
    const postRecord = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>)
    if(postRecord === null)
      return null;

    const tempPost = PostMap.toDomain(postRecord)
    //const existingComment = tempPost.getCommentFromUser(comment.userId.id)
    const commentToPersistence = CommentMap.toPersistence(comment);

    //if(existingComment===null){
      await this.postSchema.updateOne(query, {$push : {
          comments: commentToPersistence
        }});

    //}

    const updatedRecord = await this.postSchema.findOne(query as FilterQuery<IPostPersistence & Document>)
/*
    const responseDto = {
        postId: postId,
        userId: comment.props.userId.id.toString(),
        text: comment.props.text.props.value,
        reactions: comment.props.reactions.map(x=> ReactionMap.toDTO(x)),
        creationDateTime:Date.now(),
    }

    return responseDto;
    */
    return comment;
  }
    public async deletePost(postId: string): Promise<Boolean> {
        this.postSchema.deleteOne(
            { domainId: postId }
            , function (err, results) { 
                if(err!==undefined)
                return false;
            })
        return true;
    }

    public async deleteComment(postId: string,commentId:string): Promise<Boolean> {
        this.postSchema.updateOne(
            { domainId: postId },
            { $pull: { 'comments': { domainId: commentId}}}
        ,function(err,results){})
        return true;
    }
}
