using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class ContactPlist
    {
        public int Id { get; set; }
        public Guid ContactPersonUuid { get; set; }
        public int? IsDelete { get; set; }
        public Guid? ClientManager { get; set; }
        public string ContactName { get; set; }
    }
}
