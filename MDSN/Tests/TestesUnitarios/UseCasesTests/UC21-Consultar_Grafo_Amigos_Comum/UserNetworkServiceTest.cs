using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC21_Consultar_Grafo_Amigos_Comum
{
    public class UserNetworkServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IConnectionRepository> _connRepo;
        private readonly UserNetworkService _service;
        private readonly DDDSample1.Domain.Users.User _usr1;
        private readonly DDDSample1.Domain.Users.User _usr2;
        private readonly DDDSample1.Domain.Users.User _common1;
        private readonly DDDSample1.Domain.Users.User _common2;

        public UserNetworkServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _userRepo = new Mock<IUserRepository>();
            _connRepo = new Mock<IConnectionRepository>();
            _service = new UserNetworkService(_unit.Object, _connRepo.Object,_userRepo.Object,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _usr1 = new DDDSample1.Domain.Users.User("user 1", "2000-01-01", null, null, "user1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _usr2 = new DDDSample1.Domain.Users.User("user 2", "2000-01-01", null, null, "user2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _common1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _common2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
        }
        
        [Fact]
        public async void ReturnsNullWhenDoesntFindUsersGetCommonFriends()
        {
            var usr1 = new UserId(Guid.NewGuid());
            var usr2 = new UserId(Guid.NewGuid());
            _userRepo.Setup(x => x.GetByIdAsync(usr1))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            _userRepo.Setup(x => x.GetByIdAsync(usr2))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.GetCommonFriends(usr1,usr2);
            Assert.Null(result);
        }

        [Fact]
        public async void SuccessfullyReturnsExpectedListGetCommonFriends()
        {
            var uId1 = new UserId(_usr1.Id.AsGuid());
            var uId2 = new UserId(_usr2.Id.AsGuid());

            var cId1 = new UserId(_common1.Id.AsGuid());
            var cId2 = new UserId(_common2.Id.AsGuid());

            var expected = new List<UserDto>
            {
                _common1.ToDto(),
                _common2.ToDto()
            };
            var repoExpected = new List<UserId>
            {
                cId1,
                cId2
            };

            _userRepo.Setup(x => x.GetByIdAsync(uId1))
                .Returns(Task.FromResult(_usr1));
            _userRepo.Setup(x => x.GetByIdAsync(uId2))
                .Returns(Task.FromResult(_usr2));
            _connRepo.Setup(x => x.GetCommonFriends(uId1, uId2)).Returns(Task.FromResult(repoExpected));
            _userRepo.Setup(x => x.GetByIdsAsync(repoExpected))
                .Returns(Task.FromResult(new List<DDDSample1.Domain.Users.User>{_common1, _common2}));

        var result = await _service.GetCommonFriends(uId1, uId2);
            Assert.Equal(expected, result);
        }
        
        [Fact]
        public async void SuccessfullyReturnsListGetCommonFriends()
        {
            var uId1 = new UserId(_usr1.Id.AsGuid());
            var uId2 = new UserId(_usr2.Id.AsGuid());

            var cId1 = new UserId(_common1.Id.AsGuid());
            var cId2 = new UserId(_common2.Id.AsGuid());
            
            var repoExpected = new List<UserId>
            {
                cId1,
                cId2
            };

            _userRepo.Setup(x => x.GetByIdAsync(uId1))
                .Returns(Task.FromResult(_usr1));
            _userRepo.Setup(x => x.GetByIdAsync(uId2))
                .Returns(Task.FromResult(_usr2));
            _connRepo.Setup(x => x.GetCommonFriends(uId1, uId2)).Returns(Task.FromResult(repoExpected));
            _userRepo.Setup(x => x.GetByIdsAsync(repoExpected))
                .Returns(Task.FromResult(new List<DDDSample1.Domain.Users.User>{_common1, _common2}));

            var result = await _service.GetCommonFriends(uId1, uId2);
            Assert.IsType<List<UserDto>>(result);
        }
        
    }
}