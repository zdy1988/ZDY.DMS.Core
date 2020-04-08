using System;

namespace ZDY.DMS.AspNetCore.Bootstrapper.Module
{
    public interface IServiceModule : IDisposable
    {
        void Initialize();
    }
}
