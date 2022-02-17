using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using DDDSample1.Controllers;
using Xunit;
using Moq;

namespace Tests.TestesIntegracao.UC11
{
    public class UsersControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private readonly UsersController _controller;
        private readonly UserId oUser;
        private readonly UserId dUser;
        private DDDSample1.Domain.Users.User usr;
        private List<UserDto> dtos;
        private readonly UserDto dto;

        public UsersControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _controller = new UsersController(_service, null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            List<string> tags = new List<string>();
            tags.Add("A");
            usr = new DDDSample1.Domain.Users.User("Abc", "2000-10-10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
            oUser = new UserId(Guid.NewGuid());
            dUser = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(oUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            _repo.Setup(x => x.GetByIdAsync(dUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            dto = new UserDto(Guid.NewGuid(), "2000/10/10", "abc@gmail.com", tags);
            dtos = new List<UserDto>();
            dtos.Add(dto);
        }

        [Fact]
        public async void ReturnNotFoundWhenOUserDoesntExist()
        {
            var invalidUser = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(invalidUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var res = await _controller.getListOfMutualFriends(invalidUser.AsGuid(), dUser.AsGuid());
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void ReturnNotFoundWhenDUserDoesntExist()
        {
            var invalidUser = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(invalidUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var res = await _controller.getListOfMutualFriends(dUser.AsGuid(), invalidUser.AsGuid());
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void ReturnNotFoundWhenMutualFriendsListIsNull()
        {
            _repo.Setup(x => x.GetMutualFriends(oUser,dUser)).Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(null));
            var res = await _controller.getListOfMutualFriends(oUser.AsGuid(), dUser.AsGuid());
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void ReturnOkWhenSuccess()
        {
            IList<DDDSample1.Domain.Users.User> usrs = new List<DDDSample1.Domain.Users.User>();
            usrs.Add(usr);
            _repo.Setup(x => x.GetMutualFriends(oUser, dUser)).Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(usrs));
            var res = await _controller.getListOfMutualFriends(oUser.AsGuid(), dUser.AsGuid());
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async void ReturnExpectedObjectWhenSuccess()
        {
            IList<DDDSample1.Domain.Users.User> usrs = new List<DDDSample1.Domain.Users.User>();
            usrs.Add(usr);
            _repo.Setup(x => x.GetMutualFriends(oUser, dUser)).Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(usrs));
            var res = await _controller.getListOfMutualFriends(oUser.AsGuid(), dUser.AsGuid());
            var obj = res as OkObjectResult;
            Assert.Equal(dtos, obj.Value);
        }
    }
}