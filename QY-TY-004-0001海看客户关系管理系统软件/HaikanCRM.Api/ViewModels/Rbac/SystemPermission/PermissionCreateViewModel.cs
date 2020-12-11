﻿using System;
using HaikanCRM.Api.Entities.Enums;

namespace HaikanCRM.Api.ViewModels.Rbac.SystemPermission
{
    /// <summary>
    /// 权限实体类
    /// </summary>
    public class PermissionCreateViewModel
    {
        public Guid SystemPermissionUuid { get; set; }
        public Guid? SystemMenuUuid { get; set; }
        public string Name { get; set; }
        public string ActionCode { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public int? Status { get; set; }
        public int? IsDeleted { get; set; }
        public int? Type { get; set; }
        public Guid? CreatedByUserGuid { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedByUserName { get; set; }
        public string ModifiedOn { get; set; }
        public Guid? ModifiedByUserGuid { get; set; }
        public string ModifiedByUserName { get; set; }
        public string CaPower { get; set; }
        public string menuGuid { get; set; }
    }
}
