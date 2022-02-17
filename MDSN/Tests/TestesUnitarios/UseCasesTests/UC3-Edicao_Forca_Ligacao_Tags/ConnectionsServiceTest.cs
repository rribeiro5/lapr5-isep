using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC3
{
    public class ConnectionServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IConnectionRepository> _repo;

        private readonly Mock<IUserRepository> _userRepo;

        private readonly ConnectionService _service;
        private readonly UserConnectionsDTO _dto;
        private  UserId userID;

        private DDDSample1.Domain.Users.User user;

        private ConnectionId connectionId;

        private ChangeConnInfoDTO connInfoDTO;
        
        public ConnectionServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IConnectionRepository>();
            _userRepo = new Mock<IUserRepository>();
            _service = new ConnectionService(_unit.Object, _repo.Object, _userRepo.Object);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            ConnectionDTO connectionDTO = new ConnectionDTO(new Guid(),new UserId(new Guid()),new UserId(new Guid()),new List<string>(),5,0);
            var list = new List<ConnectionDTO>();
            list.Add(connectionDTO);
            _dto = new UserConnectionsDTO(list);
            userID = new UserId(new Guid());
            var listString = new List<string>();
            listString.Add("Benfica");
            user = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            this.connectionId = new ConnectionId(new Guid());
            this.connInfoDTO = new ChangeConnInfoDTO(5,listString);
        }
        
          [Fact]
        public async Task ThrowBusinessRuleValidationExceptionWhenUserDoesntExist()
        {
            _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            await Assert.ThrowsAsync<BusinessRuleValidationException>(()=>_service.GetConnectionsOfUser(userID));
        }

        
        [Fact]
        public async void ReturnNullWhenConnectionDoesntExist()
        {
             _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(user));
            
            _repo.Setup(x => x.ConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<List<Connection>>(null));
            
            var result = await this._service.GetConnectionsOfUser(this.userID);

            Assert.Null(result);
        }
        
      
        
        [Fact]
        public async void ReturnDtoWhenConnectionIsFind()
        {
             _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(user));
            
            List<String> listaString = new List<string>();
            listaString.Add("Benfica");
            Connection connection = new Connection(1,1,listaString,new UserId(new Guid()),new UserId(new Guid()));
            List<Connection> lConnections = new List<Connection>();
            lConnections.Add(connection);
        
            _repo.Setup(x => x.ConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<List<Connection>>(lConnections));
            

            var result = await _service.GetConnectionsOfUser(userID);
            Assert.IsType<UserConnectionsDTO>(result);
        }
        
       [Fact]
        public async void DesiredDtoWhenConnectionIsFind()
        {
             _userRepo.Setup(x => x.GetByIdAsync(It.IsAny<UserId>())
            ).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(user));
            
            List<String> listaString = new List<string>();
            listaString.Add("Benfica");
            Connection connection = new Connection(5,0,listaString,new UserId(new Guid()),new UserId(new Guid()));
            List<Connection> lConnections = new List<Connection>();
            lConnections.Add(connection);

            
        
            _repo.Setup(x => x.ConnectionsOfUser(It.IsAny<UserId>())
            ).Returns(Task.FromResult<List<Connection>>(lConnections));
            

            var result = await _service.GetConnectionsOfUser(userID);

            //Assert.Equal<List<ConnectionDTO>>(_dto.Connections,result.Connections); todo por causa dos Ids
        }

        [Fact]
        public async Task ReturnNullWhenConnectionDontExists()
        {
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>())
            ).Returns(Task.FromResult<Connection>(null));
            var result = await this._service.UpdateConnection(this.connectionId,this.connInfoDTO);
            Assert.Null(result);
        }

        [Fact]
        public async Task ReturnDtoWhenConnectionExists()
        {   
            var listString = new List<string>();
            listString.Add("Benfica");
            _repo.Setup(x => x.GetByIdAsync(It.IsAny<ConnectionId>())
            ).Returns(Task.FromResult<Connection>(new Connection(5,5,listString,new UserId(new Guid()),new UserId(new Guid()))));
            var result = await this._service.UpdateConnection(this.connectionId,this.connInfoDTO);
            Assert.IsType<ConnectionDTO>(result);
        }


      
    }
}