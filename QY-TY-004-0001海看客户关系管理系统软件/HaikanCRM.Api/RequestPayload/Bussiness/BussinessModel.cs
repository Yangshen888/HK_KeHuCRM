using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.RequestPayload.Bussiness
{
    public class BussinessModel : RequestPayload
    {
        public Guid? ClientUUID { get; set; }
        public string BusinessName { get; set; }
        public Guid? SystemUserUuid { get; set; }
        public string Kw { get; set; }
        public string Kw1 { get; set; }
        public string SysUuid { get; set; }
        public string stasources { get; set; }

    }
}
