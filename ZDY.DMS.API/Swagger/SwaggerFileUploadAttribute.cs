using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZDY.DMS.API.Swagger
{
    /// <summary>
    /// 文件上传标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class SwaggerFileUploadAttribute : Attribute
    {
        public bool Required { get; private set; }

        public SwaggerFileUploadAttribute(bool Required = true)
        {
            this.Required = Required;
        }
    }
}
