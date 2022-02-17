using System;
using System.Net.Http;
using System.Threading.Tasks;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace DDDSample1.Infrastructure.Users
{
    public interface IArtificialIntelligenceClient
    {
        Task<HttpResponseMessage> updateUserEmotionLikesDislikes(UpdateEmotionalStateLikesDTO emotionalStateDto);

        Task<HttpResponseMessage> updateUserEmotionGroupSugestions(UpdateEmotionalStateGroupSugestions emotionalStateDto);

        Task<HttpResponseMessage> getGroupSuggestions(GetGroupSuggestionsDTO emotionalStateDto);
        
    
    }
}