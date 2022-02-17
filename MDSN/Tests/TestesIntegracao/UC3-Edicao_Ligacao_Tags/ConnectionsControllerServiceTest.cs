using System;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Connections;
using System.Collections.Generic;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Tests.TestesIntegracao.UC3
{
    public class ConnectionsControllerServiceTest
    {
        private readonly IConnectionService _service;
        private readonly ConnectionsController _controller;

        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IConnectionRepository> _repo;

        private readonly Mock<IUserRepository> _userRepo;


        private ChangeConnInfoDTO _dto;

        private ConnectionDTO _connectionDto;

        private User user;

        private List<Connection> listConnections;
        
        public ConnectionsControllerServiceTest()
        {   
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IConnectionRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionService(_unit.Object,_repo.Object,_userRepo.Object);
            _controller = new ConnectionsController(_service);
            
            List<string> tags = new List<string>(); 
            tags.Add("Benfica");
            _dto = new ChangeConnInfoDTO(5,tags);
            user = new User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _connectionDto = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,1);
            var listString = new List<string>{"bemfica"};
            var user1 = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            var user2 = new DDDSample1.Domain.Users.User("Ribeiro","2001/10/11",null,null,"ribeiro@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);

            listConnections = new List<Connection>();
            listConnections.Add(new Connection(1,1,listString,user1.Id,user2.Id));
        
        }

        [Fact]
        public async void ThrowExceptionWhenUserDoesntExist()
        {
            _userRepo.Setup(x => 
                x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<User>(null));

            //await Assert.ThrowsAsync<BusinessRuleValidationException>(()=>_controller.GetConnectionsOfUser(user.Id.AsGuid()));
            var result = await _controller.GetConnectionsOfUser(user.Id.AsGuid());
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


         [Fact]
        public async void ReturnNotFoundResultWhenRequestIsNotFound()
        {   
            
            ConnectionDTO connectionDTO = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,0);
            var list = new List<ConnectionDTO>();
            list.Add(connectionDTO);
            var connection = new UserConnectionsDTO(new List<ConnectionDTO>(list));

            _userRepo.Setup(x => 
                x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<User>(user));

            _repo.Setup(x => 
                x.ConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<List<Connection>>(null));

            var result = await _controller.GetConnectionsOfUser(this.user.Id.AsGuid());
            
            Assert.IsType<NotFoundResult>(result.Result);
            
        }

        
        [Fact]
        public async void ReturnExpectedResultWhenRequestIsFound()
        {   
            
             ConnectionDTO connectionDTO = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,0);
            var list = new List<ConnectionDTO>();
            list.Add(connectionDTO);
            var connection = new UserConnectionsDTO(new List<ConnectionDTO>(list));

            _userRepo.Setup(x => 
                x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<User>(user));

            _repo.Setup(x => 
                x.ConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<List<Connection>>(listConnections));

            var result = await _controller.GetConnectionsOfUser(this.user.Id.AsGuid());
            
            Assert.IsType<UserConnectionsDTO>(result.Value);
            
        }
        
  
        
        

        // Test Patch Method

        [Fact]
        public async void ReturnNotFoundResultWhenConnectionDoesntExist()
        {
            _repo.Setup(x => 
                x.GetByIdAsync(It.IsAny<ConnectionId>())
            ).Returns(Task.FromResult<Connection>(null));

            var result = await _controller.ChangeConnectionInfo(new Guid(),_dto);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        
         [Fact]
        public async void ReturnOkResultWhenConnectionExist()
        {   
            

             _repo.Setup(x => 
                x.GetByIdAsync(It.IsAny<ConnectionId>())
            ).Returns(Task.FromResult<Connection>(this.listConnections[0]));

            var result = await _controller.ChangeConnectionInfo(new Guid(),_dto);
            Assert.IsType<OkObjectResult>(result.Result);
        }

      

        
    
    }
}