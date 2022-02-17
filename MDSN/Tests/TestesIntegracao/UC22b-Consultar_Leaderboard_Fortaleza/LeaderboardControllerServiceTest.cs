using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Utils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;


namespace Tests.TestesIntegracao.UC22b_Consultar_Leaderboard_Fortaleza
{
    public class LeaderboardControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IConnectionRepository> _connRepo;
        private readonly LeaderboardService _service;
        private readonly LeaderboardController _controller;
        private SortedSet<UserLeaderboardDTO> _result;
        private Dictionary<UserId, int> _repoResult;
        private User _usr1;
        private User _usr2;
        private User _usr3;
        private User _usr4;
        private User _usr5;
        

        public LeaderboardControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _userRepo = new Mock<IUserRepository>();
            _connRepo = new Mock<IConnectionRepository>();
            _service = new LeaderboardService(_userRepo.Object, _connRepo.Object);
            _controller = new LeaderboardController(_service);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _usr1 = new User("User 1", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user1@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr2 = new User("User 2", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user2@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr3 = new User("User 3", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user3@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr4 = new User("User 4", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user4@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr5 =new User("User 5", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user5@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _repoResult = new Dictionary<UserId, int>();
           
            var comparer = LeaderboardComparer.Get();
            _result = new SortedSet<UserLeaderboardDTO>(comparer)
            {
                _usr1.ToUserLeaderboardDTO(40),_usr2.ToUserLeaderboardDTO(27),_usr3.ToUserLeaderboardDTO(33),
                _usr4.ToUserLeaderboardDTO(27), _usr5.ToUserLeaderboardDTO(55)
            };

            foreach (var x in _result)
            {
                _repoResult.Add(new UserId(x.Id), x.value);
            }
            
        }

        [Fact]
        public async void ReturnBadRequestResultWhenThrowsException()
        {
            var users = _repoResult.Keys.ToList();
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult<List<User>>(null));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Throws(new BusinessRuleValidationException("test"));
            var result = await _controller.GetNetworkStrengthLeaderboard();
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ReturnOkResultWhenRequestExists()
        {
            var users = _repoResult.Keys.ToList();
            var resultUsers = new List<DDDSample1.Domain.Users.User>() {_usr1, _usr2, _usr3, _usr4, _usr5};
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult(resultUsers));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Returns(Task.FromResult(_repoResult));
            var result = await _controller.GetNetworkStrengthLeaderboard();
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ReturnOkResultWithExpectedResultWhenRequestExistsOnGet()
        {
            var users = _repoResult.Keys.ToList();
            var resultUsers = new List<DDDSample1.Domain.Users.User>() {_usr1, _usr2, _usr3, _usr4, _usr5};
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult(resultUsers));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Returns(Task.FromResult(_repoResult));
            var result = await _controller.GetNetworkStrengthLeaderboard() as OkObjectResult;
            var obtained = (SortedSet<UserLeaderboardDTO>) result?.Value;
            Assert.Empty(obtained.Intersect(_result));
        }
        
        [Fact]
        public async void ReturnOkResultWithSpecifiedSizeResultWhenRequestExistsOnGet()
        {
            var users = _repoResult.Keys.ToList();
            var resultUsers = new List<DDDSample1.Domain.Users.User>() {_usr1, _usr2, _usr3, _usr4, _usr5};
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult(resultUsers));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Returns(Task.FromResult(_repoResult));
            var result = await _controller.GetNetworkStrengthLeaderboard() as OkObjectResult;
            var obtained = (SortedSet<UserLeaderboardDTO>) result?.Value;
            Assert.Equal(5, obtained?.Count);
        }
        
    }
}