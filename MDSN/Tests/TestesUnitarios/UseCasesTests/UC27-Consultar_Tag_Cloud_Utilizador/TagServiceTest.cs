using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC27_Consultar_Tag_Cloud_Utilizador
{
    public class TagServiceTest
    {
        private readonly Mock<IUserRepository> _userMock;
        private readonly Mock<IConnectionRepository> _mock;
        private ITagService _service;
        private Dictionary<Tag, int> _repoResult;
        private List<TagCloudDTO> _result;
        private UserId _userId;

        public TagServiceTest()
        {
            _mock = new Mock<IConnectionRepository>();
            _userMock = new Mock<IUserRepository>();
            _service = new TagService(_userMock.Object,_mock.Object);
            _userId = new UserId(Guid.NewGuid());
            _repoResult = new Dictionary<Tag, int>();
            _repoResult = new Dictionary<Tag, int>();
            _repoResult.Add(new Tag("tag1"),5);
            _repoResult.Add(new Tag("tag2"),13);
            _repoResult.Add(new Tag("tag3"),13);
            _repoResult.Add(new Tag("tag4"),1);
            _result = new List<TagCloudDTO>()
            {
                new("tag1", 5),
                new("tag2", 13),
                new("tag3", 13),
                new("tag4", 1),
            };
        }
        
        [Fact]
        public async void SuccessfullyGetListOfTagClouds()
        { 
            _userMock.Setup(x => x.GetUserTagCloud(_userId))
                .Returns(Task.FromResult(_repoResult)); 
            var result = await _service.GetUserTagCloud(_userId.AsGuid()); 
            Assert.Empty(_result.Intersect(result));
        }

        [Fact]
        public async void ReturnEmptyList()
        {
            _userMock.Setup(x => x.GetUserTagCloud(_userId)).Returns(Task.FromResult(new Dictionary<Tag, int>()));
            var result = await _service.GetUserTagCloud(_userId.AsGuid());
            Assert.Empty(result);
        }

        [Fact]
        public async void ReturnListOfCount()
        {
            _userMock.Setup(x => x.GetUserTagCloud(_userId)).Returns(Task.FromResult(_repoResult));
            var result = await _service.GetUserTagCloud(_userId.AsGuid());
            Assert.Equal(4, result.Count);
        }

        
    }
}