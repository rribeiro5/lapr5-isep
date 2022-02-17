using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Xunit;
using Moq;

namespace Tests.TestesUnitarios.UseCasesTests.UC8
{
    public class UserServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private CreatingUserDto cdto;
        private DDDSample1.Domain.Users.User usr;
        private UserDto rdto;

        public UserServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            List<string> tags = new List<string>();
            tags.Add("A");
            cdto = new CreatingUserDto("Abc", "2000/10/10", "http://www.gravatar.com/avatar/a16a38cdfe8b2cbd38e8a56ab93238d3",
                "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", tags, "https://www.facebook.com/id123", "https://www.linkedin.com/in/id123");
            List<Tag> rtags = new List<Tag>();
            rtags.Add(new Tag("A"));
            usr = new DDDSample1.Domain.Users.User("Abc", "2000-10-10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
            rdto = new UserDto(Guid.NewGuid(), "2000/10/10", "abc@gmail.com", tags);
        }

        [Fact]
        public async void ThrowBusinessRuleExceptionWhenDataIsInvalid()
        {
            CreatingUserDto dto = new CreatingUserDto("", "", "", "", "", "", "", "", "", null, "", "");
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _service.RegisterUser(dto));
        }

        [Fact]
        public async void ReturnExpectedObjectWhenUserIsRegisteredSuccessfully()
        {
            _repo.Setup(x => x.AddAsync(It.IsAny<DDDSample1.Domain.Users.User>())).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            var res = await _service.RegisterUser(cdto);
            Assert.Equal(rdto, res);
        }

        [Fact]
        public async void ThrowDbUpdateExceptionWhenUserIsAlreadyRegistered()
        {
            _repo.Setup(x => x.AddAsync(It.IsAny<DDDSample1.Domain.Users.User>())).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            await _service.RegisterUser(cdto);
            _repo.Setup(x => x.AddAsync(It.IsAny<DDDSample1.Domain.Users.User>())).Throws(new DbUpdateException());
            await Assert.ThrowsAsync<DbUpdateException>(() => _service.RegisterUser(cdto));
        }
    }
}