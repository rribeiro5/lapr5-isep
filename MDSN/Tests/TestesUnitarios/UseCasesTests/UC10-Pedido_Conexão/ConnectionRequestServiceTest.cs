using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.ConnectionRequests;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC10_Pedido_Conexão
{
    public class ConnectionRequestServiceTest
    {
        private readonly Mock<IUnitOfWork> unit;
        private readonly Mock<IRequestRepository> requestRepo;
        private readonly Mock<IUserRepository> userRepo;
        private readonly Mock<IConnectionRepository> connectionRepo;
        private readonly IConnectionRequestService service;
        
        private readonly Utils utils;
        private CreatingRequestDTO createDto;
        private ConnectionRequestDTO responseDto;

        public ConnectionRequestServiceTest()
        {
            unit = new Mock<IUnitOfWork>();
            requestRepo = new Mock<IRequestRepository>();
            userRepo = new Mock<IUserRepository>();
            connectionRepo = new Mock<IConnectionRepository>();

            service = new ConnectionRequestService(unit.Object, requestRepo.Object, userRepo.Object,
                connectionRepo.Object);
            utils = new Utils();
            createDto = utils.createRequestDto();
            responseDto = utils.createResponseDto();
        }
        
        [Fact]
        public async void ThrowBusinessRuleValidationExceptionWithInvalidOrigUser()
        {
            SetCheckUser(utils.oUser.Id, null);
            try
            {
                await service.AddAsync(createDto);
                Assert.True(false);
            }
            catch (BusinessRuleValidationException)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public async void ThrowBusinessRuleValidationExceptionWithInvalidDestUser()
        {
            SetCheckUser(utils.oUser.Id, utils.oUser);
            SetCheckUser(utils.dUser.Id, null);
            try
            {
                await service.AddAsync(createDto);
                Assert.True(false);
            }
            catch (BusinessRuleValidationException)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public async void ThrowBusinessRuleValidationExceptionWithOrigUserEqualsToDestUser()
        {
            SetCheckUser(utils.oUser.Id, utils.oUser);
            createDto.DestUser = createDto.OrigUser;
            try
            {
                await service.AddAsync(createDto);
                
            }
            catch (BusinessRuleValidationException)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public async void ThrowBusinessRuleValidationExceptionWithNullRequestList()
        {
            try
            {
                SetCheckUser(utils.oUser.Id, utils.oUser);
                SetCheckUser(utils.dUser.Id, utils.dUser);
                SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,null);
                await service.AddAsync(createDto);
                Assert.True(false);
            }catch (BusinessRuleValidationException)
            {
                Assert.True(true);
            }
            
        }
        
        [Fact]
        public async void ThrowBusinessRuleValidationExceptionWithAlreadyExistingRequest()
        {
            try
            {
                SetCheckUser(utils.oUser.Id, utils.oUser);
                SetCheckUser(utils.dUser.Id, utils.dUser);
                var request = new ConnectionRequest(utils.oUser.Id, utils.dUser.Id, "wertert", 5, new List<string> {"c"});
                var l = new List<ConnectionRequest> {request};
                SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,l);
                await service.AddAsync(createDto);
                Assert.True(false);
            }catch (BusinessRuleValidationException)
            {
                Assert.True(true);
            }
        }
        
        [Fact]
        public async void ThrowBusinessRuleValidationExceptionWithAlreadyExistingConnection()
        {
            var l = new List<ConnectionRequest>();
            var connection = new Connection(5, 5, null, utils.oUser.Id, utils.dUser.Id);
            var connections = new List<Connection> {connection};
            try
            {
                SetCheckUser(utils.oUser.Id, utils.oUser);
                SetCheckUser(utils.dUser.Id, utils.dUser);

                SetGetRequestsInAcceptanceOfUser(utils.dUser.Id,l);
                SetBidirecionalConnections(utils.oUser.Id, utils.dUser.Id, connections);
                await service.AddAsync(createDto);
                Assert.True(false);
            }catch (BusinessRuleValidationException)
            {
                Assert.True(true);
            }
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
            var result=await service.AddAsync(createDto);
            Assert.Equal(responseDto.OrigUser,result.OrigUser);
            Assert.Equal(responseDto.DestUser,result.DestUser);
            Assert.Equal(responseDto.MessageOrigToDest,result.MessageOrigToDest);
        }

        private void SetCheckUser(UserId id, DDDSample1.Domain.Users.User u)
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