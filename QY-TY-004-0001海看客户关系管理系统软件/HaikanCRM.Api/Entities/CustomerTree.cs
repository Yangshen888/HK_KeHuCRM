using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class CustomerTree
    {
        public int Id { get; set; }
        public Guid ClientUuid { get; set; }
        public string ClientName { get; set; }
        public Guid? ClientManager { get; set; }
        public string BusinessUuid { get; set; }
        public string SystemUserUuid { get; set; }
        public Guid? SuperiorUuid { get; set; }
        public string ClientArea { get; set; }
        public string ClientBirthday { get; set; }
        public int? SuperiorMenu { get; set; }
        public int? IsDelete { get; set; }
        public string RealName { get; set; }
        public string StatusName { get; set; }
        public string IndustryName { get; set; }
        public string CpName { get; set; }
        public int? SpCount { get; set; }
    }
}
