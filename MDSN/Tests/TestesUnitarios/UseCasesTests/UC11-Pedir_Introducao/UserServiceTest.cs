using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Xunit;
using Moq;

namespace Tests.TestesUnitarios.UseCasesTests.UC11
{
    public class UserServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly UserService _service;
        private readonly UserId oUser;
        private readonly UserId dUser;
        private DDDSample1.Domain.Users.User usr;
        private List<UserDto> dtos;

        public UserServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unit.Object, _repo.Object,null,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            List<string> tags = new List<string>();
            tags.Add("A");
            usr = new DDDSample1.Domain.Users.User("Abc", "2000-10-10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
            oUser = new UserId(Guid.NewGuid());
            dUser = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(oUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            _repo.Setup(x => x.GetByIdAsync(dUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(usr));
            dtos = new List<UserDto>();
            dtos.Add(new UserDto(Guid.NewGuid(), "2000/10/10", "abc@gmail.com", tags));
        }

        [Fact]
        public async void ReturnNullWhenOUserDoesntExist()
        {
            var invalidUser = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(invalidUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var res = await _service.getListOfMutualFriends(invalidUser, dUser);
            Assert.Null(res);
        }

        [Fact]
        public async void ReturnNullWhenDUserDoesntExist()
        {
            var invalidUser = new UserId(Guid.NewGuid());
            _repo.Setup(x => x.GetByIdAsync(invalidUser)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var res = await _service.getListOfMutualFriends(oUser, invalidUser);
            Assert.Null(res);
        }

        [Fact]
        public async void ReturnNullWhenMutualFriendsListIsNull()
        {
            _repo.Setup(x => x.GetMutualFriends(oUser,dUser)).Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(null));
            var res = await _service.getListOfMutualFriends(oUser, dUser);
            Assert.Null(res);
        }

        [Fact]
        public async void ReturnExpectedListWhenUsersAreValid()
        {
            IList<DDDSample1.Domain.Users.User> usrs = new List<DDDSample1.Domain.Users.User>();
            usrs.Add(usr);
            _repo.Setup(x => x.GetMutualFriends(oUser, dUser)).Returns(Task.FromResult<IList<DDDSample1.Domain.Users.User>>(usrs));
            var res = await _service.getListOfMutualFriends(oUser, dUser);
            Assert.Equal(dtos, res);
        }
    }
}