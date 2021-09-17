using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Web.Infrastructure
{
    public class AliSMSSendSmsArgs
    {
        private string _regioiniId = "cn-hangzhou";
        public string RegionId
        {
            get { return _regioiniId; }
            set { _regioiniId = value; }
        }

        public string AccessKeyId
        {
            get; set;
        }

        public string AccessKeySecret
        {
            get; set;
        }

        /// <summary>
        /// 支持对多个手机号码发送短信，手机号码之间以英文逗号（,）分隔。上限为1000个手机号码。批量调用相对于单条调用及时性稍有延迟。
        /// </summary>
        public string PhoneNumbers
        {
            get;set;
        }

        private string _signName = "升讯威";
        public string SignName
        {
            get { return _signName; }
            set { _signName = value; }
        }

        /// <summary>
        /// 短信模板ID。请在控制台模板管理页面模板CODE一列查看。
        /// 必须是已添加、并通过审核的短信签名；且发送国际/港澳台消息时，请使用国际/港澳台短信模版。
        /// </summary>
        public string TemplateCode
        {
            get;set;
        }

        /// <summary>
        /// 短信模板变量对应的实际值，JSON格式。
        /// 如果JSON中需要带换行符，请参照标准的JSON协议处理。
        /// </summary>
        public string TemplateParam
        {
            get;set;
        }
    }


    /*
         {
            "Message": "OK",
            "RequestId": "CB4EB65E-BCFB-4A2A-9772-BD57967A5130",
            "BizId": "356115461460493357^0",
            "Code": "OK"
        }

            {
                "Message": "签名不合法(不存在或被拉黑)",
                "RequestId": "3E5D7230-777C-4DC7-B2ED-00422866133A",
                "Code": "isv.SMS_SIGNATURE_ILLEGAL"
            }
         */


    public class AliSMSSendSmsResult
    {
        public string Message
        {
            get; set;
        }

        public string RequestId
        {
            get; set;
        }

        public string BizId
        {
            get; set;
        }

        public string Code
        {
            get; set;
        }

        public bool Successful
        {
            get
            {
                return String.IsNullOrEmpty(Code) == false && Code == "OK";
            }
        }
    }

}
