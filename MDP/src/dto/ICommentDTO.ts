import IReactionDTO from "./IReactionDTO";

export default interface ICommentDTO {
    id: string;
    userId: string;
    text: string;
    reactions: IReactionDTO[];
}
