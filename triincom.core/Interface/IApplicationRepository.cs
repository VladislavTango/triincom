using triincom.Core.DTO;
using triincom.Core.Entities;
using triincom.Core.Enums;

namespace triincom.Core.Interface
{
    public interface IApplicationRepository
    {
        Task<ApplicationEntity?> GetByIdAsync(Guid id);
        Task<ApplicationEntity?> GetByNumberAsync(string number);
        Task<PaginatedResult<ApplicationEntity>> GetFilteredAsync(ApplicationFilterDto filter);
        Task<ApplicationEntity> AddAsync(ApplicationEntity application);
        Task ChangeStatus(ChangeStatusDto StatusDto);
    }
}
