using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Personalblog.Model.ViewModels;
using Qiniu.Storage;
using Qiniu.Util;
using StackExchange.Profiling.Internal;

namespace Personalblog.Services;

public class QiniuService
{
    private readonly QiniuCDNOptions _qiniuCDNOptions;
    private List<string> imageList { get; set; }

    public QiniuService(IOptions<QiniuCDNOptions> qiniuCDNOptions)
    {
        _qiniuCDNOptions = qiniuCDNOptions.Value;
        imageList = GetImageListAsync(_qiniuCDNOptions.AccessKey, _qiniuCDNOptions.SecretKey,"zypljblog").Result;
    }
    // 获取存储空间中的图片列表
    static async Task<List<string>> GetImageListAsync(string accessKey, string secretKey, string bucket)
    {
        return await Task.Run(() =>
        {
            Mac mac = new Mac(accessKey, secretKey);
            Config config = new Config();
            BucketManager bucketManager = new BucketManager(mac, config);
            ListResult listResult = bucketManager.ListFiles(bucket, "Top", "", 1000, "");

            var data = JsonConvert.DeserializeObject<dynamic>(listResult.Text);
            List<string> keys = new List<string>();
            foreach (var item in data.items)
            {
                keys.Add(item.key.ToString());
            }
            return keys;
        });
    }
    
    // 生成私有访问链接
    static async Task<string> GeneratePrivateUrlAsync(string accessKey, string secretKey, string image)
    {
        return await Task.Run(() =>
        {
            Mac mac = new Mac(accessKey, secretKey);
            string domain = "https://cdn.pljzy.top";
            string privateUrl = DownloadManager.CreatePrivateUrl(mac, domain, image, 300);
            return privateUrl;
        });
    }
    
    // 创建一个随机获取存储空间中图片的接口
    public async Task<string> GetRandomImageAsync()
    {
        int index = Random.Shared.Next(imageList.Count);
        string image = imageList[index];
        string privateUrl = await GeneratePrivateUrlAsync(_qiniuCDNOptions.AccessKey, _qiniuCDNOptions.SecretKey, image);
        return privateUrl;
    }
}
