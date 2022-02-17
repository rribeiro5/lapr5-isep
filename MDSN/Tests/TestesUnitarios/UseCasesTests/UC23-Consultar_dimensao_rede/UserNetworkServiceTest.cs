using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC23_Consultar_dimensao_rede
{
    public class UserNetworkServiceTest
    {
        private Mock<IUserRepository> _mock;
        private Mock<IUnitOfWork> _unit;
        private Mock<IConnectionRepository> _connRepo;
        private UserNetworkService _service;

        public UserNetworkServiceTest()
        {
            _mock = new Mock<IUserRepository>();
            _unit = new Mock<IUnitOfWork>();
            _connRepo = new Mock<IConnectionRepository>();
            _service = new UserNetworkService(_unit.Object,_connRepo.Object,_mock.Object,null);
        }

        [Fact]
        public void ReturnCorrectNetworkDimension()
        {
            var u1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var l = new List<DDDSample1.Domain.Users.User> {u1, u2};
            _mock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(l));
            var result = _service.GetNetworkDimension().Result;
            Assert.Equal(2,result.Value);
        }
    }
}