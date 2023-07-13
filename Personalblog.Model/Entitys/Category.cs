using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.Entitys
{
    /// <summary>
    /// 文章类别类
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 类别id
        /// </summary>
        [Key] //主键
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增
        public int Id { get; set; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string? Name { get; set; }

        public int? ParentId { get; set; } = 0;
        public Category? Parent { get; set; }
        public List<Post>? Posts { get; set; }
        /// <summary>
        /// 分类是否可见
        /// </summary>
        public bool Visible { get; set; } = true;
    }
}
