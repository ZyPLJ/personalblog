using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// 配置项目
/// </summary>
namespace Personalblog.Model.Entitys
{
    public class ConfigItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] //主键
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
        public string? Description { get; set; }
        public bool IsShowComment { get; set; }
    }
}
