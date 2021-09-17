using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstuteTec.Infrastructure
{
    public class AppSettings
    {
        public AppSettings_RedisCaching RedisCaching { get; set; }

        public AppSettings_Environment Environment { get; set; }
    }

    /// <summary>
    /// Redis
    /// </summary>
    public class AppSettings_RedisCaching
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 链接信息
        /// </summary>
        public string ConnectionString { get; set; }
    }

    public class AppSettings_Environment
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string BasePath { get; set; }
    }
}
