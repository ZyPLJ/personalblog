using Personalblog.Model;
using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PersonalblogServices.Links
{
    public class LinkService : ILinkService
    {
        private readonly MyDbContext _myDbContext;
        public LinkService(MyDbContext myDbContext) 
        {
            _myDbContext = myDbContext;
        }
        /// <summary>
        /// 获取全部友情链接
        /// </summary>
        /// <param name="onlyVisible">只获取显示的链接</param>
        public async Task<List<Link>> GetAll(bool onlyVisible = true)
        {
            return onlyVisible
                ? await _myDbContext.links.Where(a => a.Visible).ToListAsync()
                : await _myDbContext.links.ToListAsync();
        }

        public async Task<Link?> GetById(int id)
        {
            return await _myDbContext.links.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Link?> GetByName(string name)
        {
            return await _myDbContext.links.Where(a => a.Name == name).FirstOrDefaultAsync();
        }
        /// <summary>
        /// 查询 id 是否存在
        /// </summary>
        public async Task<bool> HasId(int id)
        {
            return await _myDbContext.links.Where(a => a.Id == id).AnyAsync();
        }

        public async Task<Link> AddOrUpdate(Link item)
        {
            var data = await _myDbContext.links.FirstOrDefaultAsync(l => l.Id == item.Id);
            if (data != null)
            {
                _myDbContext.links.Update(data);
            }
            else
            {
                await _myDbContext.links.AddAsync(item);
            }
            await _myDbContext.SaveChangesAsync();
            return item;
        }

        public async Task<Link?> SetVisibility(int id, bool visible)
        {
            var item = await GetById(id);
            if (item == null) return null;
            item.Visible = visible;
            _myDbContext.links.Update(item);
            await _myDbContext.SaveChangesAsync();
            return item;
        }

        public async Task<int> DeleteById(int id)
        {
            var link = await _myDbContext.links.FindAsync(id);
            if (link != null)
            {
                _myDbContext.links.Remove(link);
                return await _myDbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
