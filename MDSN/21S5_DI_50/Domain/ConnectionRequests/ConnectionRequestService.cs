using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDDSample1.Domain.Users;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections;

namespace DDDSample1.Domain.ConnectionRequests
{
    public class ConnectionRequestService : IConnectionRequestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRequestRepository _repo;
        private readonly IUserRepository _repoUser;
        private readonly IConnectionRepository _repoConnection;

        public ConnectionRequestService(IUnitOfWork unitOfWork, IRequestRepository repo, IUserRepository repoUsers, IConnectionRepository repoConnection)
        {
            this._unitOfWork = unitOfWork;
            this._repo = repo;
            this._repoUser = repoUsers;
            this._repoConnection = repoConnection;
        }

        public async Task<IEnumerable<ConnectionRequestDTO>> GetAllAsync()
        {
            var list = await this._repo.GetAllAsync();
            List<ConnectionRequestDTO> listDto = list.ConvertAll<ConnectionRequestDTO>(c => new ConnectionRequestDTO(c.Id.AsGuid(), c.OrigUser, c.InterUser, c.DestUser, c.MessageOrigToDest.Message, c.MessageOrigToInter.Message, c.MessageInterToDest.Message  ));
            return listDto;
        }

        public async Task<ConnectionRequestDTO> GetByIdAsync(RequestId id)
        {
            var c = await this._repo.GetByIdAsync(id);

            if (c == null) return null;

            return new ConnectionRequestDTO(c.Id.AsGuid(), c.OrigUser, c.InterUser, c.DestUser,
                c.MessageOrigToDest.Message, c.MessageOrigToInter.Message, c.MessageInterToDest.Message);
        }
        
        /// <sumary>
        /// Method that returns all the pending connections from the user
        /// Metodo que retorna todos pedidos de ligacao (diretas ou inderetas)
        /// </sumary>
        public async Task<IEnumerable<ConnectionRequestDTO>> getPendingConnectionsRequestOfUser(UserId id){
                var user = await this.CheckUser(id);

                if(user == null) 
                    return null;

                var pendingConnections = await this._repo.getPendingConnectionsRequestOfUser(id);

                if(pendingConnections == null)
                    return null;
                List<ConnectionRequest> list = pendingConnections as List<ConnectionRequest>;
                List<ConnectionRequestDTO> resultList = new List<ConnectionRequestDTO>();

                foreach(ConnectionRequest cr in list){
                    var idOrig = cr.OrigUser;
                    var orig = _repoUser.GetByIdAsync(idOrig).Result;

                    var origDto = new UserDto (orig.Id.AsGuid(),orig._BirthDayDate._BirthDayDate,orig._Email._Email, orig._Name._Name,orig._Avatar._avatarUrl ,orig._EmotionalState?.Emotion?._Emotion,(orig._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));

                    var idDest = cr.DestUser;
                    var dest = _repoUser.GetByIdAsync(idDest).Result;
                    var destDto = new UserDto (dest.Id.AsGuid(),dest._BirthDayDate._BirthDayDate,dest._Email._Email, dest._Name._Name,dest._Avatar._avatarUrl ,dest._EmotionalState?.Emotion?._Emotion,(dest._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));

                    if(cr.InterUser == null){
                        resultList.Add(new ConnectionRequestDTO(cr.Id.AsGuid(),idOrig,origDto,cr.InterUser,null,idDest,destDto,cr.MessageOrigToDest.Message,cr.MessageOrigToInter?.Message,cr.MessageInterToDest?.Message));
                        continue;
                    }

                    var idInter = cr.InterUser;
                    var inter = _repoUser.GetByIdAsync(idInter).Result;
                    var interDto = new UserDto (inter.Id.AsGuid(),inter._BirthDayDate._BirthDayDate,inter._Email._Email, inter._Name._Name,inter._Avatar._avatarUrl ,inter._EmotionalState?.Emotion?._Emotion,(inter._InterestTags.ToList<Tag>().ConvertAll<string>(tag => tag.Description)));


                    resultList.Add(new ConnectionRequestDTO(cr.Id.AsGuid(),idOrig,origDto,idInter,interDto,idDest,destDto,cr.MessageOrigToDest.Message,cr.MessageOrigToInter?.Message,cr.MessageInterToDest?.Message));

                }


                return resultList;
        }

