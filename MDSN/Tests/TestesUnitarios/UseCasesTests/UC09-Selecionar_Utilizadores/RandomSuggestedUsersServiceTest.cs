using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC09_Selecionar_Utilizadores
{
    public class RandomSuggestedUsersServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly RandomSuggestedUsersService _service;
        private List<DDDSample1.Domain.Users.User> _nonEmptyList;
        private List<DDDSample1.Domain.Users.User> _emptyList;

        public RandomSuggestedUsersServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new RandomSuggestedUsersService(_repo.Object);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _emptyList = new List<DDDSample1.Domain.Users.User>();
            _nonEmptyList = new List<DDDSample1.Domain.Users.User>()
            {
                new("Test", "2000-1-1",
                    "Porto", "Portugal", "test@gmail.com", "Password1?", "description",
                    "+351123456789", "https://www.linkedin.com/in/id123",
                    "https://www.facebook.com/id123", new List<string>() {"tag1"}),
                new("Test1", "2000-1-1",
                    "Lisbon", "Portugal", "test1@gmail.com", "Password2?", "description",
                    "+35112345678", "https://www.linkedin.com/in/id234",
                    "https://www.facebook.com/id234", new List<string>() {"tag2"})
            };
        }
        
        [Fact]
        public async void ReturnsEmptyListWhenDoesntFindUsers()
        {
            var dto = new UserSuggestedDto();
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_emptyList));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 4);
            Assert.Empty(result);
        }
        
        [Fact]
        public async void ReturnsNullWhenDoesntFindUsers()
        {
            var dto = new UserSuggestedDto();
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 4);
            Assert.Null(result);
        }
        
        [Fact]
        public async void ReturnsNullWhenCantFindNonFriendsWithoutRequests()
        {
            var dto = new UserSuggestedDto();
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(null));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 4);
            Assert.Null(result);
        }

        
        [Fact]
        public async void SuccessfullyReturnListUsers()
        {
            var dto = new UserSuggestedDto();
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>())).
                Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_nonEmptyList));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 2);
            Assert.IsType<List<UserSuggestedDto>>(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnListUserDtoOfCount()
        {
            var dto = new UserSuggestedDto();
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_nonEmptyList));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 2);
            var tmp = new List<UserSuggestedDto>();
            var tags = new List<string>();
            foreach (var user in _nonEmptyList)
            {
                tags.AddRange(user._InterestTags.Select(tag => tag.Description));
                tmp.Add(new UserSuggestedDto(){id = user.Id.AsGuid(),
                    birthdayDate = user._BirthDayDate._BirthDayDate,city = user._City._City,country = user._Country._Country,
                    profileDescription = user._ProfileDescription._Description,linkedInURL = user._LinkedInLink._ProfileUrl,
                    facebookURL = user._FacebookLink._ProfileUrl,interestTagsDtoList = tags});
            }
            Assert.Equal(tmp.Count, result.Count);
        }

        [Fact]
        public async void SuccessfullyReturnExpectedListUserDto()
        {
            var dto = new UserSuggestedDto();
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_nonEmptyList));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 2);
            var tmp = new List<UserSuggestedDto>();
            var tags = new List<string>();
            foreach (var user in _nonEmptyList)
            {
                tags.AddRange(user._InterestTags.Select(tag => tag.Description));
                tmp.Add(new UserSuggestedDto(){id = user.Id.AsGuid(),
                    birthdayDate = user._BirthDayDate._BirthDayDate,city = user._City._City,country = user._Country._Country,
                    profileDescription = user._ProfileDescription._Description,linkedInURL = user._LinkedInLink._ProfileUrl,
                    facebookURL = user._FacebookLink._ProfileUrl,interestTagsDtoList = tags});
            }

            tmp.Sort((suggestedDto, other) => suggestedDto.id.CompareTo(other.id));
            result.Sort((suggestedDto, other) => suggestedDto.id.CompareTo(other.id));
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i].id, tmp[i].id);
            }
        }
    }
}