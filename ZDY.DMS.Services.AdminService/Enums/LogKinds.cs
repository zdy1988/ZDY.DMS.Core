using System.ComponentModel;

namespace ZDY.DMS.Services.AdminService.Enums
{
    /// <summary>
    /// 日志的种类
    /// </summary>
    public enum LogKinds
    {
        /// <summary>
        /// 系统日志
        /// </summary>
        [Description("系统日志")]
        [Category("0")]
        System = 0,

        /// <summary>
        /// 操作日志
        /// </summary>
        [Description("操作日志")]
        [Category("1")]
        Action = 1,

        /// <summary>
        /// 通信日志
        /// </summary>
        [Description("通信日志")]
        [Category("2")]
        Hub = 2,

        /// <summary>
        /// 提醒日志
        /// </summary>
        [Description("提醒日志")]
        [Category("3")]
        Remind = 3,

        /// <summary>
        /// 邮件日志
        /// </summary>
        [Description("邮件日志")]
        [Category("3")]
        Email = 4,

        /// <summary>
        /// 登录日志
        /// </summary>
        [Description("登录日志")]
        [Category("5")]
        Login = 5,

        /// <summary>
        /// Windows服务日志
        /// </summary>
        [Description("Windows服务日志")]
        [Category("6")]
        WindowsService = 6
    }
}
