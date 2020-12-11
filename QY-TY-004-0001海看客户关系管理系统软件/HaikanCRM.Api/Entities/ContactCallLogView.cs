using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class ContactCallLogView
    {
        public Guid CallLogUuid { get; set; }
        public DateTime? CallTime { get; set; }
        public int? IsDelete { get; set; }
    }
}
