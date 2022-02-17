using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC23_Consultar_dimensao_rede
{
    public class UserNetworkControllerServiceTest
    {
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IUnitOfWork> _unit;
        private Mock<IConnectionRepository> _connRepoMock;
        private UserNetworkService _service;
        private UserNetworkController _controller;

        public UserNetworkControllerServiceTest()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _unit = new Mock<IUnitOfWork>();
            _connRepoMock = new Mock<IConnectionRepository>();
            _service = new UserNetworkService(_unit.Object,_connRepoMock.Object,_userRepoMock.Object,null);
            _controller = new UserNetworkController(_service);
        }
      
        [Fact]
        public void ReturnOkResultWhenNetworkDimensionIsObtained()
        {
            var u1 = new User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var l = new List<User> {u1, u2};
            _userRepoMock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(l));
            var result = _controller.GetNetworkDimension().Result;
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
              public void ReturnExpectedDtoWhenNetworkDimensionIsObtained()
              {
                  var expected = 2;
                  var u1 = new User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                      "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
                  var u2 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                      "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
                  var l = new List<User> {u1, u2};
                  _userRepoMock.Setup(x => x.GetAllAsync())
                      .Returns(Task.FromResult(l));
                  var result = (_controller.GetNetworkDimension().Result as OkObjectResult )
                      .Value as UserNetworkOperationsDTO;
                  Assert.Equal(expected,result.Value);
              }
              [Fact]
              public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
              {
                  _userRepoMock.Setup(x => x.GetAllAsync())
                      .Throws(new BusinessRuleValidationException(""));
                  var result = _controller.GetNetworkDimension().Result;
                  Assert.IsType<BadRequestObjectResult>(result);
              }
    }
}