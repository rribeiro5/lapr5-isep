using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC12_EditarEstadoHumor
{
    public class ConnectionRequestServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repoRequest;
        private readonly Mock<IUserRepository> _repoUser;
        private readonly ConnectionRequestService _service;
        private readonly UpdatedApprovalStateRequestDTO _dto;
        private readonly ConnectionRequest _c;
        private readonly DDDSample1.Domain.Users.User _user;
        
        public ConnectionRequestServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repoRequest = new Mock<IRequestRepository>();
            _repoUser = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repoRequest.Object, _repoUser.Object, null);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
            _c = new ConnectionRequest(new UserId(new Guid()),new UserId(new Guid()),new UserId(new Guid()),
                "null","null",1,new List<string>{"tag1","tag2"});
            _user = new DDDSample1.Domain.Users.User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
        }
        //OnGet
        [Fact]
        public async void ReturnNullWhenRequestDoesntExistOnGet()
        {
            _repoUser.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service
                .GetPendingApprovalRequestsOfUser(It.IsAny<UserId>());
            Assert.Null(result);
        }
        
        [Fact]
        public async void ReturnNullWhenRepoReturnsNull()
        {
            _repoUser.Setup(x => x.GetByIdAsync(It.IsAny<UserId>()))
                .Returns(Task.FromResult(_user));
            _repoRequest.Setup(x => x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<ConnectionRequest>>(null));
            var result = await _service
                .GetPendingApprovalRequestsOfUser(It.IsAny<UserId>());
            Assert.Null(result);
        }
        
        [Fact]
        public async void ReturnEmptyListWhenPendingConnectionsListIsEmpty()
        {
            var id = _user.Id;
            _repoUser.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult(_user));
            _repoRequest.Setup(x => x.GetPendingApprovalRequestsOfUser(id))
                .Returns(Task.FromResult<IList<ConnectionRequest>>(new List<ConnectionRequest>()));
            var result = await _service
                .GetPendingApprovalRequestsOfUser(id);
            Assert.True(result.ToList().Count == 0);
        }
        
        [Fact]
        public async void ReturnExpectedListWhenPendingConnectionsListHasObjects()
        {
            var id = _user.Id;
            var iId =  new UserId(new Guid("20000000-0000-0000-0000-000000000000"));
            var dId = new UserId( new Guid("30000000-0000-0000-0000-000000000000"));
            var conn = new ConnectionRequest(id,iId,dId,"","",3,new List<string>());
            var l = new List<ConnectionRequest> {conn};
            var expected=l?.ConvertAll(c => new ConnectionRequestDTO(c.Id.AsGuid(), 
                c.OrigUser, c.InterUser, c.DestUser, c.MessageOrigToDest?.Message, 
                c.MessageOrigToInter?.Message, c.MessageInterToDest?.Message));
            _repoUser.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult(_user));
            _repoRequest.Setup(x => x.GetPendingApprovalRequestsOfUser(id))
                .Returns(Task.FromResult<IList<ConnectionRequest>>(l));
            var result = await _service
                .GetPendingApprovalRequestsOfUser(id);
            Assert.Equal(expected[0].Id,result.ToList()[0].Id);
        }

        
        //OnUpdate  
        [Fact]
        public async void ReturnNullWhenRequestDoesntExistOnUpdate()
        {
            _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
                .Returns(Task.FromResult<ConnectionRequest>(null));
            var result = await _service.UpdateApprovalState(_dto);
            Assert.Null(result);
        }
        
        [Fact]
        public async Task ThrowBusinessRuleValidationExceptionForRequestNotOnApprovalOnUpdate()
        {
            _c.Disapproved();
            _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
                .Returns(Task.FromResult(_c));
            await Assert.ThrowsAsync<BusinessRuleValidationException>(()=>_service.UpdateApprovalState(_dto));
        }
        
        [Fact]
        public async void ReturnDtoWhenRequestIsUpdated()
        {
            _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
                .Returns(Task.FromResult(_c));
            
            var result = await _service.UpdateApprovalState(_dto);
            Assert.IsType<ConnectionRequestDTO>(result);
        }
        
        [Fact]
        public async void ReturnDesiredDtoWhenRequestIsUpdated()
        {
            _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
                .Returns(Task.FromResult(_c));
            
            var result = await _service.UpdateApprovalState(_dto);
            var expected=new ConnectionRequestDTO{Id = _c.Id.AsGuid(), DestUser = _c.DestUser, 
                MessageOrigToDest = _c.MessageOrigToDest.Message, OrigUser = _c.OrigUser};
            Assert.Equal(expected.Id,result.Id);
        }
    }
}