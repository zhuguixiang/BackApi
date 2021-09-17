using System;
using System.Collections.Generic;
using System.Text;

namespace AstuteTec.Infrastructure
{
    public class FileUploadResult
    {
        public FileUploadResult()
        {
            ResultItemList = new List<FileUploadResultItem>();
        }

        public List<FileUploadResultItem> ResultItemList
        {
            get;set;
        }
    }

    public class FileUploadResultItem
    {
        public string FileName
        {
            get;set;
        }

        public string Url
        {
            get;set;
        }
    }
}
