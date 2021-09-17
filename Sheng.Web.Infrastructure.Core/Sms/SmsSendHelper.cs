using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;
using Sheng.Kernal;
using System;
using System.Text;

namespace Sheng.Web.Infrastructure.Core
{
    public class SmsSendHelper
    {
        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static NormalResult SendSmsVerificationCode(SmsSendArgs args)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                builder.Clear();
                builder.Append("发送短信验证码：" + Environment.NewLine);
                builder.Append("请求参数：手机号码 " + args.CellPhone + "；验证码 " + args.VerifyCode + Environment.NewLine);

                SmsSingleSender ssender = new SmsSingleSender(args.AppId, args.AppKey);
                // 签名参数未提供或者为空时，会使用默认签名发送短信
                var result = ssender.sendWithParam("86", args.CellPhone,
                                                args.TemplateId, new[] { args.VerifyCode },
                                                args.SmsSign, "", "");
                builder.Append("响应参数：" + JsonHelper.Serializer(result) + Environment.NewLine);
                LogService.Instance.Info(builder.ToString());

                if (result == null)
                {
                    return new NormalResult("短信验证码发送失败。");
                }
                if (result.result != 0)
                {
                    return new NormalResult(result.errMsg);
                }
                return new NormalResult();
            }
            catch (Exception e)
            {
                LogService.Instance.Error("发送短信验证码异常", e, JsonHelper.Serializer(args));
                return new NormalResult("短信验证码发送失败。");
            }
        }
    }
}
