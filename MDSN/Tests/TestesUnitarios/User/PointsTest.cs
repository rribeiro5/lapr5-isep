using System;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Xunit;

namespace Tests.TestesUnitarios.User
{
    public class PointsTest
    {
        [Fact]
        public void SuccessfullyCreatePointsValue0()
        {
            var points = 0;
            Points p = new Points(points);
            Assert.True(p._Points == points);
        }
        
        [Fact]
        public void SuccessfullyCreatePointsValue100()
        {
            var points = 100;
            Points p = new Points(points);
            Assert.True(p._Points == points);
        }
        
        [Fact]
        public void SuccessfullyCreatePointsGreaterRandomValue()
        {
            Random rnd = new Random();
            var points = rnd.Next(1000, 100000);
            Points p = new Points(points);
            Assert.True(p._Points == points);
        }
        
        [Fact]
        public void FailToCreatePointsNegativeValue()
        {
            var point = -1;
            Assert.Throws<BusinessRuleValidationException>(() => new Points(point));
        }
        
        [Fact]
        public void FailToCreatePointsRandomNegativeValues()
        {
            Random rnd = new Random();
            var points = rnd.Next(-100000, -1);
            Assert.Throws<BusinessRuleValidationException>(() => new Points(points));
        }

    }
}