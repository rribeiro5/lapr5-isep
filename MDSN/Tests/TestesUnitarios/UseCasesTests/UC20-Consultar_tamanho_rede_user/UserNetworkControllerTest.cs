using System;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC20_Consultar_tamanho_rede_user
{
    public class UserNetworkControllerTest
    {
        private Mock<IUserNetworkService> _mock;
        private UserNetworkController _controller;

        public UserNetworkControllerTest()
        {
            _mock = new Mock<IUserNetworkService>();
            _controller = new UserNetworkController(_mock.Object);
        }

        [Fact]
        public void ReturnNotFoundWhenNetworkIsNull()
        {
            _mock.Setup(x => x.GetUserNetworkSize(
                    It.IsAny<UserId>(), It.IsAny<int>())
                ).Returns(Task.FromResult<UserNetworkOperationsDTO>(null));
            var result = _controller.GetUserNetworkSize(new Guid(), 1).Result;
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void ReturnOkResultWhenNetworkIsFound()
        {
            var dto = new UserNetworkOperationsDTO(3);
            _mock.Setup(x => x.GetUserNetworkSize(
                It.IsAny<UserId>(), It.IsAny<int>())
            ).Returns(Task.FromResult(dto));
            var result = _controller.GetUserNetworkSize(new Guid(), 1).Result;
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedDtoWhenNetworkIsFound()
        {
            var dto = new UserNetworkOperationsDTO(3);
            _mock.Setup(x => x.GetUserNetworkSize(
                It.IsAny<UserId>(), It.IsAny<int>())
            ).Returns(Task.FromResult(dto));
            var result = (_controller.GetUserNetworkSize(new Guid(), 1).Result as OkObjectResult)
                .Value as UserNetworkOperationsDTO;
            Assert.Equal(3,result.Value);
        }
        
        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            _mock.Setup(x => x.GetUserNetworkSize(
                It.IsAny<UserId>(), It.IsAny<int>())
            ).Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetUserNetworkSize(new Guid(), 1).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}