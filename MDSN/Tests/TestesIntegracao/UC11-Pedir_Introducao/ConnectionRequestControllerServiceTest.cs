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

namespace Tests.TestesIntegracao.UC11
{
    public class ConnectionRequestControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repo;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly ConnectionRequestService _service;
        private readonly ConnectionRequestController _controller;
        private Guid oUser;
        private Guid iUser;
        private Guid dUser;
        private CreatingIntroductionRequestDTO dto;
        private DDDSample1.Domain.Users.User usr;
        private List<string> tags;
        private ConnectionRequestDTO connReq;

        public ConnectionRequestControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IRequestRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repo.Object, _userRepo.Object, null);
            _controller = new ConnectionRequestController(_service);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));

            tags = new List<string>();
            tags.Add("A");
            usr = new DDDSample1.Domain.Users.User("Abc", "2000/10/10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
            oUser = Guid.NewGuid();
            _userRepo.Setup(x => x.GetByIdAsync(new UserId(oUser))).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            iUser = Guid.NewGuid();
            _userRepo.Setup(x => x.GetByIdAsync(new UserId(iUser))).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            dUser = Guid.NewGuid();
            _userRepo.Setup(x => x.GetByIdAsync(new UserId(dUser))).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            dto = new CreatingIntroductionRequestDTO(oUser, iUser, dUser, "abc", "cba", 5, tags);
            _userRepo.Setup(x => x.validIntroductionRequest(It.IsAny<UserId>(),It.IsAny<UserId>(),It.IsAny<UserId>()))
                .Returns(Task.FromResult<bool>(true));
            connReq = new ConnectionRequestDTO(Guid.NewGuid(), new UserId(oUser), new UserId(iUser), new UserId(dUser), "abc", "cba", "bca");
        }

        [Fact]
        public async void ReturnBadRequestWhenDataIsInvalid()
        {
            CreatingIntroductionRequestDTO dto = new CreatingIntroductionRequestDTO(oUser, iUser, dUser, null, null, 0, null);
            var res = await _controller.CreateIntroductionRequest(dto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnBadRequestWhenUserIsRepeated()
        {
            CreatingIntroductionRequestDTO cdto = new CreatingIntroductionRequestDTO(oUser, oUser, dUser, "abc", "cba", 5, null);
            var res = await _controller.CreateIntroductionRequest(cdto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnBadRequestWhenUserIsNotRegistered()
        {
            Guid invalidUser = Guid.NewGuid();
            _userRepo.Setup(x => x.GetByIdAsync(new UserId(oUser))).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            CreatingIntroductionRequestDTO cdto = new CreatingIntroductionRequestDTO(oUser, invalidUser, dUser, "abc", "cba", 5, null);
            var res = await _controller.CreateIntroductionRequest(cdto);
            Assert.IsType<BadRequestObjectResult>(res);
        }

        [Fact]
        public async void ReturnOkWhenSuccess()
        {
            ConnectionRequest req = new ConnectionRequest(new UserId(oUser), new UserId(iUser), new UserId(dUser),
                "abc", "cba", 5, tags);
            _repo.Setup(x => x.RegisterConnectionRequest(It.IsAny<ConnectionRequest>())).Returns(Task.FromResult<ConnectionRequest>(req));
            var res = await _controller.CreateIntroductionRequest(dto);
            Assert.IsType<CreatedAtActionResult>(res);
        }
    }
}