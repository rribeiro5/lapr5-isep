using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC10_Pedido_Conexão
{
    public class ConnectionRequestControllerTest
    {
        private readonly Mock<IConnectionRequestService> mock;
        private readonly ConnectionRequestController controller;
        private readonly Utils utils;

        private CreatingRequestDTO createDto;
        private ConnectionRequestDTO responseDto;
        
        
        public ConnectionRequestControllerTest()
        {
            mock = new Mock<IConnectionRequestService>();
            controller = new ConnectionRequestController(mock.Object);
            utils = new Utils();
            createDto = utils.createRequestDto();
            responseDto = utils.createResponseDto();
        }

        [Fact]
        public async void SuccessfullyCreateConnectionRequest()
        {
            mock.Setup(x => x.AddAsync(It.IsAny<CreatingRequestDTO>()))
                .Returns(Task.FromResult(responseDto));
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<CreatedAtActionResult>(response);
        }
        
        [Fact]
        public async void ReturnsExpectedDtoWithSuccessfullRequestCreation()
        {
            mock.Setup(x => x.AddAsync(It.IsAny<CreatingRequestDTO>()))
                .Returns(Task.FromResult(responseDto));
            var response=(await controller.SendConnectionRequest(createDto) as CreatedAtActionResult)
                .Value;
            Assert.Equal(response,response);
        }
        
        [Fact]
        public async void ReturnsBadRequestOnThrownBusinessRuleValidationException()
        {
            mock.Setup(x => x.AddAsync(It.IsAny<CreatingRequestDTO>()))
                .Throws(new BusinessRuleValidationException(""));
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}