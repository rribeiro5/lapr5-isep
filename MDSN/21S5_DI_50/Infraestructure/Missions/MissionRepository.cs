using DDDSample1.Domain.Mission;
using DDDSample1.Infrastructure.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDDSample1.Infrastructure.Missions
{
    public class MissionRepository : BaseRepository<Mission, MissionId>,IMissionRepository
    {
        public MissionRepository(DDDSample1DbContext context) : base(context.Missions)
        {
        }
    }
}