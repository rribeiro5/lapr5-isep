using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Posts;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC14_Comentar_o_Post
{
    public class PostsServiceTest
    {
        private Mock<IUnitOfWork> unit;
        private Mock<IMasterDataPostsHttpClient> http;
        private Mock<IUserRepository> userRepo;
        private Mock<IConnectionRepository> connRepo;
        private IPostsService service;

        private Guid commentId;
        private string postId;
        private UserId userId;
        private string text;
        private CreatingCommentDTO requestJson;
        private CreatingCommentResponseDTO responseJson;
        private DDDSample1.Domain.Users.User user;

        public PostsServiceTest()
        {
            unit = new Mock<IUnitOfWork>();
            http = new Mock<IMasterDataPostsHttpClient>();
            userRepo = new Mock<IUserRepository>();
            connRepo = new Mock<IConnectionRepository>();
            service = new PostsService(unit.Object, http.Object, userRepo.Object, connRepo.Object,null);

            commentId = new Guid();
            postId = "1";
            userId = new UserId(new Guid());
            text = "sample text";
            requestJson = new CreatingCommentDTO(postId, userId.Value, text);
            responseJson = new CreatingCommentResponseDTO(commentId,postId, userId.Value, text,new List<ReactionDTO>(),1L);
            var tags = new List<string>{"A"};
            user = new DDDSample1.Domain.Users.User("Abc", "2000/10/10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", tags);
        }

        [Fact]
        public void ReturnExpectedDtoForValidInput()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = JsonContent.Create(responseJson);
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            http.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Returns(Task.FromResult(response));
            var result = service.CreateComment(requestJson).Result;
            Assert.Equal(responseJson.id,result.id);
            Assert.Equal(responseJson.creationDateTime,result.creationDateTime);
            Assert.Equal(responseJson.postId,result.postId);
            Assert.Equal(responseJson.reactions.Count,result.reactions.Count);
            Assert.Equal(responseJson.text,result.text);
            Assert.Equal(responseJson.userId,result.userId);
        }
        
        [Fact]
        public void ThrowBusinessRuleValidationExceptionForNullUser()
        {
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.CreateComment(requestJson));
        }
        
        [Fact]
        public void ThrowBusinessRuleValidationExceptionForInvalidStatusCode()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Ambiguous);
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            http.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Returns(Task.FromResult(response));
            Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.CreateComment(requestJson));
        }
    }
}