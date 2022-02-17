using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Domain.Connections
{
    [Owned]
    public class RelationshipStrength : IValueObject
    {

        public int Strength { get; private set; }
        
        private RelationshipStrength(){}

        public RelationshipStrength(int strength)
        {
            this.Strength = strength;
        }
    }
}