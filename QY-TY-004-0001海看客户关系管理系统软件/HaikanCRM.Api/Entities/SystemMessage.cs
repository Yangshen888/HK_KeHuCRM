using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class SystemMessage
    {
        public int Id { get; set; }
        public Guid MessageUuid { get; set; }
        public Guid? ClientUuid { get; set; }
        public int? IsDelete { get; set; }
        public string AddTime { get; set; }

        public virtual Customer ClientUu { get; set; }
    }
}
