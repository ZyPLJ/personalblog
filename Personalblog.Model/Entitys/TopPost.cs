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
    /// 置顶文章
    /// </summary>
    public class TopPost
    {
        [Key] //主键
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增
        public int Id { get; set; }
        public string PostId { get; set; }
        public Post Post { get; set; }
    }
}
