using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Sheng.Kernal
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ExcelImportFieldAttribute : Attribute
    {
        public string Title
        {
            get; set;
        }

        public PropertyInfo PropertyInfo
        {
            get;set;
        }

        public ExcelImportFieldAttribute(string title)
        {
            Title = title;
        }
    }
}
