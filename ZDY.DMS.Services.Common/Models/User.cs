using System;
using System.ComponentModel.DataAnnotations;
using ZDY.DMS.DataPermission;
using ZDY.DMS.Services.Common.Models;
using ZDY.DMS.Services.Common.Models.Validations;

namespace ZDY.DMS.Services.Common.Models
{
    public class User : BaseEntity,
        IDisabledEntity<Guid>,
        ICompanyEntity<Guid>,
        IDepartmentEntity<Guid>
    {
        [Required(ErrorMessage = "账号是必须的，不能为空")]
        public string UserName { get; set; }
        public string Password { get; set; }
        public Guid Avatar { get; set; }
        public string AvatarUrl { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public Guid CompanyId { get; set; } = default(Guid);
        public Guid DepartmentId { get; set; } = default(Guid);
        public string Position { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "手机号码是必须的，不能为空")]
        [Mobile]
        public string Mobile { get; set; }
        [Email]
        public string Email { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime LastLoginTime { get; set; } = DateTime.Now;
        public string Session { get; set; }
        public string WeChatOpenId { get; set; }
        public bool IsDisabled { get; set; } = false;
    }
}
