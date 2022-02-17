using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC16_Consultar_Fortaleza_da_Rede
{
    public class UserNetworkControllerTest
    {
        private readonly Mock<IUserNetworkService> _mock;
        private readonly UserNetworkController _controller;
        private UserNetworkOperationsDTO _dto;
        
        public UserNetworkControllerTest()
        {
            _mock = new Mock<IUserNetworkService>();
            _controller = new UserNetworkController(_mock.Object);
        }
        
        [Fact]
        public async void ReturnBadRequestWhenIdDoesntExist()
        {
            _mock.Setup(x => x.GetNetworkConnectionStrength(Guid.Empty))
                .Throws(new BusinessRuleValidationException("test"));
            var result = await _controller.GetNetworkConnectionStrength(Guid.Empty);
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        
        [Fact]
        public async void ReturnOkFoundTypeWithExistingId()
        {
            var id = Guid.NewGuid();
            _mock.Setup(x => x.GetNetworkConnectionStrength(id))
                .Returns(Task.FromResult(50));
            var result=await _controller.GetNetworkConnectionStrength(id);
            Assert.IsType<OkObjectResult>(result);
        }
        
    }
}