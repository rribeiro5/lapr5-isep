using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Users;

namespace Tests.TestesUnitarios.UseCasesTests.UC27_Consultar_Tag_Cloud_Utilizador
{
    public class TagCloudControllerTest
    {
        private Mock<ITagService> _mock;
        private TagCloudController _controller;
        private Dictionary<Tag, int> _repoResult;
        private List<TagCloudDTO> _result;
        private UserId _userId;

        public TagCloudControllerTest()
        {
            _mock = new Mock<ITagService>();
            _controller = new TagCloudController(_mock.Object);
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
        public async void ReturnBadRequestResulWhenThrowsException()
        {
            _mock.Setup(x => x.GetUserTagCloud(_userId.AsGuid())).Throws(new BusinessRuleValidationException("test"));
            var obtained = await _controller.GetUserTagCloud(_userId.AsGuid());
            Assert.IsType<BadRequestObjectResult>(obtained);
        }
        
        [Fact]
        public async void ReturnOkResult()
        {
            _mock.Setup(x => x.GetUserTagCloud(_userId.AsGuid())).Returns(Task.FromResult(_result));
            var obtained = await _controller.GetUserTagCloud(_userId.AsGuid());
            Assert.IsType<OkObjectResult>(obtained);
        }
        
        [Fact]
        public async void ReturnOkResultWithExpectedList()
        {
            _mock.Setup(x => x.GetUserTagCloud(_userId.AsGuid())).Returns(Task.FromResult(_result));
            var obtained = await _controller.GetUserTagCloud(_userId.AsGuid());
            var okResult = obtained as OkObjectResult;
            var obj = (List<TagCloudDTO>)okResult?.Value;
            Assert.StrictEqual(_result,obj);
        }
        
        [Fact]
        public async void ReturnOkResultWithExpectedCount()
        {
            _mock.Setup(x => x.GetUserTagCloud(_userId.AsGuid())).Returns(Task.FromResult(_result));
            var obtained = await _controller.GetUserTagCloud(_userId.AsGuid());
            var okResult = obtained as OkObjectResult;
            var obj = (List<TagCloudDTO>)okResult?.Value;
            Assert.Equal(4, obj.Count);
        }
    }
}