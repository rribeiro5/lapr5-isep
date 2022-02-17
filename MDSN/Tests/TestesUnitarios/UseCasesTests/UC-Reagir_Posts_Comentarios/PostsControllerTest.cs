using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Posts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC_Reagir_Posts_Comentarios
{
    public class PostsControllerTest
    {
        private readonly Mock<IPostsService> _mock;
        private readonly PostsController _controller;

        private Guid _userId;
        private CreatingReactionDTO _dto;

        private CreatingReactionResponseDTO _response;

        public PostsControllerTest()
        {
            _mock = new Mock<IPostsService>();
            _controller = new PostsController(_mock.Object);
            _userId = Guid.NewGuid();
            _dto = new CreatingReactionDTO(_userId.ToString(),new Guid(),"LIKE");
            _response = new CreatingReactionResponseDTO(_userId.ToString(),new Guid().ToString(),_userId.ToString(),false,"LIKE");
        }

        [Fact]
        public async void ReturnNotFoundWhenUserDoesntExist()
        {
            _mock.Setup(x =>
                x.updateReactionPost(_dto)).Returns(Task.FromResult<CreatingReactionResponseDTO>(null));
            var result = await _controller.updateReactionPost(_dto);
            Assert.IsType<NotFoundResult>(result);
        }   

        

        [Fact]
        public async void ReturnOkWhenUserExists()
        {
            _mock.Setup(x =>
                x.updateReactionPost(_dto)).Returns(Task.FromResult<CreatingReactionResponseDTO>(_response));
            var result = await _controller.updateReactionPost(_dto);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ReturnExpectedResultWhenUserExists()
        {
            _mock.Setup(x =>
                x.updateReactionPost(_dto)).Returns(Task.FromResult<CreatingReactionResponseDTO>(_response));
            var result = await _controller.updateReactionPost(_dto);
            var action = result as OkObjectResult;
            Assert.Equal<CreatingReactionResponseDTO>(_response, action.Value as CreatingReactionResponseDTO);
        }
    }
}