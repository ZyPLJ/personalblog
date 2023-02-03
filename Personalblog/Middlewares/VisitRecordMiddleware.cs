using Microsoft.EntityFrameworkCore;
using Personalblog.Extensions;
using Personalblog.Model;
using SixLabors.ImageSharp;

namespace Personalblog.Middlewares
{
    public class VisitRecordMiddleware
    {
        private readonly RequestDelegate _next;
        public VisitRecordMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(Microsoft.AspNetCore.Http.HttpContext context,MyDbContext myDbContext)
        {
            var request = context.Request;
            var response = context.Response;
            //var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>();
            //optionsBuilder.UseSqlServer("Data Source=121.5.69.207;Initial Catalog=Personalblog;User ID=sa;Password=Zyplj1314999;Encrypt=True;TrustServerCertificate=True;");
            //using (var myDbContext = new MyDbContext(optionsBuilder.Options))
            //{

            //}
            myDbContext.visitRecords.Add(new Model.Entitys.VisitRecord
            {
                Ip = context.GetRemoteIPAddress()?.ToString().Split(":")?.Last(),
                RequestPath = request.Path,
                RequestQueryString = request.QueryString.Value,
                RequestMethod = request.Method,
                UserAgent = request.Headers.UserAgent,
                Time = DateTime.Now
            });
            myDbContext.SaveChanges();
            return _next(context);
        }
    }
}
