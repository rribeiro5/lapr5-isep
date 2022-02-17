using System;
using DDDSample1.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace DDDSample1.Domain.Mission
{
    [Owned]
    public class MissionDateTime : IValueObject
    {
        public DateTime DateTime { get; private set; }

        public MissionDateTime()
        {
            DateTime = DateTime.Now;
        }
    }
}