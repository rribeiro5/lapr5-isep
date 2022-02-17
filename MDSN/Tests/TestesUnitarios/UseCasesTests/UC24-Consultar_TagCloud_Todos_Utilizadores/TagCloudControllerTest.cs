using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC24_Consultar_TagCloud_Todos_Utilizadores
{
    public class TagCloudControllerTest
    {
        private readonly Mock<ITagService> _mock;
        private readonly TagCloudController _controller;
        private List<TagCloudDTO> _result;

        public TagCloudControllerTest()
        {
            _mock = new Mock<ITagService>();
            _controller = new TagCloudController(_mock.Object);
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
            _mock.Setup(x => x.GetUsersTagCloud()).Throws(new BusinessRuleValidationException("test"));
            var obtained = await _controller.GetUsersTagCloud();
            Assert.IsType<BadRequestObjectResult>(obtained);
        }
        
        [Fact]
        public async void ReturnOkResult()
        {
            _mock.Setup(x => x.GetUsersTagCloud()).Returns(Task.FromResult<List<TagCloudDTO>>(_result));
            var obtained = await _controller.GetUsersTagCloud();
            Assert.IsType<OkObjectResult>(obtained);
        }
        
        [Fact]
        public async void ReturnOkResultWithExpectedList()
        {
            _mock.Setup(x => x.GetUsersTagCloud()).Returns(Task.FromResult(_result));
            var obtained = await _controller.GetUsersTagCloud();
            var okResult = obtained as OkObjectResult;
            var obj = (List<TagCloudDTO>)okResult?.Value;
            Assert.StrictEqual(_result,obj);
        }
        
        [Fact]
        public async void ReturnOkResultWithExpectedCount()
        {
            _mock.Setup(x => x.GetUsersTagCloud()).Returns(Task.FromResult<List<TagCloudDTO>>(_result));
            var obtained = await _controller.GetUsersTagCloud();
            var okResult = obtained as OkObjectResult;
            var obj = (List<TagCloudDTO>)okResult?.Value;
            Assert.Equal(4, obj.Count);
        }

    }
}