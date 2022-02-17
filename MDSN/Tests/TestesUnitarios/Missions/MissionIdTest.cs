using System;
using DDDSample1.Domain.Mission;
using Xunit;

namespace Tests.TestesUnitarios.Missions
{
    public class MissionIdTest
    {
        [Fact]
        public void SuccessfullyCreateMissionIdFromRandomGuid()
        {
            Guid id = Guid.NewGuid();
            MissionId missionId = new MissionId(id);
            Assert.Equal(id.ToString(), missionId.AsString());
        }

        [Fact]
        public void SuccessfullyCreateMissionIdFromStringGuid()
        {
            Guid id = new Guid("365e3536-1f4c-431a-bafe-2e44f73dc451");
            MissionId missionId = new MissionId(id);
            Assert.Equal(id.ToString(), missionId.AsString());
        }

        [Fact]
        public void FailToCreateInvalidStringGuid()
        {
            Assert.Throws<FormatException>(() => new MissionId(new Guid("abc")));
        }
    }
}