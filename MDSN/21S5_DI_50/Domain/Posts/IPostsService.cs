using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DDDSample1.Domain.Posts
{
    public interface IPostsService
    {
        Task<CreatePostResponseDTO?> CreatePost(CreatePostRequestDTO requestDto);

        public Task<List<PostDTO>> FeedPosts(Guid userId);
        Task<CreatingCommentResponseDTO> CreateComment(CreatingCommentDTO commentDTO);
        
        Task<CreatingReactionResponseDTO> updateReactionPost(CreatingReactionDTO reactionDTO);
        Task<CreatingReactionResponseDTO> updateReactionComment(CreatingReactionDTO reactionDTO);
    }
}