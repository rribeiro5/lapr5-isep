using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC20_Consultar_tamanho_rede_user
{
    public class UserNetworkServiceTest
    {
        private Mock<IUserRepository> _userMock;
        private Mock<IUnitOfWork> _unit;
        private Mock<IConnectionRepository> _connMock;
        private UserNetworkService _service;

        public UserNetworkServiceTest()
        {
            _userMock = new Mock<IUserRepository>();
            _unit = new Mock<IUnitOfWork>();
            _connMock = new Mock<IConnectionRepository>();
            _service = new UserNetworkService(_unit.Object,_connMock.Object,_userMock.Object,null);
        }

        [Fact]
        public void ReturnNullWhenUserIsNotFound()
        {
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = _service.GetUserNetwork(new UserId(new Guid()), 3).Result;
            Assert.Null(result);
        }
        
        [Fact]
        public void ReturnExpectedDtoWhenUserIsFound()
        {
            var u1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u3 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});

            var conn1 = new Connection(1, 1, null,u1.Id, u2.Id);
            var conn2 = new Connection(1, 1, null,u2.Id, u3.Id);
            
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(u1));
            MatchUserWithConnectionList(u1, new List<Connection>{conn1});
            MatchUserWithConnectionList(u2, new List<Connection>{conn2});
            MatchUserWithConnectionList(u3, new List<Connection>());
            var result = _service.GetUserNetworkSize(u1.Id, 3).Result;
            Assert.Equal(2,result.Value);
        }
        
        [Fact]
        public void ReturnExpectedDtoWhenUserHasCommonConnectionsWithConnections()
        {
            var u1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u3 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            
            
            var conn1 = new Connection(1, 1, null,u1.Id, u2.Id);
            var conn2 = new Connection(1, 1, null,u2.Id, u3.Id);
            var conn3 = new Connection(1, 1, null,u3.Id, u1.Id);
            
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(u1));
            MatchUserWithConnectionList(u1, new List<Connection>{conn1});
            MatchUserWithConnectionList(u2, new List<Connection>{conn2});
            MatchUserWithConnectionList(u3, new List<Connection>{conn3});
            var result = _service.GetUserNetworkSize(u1.Id, 3).Result;
            Assert.Equal(2,result.Value);
        }
        
        [Fact]
        public void ReturnZeroWhenUserHasNoConnections()
        {
            var u1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u3 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            
            var conn1 = new Connection(1, 1, null,u2.Id, u3.Id);
            
            _userMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(u1));
            MatchUserWithConnectionList(u1, new List<Connection>());
            MatchUserWithConnectionList(u2, new List<Connection>{conn1});
            var result = _service.GetUserNetworkSize(u1.Id, 3).Result;
            Assert.Equal(0,result.Value);
        }

        private void MatchUserWithConnectionList(DDDSample1.Domain.Users.User u, List<Connection> l)
        {
            _connMock.Setup(x => x.ConnectionsOfUser(u.Id))
                .Returns(Task.FromResult(l));
        }
    }
}