import { reactionSchema } from './ReactionSchema';
const mongoose = require('mongoose');

export const commentSchema = new mongoose.Schema({
  domainId: { type: String },
  userId: { type: String},
  text : { type: String},
  reactions: [reactionSchema],
  creationDateTime: { type: Number },
})
