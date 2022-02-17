using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC6_Editar_estado_humor
{
    public class UsersControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private readonly UsersController _controller;
        private User user;

        public UsersControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _controller = new UsersController(_service, null);
            
            user = new User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
        }

        [Fact]
        public async void ReturnBadRequestUponCatchingBusinessRuleValidationException()
        {
            var guid= new Guid("00000000-0000-0000-0000-000000000000");
            var dto = new UserUpdateEmotionalStateDTO(guid, "state");
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));

            var result=await _controller.UpdateUserEmotionalState(guid, dto);
             Assert.IsType<BadRequestObjectResult>(result);
        }
    
        [Fact]
        public async void ReturnNotFoundWithNonExistingEmotionalState()
        {
            var guid= new Guid("00000000-0000-0000-0000-000000000000");
            var dto = new UserUpdateEmotionalStateDTO(guid, "state");
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result=await _controller.UpdateUserEmotionalState(guid, dto);
            Assert.IsType<NotFoundResult>(result);
        }
          
        [Fact]
        public async void ReturnOkFoundWithExistingEmotionalState()
        {
           var guid = user.Id.AsGuid();
           var dto = new UserUpdateEmotionalStateDTO(guid, "Grateful");
           _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
           var result=await _controller.UpdateUserEmotionalState(guid, dto);
           Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ReturnOkFoundOfSpecificExistingEmotionalState()
        {
          var dtoGuid= user.Id.AsGuid();
          //new Guid("00000000-0000-0000-0000-000000000000");
          var userGuid = user.Id.AsGuid();
          var dto = new UserUpdateEmotionalStateDTO(dtoGuid, "Grateful");
          var userDto = new UserDto(userGuid, user._BirthDayDate._BirthDayDate,
              user._Email._Email, user._InterestTags.ToList().ConvertAll(input => input.Description));
          _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult(user));
          var result=await _controller.UpdateUserEmotionalState(dtoGuid, dto);
          var okResult = result as OkObjectResult;
          var returnedDto = (UserDto)okResult?.Value;
          Assert.Equal(userDto.Id,returnedDto?.Id);
        }
    }
}