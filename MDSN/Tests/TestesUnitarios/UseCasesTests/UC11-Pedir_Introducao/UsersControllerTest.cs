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

namespace Tests.TestesUnitarios.UseCasesTests.UC11
{
    public class UsersControllerTest
    {
        private readonly Mock<IUserService> _service;
        private readonly UsersController _controller;
        private readonly Guid oUser;
        private readonly Guid dUser;
        private readonly UserDto dto;

        public UsersControllerTest()
        {
            _service = new Mock<IUserService>();
            _controller = new UsersController(_service.Object, null);
            oUser = Guid.NewGuid();
            dUser = Guid.NewGuid();
            List<string> tags = new List<string>();
            tags.Add("A");
            dto = new UserDto(Guid.NewGuid(), "2000/10/10", "abc@gmail.com", tags);
        }

        [Fact]
        public async void ReturnNotFoundWhenOUserDoesntExist()
        {
            Guid invalidId = Guid.NewGuid();
            _service.Setup(x => x.getListOfMutualFriends(new UserId(invalidId), new UserId(dUser)))
                .Returns(Task.FromResult<List<UserDto>>(null));
            var res = await _controller.getListOfMutualFriends(invalidId, dUser);
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void ReturnNotFoundWhenDUserDoesntExist()
        {
            Guid invalidId = Guid.NewGuid();
            _service.Setup(x => x.getListOfMutualFriends(new UserId(oUser), new UserId(invalidId)))
                .Returns(Task.FromResult<List<UserDto>>(null));
            var res = await _controller.getListOfMutualFriends(oUser, invalidId);
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void ReturnNotFoundWhenMutualFriendsListIsNull()
        {
            _service.Setup(x => x.getListOfMutualFriends(new UserId(oUser), new UserId(dUser)))
                .Returns(Task.FromResult<List<UserDto>>(null));
            var res = await _controller.getListOfMutualFriends(oUser, dUser);
            Assert.IsType<NotFoundResult>(res);
        }

        [Fact]
        public async void ReturnOkWhenSuccess()
        {
            List<UserDto> list = new List<UserDto>();
            list.Add(dto);
            _service.Setup(x => x.getListOfMutualFriends(new UserId(oUser), new UserId(dUser)))
                .Returns(Task.FromResult<List<UserDto>>(list));
            var res = await _controller.getListOfMutualFriends(oUser, dUser);
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async void ReturnExpectedObjectWhenSuccess()
        {
            List<UserDto> list = new List<UserDto>();
            list.Add(dto);
            _service.Setup(x => x.getListOfMutualFriends(new UserId(oUser), new UserId(dUser)))
                .Returns(Task.FromResult<List<UserDto>>(list));
            var res = await _controller.getListOfMutualFriends(oUser, dUser);
            var obj = res as OkObjectResult;
            Assert.Equal(list, obj.Value);
        }
    }
}