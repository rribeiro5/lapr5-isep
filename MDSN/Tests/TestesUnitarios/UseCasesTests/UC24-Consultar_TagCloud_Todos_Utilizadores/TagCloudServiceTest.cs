using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDNetCore.Domain.Generics;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using Moq;
using Xunit;

namespace Tests.TestesUnitarios.UseCasesTests.UC24_Consultar_TagCloud_Todos_Utilizadores
{
    public class TagCloudServiceTest
    {
        private readonly Mock<IUnitOfWork> _unit;
        private readonly Mock<IUserRepository> _repoUser;
        private readonly Mock<IConnectionRepository> _repoConn;
        private readonly TagService _service;
        private Dictionary<Tag, int> _repoResult;
        private List<TagCloudDTO> _result;
        
        public TagCloudServiceTest()
        {
            _unit = new Mock<IUnitOfWork>();
            _repoUser = new Mock<IUserRepository>();
            _repoConn = new Mock<IConnectionRepository>();
            _service = new TagService(_repoUser.Object,_repoConn.Object);
            _unit.Setup(x => x.CommitAsync()).Returns(Task.FromResult(1));
            _repoResult = new Dictionary<Tag, int>();
            _repoResult.Add(new Tag("tag1"),5);
            _repoResult.Add(new Tag("tag2"),13);
            _repoResult.Add(new Tag("tag3"),13);
            _repoResult.Add(new Tag("tag4"),1);
            _result = new List<TagCloudDTO>()
            {
                new("tag1", 5),
                new("tag2", 13),
                new("tag3", 13),
                new("tag4", 1),
            };
        }
        
        [Fact]
        public async void ReturnEmptyList()
        {
            _repoUser.Setup(x => x.GetUsersTagCloud()).Returns(Task.FromResult(new Dictionary<Tag, int>()));
            var result = await _service.GetUsersTagCloud();
            Assert.Empty(result);
        }
        
        [Fact]
        public async void ReturnExpectedList()
        {
            _repoUser.Setup(x => x.GetUsersTagCloud()).Returns(Task.FromResult(_repoResult));
            var result = await _service.GetUsersTagCloud();
            Assert.Empty(_result.Intersect(result));
        }
        
        [Fact]
        public async void ReturnListOfCount()
        {
            _repoUser.Setup(x => x.GetUsersTagCloud()).Returns(Task.FromResult(_repoResult));
            var result = await _service.GetUsersTagCloud();
            Assert.Equal(4, result.Count);
        }
        
    }
}