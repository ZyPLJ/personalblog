using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Auth;
using Personalblog.Services;
using PersonalblogServices.Response;

namespace Personalblog.Apis
{
    [Route("Api/[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost]
        public ApiResponse Login([FromBody] LoginUser loginUser)
        {
            var users = _authService.GetUserByName(loginUser.Username);
            if (users == null) return ApiResponse.Unauthorized("用户名不存在！");
            if (loginUser.Password != users.Password) return ApiResponse.Unauthorized("密码错误！");
            return ApiResponse.Ok(_authService.GenerateLoginToken(users));
        }
        /// <summary>
        /// 获取当前用户信息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public ApiResponse<User> GetUser()
        {
            var user = _authService.GetUser(User);
            if (user == null) return ApiResponse.NotFound("找不到用户资料");
            return new ApiResponse<User>(user);
        }
    }
}
