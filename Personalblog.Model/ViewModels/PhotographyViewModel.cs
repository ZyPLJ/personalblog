using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Personalblog.Model.ViewModels
{
    public class PhotographyViewModel
    {
        public IPagedList<Photo> photos { get; set; }
    }
}
