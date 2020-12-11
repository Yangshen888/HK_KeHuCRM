using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class ContactDetails
    {
        public Guid ContactDetailsUuid { get; set; }
        public int Id { get; set; }
        public string WeChat { get; set; }
        public string TencentQq { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
    }
}
