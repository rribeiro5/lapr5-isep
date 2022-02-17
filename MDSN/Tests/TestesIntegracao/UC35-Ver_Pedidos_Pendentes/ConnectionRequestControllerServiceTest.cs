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

namespace Tests.TestesIntegracao.UC35
{
    public class ConnectionRequestControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repo;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly ConnectionRequestService _service;
        private readonly ConnectionRequestController _controller;
        private UserId invalidId, validId;

        public ConnectionRequestControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IRequestRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repo.Object, _userRepo.Object, null);
            _controller = new ConnectionRequestController(_service);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            
            invalidId = new UserId(Guid.NewGuid());
            _userRepo.Setup(x => x.GetByIdAsync(invalidId)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            validId = new UserId(Guid.NewGuid());
            DDDSample1.Domain.Users.User usr = new DDDSample1.Domain.Users.User("Test", "2000-1-1", 
                "Porto", "Portugal", "test@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _userRepo.Setup(x => x.GetByIdAsync(validId))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
        }

        [Fact]
        public async void ReturnBadRequestWhenUserDoesntExist()
        {
            var res = await _controller.getPendingConnectionsRequestOfUser(invalidId.AsGuid());
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async void ReturnBadRequestWhenConnectionsAreNull()
        {
            _repo.Setup(x => x.getPendingConnectionsRequestOfUser(validId)).Returns(Task.FromResult<IList<ConnectionRequest>>(null));
            var res = await _controller.getPendingConnectionsRequestOfUser(validId.AsGuid());
            Assert.IsType<BadRequestResult>(res);
        }

        [Fact]
        public async void ReturnOkWhenUserExists()
        {
            List<ConnectionRequest> list = new List<ConnectionRequest>();
            _repo.Setup(x => x.getPendingConnectionsRequestOfUser(validId)).Returns(Task.FromResult<IList<ConnectionRequest>>(list));
            var res = await _controller.getPendingConnectionsRequestOfUser(validId.AsGuid());
            Assert.IsType<OkObjectResult>(res);
        }

        [Fact]
        public async void ReturnExpectedObjectWhenUserExists()
        {
            List<ConnectionRequest> list = new List<ConnectionRequest>();
            _repo.Setup(x => x.getPendingConnectionsRequestOfUser(validId)).Returns(Task.FromResult<IList<ConnectionRequest>>(list));
            var res = await _controller.getPendingConnectionsRequestOfUser(validId.AsGuid());
            var result = res as OkObjectResult;
            Assert.Equal(new List<ConnectionRequestDTO>(), result.Value);
        }
    }
}