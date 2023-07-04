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
    /// 推荐图片
    /// </summary>
    public class FeaturedPhoto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PhotoId { get; set; }
        public Photo Photo { get; set; }
    }
}
