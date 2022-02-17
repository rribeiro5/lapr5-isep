using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesIntegracao.UC33AceitarPedido
{
    public class ConnectionRequestControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repo;

        private readonly Mock<IUserRepository> _userRepo;
        private readonly ConnectionRequestController _controller;
        private readonly ConnectionRequestService _service;
        private ConnectionRequest _c;

       private UserId _UserId;

        private readonly List<ConnectionRequest> ListConnectionRequest; 

       private User user;

       private readonly UserRequestsDTO _dto;

       

        public ConnectionRequestControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IRequestRepository>();
            _userRepo = new Mock<IUserRepository> ();
            _service = new ConnectionRequestService(_unit.Object, _repo.Object, _userRepo.Object, null);
            _controller = new ConnectionRequestController(_service);
             
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _c = new ConnectionRequest(new UserId(new Guid()),new UserId(new Guid()),new UserId(new Guid()),
                "null","null",1,new List<string>{"tag1","tag2"});
                var listString = new List<string>();
            listString.Add("Benfica");
            user = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            _UserId = user.Id;

            var user2 = new DDDSample1.Domain.Users.User("Candido","2001/10/11",null,null,"dasdsad1@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            var user3 = new DDDSample1.Domain.Users.User("Filho","2001/10/11",null,null,"dasdsad1@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);

            _c = new ConnectionRequest(user.Id,user2.Id,user3.Id,
                "null","null",1,new List<string>{"tag1","tag2"});

            ListConnectionRequest = new List<ConnectionRequest>{_c};

            _dto = new UserRequestsDTO(ListConnectionRequest.ConvertAll<ConnectionRequestDTO>(c => 
                new ConnectionRequestDTO(c.Id.AsGuid(), c.OrigUser, c.InterUser, c.DestUser,
                c.MessageOrigToDest.Message, c.MessageOrigToInter == null ? null : c.MessageOrigToInter.Message, 
                c.MessageInterToDest == null ? null : c.MessageInterToDest.Message)));

            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user));
        }
        
        [Fact]
        public async void ReturnNotFoundResultWhenUserDoesntExist()
        {
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
  
            var result = await _controller.GetRequestsInAcceptance(this._UserId.AsGuid());
            Assert.IsType<NotFoundResult>(result.Result);
        }
      
        [Fact]
        public async void ReturnOkResultWhenRequestIsFound()
        {
            _userRepo.Setup(x => x.GetByIdAsync(this._UserId))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user));

                _repo.Setup(x=> x.GetRequestsInAcceptanceOfUser(this.user.Id))
                .Returns(Task.FromResult<List<ConnectionRequest>>(ListConnectionRequest));
            
            var result = await _controller.GetRequestsInAcceptance(this._UserId.AsGuid());
            Assert.IsType<UserRequestsDTO>(result.Value);
        }
      
        [Fact]
        public async void ReturnDesiredResultWhenRequestIsFound()
        {
            _userRepo.Setup(x => x.GetByIdAsync(this._UserId))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user));

                _repo.Setup(x=> x.GetRequestsInAcceptanceOfUser(this.user.Id))
                .Returns(Task.FromResult<List<ConnectionRequest>>(ListConnectionRequest));
            
            var result = await _controller.GetRequestsInAcceptance(this._UserId.AsGuid());
            Assert.Equal(result.Value.Requests[0],this._dto.Requests[0]);
        }

    }
}