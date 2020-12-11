using System;

namespace HaikanCRM.Api.ViewModels.Rbac
{
    public class GlobalViewModel
    {
        public Guid ClobalUuid { get; set; }
        public string ClobalName { get; set; }
        public string Globalurl { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public string GlobalLogo { get; set; }
    }
}
