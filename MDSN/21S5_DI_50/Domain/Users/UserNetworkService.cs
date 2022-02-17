using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Connections;
using DDDSample1.Infrastructure.Users;
using DDDSample1.Domain.Users;
using System.Net.Http.Json;

namespace DDDSample1.Domain.Users
{
    public class UserNetworkService : IUserNetworkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConnectionRepository _connRepository;
        private readonly IUserRepository _userRepository;

        private readonly IArtificialIntelligenceClient _artificialIntelligenceClient;

        public UserNetworkService(IUnitOfWork unitOfWork, IConnectionRepository connRepository, IUserRepository userRepository,IArtificialIntelligenceClient artificialIntelligenceClient)
        {
            _unitOfWork = unitOfWork;
            _connRepository = connRepository;
            _userRepository = userRepository;
            _artificialIntelligenceClient = artificialIntelligenceClient;
        }

        public async Task<UserNetworkDTO> GetUserNetwork(UserId id, int level)
        {
            var network = await GenerateUserNetwork(id, level, 0, new List<UserId>());
            if (network == null)
            {
                return null;
            }
            return network;
        }

        private async Task<UserNetworkDTO> GenerateUserNetwork(UserId userId, int level, int act, List<UserId> prev)
        {
            var user = await this._userRepository.GetByIdAsync(userId);
            if (user == null) {
                return null;
            }
            prev.Add(userId);
            List<UserNetworkConnDTO> netConns = new List<UserNetworkConnDTO>();
            if (act < level)
            {
                var conns = await this._connRepository.ConnectionsOfUser(userId);
                foreach (var item in conns)
                {
                    if (!prev.Contains(item.DUser))
                    {
                        netConns.Add(new UserNetworkConnDTO(item.ConnectionStrength.Strength, item.RelationshipStrength.Strength,
                        ConvertTagsToString(item.Tags), await GenerateUserNetwork(item.DUser, level, act + 1, new List<UserId>(prev))));
                    }
                }
            }
            return new UserNetworkDTO(act, userId.AsGuid(), user._Email?._Email, user._Name?._Name, ConvertTagsToString(user._InterestTags),
                user._Avatar?._avatarUrl, user._EmotionalState?.Emotion?._Emotion, netConns);
        }

        private List<string> ConvertTagsToString(IEnumerable<Tag> tags)
        {
            List<string> res = new List<string>();
            foreach (var item in tags)
            {
                res.Add(item.Description);
            }
            return res;
        }
        
        public async Task<UserNetworkOperationsDTO> GetUserNetworkSize(UserId id, int level)
        {
            var user = await this._userRepository.GetByIdAsync(id);
            if (user == null) {
                return null;
            }
            var conn = new HashSet<UserId>();
            await GenerateUserNetworkSize(id, level, conn);
            return new UserNetworkOperationsDTO(conn.Count - 1);
        }

        private async Task GenerateUserNetworkSize(UserId userId, int level, ISet<UserId> connectionsSet)
        {
            connectionsSet.Add(userId);
            if (level >= 0)
            {
                var conns = await this._connRepository.ConnectionsOfUser(userId);
                foreach (var item in conns)
                {
                    await GenerateUserNetworkSize(item.DUser, level - 1, connectionsSet);
                }
            }
      
        }
        
        public async Task<UserNetworkOperationsDTO> GetNetworkDimension()
        {
            var lst = await this._userRepository.GetAllAsync();
         
            return new UserNetworkOperationsDTO(lst.Count);
        }

        public async Task<int> GetNetworkConnectionStrength(Guid id)
        {
            var userId = new UserId(id);
            if (await _userRepository.GetByIdAsync(userId) == null)
            {
                return -1;
            }
            return await _userRepository.GetNetworkConnectionStrength(userId);
        }

        public async Task<List<UserDto>> GetCommonFriends(UserId user1, UserId user2)
        {
            //verify that users exist
            var userExists = await _userRepository.GetByIdAsync(user1);
            if (userExists == null)
            {
                return null;
            }
            userExists = await _userRepository.GetByIdAsync(user2);
            if (userExists == null)
            {
                return null;
            }
            
            var commonFriendsIds = await _connRepository.GetCommonFriends(user1, user2);
            if (!commonFriendsIds.Any())
            {
                return new List<UserDto>();
            }
            var commonFriends = await _userRepository.GetByIdsAsync(commonFriendsIds);
            return commonFriends.Select(commonFriend => commonFriend.ToDto()).ToList();
        }

        public async Task<List<String>> getGroupSuggestions(GetGroupSuggestionsDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(new Guid(dto.userId)));
            if(user == null)
                throw new BusinessRuleValidationException("User does not exist");

            var prologRequest = await _artificialIntelligenceClient.getGroupSuggestions(dto);
            
            var statusCode = (int) prologRequest.StatusCode;

            if(!(statusCode == 200 || statusCode ==201)){
                throw new BusinessRuleValidationException("Error on getting group suggestions");
            }

            var groupSuggestions = await prologRequest.Content.ReadFromJsonAsync<GroupSuggestionsDTO>();

            var updateEmotionBody = new UpdateEmotionalStateGroupSugestions(dto.userId,groupSuggestions.grupo,dto.desired,dto.toAvoid);
            
            var newEmotionStateRequest = await _artificialIntelligenceClient.updateUserEmotionGroupSugestions(updateEmotionBody);

            int emotionStatusCode = (int) newEmotionStateRequest.StatusCode;

            if((emotionStatusCode == 200 || emotionStatusCode == 201)){
                
                var emotionBody = await newEmotionStateRequest.Content.ReadFromJsonAsync<ResponseUpdateEmotionalStateGroupSugestions>();

                double newEmotionValue = emotionBody.newEmotionValue;

                user.UpdateCurrentValueEmotionalState(newEmotionValue);

                await this._unitOfWork.CommitAsync();
            }

            return groupSuggestions.grupo;
        }
    }
}