﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    [DataContract]
    public class JsTreeNodeStateDto
    {
        [DataMember(Name = "opened")]
        public bool Opened
        {
            get;set;
        }

        [DataMember(Name = "disabled")]
        public bool Disabled
        {
            get; set;
        }

        [DataMember(Name = "selected")]
        public bool Selected
        {
            get; set;
        }
    }
}
