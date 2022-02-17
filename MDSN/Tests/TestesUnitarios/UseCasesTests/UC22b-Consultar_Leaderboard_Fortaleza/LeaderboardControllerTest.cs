using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC22b_Consultar_Leaderboard_Fortaleza
{
    public class LeaderboardControllerTest
    {
        private readonly Mock<ILeaderboardService> _mock;
        private readonly Mock<IUserService> _usrmock;
        private readonly LeaderboardController _controller;
        private SortedSet<UserLeaderboardDTO> _result;

        public LeaderboardControllerTest()
        {
            var comparer = Comparer<UserLeaderboardDTO>.Create(
                (x,y) => y.value.CompareTo(x.value)
            );
            _result = new SortedSet<UserLeaderboardDTO>(comparer)
            {
                new (Guid.NewGuid(), "usr1@email.com", "User 1", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL, 40),
                new (Guid.NewGuid(), "usr2@email.com", "User 2", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL, 77),
                new (Guid.NewGuid(), "usr5@email.com", "User 5", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL, 77),
                new (Guid.NewGuid(), "usr3@email.com", "User 3", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL, 53),
                new (Guid.NewGuid(), "usr4@email.com", "User 4", DDDSample1.Domain.Users.User.DEFAULT_AVATAR_URL, 22),
            };
            _mock = new Mock<ILeaderboardService>();
            _usrmock = new Mock<IUserService>();
            _controller = new LeaderboardController(_mock.Object);
        }

        [Fact]
        public async void ReturnBadRequestResultWhenBusinessRuleValidationException()
        {
            _mock.Setup(x => x.GetNetworkStrengthLeaderboard())
                .Throws(new BusinessRuleValidationException("test"));
                
            var result = await _controller.GetNetworkStrengthLeaderboard();
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async void ReturnNotFoundWhenNull()
        {
            _mock.Setup(x => x.GetNetworkStrengthLeaderboard())
                .Returns(Task.FromResult<SortedSet<UserLeaderboardDTO>>(null));
                
            var result = await _controller.GetNetworkStrengthLeaderboard();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void ReturnOkResult()
        {
            
            _mock.Setup(x => x.GetNetworkStrengthLeaderboard())
                .Returns(Task.FromResult(_result));
                
            var result = await _controller.GetNetworkStrengthLeaderboard();
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnExpectedResult()
        {
            _mock.Setup(x => x.GetNetworkStrengthLeaderboard())
                .Returns(Task.FromResult(_result));
                
            var result = await _controller.GetNetworkStrengthLeaderboard() as OkObjectResult;
            var obtained = (SortedSet<UserLeaderboardDTO>) result?.Value;
            
            Assert.StrictEqual(_result,obtained);
        }
    }
}