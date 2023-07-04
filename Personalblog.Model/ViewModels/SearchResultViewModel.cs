using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Model.ViewModels
{
    public class SearchResultViewModel
    {
        public string Keyword { get; set; }
        public List<Post> Posts { get; set; }
    }
}
