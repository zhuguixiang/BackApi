using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Web.Infrastructure
{
    public class AliSMS
    {
        public static NormalResult<AliSMSSendSmsResult> SendSms(AliSMSSendSmsArgs args)
        {
            IClientProfile profile = DefaultProfile.GetProfile(args.RegionId, args.AccessKeyId, args.AccessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", args.PhoneNumbers);
            request.AddQueryParameters("SignName", args.SignName);
            request.AddQueryParameters("TemplateCode", args.TemplateCode);
            request.AddQueryParameters("TemplateParam", args.TemplateParam);
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                string strResponseContent = System.Text.Encoding.Default.GetString(response.HttpResponse.Content);
                AliSMSSendSmsResult aliSMSSendSmsResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AliSMSSendSmsResult>(strResponseContent);
                NormalResult<AliSMSSendSmsResult> result = new NormalResult<AliSMSSendSmsResult>()
                {
                    Data = aliSMSSendSmsResult
                };
                if (aliSMSSendSmsResult.Code != "OK")
                {
                    result.Successful = false;
                    result.Message = aliSMSSendSmsResult.Message;
                }
                return result;
            }
            catch (ServerException e)
            {
                return new NormalResult<AliSMSSendSmsResult>(e.ToString());
            }
            catch (ClientException e)
            {
                return new NormalResult<AliSMSSendSmsResult>(e.ToString());
            }
        }
    }
}
