using System;
using DDDSample1.Domain.Mission;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;
using Xunit.Abstractions;

namespace Tests.TestesUnitarios.Missions
{
    public class MissionTest
    {   

        private readonly ITestOutputHelper _testOutputHelper;
        private int _difficulty;

        private UserId _player;

        private UserId _objective;


        public MissionTest(ITestOutputHelper outputHelper){
            _testOutputHelper = outputHelper;
            _difficulty = 5;
            _player = new UserId(new Guid());
            _objective = new UserId(new Guid());
        }

        [Fact]
        public void SuccessfullyCreateMissionWithPositiveValue()
        {
            var mission = CreateMission();
            Assert.True(mission != null);
        }

        [Fact]
        public void SuccessfullyCreateMissionWithRandomPositiveValue()
        {   

            Random rnd = new Random();
            var difficulty = rnd.Next(1000, 10000);
            _difficulty = difficulty;
            var mission = CreateMission();
            Assert.True(mission != null);
        }

        [Fact]
        public void UnsuccessfullyCreateDiffucultyWithNullValue()
        {   
            _difficulty = 0;
            Assert.Throws<BusinessRuleValidationException>(() => new Difficulty(_difficulty));
        }

        [Fact]
        public void UnsuccessfullyCreateDiffucultyWithRandomNegativeValue()
        {   
            Random rnd = new Random();
            var difficulty = rnd.Next(-1000, -1);
            _difficulty = difficulty;
            Assert.Throws<BusinessRuleValidationException>(() => new Difficulty(_difficulty));
        }

        private Mission CreateMission()=>new Mission(_difficulty,_player,_objective);

    }
}