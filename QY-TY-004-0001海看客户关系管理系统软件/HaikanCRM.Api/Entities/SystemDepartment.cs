using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class SystemDepartment
    {
        public SystemDepartment()
        {
            SystemUser = new HashSet<SystemUser>();
        }

        public Guid DepartmentUuid { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public string EstablishTime { get; set; }
        public string EstablishName { get; set; }
        public int? IsDeleted { get; set; }
        public int? Dingid { get; set; }

        public virtual ICollection<SystemUser> SystemUser { get; set; }
    }
}
