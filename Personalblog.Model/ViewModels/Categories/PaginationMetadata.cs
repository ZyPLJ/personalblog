using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.ViewModels.Categories
{
    public class PaginationMetadata
    {
        public long PageCount { get; set; }

        public long TotalItemCount { get; set; }

        public long PageNumber { get; set; }

        public long PageSize { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool HasNextPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public long FirstItemOnPage { get; set; }

        public long LastItemOnPage { get; set; }
    }
}
