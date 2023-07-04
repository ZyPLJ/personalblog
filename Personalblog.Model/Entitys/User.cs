using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.Entitys
{
    public class User
    {
        [Key] //主键
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
