using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Posts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UCFeedPosts
{
    public class PostsControllerTest
    {
        private readonly Mock<IPostsService> _mock;
        private readonly PostsController _controller;

        private Guid _userId;
        private List<PostDTO> _list;

        public PostsControllerTest()
        {
            _mock = new Mock<IPostsService>();
            _controller = new PostsController(_mock.Object);
            _userId = Guid.NewGuid();
            _list = new List<PostDTO>();
            _list.Add(new PostDTO(_userId, "a", new List<string>(), new List<CommentDTO>(), new List<ReactionDTO>(), 1));
            _list.Add(new PostDTO(_userId, "b", new List<string>(), new List<CommentDTO>(), new List<ReactionDTO>(), 2));
            _list.Add(new PostDTO(_userId, "c", new List<string>(), new List<CommentDTO>(), new List<ReactionDTO>(), 3));
        }

        [Fact]
        public async void ReturnNotFoundWhenUserDoesntExist()
        {
            _mock.Setup(x =>
                x.FeedPosts(_userId)).Returns(Task.FromResult<List<PostDTO>>(null));
            var result = await _controller.FeedPosts(_userId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void ReturnOkWhenUserExists()
        {
            _mock.Setup(x =>
                x.FeedPosts(_userId)).Returns(Task.FromResult<List<PostDTO>>(_list));
            var result = await _controller.FeedPosts(_userId);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void ReturnExpectedResultWhenUserExists()
        {
            _mock.Setup(x =>
                x.FeedPosts(_userId)).Returns(Task.FromResult<List<PostDTO>>(_list));
            var result = await _controller.FeedPosts(_userId);
            var action = result as OkObjectResult;
            Assert.Equal<PostDTO>(_list, action.Value as List<PostDTO>);
        }
    }
}