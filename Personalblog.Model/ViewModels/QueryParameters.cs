namespace Personalblog.Model.ViewModels
{
    public class QueryParameters
    {
        /// <summary>
        /// 最大页面条目
        /// </summary>
        public const int MaxPageSize = 50;

        private int _pageSize = 10;

        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 搜索关键词
        /// </summary>
        public string? Search { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; } = 0;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string? SortBy { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public int TagId { get; set; } = 0;
    }
}
