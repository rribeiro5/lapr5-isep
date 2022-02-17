using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Tests.TestesUnitarios.UseCasesTests.UC04_Consular_Força_ligacao
{
    public class ConnectionServiceTest
    {
        private readonly Mock<IConnectionRepository> _mock;
        private readonly IConnectionService _service;

        public ConnectionServiceTest()
        {
            var unitOfWork=new Mock<IUnitOfWork>().Object;
            var userRepository=new Mock<IUserRepository>().Object;
            _mock = new Mock<IConnectionRepository>();
            _service = new ConnectionService(unitOfWork, _mock.Object,userRepository);
        }

        [Fact]
        public void ReturnNullForNullGet()
        {
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult<Connection>(null));
            var result = _service.GetStrengthOfConnection(It.IsAny<ConnectionId>()).Result;
            Assert.Null(result);
        }
        
        [Fact]
        public void ReturnExpectedValueForValidConnection()
        {
            var connection = new Connection(1, 2, new List<string>(), new UserId(new Guid()), new UserId(new Guid()));
            _mock.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>()))
                .Returns(Task.FromResult(connection));
            var result = _service.GetStrengthOfConnection(It.IsAny<ConnectionId>()).Result;
            Assert.Equal(1,result.ConnectionStrength);
            Assert.Equal(2,result.RelationshipStrength);
        }
    }
}