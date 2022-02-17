using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Sdk;

namespace Tests.TestesUnitarios.UC12_EditarEstadoHumor
{
    public class ConnectionRequestControllerTest
    {
        private readonly Mock<IConnectionRequestService> _mock;
        private readonly ConnectionRequestController _controller;
        
        public ConnectionRequestControllerTest()
        {
            _mock = new Mock<IConnectionRequestService>();
            _controller = new ConnectionRequestController(_mock.Object);
        }
        
        //OnGet
        [Fact]
        public async void ReturnBadRequestResultWhenRequestDoesntExistOnGet()
        {
            IEnumerable<ConnectionRequestDTO> l = null;
            _mock.Setup(x => 
                x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult(l));
            var result = await _controller.GetPendingApprovalRequestsOfUser(new Guid());
            Assert.IsType<BadRequestResult>(result);
        }
        
        [Fact]
        public async void ReturnOkResultWhenRequestExistOnGet()
        {
            var dtoGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var oId =  new UserId(new Guid("10000000-0000-0000-0000-000000000000"));
            var iId =  new UserId(new Guid("20000000-0000-0000-0000-000000000000"));
            var dId = new UserId( new Guid("30000000-0000-0000-0000-000000000000"));
            var dto = new ConnectionRequestDTO(dtoGuid,oId,iId,dId,"","");
            IEnumerable<ConnectionRequestDTO> l = new List<ConnectionRequestDTO>{dto};
            _mock.Setup(x => 
                x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult(l));
            var result = await _controller.GetPendingApprovalRequestsOfUser(new Guid());
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnOkResultWithExpectedObjectWhenRequestExistOnGet()
        {
            var dtoGuid = new Guid("00000000-0000-0000-0000-000000000000");
            var oId =  new UserId(new Guid("10000000-0000-0000-0000-000000000000"));
            var iId =  new UserId(new Guid("20000000-0000-0000-0000-000000000000"));
            var dId = new UserId( new Guid("30000000-0000-0000-0000-000000000000"));
            var dto = new ConnectionRequestDTO(dtoGuid,oId,iId,dId,"","");
            IEnumerable<ConnectionRequestDTO> l = new List<ConnectionRequestDTO>{dto};
            _mock.Setup(x => 
                x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult(l));
            var result = await _controller.GetPendingApprovalRequestsOfUser(new Guid());
            var okResult = result as OkObjectResult;
            var returnedList = okResult?.Value;
            Assert.Equal(l,returnedList);
        }
        
        [Fact]
        public async void ReturnBadRequestForBusinessValidationExceptionOnGet()
        {
            _mock.Setup(x =>
                x.GetPendingApprovalRequestsOfUser(It.IsAny<UserId>())
            ).Throws(new BusinessRuleValidationException("test"));

            var result = await _controller.GetPendingApprovalRequestsOfUser(new Guid());
            Assert.IsType<BadRequestObjectResult>(result);
        }
        
        //OnUpdate     
        [Fact]
        public async void ReturnNotFoundResultWhenRequestDoesntExistOnOnUpdate()
        {
            _mock.Setup(x => 
                x.UpdateApprovalState(It.IsAny<UpdatedApprovalStateRequestDTO>())
            ).Returns(Task.FromResult<ConnectionRequestDTO>(null));
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), false, "");
            var result = await _controller.UpdateApprovalState(dto);
            Assert.IsType<NotFoundResult>(result);
        }
        
        [Fact]
        public async void ReturnOkResultWhenRequestIsUpdated()
        {
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
            var connectionDto = new ConnectionRequestDTO(new Guid(), null, null, null, "", "", "");
            _mock.Setup(x => 
                x.UpdateApprovalState(It.IsAny<UpdatedApprovalStateRequestDTO>())
            ).Returns(Task.FromResult(connectionDto));
            
            var result = await _controller.UpdateApprovalState(dto);
            Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void ReturnExpectedObjectWhenRequestIsUpdated()
        {
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
            var connectionDto = new ConnectionRequestDTO(new Guid(), null, null, null, "", "", "");
            _mock.Setup(x => 
                x.UpdateApprovalState(It.IsAny<UpdatedApprovalStateRequestDTO>())
            ).Returns(Task.FromResult(connectionDto));

            var result = await _controller.UpdateApprovalState(dto);
            var okResult = result as OkObjectResult;
            var returnedDto = okResult?.Value;
            Assert.Equal(connectionDto,returnedDto);
        }
        
        [Fact]
        public async void ReturnBadRequestForBusinessValidationExceptionOnUpdate()
        {
            var dto = new UpdatedApprovalStateRequestDTO(new Guid(), true, "");
            _mock.Setup(x =>
                x.UpdateApprovalState(It.IsAny<UpdatedApprovalStateRequestDTO>())
            ).Throws(new BusinessRuleValidationException("test"));

            var result = await _controller.UpdateApprovalState(dto);
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}