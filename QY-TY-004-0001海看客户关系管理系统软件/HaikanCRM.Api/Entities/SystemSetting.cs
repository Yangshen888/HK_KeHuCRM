using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class SystemSetting
    {
        public Guid ClobalUuid { get; set; }
        public string ClobalName { get; set; }
        public string Globalurl { get; set; }
        public DateTime? AddTime { get; set; }
        public string AppPeople { get; set; }
        public int? IsDelete { get; set; }
        public string GlobalLogo { get; set; }
        public int Id { get; set; }
    }
}
