using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FCategory
{
    public interface IFCategoryService
    {
        List<FeaturedCategory> GetFeaturedCategories();
    }
}
