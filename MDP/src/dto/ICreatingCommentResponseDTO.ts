import IReactionDTO from "./IReactionDTO";

export default interface ICreatingCommentResponseDTO {
  id:string,
  postId: string,
  userId: string;
  text: string;
  reactions: IReactionDTO[];
  creationDateTime:number;
}
