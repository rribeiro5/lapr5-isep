import IReactionDTO from "./IReactionDTO";
import ICommentDTO from "./ICommentDTO";
export default interface IPostDTO {
    id: string;
    userId:string;
    reactions: IReactionDTO[];
    comments: ICommentDTO[];
    text: string;
    tags: string[];
    creationDateTime:number;
}
