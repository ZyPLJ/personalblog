using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalblogServices.Links
{
    public interface ILinkService
    {
        Task<List<Link>> GetAll(bool onlyVisible = true);
        Task<Link?> GetById(int id);
        Task<Link?> GetByName(string name);
        Task<bool> HasId(int id);
        Task<Link> AddOrUpdate(Link item);
        Task<Link?> SetVisibility(int id, bool visible);
        Task<int> DeleteById(int id);
    }
}
