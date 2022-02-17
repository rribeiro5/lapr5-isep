import { Service, Inject } from 'typedi';
import config from "../../config";
import { Result } from "../core/logic/Result";
import { Post } from '../domain/post';
import { UserId } from '../domain/userId'
import IPostDTO from '../dto/IPostDTO';
import { PostMap } from '../mappers/PostMap';
import IPostRepo from './IRepos/IPostRepo';
import IPostService from './IServices/IPostService';
import ICreatingCommentDTO from "../dto/ICreatingCommentDTO";
import ICreatingCommentResponseDTO from "../dto/ICreatingCommentResponseDTO";
import ICommentDTO from "../dto/ICommentDTO";
import {Comment} from "../domain/Comment/Comment";
import CommentMap from '../mappers/CommentMap';



@Service()
export default class PostService implements IPostService {
    constructor(
        @Inject(config.repos.post.name) private postRepo: IPostRepo
    ) { }

    public async createPost(postDTO: IPostDTO): Promise<Result<IPostDTO>> {
        try {

            const postOrError = await Post.create(postDTO);

            if (postOrError.isFailure) {
                return Result.fail<IPostDTO>(postOrError.errorValue());
            }

            const postResult = postOrError.getValue();
            await this.postRepo.save(postResult);

            const postDTOResult = PostMap.toDTO(postResult) as IPostDTO;
            return Result.ok<IPostDTO>(postDTOResult)
        } catch (e) {
            throw e;
        }
    }

    public async getPostById(id: string): Promise<Result<IPostDTO>>{
        try {
            const postResult = await this.postRepo.findById(id);

            if (postResult === null) {
                return Result.fail<IPostDTO>("Can't get posts");
            }
            const post = PostMap.toDTO(postResult) as IPostDTO;
            return Result.ok<IPostDTO>(post);
        } catch (e) {
            throw e;
        }
    }

    public async feedPosts(userId: string): Promise<Result<IPostDTO[]>> {
        try {
            const userIdOrError = UserId.create(userId);

            if (userIdOrError.isFailure) {
                return Result.fail<IPostDTO[]>(userIdOrError.errorValue());
            }

            const userIdRes = userIdOrError.getValue();
            const postsResult = await this.postRepo.findAllByUserId(userIdRes);

            if (postsResult === null) {
                return Result.fail<IPostDTO[]>("Can't get posts");
            }

            const feedPostsResult = postsResult.map((postResult) => PostMap.toDTO(postResult) as IPostDTO);
            return Result.ok<IPostDTO[]>(feedPostsResult);
        } catch(e) {
            throw e;
        }
    }

  public async createCommentPost(commentDTO:ICreatingCommentDTO): Promise<Result<ICreatingCommentResponseDTO>> {

    const commentOrError = await Comment.create(commentDTO as ICreatingCommentDTO )

    if(commentOrError.isFailure){
      return Result.fail<ICreatingCommentResponseDTO>(commentOrError.errorValue())
    }

    const addCommentToPost = await this.postRepo.updateCommentToPost(commentOrError.getValue(),commentDTO.postId)

    if(addCommentToPost===null)
      return Result.fail<ICreatingCommentResponseDTO>("Post not found")

      const commentDTOResult = CommentMap.toDTO(addCommentToPost,commentDTO.postId) as ICreatingCommentResponseDTO;

      return Result.ok<ICreatingCommentResponseDTO>(commentDTOResult)
  }
}
