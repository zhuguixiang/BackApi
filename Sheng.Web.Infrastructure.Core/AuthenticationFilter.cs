using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using Sheng.Kernal;

namespace Sheng.Web.Infrastructure
{
    public class AuthenticationFilter : IActionFilter
    {
        protected readonly CachingService _cache;
        protected readonly ILoggerFactory _loggerFactory;
        protected readonly IHostingEnvironment _env;

        protected UserContext _userContext;

        JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };

        public AuthenticationFilter(CachingService cachingService, ILoggerFactory loggerFactory, IHostingEnvironment env)
        {
            _cache = cachingService;
            _loggerFactory = loggerFactory;
            _env = env;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            CoreControllerBase controller = context.Controller as CoreControllerBase;
            if (controller == null)
                return;

            string token = context.HttpContext.Request.Headers["token"];
            if (controller != null)
            {
                controller.Token = token;
            }

            object[] objAllowedAnonymousArray =
                (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(
                    typeof(AllowAnonymousAttribute), false);
            if (objAllowedAnonymousArray.Length > 0)
                return;

            if (String.IsNullOrEmpty(token) == false)
            {
                _userContext = _cache.Get<UserContext>(token);
            }
            //不做登录验证，默认使用admin账号
            //_userContext = new UserContext();
            //_userContext.Token = Guid.NewGuid().ToString();
            //_userContext.UserId = Guid.Parse("4D79F993CA454A5EBCAA438D49BC0988");
            //_userContext.LoginTime = DateTime.Now;

            if (_userContext == null)
            {
                NormalResult normalResult = new NormalResult(false);
                normalResult.Reason = 7001;
                normalResult.Message = "用户尚未登录。";

                context.Result = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(normalResult, Formatting.Indented, _jsonSerializerSettings),
                    StatusCode = StatusCodes.Status200OK,
                };

                return;
            }

            controller.UserContext = _userContext;

            OnActionExecutingFinished(context);
        }


        public virtual void OnActionExecutingFinished(ActionExecutingContext context)
        {

        }
    }


}
