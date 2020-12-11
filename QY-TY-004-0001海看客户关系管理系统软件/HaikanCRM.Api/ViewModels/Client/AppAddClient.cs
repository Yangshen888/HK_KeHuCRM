using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Client
{
    public class AppAddClient
    {
        public string ClienName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientEmail { get; set; }
        public string ClientFax { get; set; }
        public string ClientPostcode { get; set; }
        public string ClientWebsite { get; set; }
        public string ClientAddress { get; set; }
        public string Remarks { get; set; }
        public string ClientArea { get; set; }
        public string ClientStatus { get; set; }
        public string ClientType { get; set; }
        public string ClientIndustry { get; set; }
        public string SystemUserUuid { get; set; }
        public string RealName { get; set; }
        public string ClmoName { get; set; }
        public string Kk1 { get; set; }
        public Guid? ClientManager { get; set; }
        public Guid? SuperiorUuid { get; set; }
        public int? SuperiorMenu { get; set; }
        public Guid? IndustryUuid { get; set; }
        public string usGuid { get; set; }
        public string usName { get; set; }
        public string ClientBirthday { get; set; }
        
    }
}
