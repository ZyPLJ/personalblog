
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Personalblog.Contrib.SiteMessage;
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
using PersonalblogServices.Links;
using SixLabors.ImageSharp.Web.DependencyInjection;
using StackExchange.Profiling.Storage;
using System.Text;
using AspNetCoreRateLimit;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.Caching.Memory;
using Personalblog.Extensions.SendEmail;
using Personalblog.Model.ViewModels;
using PersonalblogServices.ArticelArc;
using PersonalblogServices.CommentService;
using PersonalblogServices.Messages;
using PersonalblogServices.Notice;
using PersonalblogServices.Tag;

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

builder.Services.AddDbContext<MyDbContext>(opt =>
{
    //opt.UseMySql(connStr, new MySqlServerVersion(new Version(5, 7, 40)));   
    string connStr = "Data Source=app.db;Foreign Keys=False";
    opt.UseSqlite(connStr);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        opt => opt.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
        .WithExposedHeaders("http://localhost:8080/","http://109.206.247.216:8031"));
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfigs));

builder.Services.AddTransient<IArticelsService, ArticelsService>();
builder.Services.AddTransient<IPhotoService, PersonalblogServices.PhotoService>();
builder.Services.AddTransient<IFPhotoService, FPhotoService>();
builder.Services.AddTransient<IFCategoryService, FCategoryService>();
builder.Services.AddTransient<ITopPostService, TopPostService>();
builder.Services.AddTransient<IFPostService, FPostService>();
builder.Services.AddTransient<ILinkService, LinkService>();
builder.Services.AddTransient<Icommentservice, commentservice>();
builder.Services.AddTransient<INoticeService, NoticeService>();
builder.Services.AddTransient<ILinkExchangeService, LinkExchangeService>();
builder.Services.AddTransient<IMessagesService, MessagesService>();
builder.Services.AddTransient<IArcService, ArcService>();
builder.Services.AddTransient<EmailServiceFactory>();
builder.Services.AddTransient<ITagService,TagService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<IHttpService, HttpService>();


builder.Services.AddScoped<ICategoryService, PersonalblogServices.Categorys.CategoryService>();
builder.Services.AddScoped<Personalblog.Services.PhotoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<ConfigService>();
builder.Services.AddScoped<Personalblog.Services.CategoryService>();
builder.Services.AddScoped<VisitRecordService>();
builder.Services.AddSingleton<Messages>();
builder.Services.AddSingleton<CommonService>();
builder.Services.AddSingleton<PiCLibService>();
builder.Services.AddSingleton<CrawlService>();
builder.Services.AddSingleton<TempFilterService>();
builder.Services.AddSingleton<QiniuService>();


builder.Services.Configure<SecuritySetting>(builder.Configuration.GetSection(nameof(SecuritySetting)));
builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<QiniuCDNOptions>(builder.Configuration.GetSection("QiniuCDN"));

builder.Services.AddMiniProfiler(options =>
{

    options.RouteBasePath = "/profiler";

    (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);

    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();

    options.TrackConnectionOpenClose = true;

    options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;

    options.EnableMvcFilterProfiling = true;

    options.EnableMvcViewProfiling = true;
}).AddEntityFramework();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        var secSettings = builder.Configuration.GetSection(nameof(SecuritySetting)).Get<SecuritySetting>();
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

builder.Services.AddImageSharp();
builder.Services.AddOptions();

// 添加Session服务
builder.Services.AddSession();

//注册服务
builder.Services.AddRateLimit(builder.Configuration);
builder.Services.AddMemoryCache();

// 添加Hangfire服务
builder.Services.AddHangfire(configuration => configuration.UseMemoryStorage());
// 添加定时任务
builder.Services.AddHangfireServer();

// //注入Miniprofiler
// builder.Services.AddMiniProfiler(options =>
// {
//     //访问地址路由根目录；默认为：/mini-profiler-resources
//     options.RouteBasePath = "/profiler";
//     //数据缓存时间
//     (options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(60);
//     //sql格式化设置
//     options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
//     //跟踪连接打开关闭
//     options.TrackConnectionOpenClose = true;
//     //界面主题颜色方案;默认浅色
//     options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
//     //.net core 3.0以上：对MVC过滤器进行分析
//     options.EnableMvcFilterProfiling = true;
//     //对视图进行分析
//     options.EnableMvcViewProfiling = true;
//     //控制访问页面授权，默认所有人都能访问
//     //options.ResultsAuthorize;
//     //要控制分析哪些请求，默认说有请求都分析
//     //options.ShouldProfile;
//
//     //内部异常处理
//     //options.OnInternalError = e => MyExceptionLogger(e);
// }).AddEntityFramework();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



// app.UseMiniProfiler();

app.UseHttpsRedirection();

// 启用Session中间件
app.UseSession();

//添加中间件
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true
});

app.UseRateLimit();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});


//app.UseMiddleware<VisitRecordMiddleware>();
app.UseVisitRecordMiddleware();


app.UseCors("CorsPolicy");


app.UseImageSharp();

app.UseRouting();

// 使用Hangfire中间件
app.UseHangfireDashboard();
// 启动定时任务
// RecurringJob.AddOrUpdate(() => YourMethodToTrigger(), Cron.Minutely);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
