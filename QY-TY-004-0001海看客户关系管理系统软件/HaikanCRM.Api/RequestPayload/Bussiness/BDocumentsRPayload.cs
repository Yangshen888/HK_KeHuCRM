using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.RequestPayload.Bussiness
{
    public class BDocumentsRPayload:RequestPayload
    {
        public Guid? BusinessUuid { get; set; }
    }
}
