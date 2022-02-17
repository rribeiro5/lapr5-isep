using System.Collections.Generic;
using System.Threading.Tasks;
using DDDNetCore.Controllers;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Posts;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.TestesUnitarios.UseCasesTests.UC10_Pedido_Conexão;
using Xunit;

namespace Tests.TestesIntegracao.UC10_Pedido_Conexão
{
    public class ConnectionRequestControllerServiceTest
    {
        private readonly Mock<IUnitOfWork> unit;
        private readonly Mock<IRequestRepository> requestRepo;
        private readonly Mock<IUserRepository> userRepo;
        private readonly Mock<IConnectionRepository> connectionRepo;
        private readonly IConnectionRequestService service;
        private readonly ConnectionRequestController controller;
        
        private readonly Utils utils;
        private CreatingRequestDTO createDto;
        private ConnectionRequestDTO responseDto;

        public ConnectionRequestControllerServiceTest()
        {
            unit = new Mock<IUnitOfWork>();
            requestRepo = new Mock<IRequestRepository>();
            userRepo = new Mock<IUserRepository>();
            connectionRepo = new Mock<IConnectionRepository>();

            service = new ConnectionRequestService(unit.Object, requestRepo.Object, userRepo.Object,
                connectionRepo.Object);
            
            controller = new ConnectionRequestController(service);
            utils = new Utils();
            createDto = utils.createRequestDto();
            responseDto = utils.createResponseDto();
        }
        
        [Fact]
        public async void SuccessfullyCreateConnectionRequest()
        {
            var l = new List<ConnectionRequest>();
            var connection = new Connection(5, 5, null, utils.oUser.Id, utils.dUser.Id);
            var connections = new List<Connection> ();
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, utils.dUser);
            SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,l);
            SetBidirecionalConnections(utils.oUser.Id, utils.dUser.Id, connections);
            connectionRepo.Setup(x => x.AddAsync(It.IsAny<Connection>()))
                .Returns(Task.FromResult(connection));
            unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<CreatedAtActionResult>(response);
        }
        
        [Fact]
        public async void ReturnsExpectedDtoWithSuccessfullRequestCreation()
        {
            var l = new List<ConnectionRequest>();
            var connection = new Connection(5, 5, null, utils.oUser.Id, utils.dUser.Id);
            var connections = new List<Connection> ();
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, utils.dUser);
            SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,l);
            SetBidirecionalConnections(utils.oUser.Id, utils.dUser.Id, connections);
            connectionRepo.Setup(x => x.AddAsync(It.IsAny<Connection>()))
                .Returns(Task.FromResult(connection));
            unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            var result=(await controller.SendConnectionRequest(createDto) as CreatedAtActionResult)
                .Value as ConnectionRequestDTO;
            Assert.Equal(responseDto.OrigUser,result.OrigUser);
            Assert.Equal(responseDto.DestUser,result.DestUser);
            Assert.Equal(responseDto.MessageOrigToDest,result.MessageOrigToDest);
        }
        
        [Fact]
        public async void ReturnsBadRequestWithInvalidOrigUser()
        {
            SetCheckUser(utils.oUser.Id, null);
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async void ReturnsBadRequestWithInvalidDestUser()
        {
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, null);
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async void ReturnsBadRequestWithOrigUserEqualsToDestUser()
        {
            SetCheckUser(utils.oUser.Id, utils.oUser);
            createDto.DestUser = createDto.OrigUser;
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async void ReturnsBadRequestWithNullRequestList()
        {
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, utils.dUser);
            SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,null);
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async void ReturnsBadRequestWithAlreadyExistingRequest()
        {
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, utils.dUser);
            var request = new ConnectionRequest(utils.oUser.Id, utils.dUser.Id, "wertert", 5, new List<string> {"c"});
            var l = new List<ConnectionRequest> {request};
            SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,l);
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        
        [Fact]
        public async void ReturnsBadRequestWithAlreadyExistingConnection()
        {
            var l = new List<ConnectionRequest>();
            var connection = new Connection(5, 5, null, utils.oUser.Id, utils.dUser.Id);
            var connections = new List<Connection> {connection};
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, utils.dUser);
            SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,l);
            SetBidirecionalConnections(utils.oUser.Id, utils.dUser.Id, connections);
            var response=await controller.SendConnectionRequest(createDto);
            Assert.IsType<BadRequestObjectResult>(response);
        }
        
        private void SetCheckUser(UserId id, User u)
        {
            userRepo.Setup(x => x.GetByIdAsync(id))
                .Returns(Task.FromResult(u));
        }

        private void SetGetRequestsInAcceptanceOfUser(UserId id,List<ConnectionRequest>l)
        {
            requestRepo.Setup(x => x.GetRequestsInAcceptanceOfUser(id))
                .Returns(Task.FromResult<List<ConnectionRequest>>(l));
        }

        private void SetBidirecionalConnections(UserId orig,UserId dest,List<Connection> l)
        {
            connectionRepo.Setup(x => x.bidirecionalConnections(orig, dest))
                .Returns(Task.FromResult(l));
        }
    }
}