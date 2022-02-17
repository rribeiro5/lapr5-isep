using System;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using Xunit;

namespace Tests.TestesUnitarios.Generics
{
    public class ConnectionStrengthTest
    {
        [Fact]
        public void SuccessfullyCreateConnectionStrengthValue1()
        {
            var strength = 1;
            ConnectionStrength c = new ConnectionStrength(strength);
            Assert.Equal(strength, c.Strength);
        }
        
        [Fact]
        public void SuccessfullyCreateConnectionStrengthValue100()
        {
            var strength = 100;
            ConnectionStrength c = new ConnectionStrength(strength);
            Assert.Equal(strength, c.Strength);
        }
        
        [Fact]
        public void SuccessfullyCreateConnectionStrengthRandomValue()
        {
            Random rnd = new Random();
            var strength = rnd.Next(1, 100);
            ConnectionStrength c = new ConnectionStrength(strength);
            Assert.Equal(strength, c.Strength);
        }
        
        [Fact]
        public void FailToCreateConnectionStrengthValue0()
        {
            var strength = 0;
            Assert.Throws<BusinessRuleValidationException>(() => new ConnectionStrength(strength));
        }
        
        [Fact]
        public void FailToCreateConnectionStrengthValue101()
        {
            var strength = 101;
            Assert.Throws<BusinessRuleValidationException>(() => new ConnectionStrength(strength));
        }

    }
}