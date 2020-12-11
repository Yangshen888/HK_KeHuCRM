using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.RequestPayload.Contact
{
    public class CallLogRequstPaload : RequestPayload
    {
        public int Id  { get; set; }
        public string ContactName { get; set; }
        public string Kw1 { get; set; }
        public string Kw2 { get; set; }
        public string guid { get; set; }
        public string logUuid { get; set; }
    }
    
}
