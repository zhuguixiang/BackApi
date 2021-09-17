using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Sheng.Kernal;
using System;

namespace Sheng.Web.Infrastructure
{
    //https://blog.csdn.net/sD7O95O/article/details/78096113
    //https://blog.csdn.net/sd7o95o/article/details/79395767
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        readonly IHostingEnvironment _env;

        JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
        };

        public HttpGlobalExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }

        public void OnException(ExceptionContext context)
        {
            var requestParameters = JsonHelper.Serializer(context.HttpContext.Items.Values);
            LogService.Instance.Error("全局异常处理：服务器内部错误", context.Exception, requestParameters);

            NormalResult normalResult = new NormalResult("服务器内部错误：" + ExceptionHelper.GetMessage(context.Exception));

            context.Result = new ContentResult
            {
                Content = JsonConvert.SerializeObject(normalResult, Formatting.Indented, _jsonSerializerSettings),
                StatusCode = StatusCodes.Status200OK,
                //ContentType = "text/html;charset=utf-8"
            };

            context.ExceptionHandled = true; //异常已处理了
        }
    }


}
