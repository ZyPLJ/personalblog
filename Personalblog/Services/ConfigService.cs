using Personalblog.Model;
using Personalblog.Model.Entitys;

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
    }
}
