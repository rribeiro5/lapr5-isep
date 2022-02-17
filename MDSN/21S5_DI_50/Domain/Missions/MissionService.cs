using DDDSample1.Domain.Shared;

namespace DDDSample1.Domain.Mission
{
    public class MissionService : IMissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMissionRepository _repo;

        public MissionService(IUnitOfWork unitOfWork, IMissionRepository repo)
        {
            _unitOfWork = unitOfWork;
            _repo = repo;
        }
    }
}