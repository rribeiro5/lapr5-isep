using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDSample1.Domain.Connections;
using DDDSample1.Domain.Users;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DDDNetCore.Domain.Generics
{
    public class TagService : ITagService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConnectionRepository _connRepo;
 
        public TagService(IUserRepository userRepo, IConnectionRepository connRepo)
        {
            _userRepo = userRepo;
            _connRepo = connRepo;
        }

        public async Task<List<TagCloudDTO>> GetUserTagCloud(Guid id)
        {
            var userId = new UserId(id);

            /*
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }*/

            var tagCloudDict =await _userRepo.GetUserTagCloud(userId);
            return TagCloudDictToTagCloudDtoList(tagCloudDict);
        }

        public async Task<List<TagCloudDTO>> GetUsersTagCloud()
        {
            var tagCloudDict =await _userRepo.GetUsersTagCloud();
            
            if(tagCloudDict == null)
                return null;

            return TagCloudDictToTagCloudDtoList(tagCloudDict);
        }

        private List<TagCloudDTO> TagCloudDictToTagCloudDtoList(IDictionary<Tag,int> tagCloudDict)
        {
            var tagCloudList = new List<TagCloudDTO>();

            foreach (var (key, value) in tagCloudDict)
            {
                var dto = new TagCloudDTO(key.Description, value);
                tagCloudList.Add(dto);
            }
            return tagCloudList;
        }

        public async Task<List<TagCloudDTO>> GetAllUserConnectionsTagCloud(Guid userId)
        {
            var connections = await _connRepo.BidirecionalConnectionsOfUser(new UserId(userId));
            
            return connections == null ? null : GetTagCloudConn(connections);
        }

        public async Task<List<TagCloudDTO>> GetAllConnectionsTagCloud()
        {
            var connections = await _connRepo.GetAllAsync();
            
            return connections == null ? null : GetTagCloudConn(connections);
        }

        private List<TagCloudDTO> GetTagCloudConn(List<Connection> connections)
        {
            var res = new List<TagCloudDTO>();
            var tags = new List<Tag>();
            foreach (var c in connections)
            {
                tags.AddRange(c.Tags);
                foreach (var t in tags)
                {
                    var element = res.Find(x => x.value.Equals(t.Description));
                    if (element != null)
                    {
                        element.count += 1;
                        
                    }
                    else
                    {
                        res.Add(new TagCloudDTO(t.Description,1));
                    }
                }
                tags.Clear();
                
            }
            return res;
        }
        
    }
}