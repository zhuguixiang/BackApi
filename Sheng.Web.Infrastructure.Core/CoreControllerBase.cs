using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Web.Infrastructure
{
    //public interface ICoreControllerBase
    //{
    //    UserContext UserContext
    //    {
    //        get; set;
    //    }
    //}

    public class CoreControllerBase: ControllerBase
    {
        public string Token
        {
            get;set;
        }

        public UserContext UserContext
        {
            get; set;
        }
    }
}
