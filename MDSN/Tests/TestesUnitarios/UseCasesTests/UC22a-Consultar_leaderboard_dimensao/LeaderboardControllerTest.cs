using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC22a_Consultar_leaderboard_dimensao
{
    public class LeaderboardControllerTest
    {
        private readonly Mock<ILeaderboardService> _mock;
        private readonly LeaderboardController _controller;

        public LeaderboardControllerTest()
        {
            _mock = new Mock<ILeaderboardService>();
            _controller = new LeaderboardController(_mock.Object);
        }
        
        [Fact]
        public void ReturnNotFoundWhenNetworkIsNull()
        {
            _mock.Setup(x => x.GetLeaderboardDimensionCriteria()
            ).Returns(Task.FromResult<List<UserLeaderboardDTO>>(null));
            var result = _controller.GetLeaderboardDimensionCriteria().Result;
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void ReturnOkResultWhenNetworkIsNotNull()
        {
            var l = new List<UserLeaderboardDTO>();
            _mock.Setup(x => x.GetLeaderboardDimensionCriteria()
            ).Returns(Task.FromResult(l));
            var result = _controller.GetLeaderboardDimensionCriteria().Result;
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedResultWhenNetworkResultIsOk()
        {
            var l = new List<UserLeaderboardDTO>
            {
                new (new Guid(),"e1","n1","a1",10),
                new (new Guid(),"e2","n2","a2",8),
                new (new Guid(),"e3","n3","a3",7),
                new (new Guid(),"e4","n4","a4",5)
            };
            _mock.Setup(x => x.GetLeaderboardDimensionCriteria()
            ).Returns(Task.FromResult(l));
            var result = (_controller.GetLeaderboardDimensionCriteria().Result as OkObjectResult)
                .Value as List<UserLeaderboardDTO>;
            Assert.Equal(l,result);
        }

        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            _mock.Setup(x => x.GetLeaderboardDimensionCriteria()
            ).Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetLeaderboardDimensionCriteria().Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}