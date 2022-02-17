using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC05_Editar_Perfil
{
    public class UsersControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private readonly UsersController _controller;
        private User _user;
        private UserPrivateProfileDto _dto;

        public UsersControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _controller = new UsersController(_service, null);
            
            _user = new User("name", "2000-01-01", "Porto", "Portugal", "1234@gmail.com",
                "Password1?", "", "+3511234", "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string> {"tag1"});
            
            _dto= new UserPrivateProfileDto(_user.Id.AsGuid(),User.DEFAULT_AVATAR_URL, "Test","test@gmail.com","+351123456789", 
                "2000-1-1", 
                "Porto", "Portugal", "description", 5,
                "https://www.linkedin.com/in/id123","https://www.facebook.com/id123",
                new List<string>() {"tag1"}, "Distressed");
        }

        [Fact]
        public async void ReturnBadRequestUponCatchingBusinessRuleValidationException()
        {
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Throws(new BusinessRuleValidationException("test"));
            var result=await _controller.UpdatePrivateProfile(_dto.Id, _dto);
             Assert.IsType<BadRequestObjectResult>(result);
        }
    
        [Fact]
        public async void ReturnNotFoundWithNonExistingProfile()
        {
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<User>(null));
            var result=await _controller.UpdatePrivateProfile(_dto.Id, _dto);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void ReturnOkFoundWithExistingProfile()
        {
            _repo.Setup(x => x.GetByIdAsync(_user.Id)).Returns(Task.FromResult(_user));
            var result = await _controller.UpdatePrivateProfile(_dto.Id, _dto);
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async void ReturnOkFoundOfSpecificExistingProfile()
        {
          var dtoGuid= _user.Id.AsGuid();
          var userGuid = _user.Id.AsGuid();
          var userDto = new UserPrivateProfileDto(userGuid, DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,"Test", "test@gmail.com","+351123456789", 
              "2000-1-1", 
              "Porto", "Portugal", "description", 5,
              "https://www.linkedin.com/in/id123","https://www.facebook.com/id123",new List<string>() {"tag1"}, "Distressed");
          _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult(_user));
          var result=await _controller.UpdatePrivateProfile(dtoGuid, _dto);
          var okResult = result as OkObjectResult;
          var returnedDto = (UserPrivateProfileDto)okResult?.Value;
          Assert.Equal(userDto.Id,returnedDto?.Id);
        }
    }
}