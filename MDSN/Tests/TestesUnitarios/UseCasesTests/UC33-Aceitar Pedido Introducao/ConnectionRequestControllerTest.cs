using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DDDNetCore.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Tests.TestesUnitarios.UseCasesTests
{
    public class ConnectionRequestControllerTest
    {
        private readonly Mock<IConnectionRequestService> _mock;
        private readonly ConnectionRequestController _controller;
        private UserId _UserId;

        private UserRequestsDTO expectedDto;
        
        public ConnectionRequestControllerTest()
        {
            _mock = new Mock<IConnectionRequestService>();
            _controller = new ConnectionRequestController(_mock.Object);
            _UserId = new UserId(new Guid());
            var connectionDto = new ConnectionRequestDTO(new Guid(), null, null, null, "", "", "");
            var list = new List <ConnectionRequestDTO>();
            list.Add(connectionDto);
            expectedDto = new UserRequestsDTO(list);
        }

        [Fact]
        public async void ReturnNotFoundResultWhenUserDoesntExist()
        {
            _mock.Setup(x => 
                x.GetRequestsInAcceptance(It.IsAny<UserId>())
            ).Returns(Task.FromResult<UserRequestsDTO>(null));
            var result = await _controller.GetRequestsInAcceptance(this._UserId.AsGuid());
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public async void ReturnOkResultWhenRequestIsFound()
        {

            _mock.Setup(x => 
                x.GetRequestsInAcceptance(It.IsAny<UserId>())
            ).Returns(Task.FromResult(this.expectedDto));
            var result = await _controller.GetRequestsInAcceptance(this._UserId.AsGuid());
            Assert.IsType<UserRequestsDTO>(result.Value);
        }
        
        [Fact]
        public async void ReturnExpectedObjectWhenRequestIsFound()
        {
            _mock.Setup(x => 
                x.GetRequestsInAcceptance(It.IsAny<UserId>())
            ).Returns(Task.FromResult(this.expectedDto));
            var result = await _controller.GetRequestsInAcceptance(this._UserId.AsGuid());

            Assert.Equal(result.Value,this.expectedDto);
        }
        
    }
}