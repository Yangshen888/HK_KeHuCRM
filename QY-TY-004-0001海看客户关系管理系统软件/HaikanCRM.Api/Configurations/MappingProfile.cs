using AutoMapper;
using HaikanCRM.Api.ViewModels.Rbac.SystemMenu;
using HaikanCRM.Api.ViewModels.Rbac.SystemPermission;
using HaikanCRM.Api.ViewModels.Rbac.SystemRole;
using HaikanCRM.Api.ViewModels.Rbac.SystemUser;
using HaikanCRM.Api.Entities;

namespace HaikanCRM.Api.Configurations
{
    /// <summary>
    /// 用于实体类和自定义类型的转换
    /// </summary>
    public class MappingProfile: Profile
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public MappingProfile()
        {
            #region SystemUser
            CreateMap<SystemUser, UserJsonModel>();
            CreateMap<UserCreateViewModel, SystemUser>();
            CreateMap<UserEditViewModel, SystemUser>();
            CreateMap<SystemUser, UserEditViewModel>();
            #endregion
            #region SystemRole
            CreateMap<SystemRole, RoleJsonModel>();
            CreateMap<RoleCreateViewModel, SystemRole>();
            CreateMap<SystemRole, RoleCreateViewModel>();
            #endregion

            #region SystemMenu
            CreateMap<SystemMenu, MenuJsonModel>();
            CreateMap<MenuCreateViewModel, SystemMenu>();
            CreateMap<MenuEditViewModel, SystemMenu>();
            CreateMap<SystemMenu, MenuEditViewModel>();
            #endregion

            #region SystemPermission
            CreateMap<SystemPermission, PermissionJsonModel>()
                .ForMember(d => d.MenuName, s => s.MapFrom(x => x.SystemMenuUu.Name))
                .ForMember(d => d.PermissionTypeText, s => s.MapFrom(x => x.Type.ToString()));
            CreateMap<PermissionCreateViewModel, SystemPermission>();
            CreateMap<PermissionEditViewModel, SystemPermission>();
            CreateMap<SystemPermission, PermissionEditViewModel>();
            #endregion

           


            
        }
    }
}
