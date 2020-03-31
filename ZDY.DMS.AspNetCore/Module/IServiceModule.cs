using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Module
{
    public interface IServiceModule : IDisposable
    {
        void Initialize();
    }
}
