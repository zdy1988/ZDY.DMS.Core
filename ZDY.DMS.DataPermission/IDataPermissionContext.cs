using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.DataPermission
{
    public interface IDataPermissionContext
    {
        List<string> GetSearchSpecifics(string key);

        List<string> GetSearchSpecifics(string controllerName, string actionName);
    }
}
