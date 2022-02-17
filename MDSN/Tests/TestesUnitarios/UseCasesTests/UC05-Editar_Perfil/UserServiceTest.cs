using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC05_Editar_Perfil
{
    public class UserServiceTest
    {

        private readonly Mock<IUnitOfWork> _unitOfWork; 

        private readonly Mock<IUserRepository> _repo;

        private readonly UserService _service;

        private readonly DDDSample1.Domain.Users.User _user;

        private readonly UserPrivateProfileDto _dto;

        public UserServiceTest()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new UserService(_unitOfWork.Object, _repo.Object,null,null);
            _unitOfWork.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _user = new DDDSample1.Domain.Users.User("Test", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "test@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _dto = _user.toUserPrivateProfileDto();
            
        }

        [Fact]
        public async void ReturnNullWhenGetProfileDoesntExist()
        {
            Guid g = Guid.NewGuid();
            _repo.Setup(x => x.GetByIdAsync(new UserId(g)))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.GetPrivateProfileByIdAsync(new UserId(g));
            Assert.Null(result);
        }

        [Fact]
        public async void ReturnUserPrivateProfileDtoWhenProfileExists()
        {
            _repo.Setup(x => x.GetByIdAsync(_user.Id))
                .Returns(Task.FromResult(_user));
            var result = await _service.GetPrivateProfileByIdAsync(_user.Id);
            Assert.Equal(_user.toUserPrivateProfileDto().Id,result.Id);
        }
        
        [Fact]
        public async void ReturnUserPrivateProfileDtoWhenGetProfileExists()
        {
            _repo.Setup(x => x.GetByIdAsync(_user.Id))
                .Returns(Task.FromResult(_user));
            var result = await _service.GetPrivateProfileByIdAsync(_user.Id);
            Assert.IsType<UserPrivateProfileDto>(result);
        }

        [Fact]
        public async void ReturnNullWhenProfileDoesntExist()
        {
            Guid g = Guid.NewGuid();
            _repo.Setup(x => x.GetByIdAsync(new UserId(g)))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.UpdatePrivateProfileAsync(_dto);
            Assert.Null(result);
        }
        
        
        [Fact]
        public async void ReturnDesiredDtoWhenProfileIsUpdated()
        {
            _repo.Setup(x => x.GetByIdAsync(_user.Id))
                .Returns(Task.FromResult(_user));
            var result = await _service.UpdatePrivateProfileAsync(_dto);
            var expected = new UserPrivateProfileDto(_dto.Id, DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,_dto.Name, _dto.Email, _dto.PhoneNumber,
                _dto.BirthdayDate, _dto.City,
                _dto.Country, _dto.Description, _dto.Points, _dto.LinkedInURL, _dto.FacebookURL, _dto.InterestTags,
                _dto.EmotionalState);
            Assert.Equal(expected.Id,result.Id);
        }
        
        [Fact]
        public async void ReturnDtoWhenRequestIsUpdated()
        {
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(_user));
            
            var result = await _service.UpdatePrivateProfileAsync(_dto);
            Assert.IsType<UserPrivateProfileDto>(result);
        }
        
        
        
    }
}