using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class ContactPerson
    {
        public ContactPerson()
        {
            ContactCallLog = new HashSet<ContactCallLog>();
        }

        public Guid ContactPersonUuid { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Call { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string AddTime { get; set; }
        public Guid? ContactDetailsUuid { get; set; }
        public Guid? ClientUuid { get; set; }
        public Guid? CallLogUuid { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string Status { get; set; }
        public int? IsDelete { get; set; }
        public Guid? CompanyUuid { get; set; }
        public string ContactName { get; set; }
        public string Remarks { get; set; }
        public string WeChat { get; set; }
        public string TencentQq { get; set; }
        public string BgPhone { get; set; }
        public string ZjPhone { get; set; }
        public string OfficeAddress { get; set; }
        public string FamilyAddress { get; set; }
        public string DateBirth { get; set; }
        public string ScopeBusiness { get; set; }
        public string ServiceProvided { get; set; }
        public string PesServiceProvided { get; set; }
        public string CompanySize { get; set; }

        public virtual Customer ClientUu { get; set; }
        public virtual SystemUser SystemUserUu { get; set; }
        public virtual ICollection<ContactCallLog> ContactCallLog { get; set; }
    }
}
