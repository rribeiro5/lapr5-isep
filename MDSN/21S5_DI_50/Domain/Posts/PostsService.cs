using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using DDDSample1.Infrastructure.Posts;
using DDDSample1.Infrastructure.Users;
using DDDSample1.Domain.Shared;
using System.Net.Http.Json;

namespace DDDSample1.Domain.Posts
{
    public class PostsService : IPostsService
    {   
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMasterDataPostsHttpClient _httpClient;

        private readonly IArtificialIntelligenceClient artificialIntelligenceClient;
        private readonly IUserRepository _userRepo;

        private readonly IConnectionRepository _connectionRepo;

        public PostsService(IUnitOfWork unitOfWork,IMasterDataPostsHttpClient httpClient,IUserRepository userRepo,IConnectionRepository connectionRepo,IArtificialIntelligenceClient _artificialIntelligenceClient)
        {   
            this._unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _userRepo = userRepo;
            _connectionRepo = connectionRepo;
            artificialIntelligenceClient = _artificialIntelligenceClient;
        }
        
        public async Task<CreatePostResponseDTO> CreatePost(CreatePostRequestDTO requestDto)
        {
            var userId = new UserId(requestDto.userId);
            if (await _userRepo.GetByIdAsync(userId) == null)
            {
                return null;
            }
            var response= await _httpClient.CreatePost(requestDto);
            
            if ((int)response.StatusCode != 201) {
                throw new BusinessRuleValidationException(response.ReasonPhrase);
            }
            return await response.Content.ReadFromJsonAsync<CreatePostResponseDTO>();

        }

        public async Task<List<PostDTO>> FeedPosts(Guid userId) 
        {
            var userIdX = new UserId(userId);
            var user = await _userRepo.GetByIdAsync(userIdX);
            if (user == null) 
            {
                return null;
            }
            var response = await _httpClient.FeedPosts(userId);

            var statusCode = (int) response.StatusCode;

            if (statusCode != 200) {
                throw new BusinessRuleValidationException("Error getting feed");
            }

            var body = await response.Content.ReadFromJsonAsync<List<PostDTO>>();

            foreach (var post in body)
            {
                ICollection<CommentDTO> comms = new List<CommentDTO>();
                foreach (var comment in post.comments)
                {
                    try {
                        var usr = await _userRepo.GetByIdAsync(new UserId(comment.userId));
                        if (usr == null) {
                            continue;
                        }
                        comment.user = new UserDto(usr.Id.AsGuid(),usr._BirthDayDate?._BirthDayDate,usr._Email?._Email,usr._Name?._Name);
                        comms.Add(comment);
                    } catch (BusinessRuleValidationException) {
                        continue;
                    }
                }
                post.comments = comms;
            }
           
            return body;
        }

        private async Task<bool>  getNewEmotionValue(string pubId,int incrementValue){
            UpdateEmotionalStateLikesDTO dto = new UpdateEmotionalStateLikesDTO(pubId,incrementValue);
            
            try{
            var newEmotion = await  this.artificialIntelligenceClient.updateUserEmotionLikesDislikes(dto);

             var statusCodeEmotionValue = (int) newEmotion.StatusCode;

             if((statusCodeEmotionValue== 200 || statusCodeEmotionValue ==201)){
                UserUpdateEmotionalStateLikesResponseDTO responseDto = await newEmotion.Content.ReadFromJsonAsync<UserUpdateEmotionalStateLikesResponseDTO>();
                double newEmotionValue = responseDto.newEmotionValue ;
                var publicationUserId = new UserId(new Guid(pubId));
                var publicationUser = await _userRepo.GetByIdAsync(publicationUserId);
                publicationUser.UpdateCurrentValueEmotionalState(newEmotionValue);
                
            }
            }catch(Exception ){
                return false;
            }
            return true;
            
        }

        private async  Task<bool> updateConnectionValue(UserId origUser,UserId destUser,bool incrementRelation){
            Connection connection = await _connectionRepo.obtainConnection(origUser,destUser);

            if(connection!=null){
                
                if(incrementRelation){
                    connection.incrementRelationshipStrength();
                }else{
                    connection.decrementRelationshipStrength();
                }
                return true;
            }
            return  false;
        }

        public async Task<CreatingReactionResponseDTO>  updateReactionPost(CreatingReactionDTO reactionDTO)
        {
            var userId = new UserId(reactionDTO.userId);

            if (await _userRepo.GetByIdAsync(userId) == null)
            {
                throw new BusinessRuleValidationException("User that reacts not found");
            }

            var response = await _httpClient.updateReactionPost(reactionDTO);

            var statusCode = (int) response.StatusCode;

            if(!(statusCode == 200 || statusCode ==201)){
                throw new BusinessRuleValidationException("Error on updating reaction on post");
            }

            var body = await response.Content.ReadFromJsonAsync<CreatingReactionResponseDTO>();
            

            var publicationUserId = new UserId(new Guid(body.publicationUserId));

            if (await _userRepo.GetByIdAsync(publicationUserId) == null)
            {
                throw new BusinessRuleValidationException("User of the post not found");
            }

            int incrementValueEmotion = body.incrementRelation ? 1 : -1;

            var newEmotion = await getNewEmotionValue(body.publicationUserId,incrementValueEmotion);

            var newConnection = await updateConnectionValue(userId,publicationUserId,body.incrementRelation);

            await this._unitOfWork.CommitAsync();
            
            return body;
        }

        


        public async Task<CreatingReactionResponseDTO>  updateReactionComment(CreatingReactionDTO reactionDTO)
        {
            var userId = new UserId(reactionDTO.userId);

            if (await _userRepo.GetByIdAsync(userId) == null)
            {
                throw new BusinessRuleValidationException("User that reacts not found");
            }

            var response = await _httpClient.updateReactionComment(reactionDTO);

            var statusCode = (int) response.StatusCode;

            if(!(statusCode == 200 || statusCode ==201)){
                throw new BusinessRuleValidationException("Error on updating reaction on post");
            }

            var body = await response.Content.ReadFromJsonAsync<CreatingReactionResponseDTO>();
            
            var publicationUserId = new UserId(new Guid(body.publicationUserId));

            if (await _userRepo.GetByIdAsync(publicationUserId) == null)
            {
                throw new BusinessRuleValidationException("User of the post not found");
            }

            int incrementValueEmotion = body.incrementRelation ? 1 : -1;

            var emotion = await getNewEmotionValue(body.publicationUserId,incrementValueEmotion);

            var connectionValue = await updateConnectionValue(userId,publicationUserId,body.incrementRelation);

            await this._unitOfWork.CommitAsync();

            
            return body;
        }

        public async Task<CreatingCommentResponseDTO> CreateComment(CreatingCommentDTO commentDTO)
        {
            var userId = new UserId(commentDTO.userId);

            if (await _userRepo.GetByIdAsync(userId) == null)
            {
                throw new BusinessRuleValidationException("User that commented not found");
            }

            var response = await _httpClient.CreateComment(commentDTO);

            var statusCode = (int) response.StatusCode;

            if(statusCode != 201){
                throw new BusinessRuleValidationException(response.RequestMessage.ToString());
            }
            
            var body = await response.Content.ReadFromJsonAsync<CreatingCommentResponseDTO>();

            return body;
        }
    }
}