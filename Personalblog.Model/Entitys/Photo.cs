using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.Entitys
{
    /// <summary>
    /// 图片类
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key] //主键
        public string? Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// 照片的拍摄地点
        /// </summary>
        public string? Location { get; set; }
        /// <summary>
        /// 存储的相对路径
        /// </summary>
        public string? FilePath { get; set; }
        /// <summary>
        /// 图片高度
        /// </summary>
        public long Height { get; set; }
        /// <summary>
        /// 图片宽度
        /// </summary>
        public long Width { get; set; }
        /// <summary>
        /// 图片创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 压缩后的图片路径
        /// </summary>
        public string? YPath { get; set; }
    }
}
