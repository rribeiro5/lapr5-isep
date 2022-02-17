using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UC7
{
    public class UserNetworkServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repo;

        private readonly Mock<IConnectionRepository> _connRepository;
        private readonly UserNetworkService _service;

        private DDDSample1.Domain.Users.User user1 ;

         private DDDSample1.Domain.Users.User user2 ;

        private List<Connection> listConnections; 

        private UserNetworkDTO userNetwork;

        public UserNetworkServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repo = new Mock<IUserRepository>();
            _connRepository = new Mock<IConnectionRepository>();

            _service = new UserNetworkService(_unit.Object,_connRepository.Object ,_repo.Object,null);
            var listString = new List<string>();
            listString.Add("Benfica");
            this.user1 = new DDDSample1.Domain.Users.User("Eduardo","2001/10/11",null,null,"ecouto93@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);
            this.user2 = new DDDSample1.Domain.Users.User("Ribeiro","2001/10/11",null,null,"ribeiro@gmail.com","Teste1234!",null,"+351910720916",null,null,listString);


            this.listConnections = new List<Connection>();
            

            var netConns = new List<UserNetworkConnDTO>();
           this.userNetwork = new UserNetworkDTO(1,this.user1.Id.AsGuid(),this.user1._Email._Email,user1._Name?._Name,listString,user1._Avatar?._avatarUrl,this.user1._EmotionalState?.Emotion._Emotion,netConns);
        }

        [Fact]
        public async void ReturnNotFoundRequestWithNoNetwork()
        {   
            _repo.Setup(m => m.GetByIdAsync(It.IsAny<UserId>())).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(null));
            var result = await _service.GetUserNetwork(new UserId(new Guid()),5);
            Assert.Null(result);
        }

        [Fact]
        public async void ReturnEmptyTypekWhenFoundWithNoConnections()
        {   
            

            _repo.Setup(m => m.GetByIdAsync(this.user1.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user1));
            _repo.Setup(m => m.GetByIdAsync(this.user2.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user2));

            _connRepository.Setup(m => m.ConnectionsOfUser(this.user1.Id)).Returns(Task.FromResult<List<Connection>>(listConnections));

            var result = await _service.GetUserNetwork(this.user1.Id,1);
           Assert.IsType<UserNetworkDTO>(result);
        }

           [Fact]
        public async void ReturnEmptyNetworkWhenFoundWithNoConnections()
        {   
            

            _repo.Setup(m => m.GetByIdAsync(this.user1.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user1));
            _repo.Setup(m => m.GetByIdAsync(this.user2.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user2));

            _connRepository.Setup(m => m.ConnectionsOfUser(this.user1.Id)).Returns(Task.FromResult<List<Connection>>(listConnections));

            var result = await _service.GetUserNetwork(this.user1.Id,1);
            Assert.Equal<UserNetworkConnDTO>(result.Connections,this.userNetwork.Connections);
        }
        

         [Fact]
        public async void ReturnCorrectTypekWhenFoundWithConnections()
        {   
            
            var listString = new List<string>{"Benfica"};
            listConnections.Add(new Connection(1,1,listString,this.user1.Id,this.user2.Id));

            _repo.Setup(m => m.GetByIdAsync(this.user1.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user1));
            _repo.Setup(m => m.GetByIdAsync(this.user2.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user2));

            _connRepository.Setup(m => m.ConnectionsOfUser(this.user1.Id)).Returns(Task.FromResult<List<Connection>>(listConnections));

            var item = listConnections[0];

            var r = await _service.GetUserNetwork(this.user1.Id,1);
           Assert.IsType<UserNetworkDTO>(r);
        }

           [Fact]
        public async void ReturnNetworkkWhenFoundWithConnections()
        {   
            var listString = new List<string>{"Benfica"};
            listConnections.Add(new Connection(1,1,listString,this.user1.Id,this.user2.Id));

            _repo.Setup(m => m.GetByIdAsync(this.user1.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user1));
            _repo.Setup(m => m.GetByIdAsync(this.user2.Id)).Returns(Task.FromResult<DDDSample1.Domain.Users.User>(this.user2));

            _connRepository.Setup(m => m.ConnectionsOfUser(this.user1.Id)).Returns(Task.FromResult<List<Connection>>(listConnections));

            var item = listConnections[0];

            var expected = new UserNetworkConnDTO(item.ConnectionStrength.Strength, item.RelationshipStrength.Strength,
                        ConvertTagsToString(item.Tags), new UserNetworkDTO(1,this.user2.Id.AsGuid(),this.user2._Email._Email,user2._Name?._Name,ConvertTagsToString(user2._InterestTags),user2._Avatar?._avatarUrl,user2._EmotionalState?.Emotion?._Emotion,new List<UserNetworkConnDTO>()));
                       

            var r = await _service.GetUserNetwork(this.user1.Id,1);
            var result = r.Connections[0];
            Assert.Equal(result,expected);
        }

        private List<string> ConvertTagsToString(IEnumerable<Tag> tags)
        {
            List<string> res = new List<string>();
            foreach (var item in tags)
            {
                res.Add(item.Description);
            }
            return res;
        }
        
        
      
    }
}