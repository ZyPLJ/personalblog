using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using System.Linq;
using X.PagedList;

namespace Personalblog.Services
{
    public class VisitRecordService
    {
        private readonly MyDbContext _myDbContext;
        public VisitRecordService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        /// <summary>
        /// 总览数据
        /// </summary>
        /// <returns></returns>
        public object Overview()
        {
            return _myDbContext.visitRecords.
                Where(a => !a.RequestPath.StartsWith("/Api")).
                GroupBy(a => new {}).
                Select(g => new
                {
                    TotalVisit = g.Count(),
                    TodayVisit = g.Sum(g=>g.Time.Date == DateTime.Today ? 1 : 0),
                    YesterdayVisit = g.Sum(g=>g.Time.Date == DateTime.Today.AddDays(-1).Date ? 1 : 0)
                });
        }

        /// <summary>
        /// 趋势数据
        /// </summary>
        /// <param name="days">查看最近几天的数据，默认7天</param>
        /// <returns></returns>
        public object Trend(int days = 7)
        {
            return _myDbContext.visitRecords.
                Where(a => !a.RequestPath.StartsWith("/Api")).
                GroupBy(a => a.Time.Date).
                Select(a => new
                {
                    time = a.Key,
                    date = $"{a.Key.Month}-{a.Key.Day}",
                    count = a.Count()
                }).ToList();
        }
        public List<VisitRecord> GetAll()
        {
            return _myDbContext.visitRecords.OrderByDescending(a => a.Time).ToList();
        }
        public IPagedList<VisitRecord> GetPagedList(VisitRecordQueryParameters param)
        {
            var querySet = _myDbContext.visitRecords.ToArray();
            // 搜索
            if (!string.IsNullOrEmpty(param.Search))
            {
                querySet = querySet.Where(a => a.RequestPath.Contains(param.Search)).ToArray();
            }
            // 排序
            //if (!string.IsNullOrEmpty(param.SortBy))
            //{
            //    // 是否升序
            //    var isAscending = !param.SortBy.StartsWith("-");
            //    var orderByProperty = param.SortBy.Trim('-');
            //    if (isAscending)
            //    {
            //        querySet = querySet.OrderBy(a => a.Time).ToArray();
            //    }
            //    else
            //    {
            //        querySet = querySet.OrderByDescending(a => a.Time).ToArray();
            //    }
            //}
            querySet = querySet.OrderByDescending(a => a.Id).ToArray();
            return querySet.ToList().ToPagedList(param.Page, param.PageSize);
        }
    }
}
