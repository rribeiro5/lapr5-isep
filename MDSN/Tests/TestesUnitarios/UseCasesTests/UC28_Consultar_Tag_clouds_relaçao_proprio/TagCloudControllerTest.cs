using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Tests.TestesUnitarios.UseCasesTests.UC28_Consultar_Tag_clouds_relaçao_proprio
{
    public class TagCloudControllerTest
    {
        private Mock<ITagService> _mock;
        private readonly TagCloudController _controller;

        public TagCloudControllerTest(ITestOutputHelper testOutputHelper)
        {
            _mock = new Mock<ITagService>();
            _controller = new TagCloudController(_mock.Object);
        }
        [Fact]
        public void ReturnNotFoundWhenTagCloudIsNotFound()
        {
            _mock.Setup(x => x.GetAllUserConnectionsTagCloud(It.IsAny<Guid>())).Returns(Task.FromResult<List<TagCloudDTO>>(null));
            var result = _controller.GetAllUserConnectionsTagCloud(It.IsAny<Guid>()).Result;
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void ReturnOkFoundWhenTagCloudIsFound()
        {
            _mock.Setup(x => x.GetAllUserConnectionsTagCloud(It.IsAny<Guid>())).Returns(Task.FromResult(new List<TagCloudDTO>()));
            var result = _controller.GetAllUserConnectionsTagCloud(It.IsAny<Guid>()).Result;
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedTagCloudWhenTagCloudIsFound()
        {
            var cloudDto1 = new TagCloudDTO("tag1", 1);
            var cloudDto2 = new TagCloudDTO("tag2", 2);
            var expected = new List<TagCloudDTO>{cloudDto1,cloudDto2};
            _mock.Setup(x => x.GetAllUserConnectionsTagCloud(It.IsAny<Guid>()))
                .Returns(Task.FromResult(expected));
            var response = _controller.GetAllUserConnectionsTagCloud(It.IsAny<Guid>()).Result as OkObjectResult;
            var result = response?.Value;
            Assert.Equal(expected,result);
        }

        [Fact]
        public void ReturnBadResponseWhenBusinessRuleValidationExceptionIsThrown()
        {
            _mock.Setup(x => x.GetAllUserConnectionsTagCloud(It.IsAny<Guid>()))
                .Throws(new BusinessRuleValidationException(""));
            var result = _controller.GetAllUserConnectionsTagCloud(It.IsAny<Guid>()).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}