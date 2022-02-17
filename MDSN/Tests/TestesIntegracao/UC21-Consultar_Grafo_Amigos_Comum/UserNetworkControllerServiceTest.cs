using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC21_Consultar_Grafo_Amigos_Comum
{
    public class UserNetworkControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly Mock<IConnectionRepository> _conrepo;
        private readonly UserService _service;
        private readonly UserNetworkService _netservice;
        private readonly UsersController _controller;
        private readonly User _usr1;
        private readonly User _usr2;
        private readonly User _common1;
        private readonly User _common2;
        
        public UserNetworkControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _conrepo = new Mock<IConnectionRepository>();
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _netservice = new UserNetworkService(_unit.Object, _conrepo.Object, _repo.Object,null);
            _controller = new UsersController(_service, _netservice);
            _usr1 = new User("user 1", "2000-01-01", null, null, "user1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _usr2 = new User("user 2", "2000-01-01", null, null, "user2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _common1 = new User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _common2 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});

        }
        
        [Fact]
        public async void ReturnNotFoundWithNonExistingUsers()
        {
            var usr1 = new UserId(Guid.NewGuid());
            var usr2 = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(usr1))
                .Returns(Task.FromResult<User>(null));
            _repo.Setup(x => x.GetByIdAsync(usr2))
                .Returns(Task.FromResult<User>(null));
            
            var result = await _controller.GetCommonFriends(usr1.AsGuid(), usr2.AsGuid());
            Assert.IsType<NotFoundObjectResult>(result);
        }
          
        [Fact]
        public async void ReturnOkFoundWithExistingUsers()
        {
            var usr1 = new UserId(Guid.NewGuid());
            var usr2 = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(usr1))
                .Returns(Task.FromResult(_usr1));
            _repo.Setup(x => x.GetByIdAsync(usr2))
                .Returns(Task.FromResult(_usr2));
            _repo.Setup(x => x.GetByIdsAsync(new List<UserId>{_common1.Id, _common2.Id})).
                Returns(Task.FromResult(new List<User> {_common1, _common2}));
            _conrepo.Setup(x => x.GetCommonFriends(usr1, usr2)).Returns(Task.FromResult
                (new List<UserId> {_common1.Id, _common2.Id})); 
            var result = await _controller.GetCommonFriends(usr1.AsGuid(), usr2.AsGuid());
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ReturnOkFoundOfSpecificExistingUsers()
        {
            var usr1 = new UserId(Guid.NewGuid());
            var usr2 = new UserId(Guid.NewGuid());
            var common = new List<UserId> {_common1.Id, _common2.Id};
            var expected = new List<UserDto> {_common1.ToDto(), _common2.ToDto()};
            _repo.Setup(x => x.GetByIdAsync(usr1))
                .Returns(Task.FromResult(_usr1));
            _repo.Setup(x => x.GetByIdAsync(usr2))
                .Returns(Task.FromResult(_usr2));
            _repo.Setup(x => x.GetByIdsAsync(common)).Returns(Task.FromResult(new List<User> {_common1, _common2}));
            _conrepo.Setup(x => x.GetCommonFriends(usr1, usr2)).Returns(Task.FromResult
                (common)); 
            var result = await _controller.GetCommonFriends(usr1.AsGuid(), usr2.AsGuid());
            var okResult = result as OkObjectResult;
            var resultValue = (List<UserDto>)okResult?.Value;
            Assert.Equal(expected,resultValue);
        }
    }
}