using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Posts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC13_Fazer_Post
{
    public class PostsControllerServiceTest
    {
        private Mock<IUnitOfWork> unit;
        private Mock<IMasterDataPostsHttpClient> http;
        private Mock<IUserRepository> userRepo;
        private Mock<IConnectionRepository> connRepo;
        private IPostsService service;
        private PostsController controller;
        private string postId;
        private Guid userId;
        private string text;
        private CreatePostRequestDTO request;
        private CreatePostResponseDTO response;
        private User user;

        public PostsControllerServiceTest()
        {
            unit = new Mock<IUnitOfWork>();
            http = new Mock<IMasterDataPostsHttpClient>();
            userRepo = new Mock<IUserRepository>();
            connRepo = new Mock<IConnectionRepository>();
            service = new PostsService(unit.Object, http.Object, userRepo.Object, connRepo.Object,null);
            controller = new PostsController(service);
            postId = "1";
            userId = new Guid();
            text = "sample text";
            request = new CreatePostRequestDTO(userId, text, new List<string>());
            response = new CreatePostResponseDTO(userId, text, new List<string>(), 1L, new List<ReactionDTO>(),
                new List<CommentDTO>());
            user = new User("Abc", "2000/10/10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", new List<string>(){"tag"});
        }
        
        [Fact]
        public void ReturnSuccessCodeWithValidCommentData()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = JsonContent.Create(response);
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            http.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Returns(Task.FromResult(response));
            var result = controller.CreatePost(request).Result;
            Assert.IsType<CreatedResult>(result);
        }
     
        [Fact]
        public void ReturnExpectedDtoWithValidCommentData()
        {
            var httpResponse = new HttpResponseMessage(HttpStatusCode.Created);
            httpResponse.Content = JsonContent.Create(response);
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            http.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Returns(Task.FromResult(httpResponse));
            var result = (controller.CreatePost(request).Result as CreatedResult)
                .Value as CreatePostResponseDTO;
            Assert.Equal(response.id,result.id);
            Assert.Equal(response.creationDateTime,result.creationDateTime);
            Assert.Equal(response.comments,result.comments);
            Assert.Equal(response.reactions,result.reactions);
            Assert.Equal(response.reactions.Count,result.reactions.Count);
            Assert.Equal(response.text,result.text);
            Assert.Equal(response.userId,result.userId);
        }
           
         [Fact]
         public void ReturnNotFoundWhenUserDoesntExist()
         {
             userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                 .Returns(Task.FromResult<User>(null));
             var result = controller.CreatePost(request).Result;
             Assert.IsType<NotFoundObjectResult>(result);
         }
         
       [Fact]
       public void ReturnBadRequestWhenFormatExceptionIsThrown()
       {
           userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
               .Throws(new FormatException(""));
           var result = controller.CreatePost(request).Result;
           Assert.IsType<BadRequestObjectResult>(result);
       }

    }
}