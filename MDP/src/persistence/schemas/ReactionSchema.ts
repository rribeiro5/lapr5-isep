const mongoose = require('mongoose');

export const reactionSchema = new mongoose.Schema({
    userId: { type: String},
    typeReaction : { type: String},
    createdAt: { 
        type: Date, 
        default: () => Date.now(),
    }
})

