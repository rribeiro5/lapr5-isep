using DDDSample1.Domain.Shared;
using DDDSample1.Domain.Users;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace DDDSample1.Domain.Connections
{
    public class ConnectionService : IConnectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IUserRepository _userRepository;

        public ConnectionService(IUnitOfWork unitOfWork, IConnectionRepository connectionRepository, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _connectionRepository = connectionRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ConnectionDTO>> GetAllConnections(){
            var connections = await this._connectionRepository.GetAllAsync();

            List<ConnectionDTO> listDto = connections.ConvertAll<ConnectionDTO>(c => new ConnectionDTO(c.Id.AsGuid(),c.OUser,c.DUser,
            (c.Tags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)),c.ConnectionStrength.Strength,c.RelationshipStrength.Strength));
            return listDto;

        }

        public async Task<IEnumerable<BidirecionalConnectionDTO>> GetAllBidirectionalConnections(){
            var connections = await this._connectionRepository.GetAllAsync();

            List<BidirecionalConnectionDTO> biDto = new List<BidirecionalConnectionDTO>();

            foreach(Connection c in connections){
                UserId orig = c.OUser;
                UserId dest = c.DUser;

                var origToDest = await this._connectionRepository.bidirecionalConnections(orig,dest);
                var destToOrig = await this._connectionRepository.bidirecionalConnections(dest,orig);

                /*
                if(origToDest.Count !=1 || destToOrig.Count){
                    throw new BusinessRuleValidationException("Invalid Number of connections");
                }*/

                var fCon = origToDest[0];
                var lCon = destToOrig[0];

                biDto.Add(new BidirecionalConnectionDTO(fCon.OUser.AsGuid(),fCon.DUser.AsGuid(),fCon.ConnectionStrength.Strength,lCon.ConnectionStrength.Strength,fCon.RelationshipStrength.Strength,lCon.RelationshipStrength.Strength));

            }

            
            return biDto;
        }


        public async Task<UserConnectionsDTO> GetConnectionsOfUser(UserId userId)
        {
            var user = await checkUserIdAsync(userId);
            var connections = await this._connectionRepository.ConnectionsOfUser(userId);

            if (connections == null)
                return null;

            UserDto origDto = new UserDto(user.Id.AsGuid(),user._BirthDayDate?._BirthDayDate,user._Email?._Email,ConvertTagsToString(user._InterestTags));

            List<Tuple<Connection, UserDto>> conns = new List<Tuple<Connection, UserDto>>();
            foreach (Connection conn in connections)
            {
                var dest = await this._userRepository.GetByIdAsync(conn.DUser);
                UserDto destDto = new UserDto(dest.Id.AsGuid(),dest._BirthDayDate?._BirthDayDate,dest._Email?._Email,dest._Name?._Name,dest._Avatar?._avatarUrl,
                    dest._EmotionalState?.Emotion?._Emotion,ConvertTagsToString(dest._InterestTags));
                conns.Add(Tuple.Create(conn, destDto));
            }
            
            return new UserConnectionsDTO(conns.ConvertAll<ConnectionDTO>(e => 
                new ConnectionDTO(e.Item1.Id.AsGuid(), e.Item1.OUser, origDto, e.Item1.DUser, e.Item2,
                    ConvertTagsToString(e.Item1.Tags), e.Item1.ConnectionStrength.Strength, e.Item1.RelationshipStrength.Strength)));
        }

        public async Task<IEnumerable<UserDto>> GetPossibleDestinyUsers(UserId userId)
        {
            await checkUserIdAsync(userId);
            var destinyUsers = await this._connectionRepository.GetPossibleDestinyUsers(userId);

            if(destinyUsers == null)
                return null;

            return destinyUsers.ConvertAll<UserDto>(dU => new UserDto(dU.Id.AsGuid(),dU._BirthDayDate._BirthDayDate,dU._Email._Email,dU._Name._Name));
        }

        public async Task<ConnectionStrengthsDTO> GetStrengthOfConnection(ConnectionId connId)
        {
            var connection = await _connectionRepository.GetByIdAsync(connId);
            if (connection == null)
                return null;
            return new ConnectionStrengthsDTO(connection.Id.AsGuid(),
                connection.ConnectionStrength.Strength, connection.RelationshipStrength.Strength);
        }


        public async Task<ConnectionDTO> UpdateConnection(ConnectionId connId, ChangeConnInfoDTO connInfo)
        {
            var conn = await this._connectionRepository.GetByIdAsync(connId);
            if (conn == null)
            {
                return null;
            }

            conn.UpdateConnStrengthTags(connInfo.ConnectionStrength, connInfo.Tags);
            await this._unitOfWork.CommitAsync();

            return new ConnectionDTO(conn.Id.AsGuid(), conn.OUser, conn.DUser, ConvertTagsToString(conn.Tags), 
                conn.ConnectionStrength.Strength, conn.RelationshipStrength.Strength);
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

        private async Task<User> checkUserIdAsync(UserId userId)
        {
            var user = await this._userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new BusinessRuleValidationException("Invalid User Id");
            return user;
        }

    }
}