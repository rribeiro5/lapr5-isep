using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Connections;
using DDDSample1.Utils;
using DDDSample1.Domain.ConnectionRequests;


namespace DDDSample1.Domain.Users
{   
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _repo;

        private readonly IConnectionRepository _connRepo;

        private readonly IRequestRepository _requestRepo;



        public UserService(IUnitOfWork unitOfWork, IUserRepository repo,IConnectionRepository connRepo, IRequestRepository requestRepo)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._connRepo = connRepo;
            this._requestRepo = requestRepo;
        }

        public async Task<List<UserDto>> GetAllAsync(){

            var list = await this._repo.GetAllAsync();
  
            List<UserDto> listDto = list.ConvertAll<UserDto>( user=> new UserDto (user.Id.AsGuid(),user._BirthDayDate._BirthDayDate,
            user._Email._Email,user._Name._Name,user._Avatar?._avatarUrl,user._EmotionalState?.Emotion?._Emotion ,(user._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)),user._EmotionalState.Value._EmotionalValue));

            return listDto;
        }

        public async Task<UserDto> GetByIdAsync(UserId id)
        {
            var user = await this._repo.GetByIdAsync(id);

            if(user == null)
                return null;

            return new UserDto (user.Id.AsGuid(),user._BirthDayDate._BirthDayDate,user._Email._Email,
                user._EmotionalState?.Emotion?._Emotion, 
                 (user._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));
        }

        public async Task<UserSuggestedDto> GetBySuggestionIdAsync(UserId userId)
        {
            var user = await this._repo.GetByIdAsync(userId);
            return user?.toUserSuggestedDto();
        }
        
        public async Task<UserPrivateProfileDto> GetPrivateProfileByIdAsync(UserId id)
        {
            var user = await this._repo.GetByIdAsync(id);

            if(user == null)
                return null;
            var date = user._BirthDayDate?._BirthDayDate.Replace("/","-");
            return new UserPrivateProfileDto(user.Id.AsGuid(),user._Avatar?._avatarUrl,user._Name?._Name,user._Email?._Email,
                user._TelephoneNumber?._Number,date,user._City?._City,user._Country?._Country,
                user._ProfileDescription?._Description,user._Points._Points,user._LinkedInLink?._ProfileUrl,
                user._FacebookLink?._ProfileUrl,(user._InterestTags?.ToList().ConvertAll(tag => tag.Description)),
                user._EmotionalState?.Emotion?._Emotion);
        }

        public async Task<UserPrivateProfileDto> GetPrivateProfileByEmailAsync(Email email)
        {
            var user = await this._repo.GetUserByEmail(email._Email);

            if(user == null)
                return null;

            return new UserPrivateProfileDto(user.Id.AsGuid(),user._Avatar._avatarUrl,user._Name?._Name,user._Email?._Email,
                user._TelephoneNumber?._Number,user._BirthDayDate?._BirthDayDate,user._City?._City,user._Country?._Country,
                user._ProfileDescription?._Description,user._Points._Points,user._LinkedInLink?._ProfileUrl,
                user._FacebookLink?._ProfileUrl,(user._InterestTags?.ToList().ConvertAll(tag => tag.Description)),
                user._EmotionalState?.Emotion?._Emotion);
        }
        
        public async Task<UserPrivateProfileDto> UpdatePrivateProfileAsync(UserPrivateProfileDto dto)
        {
            var user = await _repo.GetByIdAsync(new UserId(dto.Id)); 

            if (user == null)
                return null;   

            // change all fields
            user.Update(dto);
            await _unitOfWork.CommitAsync();

            return new UserPrivateProfileDto(user.Id.AsGuid(),user._Avatar?._avatarUrl,user._Name?._Name,user._Email?._Email,
                user._TelephoneNumber?._Number,user._BirthDayDate?._BirthDayDate,user._City?._City,user._Country?._Country,
                user._ProfileDescription?._Description,user._Points._Points,user._LinkedInLink?._ProfileUrl,
                user._FacebookLink?._ProfileUrl,(user._InterestTags?.ToList().ConvertAll(tag => tag.Description)),
                user._EmotionalState?.ToString());
        }

        public async Task<List<UserDto>> getListOfMutualFriends(UserId origUserId, UserId destUserId){
                var origUser = await _repo.GetByIdAsync(origUserId);
                var destUser = await _repo.GetByIdAsync(destUserId);

                if(origUser == null || destUser == null)
                    return null;

                var mutualFriends = await _repo.GetMutualFriends(origUserId,destUserId);

                if(mutualFriends == null) 
                    return null;

                List<UserDto> listDto = mutualFriends.ToList<User>().ConvertAll<UserDto>( user=> new UserDto (user.Id.AsGuid(),user._BirthDayDate?._BirthDayDate,
                user._Email?._Email,user._Name?._Name,user._Avatar?._avatarUrl,user._EmotionalState?.Emotion?._Emotion ,(user._InterestTags?.ToList<Tag>().ConvertAll<string>(tag => tag.Description)))) ;



                return listDto;  
        }

        public async Task<UserDto> RegisterUser(CreatingUserDto dto)
        {
            var user = new User(dto.Name,dto.BirthDayDate,dto.Avatar,dto.City,dto.Country,dto.Email,dto.Password,dto.ProfileDescription,dto.TelephoneNumber,dto.LinkLinkedin,dto.LinkFacebook,dto.InterestTags);
            
            await this._repo.AddAsync(user);

            await this._unitOfWork.CommitAsync();

            return new UserDto (user.Id.AsGuid(),user._BirthDayDate._BirthDayDate,user._Email._Email, user._Name._Name,user._Avatar._avatarUrl ,user._EmotionalState?.Emotion?._Emotion,(user._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));
        }

         public async Task<UserDto> UpdateAsync(UserDto dto)
        {
            var user = await this._repo.GetByIdAsync(new UserId(dto.Id)); 

            if (user == null)
                return null;   

            // change all field
            // TODO UPDATE THIS
            
            await _unitOfWork.CommitAsync();

            return new UserDto (user.Id.AsGuid(),user._BirthDayDate._BirthDayDate,user._Email._Email, 
                (user._InterestTags.ToList().ConvertAll(tag => tag.Description)));
        }

         public async Task<UserDto> UpdateEmotionalState(UserUpdateEmotionalStateDTO dto)
         {
             var user = await this._repo.GetByIdAsync(new UserId(dto.Id)); 

             if (user == null)
                 return null;
             user.UpdateEmotionalState(dto.State);
             await this._unitOfWork.CommitAsync();
             return new UserDto (user.Id.AsGuid(), user._BirthDayDate?._BirthDayDate, user._Email?._Email, 
                 user._EmotionalState?.Emotion?._Emotion, 
                 user._InterestTags?.ToList<Tag>().ConvertAll<string>(tag => tag.Description));
         }

         public async Task<UserDto> DeleteAsync(UserId id)
        {
            var user = await this._repo.GetByIdAsync(id); 

            if (user == null)
                return null;   
            
            this._repo.Remove(user);

            // Apagar connections de user

            var connectionsUser = await this._connRepo.BidirecionalConnectionsOfUser(id);            
            foreach(Connection conn in connectionsUser){
                 this._connRepo.Remove(conn);
            }

            var requestsUser = await this._requestRepo.getAllRequestsOfUser(id);

            foreach(ConnectionRequest req in requestsUser){
                 this._requestRepo.Remove(req);
            }

            await this._unitOfWork.CommitAsync();

            return new UserDto (user.Id.AsGuid(),user._BirthDayDate._BirthDayDate,user._Email._Email, (user._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));
 
        }

        public async Task<UserDto> LoginUser(LoginDto loginData)
        {
            var user = await this._repo.GetUserByEmail(loginData.Email);

            if (user == null)
                return null;
            
            if (!SecurePasswordHasher.Verify(loginData.Password, user._Password._Password))
                return null;
            
            return new UserDto (user.Id.AsGuid(),user._BirthDayDate._BirthDayDate,user._Email._Email,user._Name._Name,
                user._EmotionalState?.Emotion?._Emotion ,(user._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));
        }
    }
    
}