         public async Task<ConnectionRequestDTO> CreateIntroductionRequest(CreatingIntroductionRequestDTO dto){
            
             if(dto.OrigUser.Equals(dto.DestUser)|| dto.OrigUser.Equals(dto.InterUser) || dto.InterUser.Equals(dto.DestUser))
                throw new BusinessRuleValidationException("Can't send request to the same person");

             var origUserId = new UserId(dto.OrigUser);
             var interUserId = new UserId(dto.InterUser);
             var destUserId = new UserId(dto.DestUser);

             var origUser = await this.CheckUser(origUserId);
             var interUser = await this.CheckUser(interUserId);
             var destUser = await this.CheckUser(destUserId);

            if(origUser==null || interUser == null || destUser == null) 
                throw new BusinessRuleValidationException("At least one user is not registered");

            await this._repoUser.validIntroductionRequest(origUserId,interUserId,destUserId);

            var req = new ConnectionRequest(origUserId,interUserId,destUserId,dto.MessageOrigToDest,dto.MessageOrigToInter,dto.ConnectionStrength,dto.Tags);

            await this._repo.RegisterConnectionRequest(req);

            await this._unitOfWork.CommitAsync();

            return new ConnectionRequestDTO(req.Id.AsGuid(),req.OrigUser,req.InterUser,req.DestUser,
                    req.MessageOrigToDest.Message,req.MessageOrigToInter.Message);

         }

         public async Task<IEnumerable<ConnectionRequestDTO>> GetPendingApprovalRequestsOfUser(UserId id){
             var user = await this._repoUser.GetByIdAsync(id);

             if(user == null) 
                 return null;

             var pendingConnections = await _repo.GetPendingApprovalRequestsOfUser(id);

             var list = pendingConnections as List<ConnectionRequest>;
             return  list?.ConvertAll(c => new ConnectionRequestDTO(c.Id.AsGuid(), 
                 c.OrigUser, c.InterUser, c.DestUser, c.MessageOrigToDest?.Message, 
                 c.MessageOrigToInter?.Message, c.MessageInterToDest?.Message));
         }


        public async Task<ConnectionRequestDTO> UpdateApprovalState(UpdatedApprovalStateRequestDTO dto)
        {
            var c = await this._repo.GetByIdAsync(new RequestId(dto.Id)); 

            if (c == null)
                return null;
            if (!c.OnApproval)
            {
                throw new BusinessRuleValidationException("This request is not on approval stage.");
            }
            if (dto.Approved) c.Approve(dto.MessageInterToDest);
            else c.Disapproved();
            
            await this._unitOfWork.CommitAsync();
            
            
            return new ConnectionRequestDTO(c.Id.AsGuid(), c.OrigUser, c.InterUser, c.DestUser,
                c.MessageOrigToDest.Message, c.MessageOrigToInter.Message, c.MessageInterToDest?.Message);
        }
        
        
        public async Task<ConnectionRequestDTO> AddAsync(CreatingRequestDTO dto)
        {   

            var orig = new UserId(dto.OrigUser);
            var dest = new UserId(dto.DestUser);

            var origUser = await this.CheckUser(orig);
            var destUser = await this.CheckUser(dest);
            
            if(origUser==null || destUser == null) 
                throw new BusinessRuleValidationException("At least one user is not registered");

            if(dto.OrigUser.Equals(dto.DestUser) )
                throw new BusinessRuleValidationException("Can't send request to the same person");
            
            var requests = await GetRequestsInAcceptance(dest);

            if (requests == null)
            {
                throw new BusinessRuleValidationException("There was an error verifying the connections. Try again later");
            }

            var lst = requests.Requests;
            foreach (var r in lst)
            {
                if (r.OrigUser.AsGuid().Equals(dto.OrigUser))
                    throw new BusinessRuleValidationException("A Request was already sent to this user");
            }

            var friendship = await _repoConnection.bidirecionalConnections(orig,dest);

            if(friendship.Count != 0){
                throw new BusinessRuleValidationException("These users are already friends");
            }
  
            var request = new ConnectionRequest(orig, dest, 
                dto.MessageOrigToDest, dto.ConnStrengthReq, dto.ConnTagsReq);

            await this._repo.AddAsync(request);

            await this._unitOfWork.CommitAsync();

            return new ConnectionRequestDTO{Id = request.Id.AsGuid(), DestUser = request.DestUser, 
                MessageOrigToDest = request.MessageOrigToDest.Message, OrigUser = request.OrigUser};
        }

