using System;
using System.Net.Http;
using System.Threading.Tasks;
using DDDSample1.Domain.Posts;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Infrastructure.Posts
{
    public interface IMasterDataPostsHttpClient
    {
        Task<HttpResponseMessage> CreatePost(CreatePostRequestDTO requestDto);

        Task<HttpResponseMessage> FeedPosts(Guid userId);

        Task<HttpResponseMessage> updateReactionPost(CreatingReactionDTO reactionDTO);

         Task<HttpResponseMessage> updateReactionComment(CreatingReactionDTO reactionDTO);
  
        
        Task<HttpResponseMessage> CreateComment(CreatingCommentDTO commentDTO);
    
    }
}