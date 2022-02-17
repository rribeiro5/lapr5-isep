using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC7
{
    public class UsersControllerTest
    {
        private readonly Mock<IUserService> _mock;
        private readonly Mock<IUserNetworkService> _networkService;
        private readonly UsersController _controller;

        public UsersControllerTest()
        {
            _mock = new Mock<IUserService>();
            _networkService = new Mock<IUserNetworkService>();
            _controller = new UsersController(_mock.Object,_networkService.Object);
        }

        [Fact]
        public async void ReturnNotFoundRequestWithNoNetwork()
        {
            _networkService.Setup(m => m.GetUserNetwork(It.IsAny<UserId>(),It.IsAny<int>())).Returns(Task.FromResult<UserNetworkDTO>(null));
            var result = await _controller.GetUserNetwork(new Guid(),5);
            Assert.IsType<NotFoundResult>(result.Result);
        }

         [Fact]
        public async void ReturnValidRequestWithNetwork()
        {   
            UserNetworkDTO dto = new UserNetworkDTO(5, new Guid(),"1@teste.com", new List<string>(),"adad",new List<UserNetworkConnDTO>());
            _networkService.Setup(m => m.GetUserNetwork(It.IsAny<UserId>(),It.IsAny<int>())).Returns(Task.FromResult<UserNetworkDTO>(dto));
            var result = await _controller.GetUserNetwork(new Guid(),5);

            Assert.IsType<UserNetworkDTO>(result.Value);
        }

         [Fact]
        public async void ReturnDesiredRequestWithNetwork()
        {   
            UserNetworkDTO dto = new UserNetworkDTO(5, new Guid(),"1@teste.com", new List<string>(),"adad",new List<UserNetworkConnDTO>());
            _networkService.Setup(m => m.GetUserNetwork(It.IsAny<UserId>(),It.IsAny<int>())).Returns(Task.FromResult<UserNetworkDTO>(dto));
            var result = await _controller.GetUserNetwork(new Guid(),5);

            Assert.Equal<UserNetworkDTO>(result.Value,dto);
        }


      
    }
}