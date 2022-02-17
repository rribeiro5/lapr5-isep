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

namespace Tests.TestesIntegracao.UC16_Consultar_Fortaleza_da_Rede
{
    public class UserNetworkControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repoUser;
        private readonly Mock<IConnectionRepository> _repoConn;
        private readonly UserNetworkController _controller;
        private readonly UserNetworkService _service;
        private readonly User _user;

        public UserNetworkControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repoUser = new Mock<IUserRepository>();
            _repoConn = new Mock<IConnectionRepository>();
            _service = new UserNetworkService(_unit.Object, _repoConn.Object, _repoUser.Object,null);
            _controller = new UserNetworkController(_service);
            _user = new User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
        }
        
        [Fact]
        public async void ReturnNotFoundWhenUserDoesntExistOnGetNetworkConnectionStrength()
        {
            _repoUser.Setup(x => 
                x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<User>(null));
            var result = await _controller.GetNetworkConnectionStrength(new Guid());
            Assert.IsType<NotFoundObjectResult>(result);
        } 
        
       [Fact]
       public async void ReturnOkResultWhenUserExistsOnGetNetworkConnectionStrength()
       {
           var expected = 5;
           _repoUser.Setup(x => 
               x.GetByIdAsync(It.IsAny<UserId>())
           ).Returns(Task.FromResult(_user));
           _repoUser.Setup(x => x.GetNetworkConnectionStrength(It.IsAny<UserId>()))
               .Returns(Task.FromResult(expected));
           var result = await _controller.GetNetworkConnectionStrength(_user.Id.AsGuid());
           Assert.IsType<OkObjectResult>(result);
       }
       
       [Fact]
       public async void ReturnExpectedOnGetNetworkConnectionStrength()
       {
           var expected = new UserNetworkOperationsDTO(5);
           _repoUser.Setup(x => 
               x.GetByIdAsync(It.IsAny<UserId>())
           ).Returns(Task.FromResult(_user));
           _repoUser.Setup(x => x.GetNetworkConnectionStrength(It.IsAny<UserId>()))
               .Returns(Task.FromResult(5));
           var result = await _controller.GetNetworkConnectionStrength(_user.Id.AsGuid());
           var okResult = result as OkObjectResult;
           var obtained = (UserNetworkOperationsDTO)okResult?.Value;
           Assert.Equal(expected.Value, obtained.Value);
       }
         
    }
}