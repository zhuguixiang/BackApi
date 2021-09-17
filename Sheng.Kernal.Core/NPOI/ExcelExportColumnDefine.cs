using System;
using System.Collections.Generic;
using System.Text;

namespace Sheng.Kernal
{
    public class ExcelExportColumnDefine
    {
        public string Title
        {
            get; set;
        }

        public string DataProperty
        {
            get; set;
        }

        /// <summary>
        /// 导出数据时，向单元格写入之前的加工委托
        /// </summary>
        public Func<object, string> SetValueToCellFormatFunc
        {
            get; set;
        }

    }
}
