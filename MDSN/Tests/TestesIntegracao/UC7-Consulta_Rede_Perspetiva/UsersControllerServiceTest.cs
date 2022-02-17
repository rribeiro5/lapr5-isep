using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using DDDSample1.Infrastructure.Connections;
using DDDSample1.Domain.Connections;

namespace Tests.TestesIntegracao.UC7
{
    public class UsersControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private readonly UsersController _controller;

        private readonly UserNetworkService _networkService;
        private readonly Mock<IConnectionRepository> _connectionRepository;
        private User user;

        public UsersControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _connectionRepository = new Mock<IConnectionRepository>();
            _networkService = new UserNetworkService(_unit.Object,_connectionRepository.Object,_repo.Object,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _controller = new UsersController(_service, _networkService);
            
            user = new User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
        }
    
        [Fact]
        public async void ReturnNotFoundWithNonExistingUser()
        {
            var guid= new Guid();
            var level = 1;
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));

            var result=await _controller.GetUserNetwork(guid,level);
            Assert.IsType<NotFoundResult>(result.Result);
        }
          
        [Fact]
        public async void ReturnOkFoundWithExistingEmotionalState()
        {   
            var listString = new List<string>{"bemfica"};
            var user1 = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            var user2 = new DDDSample1.Domain.Users.User("Ribeiro","2001/10/11",null,null,"ribeiro@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);

            var listConnections = new List<Connection>();
            listConnections.Add(new Connection(1,1,listString,user1.Id,user2.Id));
            
            var level = 1;
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));

            _repo.Setup(m => m.GetByIdAsync(user1.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(user1));    

            _connectionRepository.Setup(m => m.ConnectionsOfUser(user1.Id)).Returns(Task.FromResult<List<Connection>>(listConnections));


           var result=await _controller.GetUserNetwork(user1.Id.AsGuid(),level);
           Assert.IsType<UserNetworkDTO>(result.Value);
        }

        
    }
}