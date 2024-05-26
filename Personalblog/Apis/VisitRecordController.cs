using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using Personalblog.Services;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    //[Authorize]
    [ApiController]
    [Route("Api/[controller]")]
    public class VisitRecordController : Controller
    {
        private readonly VisitRecordService _service;
        public VisitRecordController(VisitRecordService service)
        {
            _service = service;
        }
        /// <summary>
        /// 总览数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ApiResponse Overview()
        {
            return ApiResponse.Ok(_service.Overview());
        }
        /// <summary>
        /// 趋势数据
        /// </summary>
        /// <param name="days">查看最近几天的数据，默认7天</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public ApiResponse Trend(int days = 7)
        {
            return ApiResponse.Ok(_service.Trend(days));
        }
        /// <summary>
        /// 获取全部访问记录
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public ApiResponse<List<VisitRecord>> GetAll()
        {
            return new ApiResponse<List<VisitRecord>>(_service.GetAll());
        }
        [HttpGet]
        public async Task<ApiResponsePaged<VisitRecord>> GetList([FromQuery] VisitRecordQueryParameters param)
        {
            var (data, meta) = await _service.GetPagedList(param);
            return new ApiResponsePaged<VisitRecord>(data, meta);
        }
    }
}
