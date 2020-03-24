using System;
using System.Collections.Generic;
using System.Linq;
using ZDY.DMS.Querying.SearchModel.Model;

namespace ZDY.DMS.DataPermission
{
    public class SearchModel
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string OrderField { get; set; }
        public bool IsAsc { get; set; } = default;
        public List<FieldItem> Fields { get; set; } = new List<FieldItem>();

        public QueryModel GetQueryModel()
        {
            QueryModel queryModel = new QueryModel();
            if (this.Fields.Count() > 0)
            {
                foreach (var item in this.Fields)
                {
                    if (item.Value != null && item.Value.ToString().Trim() != "")
                    {
                        if (item.Field.EndsWith("_Start") || item.Field.EndsWith("_End"))
                        {
                            item.Field = item.Field.Substring(0, item.Field.LastIndexOf('_'));
                        }
                        queryModel.Items.Add(new ConditionItem(item.Field, item.Method, item.Value, item.Prefix, item.OrGroup));
                    }
                }
            }
            return queryModel;
        }

        public void AppendField(FieldItem field)
        {
            Fields.Add(field);
        }

        public void AppendField(string field, QueryMethod method, string value)
        {
            Fields.Add(new FieldItem(field, method, value));
        }

        public void AppendField(string field, QueryMethod method, string value, string prefix, string orGroup)
        {
            Fields.Add(new FieldItem(field, method, value, prefix, orGroup));
        }

        public void RemoveField(string field)
        {
            var items = Fields.Where(t => t.Field == field).ToList();
            if (items.Count() > 0)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    Fields.Remove(items[i]);
                }
            }
        }
    }

    public class FieldItem
    {
        public FieldItem() { }

        public FieldItem(string field, QueryMethod method, string value)
        {
            Field = field;
            Method = method;
            Value = value;
        }

        public FieldItem(string field, QueryMethod method, string value, string prefix, string orGroup)
        {
            Field = field;
            Method = method;
            Value = value;
            Prefix = prefix;
            OrGroup = orGroup;
        }

        public string Field { get; set; }
        public QueryMethod Method { get; set; }
        public string Value { get; set; }
        public string Prefix { get; set; }
        public string OrGroup { get; set; }
    }
}
