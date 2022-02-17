using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC05_Editar_Perfil
{
    public class UsersControllerTest
    {
        
        private readonly Mock<IUserService> _mock;
        private readonly UsersController _controller;
        private UserPrivateProfileDto _dto;
        public UsersControllerTest()
        {
            _mock = new Mock<IUserService>();
            _controller = new UsersController(_mock.Object, null);
            _dto = new UserPrivateProfileDto(Guid.NewGuid(), DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,"Test", "test@gmail.com","+351123456789", 
                "2000-1-1", "Porto", "Portugal", "description", 5,
                "https://www.linkedin.com/in/id123","https://www.facebook.com/id123",
                new List<string>() {"tag1"}, "Distressed");
        }
        
        
        [Fact]
        public async void ReturnBadRequestWhenIDsAreNotEqual()
        {
            _mock.Setup(x => x.UpdatePrivateProfileAsync(_dto))
                .Throws(new BusinessRuleValidationException("test"));
            var otherGuid= new Guid("00000000-0000-0000-0000-000000000000");
            var result = await _controller.UpdatePrivateProfile(otherGuid, _dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnNotFoundWithNonExistingProfile()
        {
            _mock.Setup(x => x.UpdatePrivateProfileAsync(_dto))
                .Returns(Task.FromResult<UserPrivateProfileDto>(null));
            var result=await _controller.UpdatePrivateProfile(_dto.Id, _dto);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundTypeWithExistingProfile()
        {
            var userDto = new UserPrivateProfileDto(new Guid(), DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,"Test", "test@gmail.com","+351123456789", 
                "2000-1-1", 
                "Porto", "Portugal", "description", 5,
                "https://www.facebook.com/id123",
                "https://www.linkedin.com/in/id123",new List<string>() {"tag1"}, "Distressed");
            _mock.Setup(x => x.UpdatePrivateProfileAsync(_dto))
                .Returns(Task.FromResult(userDto));
            var result=await _controller.UpdatePrivateProfile(_dto.Id,_dto);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundOfSpecificExistingEmotionalState()
        {
            var userDto = new UserPrivateProfileDto(new Guid(), DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,"Test", "test@gmail.com","+351123456789", 
                "2000-1-1", 
                "Porto", "Portugal", "description", 5,
                "https://www.facebook.com/id123",
                "https://www.linkedin.com/in/id123",new List<string>() {"tag1"}, "Distressed");
            _mock.Setup(x => x.UpdatePrivateProfileAsync(_dto))
                .Returns(Task.FromResult(userDto));
            var result=await _controller.UpdatePrivateProfile(_dto.Id, _dto);
            var okResult = result as OkObjectResult;
            var returnedDto = okResult?.Value;
            Assert.Equal(userDto,returnedDto);
        }
        
        
        
        [Fact]
        public async void ReturnNotFoundWhenGettingProfileDoesntExist()
        {
            _mock.Setup(x => x.GetPrivateProfileByIdAsync(new UserId(_dto.Id))).
                Returns(Task.FromResult<UserPrivateProfileDto>(null));
            var result=await _controller.GetUserPrivateProfile(_dto.Id);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async void ReturnBadRequestWhenGetPrivateProfileByIdAsyncFails()
        {
            _mock.Setup(x => x.GetPrivateProfileByIdAsync(new UserId(_dto.Id))).
                Throws(new BusinessRuleValidationException("test"));
            var result=await _controller.GetUserPrivateProfile(_dto.Id);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundOfSpecificProfileWhenGetting()
        {
            _mock.Setup(x => x.GetPrivateProfileByIdAsync(new UserId(_dto.Id)))
                .Returns(Task.FromResult(_dto));
            var result=await _controller.GetUserPrivateProfile(_dto.Id);
            var okResult = result as OkObjectResult;
            var returnedDto = okResult?.Value;
            Assert.Equal(_dto,returnedDto);
        }
        
    }
}