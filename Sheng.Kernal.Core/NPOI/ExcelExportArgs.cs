using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Kernal
{
    public class ExcelExportArgs
    {
        public IList DataList
        {
            get;set;
        }

        /// <summary>
        /// 是否导出序号
        /// </summary>
        public bool RowNumber
        {
            get;set;
        }

        public List<ExcelExportColumnDefine> ColumnDefine
        {
            get;set;
        }

        public string FilePath
        {
            get;set;
        }

        public ExcelExportArgs()
        {
            ColumnDefine = new List<ExcelExportColumnDefine>();
        }

        public void AddColumnDefine(string title, string dataProperty)
        {
            ColumnDefine.Add(new ExcelExportColumnDefine()
            {
                Title = title,
                DataProperty = dataProperty
            });
        }
    }


}
