using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC25_Consultar_Sugestoes_Amigos
{
    public class TagCloudControllerServiceTest
    {
        private readonly Mock<IUserRepository> _userMock;
        private readonly Mock<IConnectionRepository> _mock;
        private readonly TagCloudController _controller;
        private readonly ITagService _service;
        private List<UserId> _idList;
        private List<string> _tagList;

        public TagCloudControllerServiceTest()
        {
            _mock = new Mock<IConnectionRepository>();
            _userMock = new Mock<IUserRepository>();
            _service = new TagService(_userMock.Object,_mock.Object);
            _controller = new TagCloudController(_service);
            InitIdList();
            InitTagList();
        }
        
        [Fact]
        public void ReturnNotFoundWhenTagCloudIsNotFound()
        {
            _mock.Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult<List<Connection>>(null));
            var result = _controller.GetAllConnectionsTagCloud().Result;
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ReturnOkFoundWhenTagCloudIsFound()
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
            var result = _controller.GetAllConnectionsTagCloud().Result;
            Assert.IsType<OkObjectResult>(result);
        }
      
        [Fact]
        public void ReturnExpectedTagCloudWhenTagCloudIsFound()
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
            var response = _controller.GetAllConnectionsTagCloud().Result as OkObjectResult;
            var expected = new List<TagCloudDTO>
            {
                new("tag1", 1),
                new("tag2", 2),
                new("tag3", 1),
            };
            var result = response?.Value as List<TagCloudDTO>;
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(result[i].value,expected[i].value);
                Assert.Equal(result[i].count,expected[i].count);
            }
        }

        [Fact]
        public void ReturnBadResponseWhenBusinessRuleValidationExceptionIsThrown()
        {
            _mock.Setup(x => x.GetAllAsync())
                .Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetAllConnectionsTagCloud().Result;
            Assert.IsType<BadRequestObjectResult>(result);
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