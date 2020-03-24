using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.Services.WorkFlowService.Exception
{
    public class AnalyzeMistakesException : InvalidOperationException
    {
        public AnalyzeMistakesException() 
            : base("流程或者子流程数据解析异常")
        {

        }

        public AnalyzeMistakesException(string message) 
            : base(message)
        {

        }
    }
}
