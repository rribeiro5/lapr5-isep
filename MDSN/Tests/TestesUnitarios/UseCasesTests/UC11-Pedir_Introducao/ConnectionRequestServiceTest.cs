using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using Xunit;
using Moq;

namespace Tests.TestesUnitarios.UseCasesTests.UC11
{
    public class ConnectionRequestServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repo;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly ConnectionRequestService _service;
        private Guid oUser;
        private Guid iUser;
        private Guid dUser;
        private CreatingIntroductionRequestDTO dto;
        private DDDSample1.Domain.Users.User usr;
        private List<string> tags;

        public ConnectionRequestServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IRequestRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repo.Object, _userRepo.Object, null);
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
        }

        [Fact]
        public async void ThrowBusinessRuleExceptionWhenRepeatedUsers()
        {
            CreatingIntroductionRequestDTO cdto = new CreatingIntroductionRequestDTO(oUser, oUser, dUser, "abc", "cba", 5, null);
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _service.CreateIntroductionRequest(cdto));
        }

        [Fact]
        public async void ThrowBusinessRuleExceptionWhenUserDoesntExist()
        {
            Guid invalidUser = Guid.NewGuid();
            _userRepo.Setup(x => x.GetByIdAsync(new UserId(oUser))).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            CreatingIntroductionRequestDTO cdto = new CreatingIntroductionRequestDTO(oUser, invalidUser, dUser, "abc", "cba", 5, null);
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _service.CreateIntroductionRequest(cdto));
        }

        [Fact]
        public async void ThrowBusinessRuleExceptionWhenInvalidData()
        {
            CreatingIntroductionRequestDTO cdto = new CreatingIntroductionRequestDTO(oUser, oUser, dUser, null, null, -5, null);
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _service.CreateIntroductionRequest(cdto));
        }

        [Fact]
        public async void ReturnExpectedDataWhenIntroductionIsCreated()
        {
            ConnectionRequest req = new ConnectionRequest(new UserId(oUser), new UserId(iUser), new UserId(dUser),
                "abc", "cba", 5, tags);
            _repo.Setup(x => x.RegisterConnectionRequest(It.IsAny<ConnectionRequest>())).Returns(Task.FromResult<ConnectionRequest>(req));
            var res = await _service.CreateIntroductionRequest(dto);
            Assert.Equal(req.OrigUser, res.OrigUser);
            Assert.Equal(req.InterUser, res.InterUser);
            Assert.Equal(req.DestUser, res.DestUser);
        }
    }
}