using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Client
{
    public class MyClientViewModel
    {
        public string Id { get; set; }

        public string birthTime { get; set; }
        public Guid? ClientUuid { get; set; }
        //public int Id { get; set; }
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
        public string ContactName { get; set; }
        public string Title { get; set; }
        public string Call { get; set; }
        public string Phone { get; set; }
        public string Cellphone { get; set; }
        public string Email { get; set; }
        public Guid? UserGuid { get; set; }
        
        public string UserName { get; set; }

        public string CustomerStatusUuid { get; set; }
        public string IndustryUuid { get; set; }
        public string IndustryName { get; set; }
        public string StatusName { get; set; }
        public string TypeUuid { get; set; }
        public string TypeName { get; set; }

        //public int? IsDelete { get; set; }
        public string SystemUserUuid { get; set; }
        public string SystemUserName { get; set; }

        public Guid? SuperiorUuid { get; set; }
        public int? SuperiorMenu { get; set; }

        //public Guid? CompanyUuid { get; set; }
    }

    public class studentyibangmodel
    {
        public string key { get; set; }
        public string label { get; set; }
        public bool disabled { get; set; }

    }
}
