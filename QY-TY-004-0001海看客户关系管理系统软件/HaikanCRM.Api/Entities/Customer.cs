using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            Business = new HashSet<Business>();
            BusinessDocuments = new HashSet<BusinessDocuments>();
            ContactCallLog = new HashSet<ContactCallLog>();
            ContactPerson = new HashSet<ContactPerson>();
            SystemMessage = new HashSet<SystemMessage>();
        }

        public Guid ClientUuid { get; set; }
        public int Id { get; set; }
        public string ClientName { get; set; }
        public Guid? ClientManager { get; set; }
        public Guid? ClientStatus { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientFax { get; set; }
        public string ClientPostcode { get; set; }
        public Guid? ClientType { get; set; }
        public Guid? ClientIndustry { get; set; }
        public string ClientArea { get; set; }
        public string ClientWebsite { get; set; }
        public string ClientAddress { get; set; }
        public string Remarks { get; set; }
        public string AddTime { get; set; }
        public string BusinessUuid { get; set; }
        public Guid? CallLogUuid { get; set; }
        public string Status { get; set; }
        public int? IsDelete { get; set; }
        public string SystemUserUuid { get; set; }
        public Guid? CompanyUuid { get; set; }
        public Guid? SuperiorUuid { get; set; }
        public int? SuperiorMenu { get; set; }
        public string ClientBirthday { get; set; }

        public virtual CustomerIndustry ClientIndustryNavigation { get; set; }
        public virtual SystemUser ClientManagerNavigation { get; set; }
        public virtual CustomerStatus ClientStatusNavigation { get; set; }
        public virtual CustomerTypeList ClientTypeNavigation { get; set; }
        public virtual ICollection<Business> Business { get; set; }
        public virtual ICollection<BusinessDocuments> BusinessDocuments { get; set; }
        public virtual ICollection<ContactCallLog> ContactCallLog { get; set; }
        public virtual ICollection<ContactPerson> ContactPerson { get; set; }
        public virtual ICollection<SystemMessage> SystemMessage { get; set; }
    }
}
