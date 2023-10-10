using Jazani.Core.Pagination;
using Jazani.Taller.Domain.Cores.Paginations;
using Jazani.Taller.Domain.Mc.Models;
using Jazani.Taller.Domain.Mc.Repositories;
using Jazani.Taller.Infrastructure.Cores.Context;
using Jazani.Taller.Infrastructure.Cores.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Infrastructure.Mc.Persistences
{
    public class InvestmentRepository : CrudRepository<Investment, int>, IInvestmentRepository
    {
        private readonly AplicationDbContext _dbContext;
        private readonly IPaginator<Investment> _paginator;
        public InvestmentRepository(AplicationDbContext dbContext, IPaginator<Investment> paginator) : base(dbContext)
        {
            _dbContext = dbContext;
            _paginator = paginator;
        }

        public override async Task<IReadOnlyList<Investment>> FindAllAsync()
        {
            return await _dbContext.Set<Investment>()
               .Include(i => i.Investmentconcept).Include(i => i.Investmenttype) 
               .Include(i => i.MeasureUnit).Include(i => i.PeriodType).Include(i => i.Holder)
               .Include(i => i.MiningConcession)
               .AsNoTracking()
               .ToListAsync();
        }
        public override async Task<Investment?> FindByIdAsync(int id)
        {
            return await _dbContext.Set<Investment>()
                .Include(i => i.Investmentconcept).Include(i => i.Investmenttype)
               .Include(i => i.MeasureUnit).Include(i => i.PeriodType).Include(i => i.Holder)
                .Include(i => i.MiningConcession)
               .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ResponsePagination<Investment>> PaginatedSearch(RequestPagination<Investment> request)
        {
            var filter = request.Filter;
            var query = _dbContext.Set<Investment>().AsQueryable();

            if (filter is not null)
            {
                query = query
                    .Where(x =>
                        (string.IsNullOrWhiteSpace(filter.Description) || x.Description.ToUpper().Contains(filter.Description.ToUpper()))       
                   && (filter.CurrencyTypeid == null || x.CurrencyTypeid == filter.CurrencyTypeid)
                    );
            }
            query = query.OrderByDescending(x => x.Id)
              .Include(t => t.Investmentconcept)
            .Include(t => t.Holder)
            .Include(t => t.Investmenttype)
            .Include(t => t.MiningConcession)
            .Include(t => t.MeasureUnit)
            .Include(t => t.PeriodType)
            ;

            return await _paginator.Paginate(query, request);
        }
    }
}
