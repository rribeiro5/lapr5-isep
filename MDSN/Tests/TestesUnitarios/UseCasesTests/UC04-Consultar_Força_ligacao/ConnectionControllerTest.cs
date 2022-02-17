using System;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC04_Consular_Força_ligacao
{
    public class ConnectionControllerTest
    {
        private readonly Mock<IConnectionService> _mock;
        private readonly ConnectionsController _controller;

        public ConnectionControllerTest()
        {
            _mock = new Mock<IConnectionService>();
            _controller = new ConnectionsController(_mock.Object);
        }

        [Fact]
        public void ReturnNotFoundForNullConnection()
        {
            _mock.Setup(x => x.GetStrengthOfConnection(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult<ConnectionStrengthsDTO>(null));
            var result = _controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result;
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void ReturnOkForValidConnection()
        {
            var dto = new ConnectionStrengthsDTO(It.IsAny<Guid>(), 1, 1);
            _mock.Setup(x => x.GetStrengthOfConnection(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult(dto));
            var result = _controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result;
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedValueForValidConnection()
        {
            var dto = new ConnectionStrengthsDTO(It.IsAny<Guid>(), 1, 2);
            _mock.Setup(x => x.GetStrengthOfConnection(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult(dto));
            var result = (_controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result as OkObjectResult)
                .Value as ConnectionStrengthsDTO;
            Assert.Equal(1,result.ConnectionStrength);
            Assert.Equal(2,result.RelationshipStrength);
        }
        
        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            _mock.Setup(x => x.GetStrengthOfConnection(It.IsAny<ConnectionId>()))
                .Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }

}