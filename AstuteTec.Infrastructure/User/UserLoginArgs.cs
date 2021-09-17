using AstuteTec.Models.Dto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace AstuteTec.Infrastructure
{
    public class UserLoginArgs
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }

    public class UserLoginResult
    {
        public UserContext UserContext { get; set; }

        public UserOutDto User { get; set; }
    }
}
