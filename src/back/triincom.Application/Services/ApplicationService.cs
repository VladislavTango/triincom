using AutoMapper;
using triincom.Core.DTO;
using triincom.Core.Entities;
using triincom.Core.Interface;

namespace triincom.Application.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repository;
        private readonly IMapper _mapper;

        public ApplicationService(IApplicationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task AddAplication(AddApplicationDto applicationDto)
        {
            if (applicationDto.InterestValue <= 0 ||
                applicationDto.TermValue <= 0 ||
                applicationDto.Amount <= 0)
                throw new Exception("не все данные заполнены");

            var application = _mapper.Map<ApplicationEntity>(applicationDto);

            await _repository.AddAsync(application);
        }

        public async Task ChangeApplicationStatus(ChangeStatusDto statusDto)
        {
            await _repository.ChangeStatus(statusDto);
        }

        public async Task<PaginatedResult<ApplicationEntity>> GetAllApplicationsAsync()
        {
            return await _repository.GetFilteredAsync(null);
        }

        public async Task<PaginatedResult<ApplicationEntity>> GetFilteredApplicationsAsync(ApplicationFilterDto filter)
        {
            return await _repository.GetFilteredAsync(filter);
        }
    }
}
