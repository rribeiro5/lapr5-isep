import apiMDRS from "./apiMDRS"

export const createPost =(post)=>apiMDRS.post('/api/posts',post)

export const feedPosts = (userId) => apiMDRS.get(`/api/posts/feed/${userId}`)

export const updateReactionPost = (reactionDto) => apiMDRS.post("/api/posts/reactions",reactionDto)

export const updateReactionComment = (reactionDto) => apiMDRS.post("/api/posts/comments/reactions",reactionDto)

export const createComment = (comment) => apiMDRS.post('/api/posts/comments',comment)