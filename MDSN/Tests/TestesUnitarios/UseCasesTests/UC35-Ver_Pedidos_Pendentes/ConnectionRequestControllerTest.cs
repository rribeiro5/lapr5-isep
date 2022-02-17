using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using DDDNetCore.Controllers;
using Xunit;
using Moq;

namespace Tests.TestesUnitarios.UseCasesTests.UC35
{
    public class ConnectionRequestControllerTest
    {
        private readonly Mock<IConnectionRequestService> _mock;
        private readonly ConnectionRequestController _controller;

        public ConnectionRequestControllerTest()
        {
            _mock = new Mock<IConnectionRequestService>();
            _controller = new ConnectionRequestController(_mock.Object);
        }

        [Fact]
        public async void ReturnBadRequestWhenUserDoesntExist()
        {
            Guid id = Guid.NewGuid();
            _mock.Setup(x => x.getPendingConnectionsRequestOfUser(new UserId(id)))
                .Returns(Task.FromResult<IEnumerable<ConnectionRequestDTO>>(null));
            var res = await _controller.getPendingConnectionsRequestOfUser(id);
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async void ReturnBadRequestWhenBusinessRuleException()
        {
            Guid id = Guid.NewGuid();
            _mock.Setup(x => x.getPendingConnectionsRequestOfUser(new UserId(id)))
                .Throws(new BusinessRuleValidationException("test"));
            var res = await _controller.getPendingConnectionsRequestOfUser(id);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnOkResultWhenUserExists()
        {
            Guid id = Guid.NewGuid();
            _mock.Setup(x => x.getPendingConnectionsRequestOfUser(new UserId(id)))
                .Returns(Task.FromResult<IEnumerable<ConnectionRequestDTO>>(new List<ConnectionRequestDTO>()));
            var res = await _controller.getPendingConnectionsRequestOfUser(id);
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async void ReturnExpectedObject()
        {
            Guid id = Guid.NewGuid();
            List<ConnectionRequestDTO> list = new List<ConnectionRequestDTO>();
            list.Add(new ConnectionRequestDTO(Guid.NewGuid(), new UserId(id), new UserId(Guid.NewGuid()), new UserId(Guid.NewGuid()), "abc", "abc"));
            _mock.Setup(x => x.getPendingConnectionsRequestOfUser(new UserId(id)))
                .Returns(Task.FromResult<IEnumerable<ConnectionRequestDTO>>(list));
            var res = await _controller.getPendingConnectionsRequestOfUser(id);
            var okResult = res as OkObjectResult;
            Assert.Equal(list, okResult.Value);
        }
    }
}