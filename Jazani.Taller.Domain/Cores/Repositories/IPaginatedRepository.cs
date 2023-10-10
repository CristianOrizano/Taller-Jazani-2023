using Jazani.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Domain.Cores.Repositories
{
    public interface IPaginatedRepository<T>
    {
        Task<ResponsePagination<T>> PaginatedSearch(RequestPagination<T> request);
    }
}
