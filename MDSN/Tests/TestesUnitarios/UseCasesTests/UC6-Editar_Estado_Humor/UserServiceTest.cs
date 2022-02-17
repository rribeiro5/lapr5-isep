using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC6_Editar_Estado_Humor
{
    public class UserServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;

        public UserServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
        }
        
        [Fact]
        public async void ReturnsNullWhenDoesntFindUser()
        {
            var dto = new UserUpdateEmotionalStateDTO(new Guid(), "state");
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.UpdateEmotionalState(dto);
            Assert.Null(result);
        }

        [Fact]
        public async void SuccessfullyReturnUserDto()
        {
            var user = new DDDSample1.Domain.Users.User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var dto = new UserUpdateEmotionalStateDTO(user.Id.AsGuid(), "grateful");
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            var result = await _service.UpdateEmotionalState(dto);
            Assert.IsType<UserDto>(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnExpectedUserDto()
        {
            var user = new DDDSample1.Domain.Users.User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var guid = user.Id.AsGuid();
            var dto = new UserUpdateEmotionalStateDTO(guid, "grateful");
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
            var result = await _service.UpdateEmotionalState(dto); ;
            Assert.Equal(guid,result.Id);
        }
    }
}