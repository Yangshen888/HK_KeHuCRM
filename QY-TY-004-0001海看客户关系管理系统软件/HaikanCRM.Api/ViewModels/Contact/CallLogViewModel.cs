using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Contact
{
    public class CallLogViewModel
    {
        public string CallLogUuid { get; set; }
        public string ClientUuid { get; set; }
        public string ContactPersonUuid { get; set; }
        public string CallContent { get; set; }
        public string CallTime { get; set; }
        public string CallResults { get; set; }
        public string Remarks { get; set; }
        public string ContactName { get; set; }
        public string ClientName { get; set; }
        public string BusinessUuid { get; set; }
        public string ContactDetailsUuid { get; set; }
        public string contactDetailsText { get; set; }
        public string SystemUserUuid { get; set; }

        public string cdText { get; set; }
        public string cdName { get; set; }
        public string ContactDetailsName { get; set; }
        public string usName { get; set; }
        public string usGuid { get; set; }
        public string userName { get; set; }
        public string userGuid { get; set; }

    }
}
