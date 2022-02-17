using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Infrastructure.Posts;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Moq;
using Xunit;
using System.Net.Http.Formatting;

namespace Tests.TestesUnitarios.UC_Reagir_Posts_Comentarios
{
    public class PostsServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IMasterDataPostsHttpClient> _client;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly Mock<IConnectionRepository> _connRepo;
        private readonly PostsService _service;

        private Guid _userId;
        private DDDSample1.Domain.Users.User _user;
        private List<PostDTO> _list;

        private CreatingReactionDTO _dto;

        private CreatingReactionResponseDTO _response;

        private HttpResponseMessage _validResponse;
        private HttpResponseMessage _invalidResponse;

        public PostsServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _client = new Mock<IMasterDataPostsHttpClient>();
            _userRepo = new Mock<IUserRepository>();
            _connRepo = new Mock<IConnectionRepository>();
            _service = new PostsService(_unit.Object, _client.Object, _userRepo.Object, _connRepo.Object,null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _userId = Guid.NewGuid();
            _list = new List<PostDTO>();
            var listString = new List<string>();
            listString.Add("Benfica");
            _user = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            _validResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            
            _invalidResponse = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);

            _dto = new CreatingReactionDTO(_userId.ToString(),new Guid(),"LIKE");
            _response = new CreatingReactionResponseDTO(_userId.ToString(),new Guid().ToString(),_userId.ToString(),false,"LIKE");
            _validResponse.Content = new ObjectContent(_response.GetType(),_response,new JsonMediaTypeFormatter());
        }

        [Fact]
        public async void ThrowBusinessRuleExceptionWhenUserDoesntExist()
        {
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _service.updateReactionPost(_dto));
        }

        [Fact]
        public async void ThrowBusinessRuleExceptionWhenStatusCodeIsNotOk()
        {
            _userRepo.Setup(x => x.GetByIdAsync(new UserId(_userId)))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(_user));
            _client.Setup(x => x.updateReactionPost(_dto))
                .Returns(Task.FromResult<HttpResponseMessage>(_invalidResponse));
            await Assert.ThrowsAsync<BusinessRuleValidationException>(() => _service.updateReactionPost(_dto));
        }

        [Fact]
        public async void ReturnExpectedResultWhenUserExists()
        {
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(_user));

            _client.Setup(x => x.updateReactionPost(_dto))
                .Returns(Task.FromResult<HttpResponseMessage>(_validResponse));

            _connRepo.Setup(x => x.obtainConnection(It.IsAny<UserId>(),It.IsAny<UserId>()))
            .Returns(Task.FromResult<Connection>(null));    

            var result = await _service.updateReactionPost(_dto);
            Assert.Equal<CreatingReactionResponseDTO>(_response, result);
        }
    }
}