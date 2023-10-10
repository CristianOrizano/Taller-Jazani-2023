using Jazani.Core.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Taller.Domain.Cores.Paginations
{
    public interface IPaginator<T>
    {
        Task<ResponsePagination<T>> Paginate(IQueryable<T> query, RequestPagination<T> request);
    }
}
