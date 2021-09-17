using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using AstuteTec.Core;
using AstuteTec.Infrastructure;
using AstuteTec.Models;
using Sheng.Kernal;
using Sheng.Web.Infrastructure;
using System.IO;
using UEditorNetCore;
using Microsoft.AspNetCore.Http;

namespace AstuteTec.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            AutoMapperConfig.Initialize();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //允许跨域调用
            //https://blog.csdn.net/wdeng2011/article/details/79204836
            //http://www.cnblogs.com/hobinly/p/9437143.html
            services.AddCors();

            //appSettings.json
            services.AddOptions();
            IConfigurationSection configurationSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(configurationSection);

            //log4net.config
            var repository = LogManager.CreateRepository("NETCoreLogRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            // services.AddSingleton<ILoggerRepository>(repository);
            LogService.Instance.SetRepository(repository);

            //连接redis
            string redisConnectionString = configurationSection.GetSection("RedisCaching").GetValue(typeof(string), "ConnectionString").ToString();
            CachingService.Instance.Connect(redisConnectionString);
            services.AddSingleton<CachingService>(CachingService.Instance);

            //配置业务层
            services.AddSingleton<DictionaryManager>();
            services.AddSingleton<UserContextManager>();
            services.AddSingleton<UserManager>();

            //https://www.cnblogs.com/ITCoNan/p/7371591.html
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Version = "v1",
                    Title = " API 文档",
                    Description = "",
                });

                //https://www.cnblogs.com/suxinlcq/p/6757556.html
                //https://cloud.tencent.com/developer/article/1039673
                //Set the comments path for the swagger json and ui.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "AstuteTec.Api.xml");
                options.IncludeXmlComments(xmlPath);

                xmlPath = Path.Combine(basePath, "AstuteTec.Models.xml");
                options.IncludeXmlComments(xmlPath);

                xmlPath = Path.Combine(basePath, "AstuteTec.Models.Dto.xml");
                options.IncludeXmlComments(xmlPath);

                xmlPath = Path.Combine(basePath, "AstuteTec.Infrastructure.xml");
                options.IncludeXmlComments(xmlPath);

                //ApiKeyScheme/BasicAuthScheme/OAuth2Scheme/SecurityScheme
                //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                // //Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                // Name = "Authorization",
                // In = "header",
                // Type = "apiKey"
                //});
                // options.OperationFilter<HttpHeaderFilter>();
            });

            //百度富文本编辑器，注入UEditor服务
            services.AddUEditorService("config.json", true);
            services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
                options.Filters.Add<AstuteTecAuthenticationFilter>();
            })
           .AddJsonOptions(options =>
           {
               //忽略循环引用
               options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
               //设置时间格式
               options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
           })
           .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //https://cloud.tencent.com/developer/article/1023323
            //oracle数据库
            //使用oracle 11g请使用该下面方法
            //services.AddDbContextPool<Entities>(options => options.UseOracle(Configuration.GetConnectionString("DefaultConnection"), b => b.UseOracleSQLCompatibility("11")));

            //使用oracle 12c请使用该下面方法
            services.AddDbContextPool<Entities>(options => options.UseOracle(Configuration.GetConnectionString("DefaultConnection")));


            //sql server数据库
            //services.AddDbContextPool<Entities>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            Entities.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(
              builder => builder
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
             );

            app.UseMvc();

            //配置静态文件目录
            string currentDirectory = Directory.GetCurrentDirectory();
            string uploadPath = Path.Combine(currentDirectory, "Upload");
            if (Directory.Exists(uploadPath) == false)
                Directory.CreateDirectory(uploadPath);
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(uploadPath),
                RequestPath = "/Upload",
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=36000");
                }
            });

            string exportPath = Path.Combine(currentDirectory, "Export");
            if (Directory.Exists(exportPath) == false)
                Directory.CreateDirectory(exportPath);
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(exportPath),
                RequestPath = "/Export"
            });
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                //c.DocExpansion("none");
            });
        }
    }
}
