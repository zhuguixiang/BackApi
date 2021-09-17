using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    /// <summary>
    /// 存储在 redis 中作为用户上下文
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class UserContext
    {
        /// <summary>
        /// 登录成功后分配一个 Token
        /// </summary>
        public string Token
        {
            get; set;
        }

        /// <summary>
        /// 用户对象
        /// </summary>
        public Guid UserId
        {
            get; set;
        }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime
        {
            get; set;
        }
    }
}
