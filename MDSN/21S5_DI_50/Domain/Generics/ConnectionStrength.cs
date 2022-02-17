using Microsoft.EntityFrameworkCore;
using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Connections
{
    [Owned]
    public class ConnectionStrength : IValueObject
    {
        //unidirecional

        public const int MIN = 0;
        
        public const int MAX = 100;
        public int Strength { get; private set; }
        
        
        private ConnectionStrength(){}

        public ConnectionStrength(int strength)
        {
            if (strength <= MIN || strength > MAX)
                throw new BusinessRuleValidationException("Connection Strength value must be between 1 and 100");
            this.Strength = strength;
        }
        
    }
}