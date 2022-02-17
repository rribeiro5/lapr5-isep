using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Utils;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC22b_Consultar_Leaderboard_Fortaleza
{
    public class LeaderboardServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IConnectionRepository> _connRepo;
        private readonly LeaderboardService _service;
        private SortedSet<UserLeaderboardDTO> _result;
        private Dictionary<UserId, int> _repoResult;
        private DDDSample1.Domain.Users.User _usr1;
        private DDDSample1.Domain.Users.User _usr2;
        private DDDSample1.Domain.Users.User _usr3;
        private DDDSample1.Domain.Users.User _usr4;
        private DDDSample1.Domain.Users.User _usr5;
        

        public LeaderboardServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _userRepo = new Mock<IUserRepository>();
            _connRepo = new Mock<IConnectionRepository>();
            _service = new LeaderboardService(_userRepo.Object, _connRepo.Object);
            _usr1 = new DDDSample1.Domain.Users.User("User 1", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user1@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr2 = new DDDSample1.Domain.Users.User("User 2", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user2@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr3 = new DDDSample1.Domain.Users.User("User 3", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user3@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr4 = new DDDSample1.Domain.Users.User("User 4", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
                "Porto", "Portugal", "user4@gmail.com", "Password1?","description",
                "+351123456789" , "https://www.linkedin.com/in/id123", 
                "https://www.facebook.com/id123", new List<string>() {"tag1"});
            _usr5 =new DDDSample1.Domain.Users.User("User 5", "2000-1-1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL,
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
        public async void ReturnsEmptySetWhenDoesntFindStrengthData()
        {
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength()).Returns(Task.FromResult(new Dictionary<UserId, int>()));
            var result = await _service.GetNetworkStrengthLeaderboard();
            Assert.Empty(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnSetOfDTOs()
        {
            var users = _repoResult.Keys.ToList();
            var resultUsers = new List<DDDSample1.Domain.Users.User>() {_usr1, _usr2, _usr3, _usr4, _usr5};
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult(resultUsers));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Returns(Task.FromResult(_repoResult));
            var result = await _service.GetNetworkStrengthLeaderboard();
            Assert.IsType<SortedSet<UserLeaderboardDTO>>(result);
        }
        
        [Fact]
        public async void SuccessfullyReturnSetOfCount()
        {
            var users = _repoResult.Keys.ToList();
            var resultUsers = new List<DDDSample1.Domain.Users.User>() {_usr1, _usr2, _usr3, _usr4, _usr5};
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult(resultUsers));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Returns(Task.FromResult(_repoResult));
            var result = await _service.GetNetworkStrengthLeaderboard();
            Assert.Equal(5, result.Count);
        }
        
        [Fact]
        public async void SuccessfullyReturnExpectedSetOfDTOs()
        {
            var users = _repoResult.Keys.ToList();
            var resultUsers = new List<DDDSample1.Domain.Users.User>() {_usr1, _usr2, _usr3, _usr4, _usr5};
            _userRepo.Setup(x => x.GetByIdsAsync(users))
                .Returns(Task.FromResult(resultUsers));
            _connRepo.Setup(x => x.GetAllUsersNetworkStrength())
                .Returns(Task.FromResult(_repoResult));
            var result = await _service.GetNetworkStrengthLeaderboard();
            var res = result.Intersect(_result);
            Assert.Empty(res);
        }

    }
}