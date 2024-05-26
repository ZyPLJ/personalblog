using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using Personalblog.Model.ViewModels.Categories;

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
        public async Task<(List<VisitRecord>, PaginationMetadata)>GetPagedList(VisitRecordQueryParameters param)
        {
            var querySet = _myDbContext.visitRecords.AsQueryable();
            // 搜索
            if (!string.IsNullOrEmpty(param.Search))
            {
                querySet = querySet.Where(a => a.RequestPath.Contains(param.Search));
            }

            var data = await querySet
                .OrderByDescending(a => a.Id)
                .Skip((param.Page - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToListAsync();
            
            var pagination = new PaginationMetadata()
            {
                PageNumber = param.Page,
                PageSize = param.PageSize,
                TotalItemCount = await querySet.CountAsync()
            };
            
            return (data, pagination);
        }
    }
}
