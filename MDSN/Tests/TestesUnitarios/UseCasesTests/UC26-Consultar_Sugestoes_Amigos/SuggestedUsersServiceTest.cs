using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC26_Consultar_Sugestoes_Amigos
{
    public class SuggestedUsersServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly RandomSuggestedUsersService _service;
        private List<DDDSample1.Domain.Users.User> _nonEmptyList;
        private List<DDDSample1.Domain.Users.User> _emptyList;
        private readonly DDDSample1.Domain.Users.User _usr;
        private readonly DDDSample1.Domain.Users.User _sug1;
        private readonly DDDSample1.Domain.Users.User _sug2;

        public SuggestedUsersServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _userRepo = new Mock<IUserRepository>();
            _service = new RandomSuggestedUsersService(_userRepo.Object);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));

            _usr = new DDDSample1.Domain.Users.User("user 1", "2000-01-01", null, null, "user1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _sug1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _sug2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _emptyList = new List<DDDSample1.Domain.Users.User>();
            _nonEmptyList = new List<DDDSample1.Domain.Users.User>() {_sug1, _sug2};
        }
        
        [Fact]
        public async void ReturnsEmptyListWhenDoesntFindUsers()
        {
            _userRepo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_emptyList));
            var result = await _service.GetSuggestedUsers(_usr.Id, 4);
            Assert.Empty(result);
        }
        
        [Fact]
        public async void ReturnsNullWhenDoesntFindUsers()
        {
            var dto = new UserSuggestedDto();
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.GetSuggestedUsers(new UserId(dto.id), 4);
            Assert.Null(result);
        }
        
        [Fact]
        public async void ReturnsNullWhenCantFindNonFriendsWithoutRequests()
        {
            _userRepo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(null));
            var result = await _service.GetSuggestedUsers(_usr.Id, 4);
            Assert.Null(result);
        }

        
        [Fact]
        public async void SuccessfullyReturnListUsers()
        {
            _userRepo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>())).
                Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_nonEmptyList));
            var result = await _service.GetSuggestedUsers(_usr.Id, 2);
            Assert.IsType<List<UserSuggestedDto>>(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnListUserDtoOfCount()
        {
            
            _userRepo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_nonEmptyList));
            var result = await _service.GetSuggestedUsers(_usr.Id, 2);
            
            Assert.Equal(_nonEmptyList.Count, result.Count);
        }

        [Fact]
        public async void SuccessfullyReturnExpectedListUserDto()
        {

            _userRepo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(_nonEmptyList));
            var result = await _service.GetSuggestedUsers(_usr.Id, 2);


            
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Contains(_nonEmptyList, u =>u.Id.AsGuid().Equals(result[i].id));
     
            }
        }
    }
}