using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class ContactPersonView
    {
        public int Id { get; set; }
        public string ContactName { get; set; }
        public string AddTime { get; set; }
        public string Call { get; set; }
        public Guid ContactPersonUuid { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Sex { get; set; }
        public string WeChat { get; set; }
        public string BgPhone { get; set; }
        public string ZjPhone { get; set; }
        public string DateBirth { get; set; }
        public string TencentQq { get; set; }
        public string OfficeAddress { get; set; }
        public string FamilyAddress { get; set; }
        public string Remarks { get; set; }
        public int? IsDelete { get; set; }
        public string RealName { get; set; }
        public Guid? ClientUuid { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public Guid? Manager { get; set; }
        public string ClientManager { get; set; }
    }
}
