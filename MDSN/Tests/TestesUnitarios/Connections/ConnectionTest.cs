using System;
using System.Collections.Generic;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Shared;
using Xunit;

namespace Tests.TestesUnitarios.Connections
{
    public class ConnectionTest
    {
        private List<string> Tags { get; set; } = new List<string>();
        private const int connectionStrength = 5;
        private const int relationshipStrength = 5;
        private UserId oUser = new UserId(Guid.NewGuid());
        private UserId dUser = new UserId(Guid.NewGuid());
        public ConnectionTest()
        {
            Tags.Add("A");
            Tags.Add("B");
            Tags.Add("C");
        }

        [Fact]
        public void SuccessfullyCreateConnectionWithTags()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            Assert.Equal(connectionStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(relationshipStrength, conn.RelationshipStrength.Strength);
            Assert.Equal(oUser, conn.OUser);
            Assert.Equal(dUser, conn.DUser);
            Assert.Equal(Tags.Count, conn.Tags.Count);
            Assert.Equal(Tags, ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void SuccessfullyCreateConnectionWithoutTags()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, new List<string>(), oUser, dUser);
            Assert.Equal(connectionStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(relationshipStrength, conn.RelationshipStrength.Strength);
            Assert.Equal(oUser, conn.OUser);
            Assert.Equal(dUser, conn.DUser);
            Assert.Equal(0, conn.Tags.Count);
            Assert.Equal(new List<string>(), ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void SuccessfullyCreateConnectionWithNullListTags()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, null, oUser, dUser);
            Assert.Equal(connectionStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(relationshipStrength, conn.RelationshipStrength.Strength);
            Assert.Equal(oUser, conn.OUser);
            Assert.Equal(dUser, conn.DUser);
            Assert.Equal(0, conn.Tags.Count);
            Assert.Equal(new List<string>(), ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void SuccessfullyCreateConnectionNegativeRelationshipStrength()
        {
            int relStrength = -5;
            Connection conn = new Connection(connectionStrength, relStrength, Tags, oUser, dUser);
            Assert.Equal(connectionStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(relStrength, conn.RelationshipStrength.Strength);
            Assert.Equal(oUser, conn.OUser);
            Assert.Equal(dUser, conn.DUser);
            Assert.Equal(Tags.Count, conn.Tags.Count);
            Assert.Equal(Tags, ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void FailToCreateConnectionNegativeConnectionStrength()
        {
            int connStrength = -5;
            Assert.Throws<BusinessRuleValidationException>(() => new Connection(connStrength, relationshipStrength, Tags, oUser, dUser));
        }

        [Fact]
        public void FailToCreateConnectionConnectionStrengthValue105()
        {
            int connStrength = 105;
            Assert.Throws<BusinessRuleValidationException>(() => new Connection(connStrength, relationshipStrength, Tags, oUser, dUser));
        }

        [Fact]
        public void FailToCreateConnectionInvalidOUserId()
        {
            Assert.Throws<FormatException>(() => new Connection(connectionStrength, relationshipStrength, Tags, new UserId(new Guid("abc")), dUser));
        }

        [Fact]
        public void FailToCreateConnectionInvalidDUserId()
        {
            Assert.Throws<FormatException>(() => new Connection(connectionStrength, relationshipStrength, Tags, oUser, new UserId(new Guid("abc"))));
        }

        [Fact]
        public void FailToCreateConnectionEmptyTag()
        {
            List<string> tags = new List<string>();
            tags.Add("");
            Assert.Throws<BusinessRuleValidationException>(() => new Connection(connectionStrength, relationshipStrength, tags, oUser, dUser));
        }

        [Fact]
        public void FailToCreateConnectionNullTag()
        {
            List<string> tags = new List<string>();
            tags.Add(null);
            Assert.Throws<BusinessRuleValidationException>(() => new Connection(connectionStrength, relationshipStrength, tags, oUser, dUser));
        }

        [Fact]
        public void SuccessfullyUpdateConnStrengthAndWithoutTags()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = 50;
            List<string> tags = new List<string>();
            conn.UpdateConnStrengthTags(connStrength, tags);
            Assert.Equal(connStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(0, conn.Tags.Count);
            Assert.Equal(tags, ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void SuccessfullyUpdateConnStrengthAndTags()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = 100;
            List<string> tags = new List<string>();
            tags.Add("ABC");
            conn.UpdateConnStrengthTags(connStrength, tags);
            Assert.Equal(connStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(tags.Count, conn.Tags.Count);
            Assert.Equal(tags, ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void SuccessfullyUpdateConnStrengthAndNullListTags()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = 100;
            conn.UpdateConnStrengthTags(connStrength, null);
            Assert.Equal(connStrength, conn.ConnectionStrength.Strength);
            Assert.Equal(0, conn.Tags.Count);
            Assert.Equal(new List<string>(), ConvertTagsToString(conn.Tags));
        }

        [Fact]
        public void FailToUpdateConnStrengthTagsNegativeValue()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = -5;
            List<string> tags = new List<string>();
            Assert.Throws<BusinessRuleValidationException>(() => conn.UpdateConnStrengthTags(connStrength, tags));
        }

        [Fact]
        public void FailToUpdateConnStrengthTagsValue105()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = 105;
            List<string> tags = new List<string>();
            Assert.Throws<BusinessRuleValidationException>(() => conn.UpdateConnStrengthTags(connStrength, tags));
        }

        [Fact]
        public void FailToUpdateConnStrengthTagsEmptyTag()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = 5;
            List<string> tags = new List<string>();
            tags.Add("");
            Assert.Throws<BusinessRuleValidationException>(() => conn.UpdateConnStrengthTags(connStrength, tags));
        }

        [Fact]
        public void FailToUpdateConnStrengthTagsNullTag()
        {
            Connection conn = new Connection(connectionStrength, relationshipStrength, Tags, oUser, dUser);
            int connStrength = 5;
            List<string> tags = new List<string>();
            tags.Add(null);
            Assert.Throws<BusinessRuleValidationException>(() => conn.UpdateConnStrengthTags(connStrength, tags));
        }

        private List<string> ConvertTagsToString(ICollection<Tag> tags)
        {
            List<string> list = new List<string>();
            foreach (var item in tags)
            {
                list.Add(item.Description);
            }
            return list;
        }
    }
}