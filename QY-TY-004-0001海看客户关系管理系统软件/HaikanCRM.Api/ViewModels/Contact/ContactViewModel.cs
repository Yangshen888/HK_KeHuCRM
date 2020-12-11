using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Contact
{
    public class ContactViewModel
    {
        public string ContactPersonUuid { get; set; }
        public string Title { get; set; }
        public string Call { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public string AddTime { get; set; }
        public string ContactDetailsUUID { get; set; }
        public string ContactAddressUUID { get; set; }
        public string CallLogUUID { get; set; }
        public string ContactName { get; set; }
        public string Remarks { get; set; }
        public string ClientName { get; set; }
        public string ClientUuid { get; set; }
        public string ClientPhone { get; set; }
        public string FamilyAddress { get; set; }
        public string OfficeAddress { get; set; }
        public string TencentQq { get; set; }
        public string TencentQQ { get; set; }
        public string ZjPhone { get; set; }
        public string BgPhone { get; set; }
        public string WeChat { get; set; }
        public string PhoneType { get; set; }
        public string AddressType { get; set; }
        public string PhoneText { get; set; }
        public string AddressText { get; set; }
        public Guid? UserGuid { get; set; }
        public string UserName { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string usGuid { get; set; }
        public string usName { get; set; }
        public string DateBirth { get; set; }
    }
}
