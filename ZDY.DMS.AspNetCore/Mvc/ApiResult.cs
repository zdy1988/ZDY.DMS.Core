using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Mvc
{
    public class ApiResult
    {
        public int StatusCode { get; set; }

        public object Data { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }
    }
}
