using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;
using PersonalblogServices.Response;

namespace Personalblog.Services
{
    public class ConfigService
    {
        private readonly IConfiguration _config;
        private readonly MyDbContext _mydbContext;
        public ConfigService(IConfiguration config, MyDbContext mydbContext)
        {
            _config = config;
            _mydbContext = mydbContext;
        }
        public List<ConfigItem> GetAll()
        {
            return _mydbContext.configItems.ToList();
        }
        public ConfigItem? GetById(int id)
        {
            return _mydbContext.configItems.Where(a => a.Id == id).First();
        }
        public ConfigItem? GetByKey(string key)
        {
            var item = _mydbContext.configItems.FirstOrDefault(a => a.Key == key);
            if(item == null)
            {
                // 尝试读取初始化配置
                var section = _config.GetSection($"Personalblog:Initial:{key}");
                if (!section.Exists()) return null;
                item = new ConfigItem { Key = key, Value = section.Value, Description = "Initial" };
                item = AddOrUpdate(item);
            }
            return item;
        }
        public ConfigItem AddOrUpdate(ConfigItem item)
        {
            if (item.Id == 0)
            {
                //新增
                _mydbContext.configItems.Add(item);
            }
            else
            {
                //修改
                _mydbContext.Update(item);
            }
            try
            {
                _mydbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return item;
        }
        public string this[string key]
        {
            get {
                var item = GetByKey(key);
                return item == null ? "" : item.Value;
            }
            set {
                var item = GetByKey(key) ?? new ConfigItem { Key = key };
                item.Value = value;
                AddOrUpdate(item);
            }
        }

        public async Task<ApiResponse> IsShow(int id)
        {
            var data = await _mydbContext.configItems.FirstOrDefaultAsync(c => c.Id == id);
            if (data.IsShowComment)
            {
                data.IsShowComment = false;
            }
            else
            {
                data.IsShowComment = true;
            }
            await _mydbContext.SaveChangesAsync();
            return new ApiResponse() { Message = "修改成功！" };
        }

        public async Task<ApiResponse> DelAsync(int id)
        {
            ConfigItem configItem = new ConfigItem() { Id = id };
            _mydbContext.configItems.Attach(configItem);
            _mydbContext.configItems.Remove(configItem);
            try
            {
                await _mydbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse() { Message = $"删除失败,{e.Message}", StatusCode = 500 };
            }

            return new ApiResponse() { Message = $"删除成功" };
        }

        public async Task<ApiResponse> AddAsync(ConfigItem configItem)
        {
            try
            {
                await _mydbContext.configItems.AddAsync(configItem);
                await _mydbContext.SaveChangesAsync();
                return new ApiResponse() { Message = "添加成功！" };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new ApiResponse() { Message = $"添加失败！{e.Message}" ,StatusCode = 500};
            }
        }
    }
}
