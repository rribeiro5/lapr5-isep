using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC13_Fazer_Post
{
    public class PostsControllerTest
    {

        private Mock<IPostsService> mock;
        private PostsController controller;
        private string postId;
        private Guid userId;
        private string text;
        private CreatePostRequestDTO request;
        private CreatePostResponseDTO response;


        public PostsControllerTest()
        {
            mock = new Mock<IPostsService>();
            controller = new PostsController(mock.Object);
            postId = "1";
            userId = new Guid();
            text = "sample text";
            request = new CreatePostRequestDTO(userId, text, new List<string>());
            response = new CreatePostResponseDTO(userId, text, new List<string>(), 1L, new List<ReactionDTO>(),
                new List<CommentDTO>());
        }
        
        [Fact]
        public void ReturnSuccessCodeWithValidPostData()
        {
            mock.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Returns(Task.FromResult(response));
            var result = controller.CreatePost(request).Result;
            Assert.IsType<CreatedResult>(result);
        }
        
        [Fact]
        public void ReturnExpectedDtoWithValidCommentData()
        {
            mock.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Returns(Task.FromResult(response));
            var result = (controller.CreatePost(request).Result as CreatedResult)
                .Value as CreatePostResponseDTO;
            Assert.Equal(response,result);
        }
        
        [Fact]
        public void ReturnNotFoundWithNullResponse()
        {
            mock.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Returns(Task.FromResult<CreatePostResponseDTO>(null));
            var result = controller.CreatePost(request).Result;
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        [Fact]
        public void ReturnBadRequestWhenBusinessRuleValidationExceptionIsThrown()
        {
            mock.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Throws(new BusinessRuleValidationException(""));
            var result = controller.CreatePost(request).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        [Fact]
        public void ReturnBadRequestWhenFormatExceptionIsThrown()
        {
            mock.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Throws(new FormatException(""));
            var result = controller.CreatePost(request).Result;
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}