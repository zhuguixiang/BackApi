using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Web.Infrastructure.Core
{
    public class SmsSendArgs
    {
        /// <summary>
        /// SDK AppID：短信应用的唯一标识，调用短信API接口时，需要提供该参数。
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// App Key：用来校验短信发送合法性的密码，与SDK AppID对应，需要业务方高度保密，切勿把密码存储在客户端。
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 短信模板ID，需要在短信应用中申请
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        /// 短信签名内容；请使用真实的已申请的签名, 签名参数使用的是`签名内容`，而不是`签名ID`
        /// </summary>
        public string SmsSign { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
    }
}
