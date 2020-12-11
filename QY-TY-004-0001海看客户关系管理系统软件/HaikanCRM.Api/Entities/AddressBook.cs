using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class AddressBook
    {
        public Guid SystemUserUuid { get; set; }
        public int? IsDeleted { get; set; }
        public string RealName { get; set; }
        public int? Txlu { get; set; }
    }
}
