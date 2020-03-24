using System;

namespace ZDY.DMS.Models
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int Order { get; set; } = 0;
    }
}
