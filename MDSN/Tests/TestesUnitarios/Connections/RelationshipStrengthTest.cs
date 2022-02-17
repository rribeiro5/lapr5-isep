using System;
using DDDSample1.Domain.Connections;
using Xunit;

namespace Tests.TestesUnitarios.Connections
{
    public class RelationshipStrengthTest
    {
        [Fact]
        public void SuccessfullyCreateRelationshipStrengthValue0()
        {
            int strength = 0;
            RelationshipStrength s = new RelationshipStrength(strength);
            Assert.Equal(s.Strength, strength);
        }

        [Fact]
        public void SuccessfullyCreateRelationshipStrengthPositiveValue()
        {
            int strength = 5;
            RelationshipStrength s = new RelationshipStrength(strength);
            Assert.Equal(s.Strength, strength);
        }

        [Fact]
        public void SuccessfullyCreateRelationshipStrengthNegativeValue()
        {
            int strength = -5;
            RelationshipStrength s = new RelationshipStrength(strength);
            Assert.Equal(s.Strength, strength);
        }

        [Fact]
        public void SuccessfullyCreateRelationshipStrengthRandomValue()
        {
            Random rand = new Random();
            int strength = rand.Next();
            RelationshipStrength s = new RelationshipStrength(strength);
            Assert.Equal(s.Strength, strength);
        }
    }
}