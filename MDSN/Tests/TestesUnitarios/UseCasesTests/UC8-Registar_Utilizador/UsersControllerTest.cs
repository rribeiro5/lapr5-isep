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

namespace Tests.TestesUnitarios.UseCasesTests.UC8
{
    public class UsersControllerTest
    {
        private readonly Mock<IUserService> _service;
        private readonly UsersController _controller;
        private CreatingUserDto cdto;
        private UserDto rdto;

        public UsersControllerTest()
        {
            _service = new Mock<IUserService>();
            _controller = new UsersController(_service.Object, null);
            List<string> tags = new List<string>();
            tags.Add("A");
            cdto = new CreatingUserDto("Abc", "2000/10/10", "http://www.gravatar.com/avatar/a16a38cdfe8b2cbd38e8a56ab93238d3",
                "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", tags, "https://www.facebook.com/id123", "https://www.linkedin.com/in/id123");
            rdto = new UserDto(Guid.NewGuid(), "2000/10/10", "abc@gmail.com", tags);
        }

        [Fact]
        public async void ReturnBadRequestWhenDataIsInvalid()
        {
            CreatingUserDto dto = new CreatingUserDto("", "", "", "", "", "", "", "", "", null, "", "");
            _service.Setup(x => x.RegisterUser(dto)).Throws(new BusinessRuleValidationException("test"));
            var res = await _controller.PostUser(dto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnCreatedAtActionWhenUserIsRegisteredSuccessfully()
        {
            _service.Setup(x => x.RegisterUser(cdto)).Returns(Task.FromResult<UserDto>(rdto));
            var res = await _controller.PostUser(cdto);
            Assert.IsType<CreatedAtActionResult>(res);
        }

        [Fact]
        public async void ReturnExpectedObjectWhenUserIsRegisteredSuccessfully()
        {
            _service.Setup(x => x.RegisterUser(cdto)).Returns(Task.FromResult<UserDto>(rdto));
            var res = await _controller.PostUser(cdto);
            var action = res as CreatedAtActionResult;
            Assert.Equal(rdto, action.Value);
        }

        [Fact]
        public async void ReturnBadRequestWhenUserAlreadyExists()
        {
            _service.Setup(x => x.RegisterUser(cdto)).Returns(Task.FromResult<UserDto>(rdto));
            await _controller.PostUser(cdto);
            _service.Setup(x => x.RegisterUser(cdto)).Throws(new DbUpdateException());
            var res = await _controller.PostUser(cdto);
            Assert.IsType<BadRequestObjectResult>(res);
        }
    }
}