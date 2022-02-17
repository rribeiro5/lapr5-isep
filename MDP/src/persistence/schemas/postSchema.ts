import { IPostPersistence } from '../../dataschema/IPostPersistence';
import mongoose from 'mongoose';
import { reactionSchema } from './ReactionSchema';
import { commentSchema } from './CommentSchema';

const PostSchema = new mongoose.Schema(
    {
        domainId: { type: String, unique: true },
        userId: { type: String },
        reactions: [reactionSchema],
        comments: [commentSchema],
        text: { type: String },
        tags: { type: [String] },
        creationDateTime: { type: Number },
    },
    {
        timestamps: true
    }
);

export default mongoose.model<IPostPersistence & mongoose.Document>('Post', PostSchema);
