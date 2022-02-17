using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC22a_Consultar_leaderboard_dimensao
{
    public class LeaderboardServiceTest
    {
        private readonly Mock<IUserRepository> _usersMock;
        private readonly Mock<IConnectionRepository> _connsMock;
        private readonly ILeaderboardService _service;
        private List<DDDSample1.Domain.Users.User> _users;

        public LeaderboardServiceTest()
        {
            _usersMock = new Mock<IUserRepository>();
            _connsMock = new Mock<IConnectionRepository>();
            _service = new LeaderboardService(_usersMock.Object, _connsMock.Object);
            InitUsersArray();
        }

        [Fact]
        public void SuccessfullyReturnLeaderboardListWith5Users()
        {
            _usersMock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(_users));
            MatchUserWithFriends(_users[0],_users[1],_users[2],_users[3],_users[4]);
            MatchUserWithFriends(_users[1],_users[0],_users[2],_users[3]);
            MatchUserWithFriends(_users[2],_users[0],_users[3]);
            MatchUserWithFriends(_users[3],_users[0]);
            MatchUserWithFriends(_users[4],_users[0]);

            var expected = 5;
            var result = _service.GetLeaderboardDimensionCriteria().Result;
            Assert.Equal(expected,result.Count);
        }
        
        [Fact]
        public void SuccessfullyReturnLeaderboardListWith5UsersAllWithTheSamePoints()
        {
            _usersMock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(_users));
            MatchUserWithFriends(_users[0]);
            MatchUserWithFriends(_users[1]);
            MatchUserWithFriends(_users[2]);
            MatchUserWithFriends(_users[3]);
            MatchUserWithFriends(_users[4]);

            var expected = 5;
            var result = _service.GetLeaderboardDimensionCriteria().Result;
            Assert.Equal(expected,result.Count);
        }
        
        [Fact]
        public void SuccessfullyReturnLeaderboardListWithTheExpectedUsers()
        {
            _usersMock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(_users));
            MatchUserWithFriends(_users[0],_users[1],_users[2],_users[3],_users[4]);
            MatchUserWithFriends(_users[1],_users[0],_users[2],_users[3]);
            MatchUserWithFriends(_users[2],_users[0],_users[3]);
            MatchUserWithFriends(_users[3],_users[0]);
            MatchUserWithFriends(_users[4],_users[0]);
            
            var expected= new List<UserLeaderboardDTO>
            {
                _users[0].ToUserLeaderboardDTO(4),
                _users[1].ToUserLeaderboardDTO(3),
                _users[2].ToUserLeaderboardDTO(2),
                _users[3].ToUserLeaderboardDTO(1),
                _users[4].ToUserLeaderboardDTO(1),
            };
            var result = _service.GetLeaderboardDimensionCriteria().Result;

            var i = 0;
            foreach (var user in result)
            {
                Assert.Equal(user.Id, expected[i++].Id);
            }
        }

        private void MatchUserWithFriends(DDDSample1.Domain.Users.User u, params DDDSample1.Domain.Users.User[] users)
        {
            var set = new HashSet<UserId>();
            foreach (var friend in users)
            {
                set.Add(friend.Id);
            }
            _usersMock.Setup(x => x.GetUserFriends(u.Id))
                .Returns(Task.FromResult(set));
        }
        
        private void InitUsersArray()
        {
            var u1 = new DDDSample1.Domain.Users.User("common 1", "2000-01-01", null, null, "common1@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u2 = new DDDSample1.Domain.Users.User("common 2", "2000-01-01", null, null, "common2@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u3 = new DDDSample1.Domain.Users.User("common 3", "2000-01-01", null, null, "common3@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u4 = new DDDSample1.Domain.Users.User("common 4", "2000-01-01", null, null, "common4@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
            var u5 = new DDDSample1.Domain.Users.User("common 5", "2000-01-01", null, null, "common5@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});

            _users = new List<DDDSample1.Domain.Users.User> {u1, u2, u3, u4, u5};
        }
    }
}