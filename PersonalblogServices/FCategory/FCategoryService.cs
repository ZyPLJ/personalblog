using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.FCategory
{
    
    public class FCategoryService : IFCategoryService
    {
        private readonly MyDbContext _myDbContext;
        public FCategoryService(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }

        public List<FeaturedCategory> GetFeaturedCategories()
        {
             return _myDbContext.featuredCategories.Include(f=>f.Category).ToList();
        }
    }
}
