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

namespace Tests.TestesIntegracao.UC20_Consultar_tamanho_rede_user
{
    public class UserNetworkControllerServiceTest
    {
        private Mock<IUserRepository> _userMock;
        private Mock<IUnitOfWork> _unit;
        private Mock<IConnectionRepository> _connMock;
        private UserNetworkService _service;
        private UserNetworkController _controller;

        public UserNetworkControllerServiceTest()
        {
            _userMock = new Mock<IUserRepository>();
            _unit = new Mock<IUnitOfWork>();
            _connMock = new Mock<IConnectionRepository>();
            _service = new UserNetworkService(_unit.Object,_connMock.Object,_userMock.Object,null);
            _controller = new UserNetworkController(_service);
        }
        
        [Fact]
        public void ReturnNotFoundWhenNetworkIsNull()
        {
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = _controller.GetUserNetworkSize(new Guid(), 1).Result;
            Assert.IsType<NotFoundResult>(result);
        }
      
        [Fact]
        public void ReturnOkResultWhenNetworkIsFound()
        {
            var u1 = new User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u3 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});

            var conn1 = new Connection(1, 1, null,u1.Id, u2.Id);
            var conn2 = new Connection(1, 1, null,u2.Id, u3.Id);
            
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(u1));
            MatchUserWithConnectionList(u1, new List<Connection>{conn1});
            MatchUserWithConnectionList(u2, new List<Connection>{conn2});
            MatchUserWithConnectionList(u3, new List<Connection>());
            var result = _controller.GetUserNetworkSize(u1.Id.AsGuid(), 3).Result;
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedDtoWhenNetworkIsFound()
        {
            var u1 = new User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u3 = new User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});

            var conn1 = new Connection(1, 1, null,u1.Id, u2.Id);
            var conn2 = new Connection(1, 1, null,u2.Id, u3.Id);
            
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(u1));
            MatchUserWithConnectionList(u1, new List<Connection>{conn1});
            MatchUserWithConnectionList(u2, new List<Connection>{conn2});
            MatchUserWithConnectionList(u3, new List<Connection>());;
            var result = (_controller.GetUserNetworkSize(u1.Id.AsGuid(), 3).Result as OkObjectResult)
                .Value as UserNetworkOperationsDTO;
            Assert.Equal(2,result.Value);
        }
        
        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())
            ).Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetUserNetworkSize(new Guid(), 3).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }

        private void MatchUserWithConnectionList(User u, List<Connection> l)
        {
            _connMock.Setup(x => x.ConnectionsOfUser(u.Id))
                .Returns(Task.FromResult(l));
        }
    }
}