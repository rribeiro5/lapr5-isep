using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC09_Selecionar_Utilizadores
{
    public class RandomSuggestedUsersServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly Mock<IConfiguration> _config;
        private readonly SuggestUsersController _controller;
        private readonly RandomSuggestedUsersService _service;

        private IList<User> _nonEmptyList;
        private User _usr;

        public RandomSuggestedUsersServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new RandomSuggestedUsersService(_repo.Object);
            _config = new Mock<IConfiguration>();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("2");
            _controller = new SuggestUsersController(_service, _config.Object);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            
            _usr = new User("Test", "2000-1-1", 
                "Porto", "Portugal", "test@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            
            _nonEmptyList = new List<User>()
            {
                new User("Test0", "2000-1-1", 
                    "Porto", "Portugal", "test1@gmail.com", "Password3?","description",
                    "+351123456789" , "https://www.linkedin.com/in/id000", 
                    "https://www.facebook.com/id000", new List<string>() {"tagII"}),
                new User("Test1", "2000-10-1", 
                    "Lisbon", "Portugal", "test2@gmail.com", "Password3?","description",
                    "+35112345679" , "https://www.linkedin.com/in/id111", 
                    "https://www.facebook.com/id111", new List<string>() {"tagA"})
            };
            
        }
        
        [Fact]
        public async void ReturnNotFoundResultWhenDoesntFindUsersToSuggest()
        {
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<User>>(null));
            var result = await _controller.GetFriendSuggestions(_usr.Id.AsGuid());
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnBadRequestWhenBusinessRuleFails()
        {
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Throws(new BusinessRuleValidationException("test"));
            var result = await _controller.GetFriendSuggestions(_usr.Id.AsGuid());
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkResultWhenUsersAreFound()
        {
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult(_nonEmptyList));
            var result = await _controller.GetFriendSuggestions(_usr.Id.AsGuid());
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkResultWhenSpecificUsersAreFound()
        {
            _repo.Setup(x => x.GetAllNonFriendsAndWithoutFriendRequests(It.IsAny<UserId>()))
                .Returns(Task.FromResult(_nonEmptyList));
            var result = await _controller.GetFriendSuggestions(_usr.Id.AsGuid());
            var okResult = result as OkObjectResult;
            var returnedList = (List<UserSuggestedDto>)okResult?.Value;
            
            
            for (var i = 0; i < returnedList.Count; i++)
            {
                Assert.Contains(_nonEmptyList, u =>u.Id.AsGuid().Equals(returnedList[i].id));
     
            }
        }

    }
}