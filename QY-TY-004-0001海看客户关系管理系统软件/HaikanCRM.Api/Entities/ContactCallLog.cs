using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class ContactCallLog
    {
        public Guid CallLogUuid { get; set; }
        public int Id { get; set; }
        public Guid? ClientUuid { get; set; }
        public Guid? ContactPersonUuid { get; set; }
        public string CallContent { get; set; }
        public string CallTime { get; set; }
        public string SheBeiId { get; set; }
        public string Status { get; set; }
        public int? IsDelete { get; set; }
        public Guid? CompanyUuid { get; set; }
        public string Remarks { get; set; }
        public string ContactName { get; set; }
        public string ClientName { get; set; }
        public Guid? BusinessUuid { get; set; }
        public Guid? ContactDetailsUuid { get; set; }
        public string ContactDetailsName { get; set; }

        public virtual Business BusinessUu { get; set; }
        public virtual Customer ClientUu { get; set; }
        public virtual ContactPerson ContactPersonUu { get; set; }
    }
}
