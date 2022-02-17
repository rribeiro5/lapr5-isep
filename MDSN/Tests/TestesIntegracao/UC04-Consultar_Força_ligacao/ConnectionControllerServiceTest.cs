using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC04_Consultar_Força_ligacao
{
    public class ConnectionControllerServiceTest
    {
        private readonly Mock<IConnectionRepository> _mock;
        private readonly ConnectionsController _controller;

        public ConnectionControllerServiceTest()
        {
            var unitOfWork=new Mock<IUnitOfWork>().Object;
            var userRepository=new Mock<IUserRepository>().Object;
            _mock = new Mock<IConnectionRepository>();
            var service = new ConnectionService(unitOfWork, _mock.Object,userRepository);
            _controller = new ConnectionsController(service);
        }
        [Fact]
        public void ReturnNotFoundForNullConnection()
        {
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult<Connection>(null));
            var result = _controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result;
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ReturnOkForValidConnection()
        {
            var connection = new Connection(1, 2, new List<string>(), new UserId(new Guid()), new UserId(new Guid()));
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult(connection));
            var result = _controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result;
            Assert.IsType<OkObjectResult>(result);
        }
          
          [Fact]
          public void ReturnExpectedValueForValidConnection()
          {
              var connection = new Connection(1, 2, new List<string>(), new UserId(new Guid()), new UserId(new Guid()));
              _mock.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>()))
                  .Returns(Task.FromResult(connection));
              var result = (_controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result as OkObjectResult)
                  .Value as ConnectionStrengthsDTO;
              Assert.Equal(1,result.ConnectionStrength);
              Assert.Equal(2,result.RelationshipStrength);
          }
      
          [Fact]
          public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
          {
              _mock.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>()))
                  .Throws(new BusinessRuleValidationException(""));
              var result = _controller.GetStrengthOfConnection(It.IsAny<Guid>()).Result;
              Assert.IsType<BadRequestObjectResult>(result);
          }
    }
}