        public async Task<UserRequestsDTO> GetRequestsInAcceptance(UserId userId)
        {
            var user = await this.CheckUser(userId);
            if(user == null) 
                return null;

            var reqs = await _repo.GetRequestsInAcceptanceOfUser(userId);
            if (reqs == null)
                return null;
            
            UserDto origDto = new UserDto(user.Id.AsGuid(),user._BirthDayDate?._BirthDayDate,user._Email?._Email,ConvertTagsToString(user._InterestTags));

            List<Tuple<ConnectionRequest,UserDto,UserDto>> requests = new List<Tuple<ConnectionRequest, UserDto, UserDto>>();
            foreach (ConnectionRequest req in reqs)
            {
                var dest = await this._repoUser.GetByIdAsync(req.DestUser);
                UserDto destDto = new UserDto(dest.Id.AsGuid(),dest._BirthDayDate?._BirthDayDate,dest._Email?._Email,ConvertTagsToString(dest._InterestTags));
                var inter = req.InterUser == null ? null : await this._repoUser.GetByIdAsync(req.InterUser);
                UserDto interDto = inter == null ? null : new UserDto(inter.Id.AsGuid(),inter._BirthDayDate?._BirthDayDate,inter._Email?._Email,ConvertTagsToString(inter._InterestTags));
                requests.Add(Tuple.Create(req, interDto, destDto));
            }
            
            return new UserRequestsDTO(requests.ConvertAll<ConnectionRequestDTO>(c => 
                new ConnectionRequestDTO(c.Item1.Id.AsGuid(), c.Item1.OrigUser, origDto, c.Item1.InterUser, c.Item2, c.Item1.DestUser, c.Item3,
                c.Item1.MessageOrigToDest.Message, c.Item1.MessageOrigToInter == null ? null : c.Item1.MessageOrigToInter.Message, 
                c.Item1.MessageInterToDest == null ? null : c.Item1.MessageInterToDest.Message)));
        }

        public async Task<ResultConnectionDTO> RequestAcceptance(RequestId reqId, RequestAcceptanceDTO answer)
        {
            var request = await _repo.GetByIdAsync(reqId);
            if (request == null)
            {
                return null;
            }
            if (!request.InAcceptance)
            {
                throw new BusinessRuleValidationException("This request is not in acceptance stage.");
            }
            
            if (!answer.Answer)
            {
                request.RejectRequest();
                await this._unitOfWork.CommitAsync();
                return new ResultConnectionDTO(Guid.Empty, Guid.Empty, null, null);
            }

            request.AcceptRequest();

            var conn1 = new Connection(request.ConnStrengthReq.Strength, 0, 
                request.ConnTagsReq.ConvertAll<string>(e => e.Description), request.OrigUser, request.DestUser);
            await this._repoConnection.AddAsync(conn1);

            var conn2 = new Connection(answer.ConnectionStrength, 0, answer.Tags, request.DestUser, request.OrigUser);
            await this._repoConnection.AddAsync(conn2);

            await this._unitOfWork.CommitAsync();

            return new ResultConnectionDTO(conn1.OUser.AsGuid(), conn2.OUser.AsGuid(),
                new ConnectionInfoDTO(conn1.Id.AsGuid(), conn1.ConnectionStrength.Strength, conn1.RelationshipStrength.Strength, ConvertTagsToString(conn1.Tags)),
                new ConnectionInfoDTO(conn2.Id.AsGuid(), conn2.ConnectionStrength.Strength, conn2.RelationshipStrength.Strength, ConvertTagsToString(conn2.Tags)));
        }

        private async Task<User> CheckUser(UserId id){
            return await this._repoUser.GetByIdAsync(id);
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