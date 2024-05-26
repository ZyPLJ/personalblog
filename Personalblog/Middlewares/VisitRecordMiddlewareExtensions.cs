using Personalblog.Extensions;
using Personalblog.Model.Entitys;
using Personalblog.Model;

namespace Personalblog.Middlewares
{
    public static class VisitRecordMiddlewareExtensions
    {
        public static IApplicationBuilder UseVisitRecordMiddleware(this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                var request = context.Request;
                var response = context.Response;
                string ip = context.GetRemoteIPAddress()?.ToString().Split(":")?.Last();
                //ip == "222.244.120.104" || ip == "1" || 
                // 检查IP地址是否以"222.244.120"开头
                bool isFilteredIp = ip.StartsWith("222.244.120");
                if (!request.Path.Value.Contains(".ttf") && !request.Path.Value.Contains(".woff2") &&
                !isFilteredIp && ip != "1" && ip != "116.162.0.109" && ip != "36.148.41.245")
                {
                    var dbContext = context.RequestServices.GetService(typeof(MyDbContext)) as MyDbContext;
                    var visitRecord = await CreateVisitRecord(context);
                    dbContext.visitRecords.Add(visitRecord);
                    await dbContext.SaveChangesAsync();
                }

                await next.Invoke();
            });
        }

        private static async Task<VisitRecord> CreateVisitRecord(HttpContext context)
        {
            return await Task.Run(() =>
            {
                var request = context.Request;
                var serverTime = DateTime.UtcNow;
                var cstTime = ConvertToChinaStandardTime(serverTime);
                var visitRecord = new VisitRecord
                {
                    Ip = context.GetRemoteIPAddress()?.ToString().Split(":")?.Last(),
                    RequestPath = request.Path,
                    RequestQueryString = request.QueryString.Value,
                    RequestMethod = request.Method,
                    UserAgent = request.Headers.UserAgent,
                    Time = cstTime
                };
                return visitRecord;
            });
        }

        private static DateTime ConvertToChinaStandardTime(DateTime utcTime)
        {
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            var cstTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, cstZone);
            return cstTime;
        }
    }
}
