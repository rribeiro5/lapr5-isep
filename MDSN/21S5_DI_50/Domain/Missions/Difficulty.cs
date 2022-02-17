using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Domain.Mission
{
    [Owned]
    public class Difficulty : IValueObject
    {
        public int DifficultyValue { get; private set;}

        public Difficulty()
        {
        }

        public Difficulty(int difficultyValue)
        {
            if (difficultyValue < 1)
                throw new BusinessRuleValidationException("Difficulty value has to be greater than 0");
            this.DifficultyValue = difficultyValue;
        }
    }
}