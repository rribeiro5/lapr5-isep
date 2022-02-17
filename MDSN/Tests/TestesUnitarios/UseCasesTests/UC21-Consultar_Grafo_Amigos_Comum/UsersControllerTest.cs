using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC21_Consultar_Grafo_Amigos_Comum
{
    public class UsersControllerTest
    {
        private readonly Mock<IUserService> _mock;
        private readonly Mock<IUserNetworkService> _netmock;
        private readonly UsersController _controller;
        private readonly DDDSample1.Domain.Users.User _usr1;
        private readonly DDDSample1.Domain.Users.User _usr2;
        private readonly DDDSample1.Domain.Users.User _common1;
        private readonly DDDSample1.Domain.Users.User _common2;

        public UsersControllerTest()
        {
            _mock = new Mock<IUserService>();
            _netmock = new Mock<IUserNetworkService>();
            _controller = new UsersController(_mock.Object, _netmock.Object);
            
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
        public async void ReturnNotFoundWhenUsersDontExist()
        {
            var u1 = Guid.NewGuid();
            var u2 = Guid.NewGuid();
            _mock.Setup(x=>x.GetByIdAsync(new UserId(u1)))
                .Returns(Task.FromResult<UserDto>(null));
            _mock.Setup(x=>x.GetByIdAsync(new UserId(u2)))
                .Returns(Task.FromResult<UserDto>(null));
            
            var result = await _controller.GetCommonFriends(u1,u2);
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnBadRequestUponCatchingBusinessRuleValidationException()
        {
            var u1 = new UserId(Guid.NewGuid());
            var u2 = new UserId(Guid.NewGuid());

            _netmock.Setup(x => x.GetCommonFriends(u1,u2))
                .Throws(new BusinessRuleValidationException("test"));
            var result=await _controller.GetCommonFriends(u1.AsGuid(), u2.AsGuid());
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundWithExistingUsers()
        {
            var u1 = _usr1.Id;
            var u2 = _usr2.Id;

            _mock.Setup(x => x.GetByIdAsync(u1)).Returns(Task.FromResult(_usr1.ToDto()));
            _mock.Setup(x => x.GetByIdAsync(u2)).Returns(Task.FromResult(_usr2.ToDto()));
            _netmock.Setup(x => x.GetCommonFriends(u1, u2)).Returns(Task.FromResult
                (new List<UserDto>{_common1.ToDto(), _common2.ToDto()}));
            var result=await _controller.GetCommonFriends(u1.AsGuid(), u2.AsGuid());
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundOfSpecificExistingUsers()
        {
            var u1 = _usr1.Id;
            var u2 = _usr2.Id;
            var expected = new List<UserDto> { _common1.ToDto(), _common2.ToDto()};
            _mock.Setup(x => x.GetByIdAsync(u1)).Returns(Task.FromResult(_usr1.ToDto()));
            _mock.Setup(x => x.GetByIdAsync(u2)).Returns(Task.FromResult(_usr2.ToDto()));
            _netmock.Setup(x => x.GetCommonFriends(u1, u2)).Returns(Task.FromResult
                (new List<UserDto>{_common1.ToDto(), _common2.ToDto()}));
            var result=await _controller.GetCommonFriends(u1.AsGuid(), u2.AsGuid());
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value;
            Assert.Equal(expected,resultValue);
        }
    }
}