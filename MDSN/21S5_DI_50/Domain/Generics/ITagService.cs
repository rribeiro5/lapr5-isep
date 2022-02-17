using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDDNetCore.Domain.Generics
{
    public interface ITagService
    {
        Task<List<TagCloudDTO>> GetUserTagCloud(Guid id);
        Task<List<TagCloudDTO>> GetUsersTagCloud();
        Task<List<TagCloudDTO>> GetAllUserConnectionsTagCloud(Guid userId);
        Task<List<TagCloudDTO>> GetAllConnectionsTagCloud();
    }
}