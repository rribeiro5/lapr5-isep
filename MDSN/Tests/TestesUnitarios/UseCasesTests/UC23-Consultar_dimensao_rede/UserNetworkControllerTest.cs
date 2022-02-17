using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC23_Consultar_dimensao_rede
{
    public class UserNetworkControllerTest
    {
        private readonly Mock<IUserNetworkService> _mock;
        private readonly UserNetworkController _controller;
        
        public UserNetworkControllerTest()
        {
            _mock = new Mock<IUserNetworkService>();
            _controller = new UserNetworkController(_mock.Object);
        }

        [Fact]
        public void ReturnNotFoundWhenNetworkIsNull()
        {
            _mock.Setup(x => x.GetNetworkDimension())
                .Returns(Task.FromResult<UserNetworkOperationsDTO>(null));
            var result = _controller.GetNetworkDimension().Result;
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void ReturnOkResultWhenNetworkDimensionIsObtained()
        {
            var dto = new UserNetworkOperationsDTO(0);
            _mock.Setup(x => x.GetNetworkDimension())
                .Returns(Task.FromResult(dto));
            var result = _controller.GetNetworkDimension().Result;
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedDtoWhenNetworkDimensionIsObtained()
        {
            var expected = 3;
            var dto = new UserNetworkOperationsDTO(3);
            _mock.Setup(x => x.GetNetworkDimension())
                .Returns(Task.FromResult(dto));
            var response = _controller.GetNetworkDimension().Result as OkObjectResult;
            var result = response.Value as UserNetworkOperationsDTO;
            Assert.Equal(expected,result.Value);
        }

        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            _mock.Setup(x => x.GetNetworkDimension())
                .Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetNetworkDimension().Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}