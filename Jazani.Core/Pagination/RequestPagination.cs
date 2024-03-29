﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jazani.Core.Pagination
{
    public class RequestPagination<T>
    {
        private int _page = 1;
        private int _perPage;

        public int Page { get => _page <= 0 ? 1 : _page; set => _page = value; }
        public int PerPage { get => _perPage <= 0 ? 10 : _perPage; set => _perPage = value; }
        public T? Filter { get; set; }

    }
}
