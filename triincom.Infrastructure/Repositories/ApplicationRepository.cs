using Microsoft.EntityFrameworkCore;
using triincom.Core.DTO;
using triincom.Core.Entities;
using triincom.Core.Enums;
using triincom.Core.Interface;
using triincom.DataPersistence.AppContext;

namespace triincom.Infrastructure.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationEntity> AddAsync(ApplicationEntity application)
        {
            application.Status = Status.Published;
            application.Number = Guid.NewGuid().ToString();
            application.CreatedAt = DateTime.UtcNow;
            application.ModifiedAt = DateTime.UtcNow;
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task ChangeStatus(ChangeStatusDto StatusDto)
        {
            var application = await GetByNumberAsync(StatusDto.Number);
            if (application == null)
                throw new Exception("number not found");

            application.Status = StatusDto.Status;
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Applications.FindAsync(id);
        }

        public async Task<ApplicationEntity?> GetByNumberAsync(string number)
        {
            return await _context.Applications.FirstOrDefaultAsync(x => x.Number == number);
        }

        public async Task<PaginatedResult<ApplicationEntity>> GetFilteredAsync(ApplicationFilterDto filter)
        {
            if (filter == null)
            {
                var allApplications = await _context.Applications.ToListAsync();
                return new PaginatedResult<ApplicationEntity>
                {
                    Data = allApplications,
                    TotalCount = allApplications.Count,
                    PageNumber = 1,
                    PageSize = allApplications.Count
                };
            }

            var query = _context.Applications.AsQueryable();

            if (filter.Status != null)
            {
                query = query.Where(x => x.Status == filter.Status);
            }

            if (filter.MinAmount != null)
            {
                query = query.Where(x => x.Amount >= filter.MinAmount);
            }

            if (filter.MaxAmount != null)
            {
                query = query.Where(x => x.Amount <= filter.MaxAmount);
            }

            if (filter.MinTerm != null)
            {
                query = query.Where(x => x.TermValue >= filter.MinTerm);
            }

            if (filter.MaxTerm != null)
            {
                query = query.Where(x => x.TermValue <= filter.MaxTerm);
            }

            var totalCount = await query.CountAsync();

            if (filter.PageNumber.HasValue && filter.PageSize.HasValue)
            {
                var pageNumber = filter.PageNumber.Value;
                var pageSize = filter.PageSize.Value;

                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            var data = await query.ToListAsync();

            return new PaginatedResult<ApplicationEntity>
            {
                Data = data,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber ?? 1,
                PageSize = filter.PageSize ?? totalCount
            };
        }       
    }
}
