using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using DDDSample1.Domain.Users;
using Microsoft.Extensions.Configuration;

namespace DDDSample1.Infrastructure.Users
{
    public class ArtificialIntelligenceClient : IArtificialIntelligenceClient
    {
        private const string Uri = "api/posts";
        private const string UPDATE_EMOTION_LIKES_DISLIKES = "/api/updateEmotionLikesDislikes";
        private const string UPDATE_EMOTION_GROUP_SUGGESTION = "/api/updateEmotionGroupSuggestion";

        private const string GET_GROUP_SUGGESTION = "/api/suggestGroup";
 
        private static readonly HttpClient Client=new();

        public ArtificialIntelligenceClient(IConfiguration conf)
        {   
            string uri = conf.GetConnectionString("ArtificialIntelligenceUri")!=null? conf.GetConnectionString("ArtificialIntelligenceUri"): conf.GetConnectionString("DefaultArtificialIntelligenceUri");

            Client.BaseAddress = new Uri(uri);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async  Task<HttpResponseMessage> updateUserEmotionLikesDislikes(UpdateEmotionalStateLikesDTO emotionalStateDto)
        {   
            // todo meter async depois de public
            return await Client.PostAsJsonAsync(UPDATE_EMOTION_LIKES_DISLIKES,emotionalStateDto);
        }

        public async Task<HttpResponseMessage> updateUserEmotionGroupSugestions(UpdateEmotionalStateGroupSugestions emotionalStateDto)
        {
            // todo meter async depois de public
            return await Client.PostAsJsonAsync(UPDATE_EMOTION_GROUP_SUGGESTION,emotionalStateDto);
        }
        
        public async Task<HttpResponseMessage> getGroupSuggestions(GetGroupSuggestionsDTO emotionalStateDto)
        {
            return await Client.PostAsJsonAsync(GET_GROUP_SUGGESTION,emotionalStateDto);
        }
      

        
    }
}