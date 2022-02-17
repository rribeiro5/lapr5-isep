using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC26_Consultar_Sugestoes_Amigos
{
    public class SuggestedUsersControllerTest
    {
        private readonly Mock<ISuggestedUsersService> _serviceMock;
        private readonly Mock<IUserService> _usrMock;
        private readonly Mock<IConfiguration> _config;
        private readonly SuggestUsersController _controller;
        private List<UserSuggestedDto> _nonEmptyList;
        private readonly DDDSample1.Domain.Users.User _usr;
        private readonly DDDSample1.Domain.Users.User _sug1;
        private readonly DDDSample1.Domain.Users.User _sug2;
        private List<UserSuggestedDto> _emptyList;

        public SuggestedUsersControllerTest()
        {
            _serviceMock = new Mock<ISuggestedUsersService>();
            _usrMock = new Mock<IUserService>();
            _config = new Mock<IConfiguration>();
            _controller = new SuggestUsersController(_serviceMock.Object, _config.Object);
            _usr = new DDDSample1.Domain.Users.User("user 1", "2000-01-01", null, null, "user1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _sug1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _sug2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _emptyList = new List<UserSuggestedDto>();
            _nonEmptyList = new List<UserSuggestedDto>() {_sug1.toUserSuggestedDto(), _sug2.toUserSuggestedDto()};
        }
        
        [Fact]
        public async void ReturnsNullWhenDoesntFindUsers()
        {
            var dto = _usr.ToDto();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<UserDto>
            (dto));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2))
                .Returns(Task.FromResult<List<UserSuggestedDto>>(null));
            var result = await _controller.GetFriendSuggestions(dto.Id);
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async void ReturnsBadRequestWhenBusinessRuleFails()
        {
            var dto = _usr.ToDto();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<UserDto>
                (dto));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2))
                .Throws(new BusinessRuleValidationException("test"));
            var result = await _controller.GetFriendSuggestions(dto.Id);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void SuccessfullyReturnListUsers()
        {
            var dto = _usr.ToDto();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("2");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<UserDto>
                (dto));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2)).
                Returns(Task.FromResult(_nonEmptyList));
            var result = await _controller.GetFriendSuggestions(dto.Id);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnExpectedListUserDto()
        {
            var dto = _usr.ToDto();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("2");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<UserDto>
                (dto));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2)).
                Returns(Task.FromResult(_nonEmptyList));
            var result =  await _controller.GetFriendSuggestions(dto.Id) as OkObjectResult;
            var resultList = (List<UserSuggestedDto>)result?.Value;
           
            
            for (var i = 0; i < resultList.Count; i++)
            {
                Assert.Contains(_nonEmptyList, u =>u.id.Equals(resultList[i].id));
     
            }
        }
    }
}