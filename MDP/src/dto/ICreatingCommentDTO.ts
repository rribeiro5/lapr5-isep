import IReactionDTO from "./IReactionDTO";

export default interface ICreatingCommentDTO {
  postId: string,
  userId: string;
  text: string;
  reactions: IReactionDTO[];
}

