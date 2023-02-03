using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Personalblog.Filter;
using Personalblog.Middlewares;
using Personalblog.Model;
using Personalblog.Models.Config;
using Personalblog.Services;
using PersonalblogServices;
using PersonalblogServices.Articels;
using PersonalblogServices.Categorys;
using PersonalblogServices.Config;
using PersonalblogServices.FCategory;
using PersonalblogServices.FPhoto;
using PersonalblogServices.FPost;
using PersonalblogServices.FtopPost;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SkiaSharp;
using StackExchange.Profiling.Storage;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllersWithViews(
    options => { options.Filters.Add<ResponseWrapperFilter>(); }
);
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.WebHost.UseUrls("http://*:7031");
// Add services to the container.
builder.Services.AddControllersWithViews();
//数据库连接
builder.Services.AddDbContext<MyDbContext>(opt =>
{
    //string connStr = builder.Configuration.GetSection("ConnStr").Value;
    string connStr = "Data Source=101.43.25.210;port=3306;Initial Catalog=Personalblog;user id=root;password=123456;";
    opt.UseMySql(connStr,new MySqlServerVersion(new Version(5,7,40)));
});
//跨域
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policyBuilder => {
        policyBuilder.AllowCredentials();
        policyBuilder.AllowAnyHeader();
        policyBuilder.AllowAnyMethod();
        policyBuilder.AllowAnyOrigin();
        policyBuilder.WithOrigins("http://101.43.25.210:8031");
    });
});
//注册AotoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));
//服务层注入
builder.Services.AddTransient<IArticelsService, ArticelsService>();
builder.Services.AddTransient<IPhotoService, PersonalblogServices.PhotoService>();
builder.Services.AddTransient<IFPhotoService, FPhotoService>();
builder.Services.AddTransient<IFCategoryService, FCategoryService>();
builder.Services.AddTransient<ITopPostService, TopPostService>();
builder.Services.AddTransient<IFPostService, FPostService>();

builder.Services.AddHttpContextAccessor();
// 注册 IHttpClientFactory，参考：https://docs.microsoft.com/zh-cn/dotnet/core/extensions/http-client
builder.Services.AddHttpClient();
//注入自定义服务
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<Personalblog.Services.PhotoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ConfigService>();
builder.Services.AddScoped<VisitRecordService>();
builder.Services.AddSingleton<CommonService>();
builder.Services.AddSingleton<PiCLibService>();
//注入jwt服务
builder.Services.Configure<SecuritySetting>(builder.Configuration.GetSection(nameof(SecuritySetting)));
//注入Miniprofiler
builder.Services.AddMiniProfiler(options =>
{
    //访问地址路由根目录；默认为：/mini-profiler-resources
    options.RouteBasePath = "/profiler";
    //数据缓存时间
    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
    //sql格式化设置
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    //跟踪连接打开关闭
    options.TrackConnectionOpenClose = true;
    //界面主题颜色方案;默认浅色
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
    //.net core 3.0以上：对MVC过滤器进行分析
    options.EnableMvcFilterProfiling = true;
    //对视图进行分析
    options.EnableMvcViewProfiling = true;
}).AddEntityFramework();
//添加jwt
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        // 这里用到我们之前定义好的配置类
        var secSettings = builder.Configuration.GetSection(nameof(SecuritySetting)).Get<SecuritySetting>();
        // 设置jwt token的各种信息用于验证
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = secSettings.Token.Issuer,
            ValidAudience = secSettings.Token.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secSettings.Token.Key)),
            ClockSkew = TimeSpan.Zero
        };
    });
//图片缩略图
// 注册服务
builder.Services.AddImageSharp();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//是否开启监听
//app.UseMiniProfiler();

app.UseHttpsRedirection();
app.UseStaticFiles();

//中间件 用于保存接口访问信息
app.UseMiddleware<VisitRecordMiddleware>();

//开启跨域
app.UseCors();

// 添加中间件
app.UseImageSharp();

app.UseRouting();


//开启jwt服务
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
