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

namespace Tests.TestesIntegracao.UC6_Editar_estado_humor
{
    public class ConnectionRequestControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IRequestRepository> _repoRequest;
        private readonly Mock<IUserRepository> _repoUser;
        private readonly ConnectionRequestController _controller;
        private readonly ConnectionRequestService _service;
        private readonly ConnectionRequest _c;
        private readonly User _user;

        public ConnectionRequestControllerServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repoRequest = new Mock<IRequestRepository>();
            _repoUser = new Mock<IUserRepository>();
            _service = new ConnectionRequestService(_unit.Object, _repoRequest.Object, _repoUser.Object, null);
            _controller = new ConnectionRequestController(_service);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _c = new ConnectionRequest(new UserId(new Guid()),new UserId(new Guid()),new UserId(new Guid()),
                "null","null",1,new List<string>{"tag1","tag2"});
            _user = new User("name", "2000-01-01", null, null, "1234@gmail.com",
                "Password1?", "", "+3511234", null, null, new List<string> {"tag1"});
        }
        //OnGet
        [Fact]
        public async void ReturnBadRequestResultWhenRequestDoesntExistOnGet()
        {
            _repoUser.Setup(x => 
                x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<User>(null));
            var result = await _controller.GetPendingApprovalRequestsOfUser(new Guid());
            Assert.IsType<BadRequestResult>(result);
        }
   
        [Fact]
        public async void ReturnBadRequestResultWhenServiceReturnsNullList()
        {
            var dtoGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var oId =  new UserId(new Guid("10000000-0000-0000-0000-000000000000"));
            var iId =  new UserId(new Guid("20000000-0000-0000-0000-000000000000"));
            var dId = new UserId( new Guid("30000000-0000-0000-0000-000000000000"));
            var dto = new ConnectionRequestDTO(dtoGuid,oId,iId,dId,"","");
            IEnumerable<ConnectionRequestDTO> l = new List<ConnectionRequestDTO>{dto};
            _repoUser.Setup(x => 
                x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult(_user));
            _repoRequest.Setup(x => x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>()))
                .Returns(Task.FromResult<IList<ConnectionRequest>>(null));
            var result = await _controller.GetPendingApprovalRequestsOfUser(new Guid());
            Assert.IsType<BadRequestResult>(result);
        }
         
       [Fact]
       public async void ReturnOkResultWhenRequestExistsOnGet()
       {
           var id = _user.Id;
           var iId =  new UserId(new Guid("20000000-0000-0000-0000-000000000000"));
           var dId = new UserId( new Guid("30000000-0000-0000-0000-000000000000"));
           var conn = new ConnectionRequest(id,iId,dId,"","",3,new List<string>());
           var l = new List<ConnectionRequest> {conn};
           var expected=l?.ConvertAll(c => new ConnectionRequestDTO(c.Id.AsGuid(), 
               c.OrigUser, c.InterUser, c.DestUser, c.MessageOrigToDest?.Message, 
               c.MessageOrigToInter?.Message, c.MessageInterToDest?.Message));
           _repoUser.Setup(x => 
               x.GetByIdAsync(It.IsAny<UserId>())
           ).Returns(Task.FromResult(_user));
           _repoRequest.Setup(x => x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>()))
               .Returns(Task.FromResult<IList<ConnectionRequest>>(l));
           var result = await _controller.GetPendingApprovalRequestsOfUser(_user.Id.AsGuid());
           var okResult = result as OkObjectResult;
           var returnedList = (List<ConnectionRequestDTO>)okResult?.Value;
           Assert.Equal(expected[0].Id,returnedList?[0].Id);
       }
              
       [Fact]
       public async void ReturnOkResultWithExpectedResultWhenRequestExistsOnGet()
       {
           var id = _user.Id;
           var iId =  new UserId(new Guid("20000000-0000-0000-0000-000000000000"));
           var dId = new UserId( new Guid("30000000-0000-0000-0000-000000000000"));
           var conn = new ConnectionRequest(id,iId,dId,"","",3,new List<string>());
           var l = new List<ConnectionRequest> {conn};
           var expected=l?.ConvertAll(c => new ConnectionRequestDTO(c.Id.AsGuid(), 
               c.OrigUser, c.InterUser, c.DestUser, c.MessageOrigToDest?.Message, 
               c.MessageOrigToInter?.Message, c.MessageInterToDest?.Message));
           _repoUser.Setup(x => 
               x.GetByIdAsync(It.IsAny<UserId>())
           ).Returns(Task.FromResult(_user));
           _repoRequest.Setup(x => x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>()))
               .Returns(Task.FromResult<IList<ConnectionRequest>>(l));
           var result = await _controller.GetPendingApprovalRequestsOfUser(_user.Id.AsGuid());
           Assert.IsType<OkObjectResult>(result);
       } 

        //OnUpdate
        [Fact]
        public async void ReturnNotFoundResultWhenRequestDoesntExistOnUpdate()
        {
            _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
                .Returns(Task.FromResult<ConnectionRequest>(null));
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), false, "");
            var result = await _controller.UpdateApprovalState(dto);
            Assert.IsType<NotFoundResult>(result);
        }
      
        [Fact]
        public async void ReturnOkResultWhenRequestIsFoundOnUpdate()
        {
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
            _repoRequest.Setup(x => 
                x.GetByIdAsync(It.IsAny<RequestId>())
            ).Returns(Task.FromResult(_c));
            
            var result = await _controller.UpdateApprovalState(dto);
            Assert.IsType<OkObjectResult>(result);
        }
      
        [Fact]
        public async void ReturnExpectedObjectWhenRequestIsFoundOnUpdate()
        {
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
            var connectionDto = new ConnectionRequestDTO(_c.Id.AsGuid(), null, null, null, "", "", "");
            _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
                .Returns(Task.FromResult(_c));

            var result = await _controller.UpdateApprovalState(dto);
            var okResult = result as OkObjectResult;
            var returnedDto = (ConnectionRequestDTO)okResult?.Value;
            //Assert.Equal(connectionDto.Id,((ConnectionRequestDTO)returnedDto).Id);
            Assert.Equal(connectionDto.Id,returnedDto?.Id);
        }
      
       [Fact]
       public async void ReturnBadRequestForBusinessValidationExceptionOnUpdate()
       {
           var dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
           _c.Disapproved();
           _repoRequest.Setup(x => x.GetByIdAsync(It.IsAny<RequestId>()))
               .Returns(Task.FromResult(_c));

           var result = await _controller.UpdateApprovalState(dto);
           Assert.IsType<BadRequestObjectResult>(result);
       }

    }
}