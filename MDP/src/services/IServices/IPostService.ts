import { Result } from "../../core/logic/Result";
import IPostDTO from "../../dto/IPostDTO";
import ICreatingCommentResponseDTO from "../../dto/ICreatingCommentResponseDTO";
import ICreatingCommentDTO from "../../dto/ICreatingCommentDTO";

export default interface IPostService {
    createPost(postDTO: IPostDTO): Promise<Result<IPostDTO>>;
    getPostById(id: string): Promise<Result<IPostDTO>>;
    feedPosts(userId: string): Promise<Result<IPostDTO[]>>;
    createCommentPost(commentDto: ICreatingCommentDTO): Promise<Result<ICreatingCommentResponseDTO>>;
}
