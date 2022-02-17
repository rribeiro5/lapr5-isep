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

namespace Tests.TestesIntegracao.UC26_Consultar_Sugestoes_Amigos
{
    public class SuggestedUsersControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;
        private readonly Mock<IConfiguration> _config;
        private readonly SuggestUsersController _controller;
        private readonly RandomSuggestedUsersService _service;
        private readonly User _sug1;
        private readonly User _sug2;
        private IList<User> _nonEmptyList;
        private User _usr;

        public SuggestedUsersControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _service = new RandomSuggestedUsersService(_repo.Object);
            _config = new Mock<IConfiguration>();
            _config.Setup(x => x.GetSection("FriendSuggestionsAlgorithm")
                .GetSection("NumberOfSuggestions").Value).Returns("3");
            _controller = new SuggestUsersController(_service, _config.Object);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            
            _usr = new DDDSample1.Domain.Users.User("user 1", "2000-01-01", null, null, "user1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _sug1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _sug2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            _nonEmptyList = new List<User>() {_sug1, _sug2};
            
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