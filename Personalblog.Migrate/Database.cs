using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Personalblog.Migrate
{
    public class Database
    {
        //注入服务
        private readonly MyDbContext _myDbContext;


        public Database(MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
        }
        /// <summary>
        /// 这个方法用来添加目录
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="categoryNames"></param>
        public void InsertC(List<Category> categories, string[] categoryNames)
        {
            var rootCategory = _myDbContext.categories.FirstOrDefault((a => a.Name == categoryNames[1]));
            if(rootCategory == null)
            {
                _myDbContext.Entry<Category>(new Category { Name = categoryNames[1]}).State = EntityState.Added;
                int result = _myDbContext.SaveChanges();
                if (result > 0)
                {
                    Console.WriteLine("创建分类成功！");
                }
            }
            else
            {
                categories.Add(rootCategory);
                Console.WriteLine($"+ 添加分类: {rootCategory.Id}.{rootCategory.Name}");
            }
        }
        /// <summary>
        /// 查一下目录 搞出他的id来
        /// </summary>
        /// <param name="categoryNames"></param>
        /// <returns></returns>
        public Category Select(string categoryNames)
        {
            return _myDbContext.categories.Where(c => c.Name == categoryNames).First();
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="post"></param>
        public void Add(Post post)
        {
            //先查一下 如果文章有，说明这是修改后的文章，然后改一下修改时间和内容就行
            var p = _myDbContext.posts.FirstOrDefault(p => p.Title == post.Title);
            if(p != null)
            {
                p.Content = post.Content;
                p.LastUpdateTime = DateTime.Now;
                _myDbContext.SaveChanges();
            }
            else
            {
                try
                {
                    _myDbContext.Entry<Post>(post).State = EntityState.Added;
                    int result = _myDbContext.SaveChanges();
                    if (result > 0)
                    {
                        Console.WriteLine("添加文章成功！");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }
    }
}
