using AstuteTec.Models.Dto;
using Sheng.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AstuteTec.Api
{
    public class AstuteTecControllerBase : CoreControllerBase
    {
        public UserOutDto User
        {
            get;set;
        }
    }
}
