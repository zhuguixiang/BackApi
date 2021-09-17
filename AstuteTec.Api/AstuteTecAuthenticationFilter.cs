using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using AstuteTec.Models.Dto;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstuteTec.Api
{

    public class AstuteTecAuthenticationFilter : AuthenticationFilter
    {

        public AstuteTecAuthenticationFilter(CachingService cachingService, ILoggerFactory loggerFactory, IHostingEnvironment env)
            : base(cachingService, loggerFactory, env)
        {

        }

        public override void OnActionExecutingFinished(ActionExecutingContext context)
        {
            AstuteTecControllerBase controller = context.Controller as AstuteTecControllerBase;

            UserOutDto user = _cache.Get<UserOutDto>(_userContext.UserId.ToString());
            //不做登录验证，默认使用admin账号
            //user = new UserOutDto()
            //{
            //    Id = Guid.Parse("4D79F993CA454A5EBCAA438D49BC0988"),
            //    Account = "admin",
            //    Name = "超级管理员",
            //    CreateTime = DateTime.Now
            //};

            controller.User = user;
        }
    }
}
