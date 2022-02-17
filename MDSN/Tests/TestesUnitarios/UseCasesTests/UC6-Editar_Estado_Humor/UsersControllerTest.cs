using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC6_Editar_Estado_Humor
{
    public class UsersControllerTest
    {
        private readonly Mock<IUserService> _mock;
        private readonly UsersController _controller;

        public UsersControllerTest()
        {
            _mock = new Mock<IUserService>();
            _controller = new UsersController(_mock.Object, null);
        }
        
        [Fact]
        public async void ReturnBadRequestWhenIDsAreNotEqual()
        {
            var dto = new UserUpdateEmotionalStateDTO(new Guid("01234567-0123-4567-0123-456701234567"), "state");
            var otherGuid= new Guid("00000000-0000-0000-0000-000000000000");
            var result = await _controller.UpdateUserEmotionalState(otherGuid, dto);
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async void ReturnBadRequestUponCatchingBusinessRuleValidationException()
        {
            var guid= new Guid("00000000-0000-0000-0000-000000000000");
            var dto = new UserUpdateEmotionalStateDTO(guid, "state");
            _mock.Setup(x => x.UpdateEmotionalState(dto))
                .Throws(new BusinessRuleValidationException("test"));
            var result=await _controller.UpdateUserEmotionalState(guid, dto);
             Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnNotFoundWithNonExistingEmotionalState()
        {
            var guid= new Guid("00000000-0000-0000-0000-000000000000");
            var dto = new UserUpdateEmotionalStateDTO(guid, "state");
            _mock.Setup(x => x.UpdateEmotionalState(dto))
                .Returns(Task.FromResult<UserDto>(null));
            var result=await _controller.UpdateUserEmotionalState(guid, dto);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundWithExistingEmotionalState()
        {
            var guid= new Guid("00000000-0000-0000-0000-000000000000");
            var dto = new UserUpdateEmotionalStateDTO(guid, "state");
            var userDto = new UserDto(new Guid(), "2000-01-01", "1234@gmail.com", new List<string> {"tag1"});
            _mock.Setup(x => x.UpdateEmotionalState(dto))
                .Returns(Task.FromResult(userDto));
            var result=await _controller.UpdateUserEmotionalState(guid, dto);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkFoundOfSpecificExistingEmotionalState()
        {
            var dtoGuid= new Guid("00000000-0000-0000-0000-000000000000");
            var userGuid = new Guid("10000000-1000-1000-1000-100000000000");
            var dto = new UserUpdateEmotionalStateDTO(dtoGuid, "state");
            var userDto = new UserDto(userGuid, "2000-01-01", "1234@gmail.com", new List<string> {"tag1"});
            _mock.Setup(x => x.UpdateEmotionalState(dto))
                .Returns(Task.FromResult(userDto));
            var result=await _controller.UpdateUserEmotionalState(dtoGuid, dto);
            var okResult = result as OkObjectResult;
            var returnedDto = okResult?.Value;
            Assert.Equal(userDto,returnedDto);
        }
    }
}