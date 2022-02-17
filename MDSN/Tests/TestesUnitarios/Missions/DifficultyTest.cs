using System;
using DDDSample1.Domain.Mission;
using DDDSample1.Domain.Shared;
using Xunit;

namespace Tests.TestesUnitarios.Missions
{
    public class DifficultyTest
    {
        [Fact]
        public void SuccessfullyCreateDifficultyWithPositiveValue()
        {
            int difficulty = 1;
            Difficulty d = new Difficulty(difficulty);
            Assert.Equal(d.DifficultyValue, difficulty);
        }

        [Fact]
        public void SuccessfullyCreateDiffucultyWithHighPositiveValue()
        {
            int difficulty = 14;
            Difficulty d = new Difficulty(difficulty);
            Assert.Equal(d.DifficultyValue, difficulty);
        }
        [Fact]
         public void SuccessfullyCreateDiffucultyWithRamdomPositiveValue()
        {
            
            Random rnd = new Random();
            var difficulty = rnd.Next(1000, 10000);
            Difficulty d = new Difficulty(difficulty);
            Assert.Equal(d.DifficultyValue, difficulty);
        }

        [Fact]
        public void UnsuccessfullyCreateDiffucultyWithNullValue()
        {
            int difficulty = 0;
            Assert.Throws<BusinessRuleValidationException>(() => new Difficulty(difficulty));
        }
        [Fact]
        public void UnsuccessfullyCreateDiffucultyWithNegativeValue()
        {
            int difficulty = -5;
            
            Assert.Throws<BusinessRuleValidationException>(() => new Difficulty(difficulty));
        }
        [Fact]
         public void UnsuccessfullyCreateDiffucultyWithRamdomNegativeValue()
        {
            
            Random rnd = new Random();
            var difficulty = rnd.Next(-10000, -1);
            Assert.Throws<BusinessRuleValidationException>(() => new Difficulty(difficulty));
        }

    }
}