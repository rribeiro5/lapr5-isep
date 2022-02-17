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

namespace Tests.TestesUnitarios.UseCasesTests.UC3
{
    public class ConnectionsControllerTest
    {
        private readonly Mock<IConnectionService> _mock;
        private readonly ConnectionsController _controller;

        private ChangeConnInfoDTO _dto;

        private ConnectionDTO _connectionDto;
        
        public ConnectionsControllerTest()
        {
            _mock = new Mock<IConnectionService>();
            _controller = new ConnectionsController(_mock.Object);
            List<string> tags = new List<string>(); 
            tags.Add("Benfica");
            _dto = new ChangeConnInfoDTO(5,tags);
            _connectionDto = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,1);
        }

        [Fact]
        public async void ReturnNotFoundResultWhenRequestDoesntExist()
        {
            _mock.Setup(x => 
                x.GetConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<UserConnectionsDTO>(null));
            var result = await _controller.GetConnectionsOfUser(new Guid());
            Assert.IsType<NotFoundResult>(result.Result);
        }

        
        [Fact]
        public async void ReturnExpectedResultWhenRequestIsFound()
        {   
            
            ConnectionDTO connectionDTO = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,0);
            var list = new List<ConnectionDTO>();
            list.Add(connectionDTO);
            var connection = new UserConnectionsDTO(new List<ConnectionDTO>(list));
            _mock.Setup(x => 
                x.GetConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<UserConnectionsDTO>(connection));
            var result = await _controller.GetConnectionsOfUser(new Guid());
            
            Assert.Equal<UserConnectionsDTO>(connection,result.Value);
            
        }
        
         [Fact]
        public async void ReturnOkResultWhenRequestIsFound()
        {   
            
            ConnectionDTO connectionDTO = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,0);
            var list = new List<ConnectionDTO>();
            list.Add(connectionDTO);
            var connection = new UserConnectionsDTO(new List<ConnectionDTO>(list));
            _mock.Setup(x => 
                x.GetConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<UserConnectionsDTO>(connection));
            var result = await _controller.GetConnectionsOfUser(new Guid());
            
            Assert.IsType<UserConnectionsDTO>(result.Value);
            
        }

         
         [Fact]
        public async void ReturnBADResultWhenRequestIsFound()
        {   
            
            ConnectionDTO connectionDTO = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,0);
            var list = new List<ConnectionDTO>();
            list.Add(connectionDTO);
            var connection = new UserConnectionsDTO(new List<ConnectionDTO>(list));
            _mock.Setup(x => 
                x.GetConnectionsOfUser(It.IsAny<UserId>())
            ).Throws(new BusinessRuleValidationException("Error"));
            var result = await _controller.GetConnectionsOfUser(new Guid());
            
            Assert.IsType<BadRequestObjectResult>(result.Result);
            
        }
        

        // Test Patch Method

        [Fact]
        public async void ReturnNotFoundResultWhenConnectionDoesntExist()
        {
            _mock.Setup(x => 
                x.UpdateConnection(It.IsAny<ConnectionId>(),_dto)
            ).Returns(Task.FromResult<ConnectionDTO>(null));

            var result = await _controller.ChangeConnectionInfo(new Guid(),_dto);
            Assert.IsType<NotFoundResult>(result.Result);
        }

         [Fact]
        public async void ReturnOkResultWhenConnectionExist()
        {   
            

            _mock.Setup(x => 
                x.UpdateConnection(It.IsAny<ConnectionId>(),_dto)
            ).Returns(Task.FromResult<ConnectionDTO>(this._connectionDto));

            var result = await _controller.ChangeConnectionInfo(new Guid(),_dto);
            Assert.IsType<OkObjectResult>(result.Result);
        }

      

        
    
    }
}