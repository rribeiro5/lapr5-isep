using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests
{
    public class ConnectionRequestServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repo;
        private readonly Mock<IUserRepository> _userRepo;
        private readonly ConnectionRequestService _service;

        private DDDSample1.Domain.Users.User user;

        private readonly UserRequestsDTO _dto;

        private readonly List<ConnectionRequest> ListConnectionRequest; 
        private readonly ConnectionRequest _c;


        
        public ConnectionRequestServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IRequestRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repo.Object, _userRepo.Object, null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            var listString = new List<string>();
            listString.Add("Benfica");
            user = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            var user2 = new DDDSample1.Domain.Users.User("Candido","2001/10/11",null,null,"dasdsad1@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            var user3 = new DDDSample1.Domain.Users.User("Filho","2001/10/11",null,null,"dasdsad1@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);

            _c = new ConnectionRequest(user.Id,user2.Id,user3.Id,
                "null","null",1,new List<string>{"tag1","tag2"});

            ListConnectionRequest = new List<ConnectionRequest>{_c};

            _dto = new UserRequestsDTO(ListConnectionRequest.ConvertAll<ConnectionRequestDTO>(c => 
                new ConnectionRequestDTO(c.Id.AsGuid(), c.OrigUser, c.InterUser, c.DestUser,
                c.MessageOrigToDest.Message, c.MessageOrigToInter == null ? null : c.MessageOrigToInter.Message, 
                c.MessageInterToDest == null ? null : c.MessageInterToDest.Message)));
        }
        
        [Fact]
        public async void ReturnNullWhenUserDoesntExist()
        {   
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.GetRequestsInAcceptance(this.user.Id);
            Assert.Null(result);
        }


        [Fact]
        public async void ReturnNullWhenRequestDoesntExist()
        {   
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user));

            _repo.Setup(x=> x.GetRequestsInAcceptanceOfUser(this.user.Id))
                .Returns(Task.FromResult<List<ConnectionRequest>>(null));

            var result = await _service.GetRequestsInAcceptance(this.user.Id);
            Assert.Null(result);
        }

        
        [Fact]
        public async void ReturnDtoWhenRequestIsValid()
        {   


            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user));

            _repo.Setup(x=> x.GetRequestsInAcceptanceOfUser(this.user.Id))
                .Returns(Task.FromResult<List<ConnectionRequest>>(ListConnectionRequest));

            var result = await _service.GetRequestsInAcceptance(this.user.Id);
            Assert.IsType<UserRequestsDTO>(result);
        }


        [Fact]
        public async void ReturnDesiredResultWhenRequestIsValid()
        {   


            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user));

            _repo.Setup(x=> x.GetRequestsInAcceptanceOfUser(this.user.Id))
                .Returns(Task.FromResult<List<ConnectionRequest>>(ListConnectionRequest));

            var result = await _service.GetRequestsInAcceptance(this.user.Id);

            var requests = result.Requests;

            var expectedRequest = this._dto.Requests;

            Assert.Equal(requests[0],expectedRequest[0]);
        }
        
       
    }
}