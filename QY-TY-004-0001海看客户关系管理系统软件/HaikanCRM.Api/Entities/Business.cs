using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class Business
    {
        public Business()
        {
            BusinessDocuments = new HashSet<BusinessDocuments>();
            ContactCallLog = new HashSet<ContactCallLog>();
        }

        public Guid BusinessUuid { get; set; }
        public int Id { get; set; }
        public string BusinessStage { get; set; }
        public string SalesAmount { get; set; }
        public string Winrate { get; set; }
        public string BusinessSource { get; set; }
        public string BusinessType { get; set; }
        public string CheckTime { get; set; }
        public string Remarks { get; set; }
        public string SystemUserUuid { get; set; }
        public string Status { get; set; }
        public int? IsDelete { get; set; }
        public Guid? CompanyUuid { get; set; }
        public string BusinessName { get; set; }
        public Guid? ClientUuid { get; set; }
        public string ContactBusName { get; set; }
        public string AddTime { get; set; }

        public virtual Customer ClientUu { get; set; }
        public virtual ICollection<BusinessDocuments> BusinessDocuments { get; set; }
        public virtual ICollection<ContactCallLog> ContactCallLog { get; set; }
    }
}
