using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using AstuteTec.Infrastructure;
using Sheng.Web.Infrastructure;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AstuteTec.Api.Controllers
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : AstuteTecControllerBase
    {
        private IHostingEnvironment _hostingEnvironment;

        public FileController(IMemoryCache memoryCache, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 文件上传
        /// 以表单形式提交上传，支持多文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task<ActionResult<NormalResult<FileUploadResult>>> Upload()
        {
            IFormFileCollection files = Request.Form.Files;
            if (files.Count == 0)
                return new NormalResult<FileUploadResult>("没有要上传的文件。");

            try
            {
                FileUploadResult result = new FileUploadResult();
                string currentDirectory = Directory.GetCurrentDirectory();

                foreach (var fileItem in files)
                {
                    if (fileItem.Length == 0)
                        continue;

                    string dateFile = DateTime.Now.ToString("yyyy-MM-dd");
                    string _dir = Path.Combine("Upload", dateFile);
                    string targetDir = Path.Combine(currentDirectory, _dir);
                    if (Directory.Exists(targetDir) == false)
                        Directory.CreateDirectory(targetDir);

                    string extension = new FileInfo(fileItem.FileName).Extension;
                    string newFileName = Guid.NewGuid().ToString() + extension; //随机生成新的文件名

                    string filePath = Path.Combine(_dir, newFileName);
                    string fullPath = Path.Combine(targetDir, newFileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await fileItem.CopyToAsync(stream);
                    }

                    result.ResultItemList.Add(new FileUploadResultItem()
                    {
                        FileName = fileItem.FileName,
                        Url = filePath
                    });
                }

                return new NormalResult<FileUploadResult>()
                {
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new NormalResult<FileUploadResult>()
                {
                    Successful = false,
                    Message = ex.Message
                };
            }
        }
    }
}