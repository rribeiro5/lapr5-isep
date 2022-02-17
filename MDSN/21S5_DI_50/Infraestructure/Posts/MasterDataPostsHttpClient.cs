using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DDDSample1.Domain.Posts;
using Microsoft.Extensions.Configuration;

namespace DDDSample1.Infrastructure.Posts
{
    public class MasterDataPostsHttpClient : IMasterDataPostsHttpClient
    {
        private const string Uri = "api/posts";
        private const string FEED_POSTS_URI = "api/posts/feed/";
        private const string UPDATE_REACTION_POST = "api/reactions/posts";

        private const string UPDATE_REACTION_COMMENT = "api/reactions/comments";
        private static readonly HttpClient Client=new();
        private const string UPDATE_COMMENT= "api/posts/comments";

        public MasterDataPostsHttpClient(IConfiguration conf)
        {
            Client.BaseAddress = new Uri(conf.GetConnectionString("MasterDataPostsUri"));
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<HttpResponseMessage> CreatePost(CreatePostRequestDTO requestDto)
        {
            return await Client.PostAsJsonAsync(Uri,requestDto);
        }

        public async Task<HttpResponseMessage> FeedPosts(Guid userId)
        {
            return await Client.GetAsync(FEED_POSTS_URI + userId.ToString());
        }

         public async Task<HttpResponseMessage> updateReactionPost(CreatingReactionDTO reactionDTO)
         {
             return await Client.PostAsJsonAsync(UPDATE_REACTION_POST,reactionDTO);
         }

         public async Task<HttpResponseMessage> updateReactionComment(CreatingReactionDTO reactionDTO)
         {
             return await Client.PostAsJsonAsync(UPDATE_REACTION_COMMENT,reactionDTO);
         }

         public async Task<HttpResponseMessage> CreateComment(CreatingCommentDTO commentDTO)
         {
             return await Client.PostAsJsonAsync(UPDATE_COMMENT, commentDTO);
         }
    }
}