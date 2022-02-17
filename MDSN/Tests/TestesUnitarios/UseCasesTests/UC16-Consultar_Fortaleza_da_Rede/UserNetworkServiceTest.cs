using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC16_Consultar_Fortaleza_da_Rede
{
    public class UserNetworkServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly Mock<IConnectionRepository> _connRepository;
        private readonly UserNetworkService _service;
        private DDDSample1.Domain.Users.User user1;
        private DDDSample1.Domain.Users.User user2;
        private DDDSample1.Domain.Users.User user3;

        private List<Connection> listConnections; 
        
        public UserNetworkServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _connRepository = new Mock<IConnectionRepository>();

            _service = new UserNetworkService(_unit.Object,_connRepository.Object ,_repo.Object,null);
            var listString = new List<string>();
            listString.Add("Benfica");
            this.user1 = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            this.user2 = new DDDSample1.Domain.Users.User("Ribeiro","2001/10/11",null,null,"ribeiro@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            this.user2 = new DDDSample1.Domain.Users.User("Moreira","1997/01/02",null,"Portugal","rfp@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
        }
        
        [Fact]
        public async void ReturnZeroWhenUserHasNoConnections()
        {   
            _repo.Setup(m => m.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(user1));
            var result = await _service.GetNetworkConnectionStrength(user1.Id.AsGuid());
            Assert.Equal(0, result);
        }
        
        [Fact]
        public async void ReturnStrengthWhenUserHasConnections()
        {
            _repo.Setup(m => m.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(user1));
            _repo.Setup(m => m.GetNetworkConnectionStrength(user1.Id)).Returns(Task.FromResult(5));
            var result = await _service.GetNetworkConnectionStrength(user1.Id.AsGuid());
            Assert.Equal(5, result);
        }
        
        
        [Fact]
        public async void ReturnNotFoundWhenUserDoesntExist()
        {
            _repo.Setup(m => m.GetByIdAsync(new UserId(Guid.Empty))).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            _repo.Setup(m => m.GetNetworkConnectionStrength(new UserId(Guid.Empty))).Returns(Task.FromResult<int>(-1));
            var result = await _service.GetNetworkConnectionStrength(Guid.Empty);
            Assert.Equal(-1, result);
        }
    }
}