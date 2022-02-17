using System;
using DDDSample1.Domain.Connections;
using Xunit;

namespace Tests.TestesUnitarios.Connections
{
    public class ConnectionIdTest
    {
        [Fact]
        public void SuccessfullyCreateConnectionIdFromRandomGuid()
        {
            Guid id = Guid.NewGuid();
            ConnectionId connId = new ConnectionId(id);
            Assert.Equal(id, connId.AsGuid());
        }

        [Fact]
        public void SuccessfullyCreateConnectionIdFromStringGuid()
        {
            Guid id = new Guid("365e3536-1f4c-431a-bafe-2e44f73dc451");
            ConnectionId connId = new ConnectionId(id);
            Assert.Equal(id, connId.AsGuid());
        }

        [Fact]
        public void FailToCreateInvalidStringGuid()
        {
            Assert.Throws<FormatException>(() => new ConnectionId(new Guid("abc")));
        }
    }
}