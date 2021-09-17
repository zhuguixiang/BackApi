using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UEditorNetCore;

namespace AstuteTec.Api.Controllers
{
    /// <summary>
    /// 百度富文本编辑器UEditor
    /// 配置config.json
    /// </summary>
    [Route("api/UEditor")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UEditorController : Controller
    {
        private UEditorService ue;

        public UEditorController(UEditorService ue)
        {
            this.ue = ue;
        }

        /// <summary>
        /// do
        /// </summary>
        public void Do()
        {
            //修改UEditor上传文件的路径
            Config.WebRootPath = Directory.GetCurrentDirectory();
            ue.DoAction(HttpContext);
        }
    }
}