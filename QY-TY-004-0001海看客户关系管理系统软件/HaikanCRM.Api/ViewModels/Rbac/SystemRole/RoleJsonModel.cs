using System;
using HaikanCRM.Api.Entities.Enums;

namespace HaikanCRM.Api.ViewModels.Rbac.SystemRole
{
    /// <summary>
    /// 
    /// </summary>
    public class RoleJsonModel
    {
        public Guid SystemRoleUuid { get; set; }
        public string RoleName { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }

        public int? IsFixation { get; set; }
    }
}
