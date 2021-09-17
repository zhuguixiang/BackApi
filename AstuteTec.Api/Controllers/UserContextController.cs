using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AstuteTec.Core;
using AstuteTec.Infrastructure;
using AstuteTec.Models;
using AstuteTec.Models.Dto;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System.Threading.Tasks;

namespace AstuteTec.Api.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserContextController : AstuteTecControllerBase
    {
        private CachingService _cachingService;

        private readonly UserContextManager _userContextManager;
        private readonly UserManager _userManager;

        public UserContextController(CachingService cachingService, UserContextManager userContextManager, UserManager userManager)
        {
            _cachingService = cachingService;

            _userContextManager = userContextManager;
            _userManager = userManager;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<NormalResult<UserLoginResult>> Login(UserLoginArgs args)
        {
            if (args == null)
            {
                return new NormalResult<UserLoginResult>("用户名或密码不能为空。");
            }

            NormalResult<UserContext> result = _userContextManager.Login(args);
            if (result.Successful == false)
            {
                return new NormalResult<UserLoginResult>()
                {
                    Successful = false,
                    Message = result.Message
                };
            }

            NormalResult<User> getUserResult = _userManager.GetUser(result.Data.UserId);
            UserOutDto userDto = Mapper.Map<UserOutDto>(getUserResult.Data);

            _cachingService.Set<UserOutDto>(userDto.Id.ToString(), userDto);

            UserLoginResult userLoginResult = new UserLoginResult
            {
                User = userDto,
                UserContext = result.Data
            };
            return new NormalResult<UserLoginResult>()
            {
                Data = userLoginResult
            };
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost("Logout")]
        public NormalResult Logout()
        {
            _userContextManager.Logout(this.Token);
            return new NormalResult();
        }
    }
}