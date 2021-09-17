using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Kernal
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExcelExportFieldAttribute : Attribute
    {
        public string Title
        {
            get; set;
        }

        public string SubProperty
        {
            get; set;
        }


        public ExcelExportFieldAttribute(string title)
        {
            Title = title;
        }

        public ExcelExportFieldAttribute(string title, string subProperty)
        {
            Title = title;
            SubProperty = subProperty;
        }
    }
}
