using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC14_Comentar_o_Post
{
    public class PostsControllerTest
    {
        private Mock<IPostsService> mock;
        private PostsController controller;
        private Guid commentId;
        private string postId;
        private string userId;
        private string text;
        private CreatingCommentDTO request;
        private CreatingCommentResponseDTO response;
        
        public PostsControllerTest()
        {
            mock = new Mock<IPostsService>();
            controller = new PostsController(mock.Object);
            commentId = new Guid();
            postId = "1";
            userId = "2";
            text = "sample text";
            response = new CreatingCommentResponseDTO(commentId,postId, userId, text,new List<ReactionDTO>(),1);
        }

        [Fact]
        public void ReturnSuccessCodeWithValidCommentData()
        {
            mock.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Returns(Task.FromResult(response));
            var result = controller.CreateComment(request).Result;
            Assert.IsType<CreatedResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedDtoWithValidCommentData()
        {
            mock.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Returns(Task.FromResult(response));
            var result = (controller.CreateComment(request).Result as CreatedResult)
                .Value as CreatingCommentResponseDTO;
            Assert.Equal(response,result);
        }
        
        [Fact]
        public void ReturnNotFoundWithNullResponse()
        {
            mock.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Returns(Task.FromResult<CreatingCommentResponseDTO>(null));
            var result = controller.CreateComment(request).Result;
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            mock.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Throws(new BusinessRuleValidationException(""));
            var result = controller.CreateComment(request).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public void ReturnBadRequestWhenFormatExceptionIsThrown()
        {
            mock.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Throws(new FormatException(""));
            var result = controller.CreateComment(request).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}