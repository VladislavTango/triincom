using triincom.Core.DTO;
using triincom.Core.Entities;
using triincom.Core.Enums;

namespace triincom.Core.Interface
{
    public interface IApplicationService
    {
        Task<PaginatedResult<ApplicationEntity>> GetFilteredApplicationsAsync(ApplicationFilterDto filter);
        Task<PaginatedResult<ApplicationEntity>> GetAllApplicationsAsync();
        Task AddAplication(AddApplicationDto applicationDto);
        Task ChangeApplicationStatus(ChangeStatusDto statusDto);
    }
}
