using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC25_Consultar_Tag_clouds_relacoes
{
    public class TagServiceTest
    {
        private Mock<IUserRepository> _userMock;
        private Mock<IConnectionRepository> _mock;
        private ITagService _service;
        private List<UserId> _idList;
        private List<string> _tagList;

        public TagServiceTest()
        {
            _mock = new Mock<IConnectionRepository>();
            _userMock = new Mock<IUserRepository>();
            _service = new TagService(_userMock.Object,_mock.Object);
            InitIdList();
            InitTagList();
        }
        
        [Fact]
        public void SuccessfullyGetListOfTagClouds()
        {

            var u1 = _idList[0];
            var u2 = _idList[1];
            var u3 = _idList[2];
            var tags1 = new List<string> {"tag1", "tag2"};
            var conn1 = new Connection(1, 1, tags1, u1, u2);
            var tags2 = new List<string> {"tag2", "tag3"};
            var conn2 = new Connection(1, 1, tags2, u1, u3);
            var connList = new List<Connection> {conn1, conn2};
            _mock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(connList));
            var result = _service.GetAllConnectionsTagCloud().Result;
            var dto1 = new TagCloudDTO(tags1[0], 1);
            var dto2 = new TagCloudDTO(tags1[1], 2);
            var dto3 = new TagCloudDTO(tags2[1], 1);
            var expected = new List<TagCloudDTO>{dto1,dto2,dto3};

            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i].value,expected[i].value);
                Assert.Equal(result[i].count,expected[i].count);
            }
        }

        [Fact]
        public void SuccessfullyReturnNullWhenListIsNull()
        {
            _mock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult<List<Connection>>(null));
            var result = _service.GetAllConnectionsTagCloud().Result;
            Assert.Null(result);
        }

        
        private void InitIdList()
        {
            const int numUsers = 5;
            _idList = new List<UserId>(numUsers);
            for (var i = 0; i < numUsers; i++)
            {
                _idList.Add(new UserId(new Guid()));
            }
        }

        private void InitTagList()
        {
            const int numTags = 3;
            _tagList = new List<string>(numTags)
            {
                "tag1",
                "tag2",
                "tag3"
            };
        }
    }
}