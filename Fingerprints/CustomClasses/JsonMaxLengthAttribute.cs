using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fingerprints.CustomClasses
{
    public class JsonMaxLengthAttribute : FilterAttribute, IResultFilter
    {
        void IResultFilter.OnResultExecuted(ResultExecutedContext filterContext)
        {
           
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext filterContext)
        {
            JsonResult result = (JsonResult)filterContext.Result;
            result.MaxJsonLength = Int32.MaxValue;
           
        }
    }
}