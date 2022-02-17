using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using Xunit;
using Moq;

namespace Tests.TestesUnitarios.UseCasesTests.UC35
{
    public class ConnectionRequestServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repo;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly ConnectionRequestService _service;
        private UserId invalidId, validId;

        public ConnectionRequestServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IRequestRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repo.Object, _userRepo.Object, null);
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
        public async void ReturnNullWhenUserDoesntExists()
        {
            var res = await _service.getPendingConnectionsRequestOfUser(invalidId);
            Assert.Null(res);
        }

        [Fact]
        public async void ReturnNullWhenConnectionsAreNull()
        {
            _repo.Setup(x => x.getPendingConnectionsRequestOfUser(validId)).Returns(Task.FromResult<IList<ConnectionRequest>>(null));
            var res = await _service.getPendingConnectionsRequestOfUser(validId);
            Assert.Null(res);
        }

        [Fact]
        public async void ReturnPendingConnectionsOfValidUser()
        {
            List<ConnectionRequest> list = new List<ConnectionRequest>();
            _repo.Setup(x => x.getPendingConnectionsRequestOfUser(validId)).Returns(Task.FromResult<IList<ConnectionRequest>>(list));
            var res = await _service.getPendingConnectionsRequestOfUser(validId);
            Assert.Equal(new List<ConnectionRequestDTO>(), res);
        }
    }
}