using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using Microsoft.Extensions.Configuration;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC09_Selecionar_Utilizadores
{
    public class SuggestUsersControllerTest
    {
        private readonly Mock<ISuggestedUsersService> _serviceMock;
        private readonly Mock<IUserService> _usrMock;
        private readonly Mock<IConfiguration> _config;
        private readonly SuggestUsersController _controller;
        private List<UserSuggestedDto> _nonEmptyList;
        private List<UserSuggestedDto> _emptyList;

        public SuggestUsersControllerTest()
        {
            _serviceMock = new Mock<ISuggestedUsersService>();
            _usrMock = new Mock<IUserService>();
            _config = new Mock<IConfiguration>();
            _controller = new SuggestUsersController(_serviceMock.Object, _config.Object);
            _emptyList = new List<UserSuggestedDto>();
            _nonEmptyList = new List<UserSuggestedDto>()
            {
                new()
                {
                    id = Guid.NewGuid(), city = "Porto", country = "Portugal", birthdayDate = "2000-1-1",
                    profileDescription = "desc", facebookURL = "https://www.facebook.com/id123", linkedInURL = "https://www.linkedin.com/in/id123",
                    interestTagsDtoList =  new List<string>() {"tag1"}
                },
                
                new()
                {
                id = Guid.NewGuid(), city = "Lisbon", country = "Portugal", birthdayDate = "2002-2-2",
                profileDescription = "desc1", facebookURL = "https://www.facebook.com/id234", linkedInURL = "https://www.linkedin.com/in/id234",
                interestTagsDtoList =  new List<string>() {"tagA", "tagB"}
                }
                
            };
        }
        
        [Fact]
        public async void ReturnsNullWhenDoesntFindUsers()
        {
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<UserDto>
            (new UserDto(_nonEmptyList[0].id, "2005-1-3", "123@gmail.com", new List<string>() {"tag1"})));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2))
                .Returns(Task.FromResult<List<UserSuggestedDto>>(null));
            var result = await _controller.GetFriendSuggestions(_nonEmptyList[0].id);
            Assert.IsType<NotFoundObjectResult>(result);
        }


        [Fact]
        public async void ReturnsBadRequestWhenBusinessRuleFails()
        {
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<UserDto>
                (new UserDto(_nonEmptyList[0].id, "2005-1-3", "123@gmail.com", new List<string>() {"tag1"})));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2))
                .Throws(new BusinessRuleValidationException("test"));
            var result = await _controller.GetFriendSuggestions(_nonEmptyList[0].id);
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void SuccessfullyReturnListUsers()
        {
            var id = Guid.NewGuid();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("");
            _usrMock.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult
                (new UserDto(id, "2005-1-3", "123@gmail.com", new List<string>
                    {"tag1"})));
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),3)).
                Returns(Task.FromResult(_nonEmptyList));
            var result = await _controller.GetFriendSuggestions(id);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnExpectedListUserDto()
        {
            var id = Guid.NewGuid();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("2");
            _serviceMock.Setup(x => x.GetSuggestedUsers(It.IsAny<UserId>(),2)).
                Returns(Task.FromResult(_nonEmptyList));
            var result =  await _controller.GetFriendSuggestions(id) as OkObjectResult;
            var resultList = (List<UserSuggestedDto>)result?.Value;
            var tmp = new List<UserSuggestedDto>();
            foreach (var user in _nonEmptyList)
            {
                tmp.Add(user);
            }
          
            tmp.Sort((suggestedDto, other) => suggestedDto.id.CompareTo(other.id));
            resultList.Sort((suggestedDto, other) => suggestedDto.id.CompareTo(other.id));
            for (var i = 0; i < resultList.Count; i++)
            {
                Assert.Equal(resultList[i].id, tmp[i].id);
            }
        }
    }
}