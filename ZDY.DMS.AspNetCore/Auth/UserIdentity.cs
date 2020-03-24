using System;
using System.Collections.Generic;
using System.Text;

namespace ZDY.DMS.AspNetCore.Auth
{
    public class UserIdentity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
