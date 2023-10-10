using Jazani.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Aplication.Cores.Services
{
    public interface IPaginatedService<TDto, TDtoFilter>
    {
        Task<ResponsePagination<TDto>> PaginatedSearch(RequestPagination<TDtoFilter> request);
    }
}
