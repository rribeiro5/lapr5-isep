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

namespace Tests.TestesUnitarios.UseCasesTests.UC11
{
    public class ConnectionRequestControllerTest
    {
        private readonly Mock<IConnectionRequestService> _mock;
        private readonly ConnectionRequestController _controller;
        private Guid oUser;
        private Guid iUser;
        private Guid dUser;
        private CreatingIntroductionRequestDTO cdto;
        private ConnectionRequestDTO connReq;

        public ConnectionRequestControllerTest()
        {
            _mock = new Mock<IConnectionRequestService>();
            _controller = new ConnectionRequestController(_mock.Object);
            oUser = Guid.NewGuid();
            iUser = Guid.NewGuid();
            dUser = Guid.NewGuid();
            List<string> tags = new List<string>();
            tags.Add("A");
            cdto = new CreatingIntroductionRequestDTO(oUser, iUser, dUser, "abc", "cba", 5, tags);
            connReq = new ConnectionRequestDTO(Guid.NewGuid(), new UserId(oUser), new UserId(iUser), new UserId(dUser), "abc", "cba", "bca");
        }

        [Fact]
        public async void ReturnBadRequestWhenDataIsInvalid()
        {
            CreatingIntroductionRequestDTO dto = new CreatingIntroductionRequestDTO(oUser, iUser, dUser, null, null, 0, null);
            _mock.Setup(x => x.CreateIntroductionRequest(dto)).Throws(new BusinessRuleValidationException("test"));
            var res = await _controller.CreateIntroductionRequest(dto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnBadRequestWhenUserIsRepeated()
        {
            CreatingIntroductionRequestDTO dto = new CreatingIntroductionRequestDTO(oUser, oUser, dUser, null, null, 0, null);
            _mock.Setup(x => x.CreateIntroductionRequest(dto)).Throws(new BusinessRuleValidationException("test"));
            var res = await _controller.CreateIntroductionRequest(dto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnBadRequestWhenUserIsNotRegistered()
        {
            CreatingIntroductionRequestDTO dto = new CreatingIntroductionRequestDTO(oUser, Guid.NewGuid(), dUser, null, null, 0, null);
            _mock.Setup(x => x.CreateIntroductionRequest(dto)).Throws(new BusinessRuleValidationException("test"));
            var res = await _controller.CreateIntroductionRequest(dto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnOkWhenSuccess()
        {
            _mock.Setup(x => x.CreateIntroductionRequest(cdto)).Returns(Task.FromResult<ConnectionRequestDTO>(connReq));
            var res = await _controller.CreateIntroductionRequest(cdto);
            Assert.IsType<CreatedAtActionResult>(res);
        }
    }
}