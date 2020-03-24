namespace ZDY.DMS.Metronic.TagHelpers.Untils
{

    public class DataRuleName
    {
        public DataRuleName(string ruleName, bool isAppend)
        {
            this.RuleName = ruleName;
            this.IsAppend = isAppend;
        }

        public string RuleName { get; set; }
        public bool IsAppend { get; set; }
    }
}
