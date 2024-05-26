using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Personalblog.Model.ViewModels.Arc;

namespace Personalblog.Model.Entitys
{
    /// <summary>
    /// 文章类
    /// </summary>
    public class Post
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 前200字的概括
        /// </summary>
        public string Summary { get; set; }
        /// <summary>
        /// 文字内容 格式是markdown格式，交给前端去解析
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 相对路径
        /// </summary>
        public string? Path { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 上次更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// 导航属性
        /// </summary>
        public Category? Categories { get; set; }
        public List<Comment>? Comments { get; set; }
        /// <summary>
        /// 浏览量
        /// </summary>
        public int ViewCount { get; set; }
        /// <summary>
        /// 导航属性
        /// </summary>
        public List<Tag> Tags { get; set; }
        public List<PostTag> PostTags { get; } = new();
        /// <summary>
        /// 状态
        /// 0 未发布
        /// 1 已发布
        /// </summary>
        public int? Status { get; set; }
    }
}
