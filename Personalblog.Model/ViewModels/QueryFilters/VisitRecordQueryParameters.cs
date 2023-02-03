using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.ViewModels.QueryFilters
{
    public  class VisitRecordQueryParameters:QueryParameters
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        public new string? SortBy { get; set; } = "-Time";
    }
}
