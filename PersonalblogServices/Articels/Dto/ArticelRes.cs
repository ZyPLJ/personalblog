using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.Articels.Dto
{
    public class ArticelRes
    {
        public string Title { get; set; }
        public string? Summary { get; set; }
        public string? Content { get; set; }
        public string? Path { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
    }
}
