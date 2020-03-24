using System;
using System.Collections.Generic;
using ZDY.DMS.Querying.SearchModel.Model;

namespace ZDY.DMS.Querying.SearchModel.TransformProviders
{
    public interface ITransformProvider
    {
        bool Match(ConditionItem item, Type type);
        IEnumerable<ConditionItem> Transform(ConditionItem item, Type type);
    }
}
