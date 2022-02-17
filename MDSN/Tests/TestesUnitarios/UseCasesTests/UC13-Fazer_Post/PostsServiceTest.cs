using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Infrastructure.Posts;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC13_Fazer_Post
{
    public class PostsServiceTest
    {
        private Mock<IUnitOfWork> unit;
        private Mock<IMasterDataPostsHttpClient> http;
        private Mock<IUserRepository> userRepo;
        private Mock<IConnectionRepository> connRepo;
        private IPostsService service;
        private string postId;
        private Guid userId;
        private string text;
        private CreatePostRequestDTO _requestDto;
        private CreatePostResponseDTO _responseDto;
        private DDDSample1.Domain.Users.User user;
        
        public PostsServiceTest()
        {
            unit = new Mock<IUnitOfWork>();
            http = new Mock<IMasterDataPostsHttpClient>();
            userRepo = new Mock<IUserRepository>();
            connRepo = new Mock<IConnectionRepository>();
            service = new PostsService(unit.Object, http.Object, userRepo.Object, connRepo.Object,null);
            postId = "1";
            userId = new Guid();
            text = "sample text";
            _requestDto = new CreatePostRequestDTO(userId, text, new List<string>());
            _responseDto = new CreatePostResponseDTO(userId, text, new List<string>(), 1L, new List<ReactionDTO>(),
                new List<CommentDTO>());
            user = new DDDSample1.Domain.Users.User("Abc", "2000/10/10", "Porto", "Portugal", "abc@gmail.com", "Abcde123!", 
                "aaabbbccc", "+351987654321", "https://www.linkedin.com/in/id123", "https://www.facebook.com/id123", new List<string>(){"tag"});

        }

        [Fact]
        public void ReturnExpectedDtoForValidInput()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = JsonContent.Create(_responseDto);
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            http.Setup(x => x.CreatePost(It.IsAny<CreatePostRequestDTO>()))
                .Returns(Task.FromResult(response));
            var result = service.CreatePost(_requestDto).Result;
            Assert.Equal(_responseDto.id,result.id);
            Assert.Equal(_responseDto.creationDateTime,result.creationDateTime);
            Assert.Equal(_responseDto.comments,result.comments);
            Assert.Equal(_responseDto.reactions,result.reactions);
            Assert.Equal(_responseDto.reactions.Count,result.reactions.Count);
            Assert.Equal(_responseDto.text,result.text);
            Assert.Equal(_responseDto.userId,result.userId);
        }
        
        [Fact]
        public void ThrowBusinessRuleValidationExceptionForNullUser()
        {
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.CreatePost(_requestDto));
        }
        
        [Fact]
        public void ThrowBusinessRuleValidationExceptionForInvalidStatusCode()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Ambiguous);
            userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(user));
            http.Setup(x => x.CreateComment(It.IsAny<CreatingCommentDTO>()))
                .Returns(Task.FromResult(response));
            Assert.ThrowsAsync<BusinessRuleValidationException>(() => service.CreatePost(_requestDto));
        }
    }
}