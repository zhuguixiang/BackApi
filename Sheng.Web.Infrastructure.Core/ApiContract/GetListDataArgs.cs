using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sheng.Web.Infrastructure
{
    public class GetListDataArgs
    {
        public GetPagingInfo PagingInfo
        {
            get; set;
        }

        public string OrderBy
        {
            get;set;
        }

        public ParametersContainer Parameters
        {
            get; set;
        }

        public GetListDataArgs()
        {
            PagingInfo = new GetPagingInfo();
            Parameters = new ParametersContainer();
        }
    }

    public class GetListDataArgs<T>
    {
        public GetPagingInfo PagingInfo
        {
            get; set;
        }

        public string OrderBy
        {
            get; set;
        }

        public T Parameters
        {
            get; set;
        }

        public GetListDataArgs()
        {
            PagingInfo = new GetPagingInfo();
        }
    }

    public class GetListDataResult<T>
    {
        public ResultPagingInfo PagingInfo
        {
            get; set;
        }

        public List<T> Data
        {
            get;
            set;
        }

        public GetListDataResult()
        {
            PagingInfo = new ResultPagingInfo();
        }
    }

    public class GetListDataResult
    {
        public ResultPagingInfo PagingInfo
        {
            get; set;
        }

        public DataTable Data
        {
            get;
            set;
        }

        public GetListDataResult()
        {
            PagingInfo = new ResultPagingInfo();
        }
    }
}
