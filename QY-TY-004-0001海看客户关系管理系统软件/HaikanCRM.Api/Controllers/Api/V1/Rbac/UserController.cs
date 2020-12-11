using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Haikan3.Utils;
using HaikanCRM.Api.Auth;
using HaikanCRM.Api.Configurations;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Entities.Enums;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.Extensions.DataAccess;
using HaikanCRM.Api.Models.Response;
using HaikanCRM.Api.RequestPayload.Rbac.User;
using HaikanCRM.Api.ViewModels.Rbac.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HaikanCRM.Api.Controllers.Api.V1.Rbac
{
    /// <summary>
    /// 
    /// </summary>
    //[CustomAuthorize]
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    //[CustomAuthorize]
    public class UserController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MdDesEncrypt MdDesEncrypt;
        private readonly AppAuthenticationSettings _appSettings;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public UserController(HaikanCRMContext dbContext, IMapper mapper, IOptions<MdDesEncrypt> mdDesEncrypt, IOptions<AppAuthenticationSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _mapper = mapper;
            MdDesEncrypt = mdDesEncrypt.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(UserRequestPayload payload)
        {
            using (_dbContext)
            {
                var roles = _dbContext.SystemRole.Where(x => x.IsDeleted == 0).ToList();
                var query = from u in _dbContext.SystemUser
                            where u.UserType!=5
                            select new
                            {
                                SystemUserUuid = u.SystemUserUuid,
                                LoginName = u.LoginName,
                                RealName = u.RealName,
                                UserIdCard = u.UserIdCard,
                                AddTime = u.AddTime,
                                AddPeople = u.AddPeople,
                                //UserType = GetRoleName(u.SystemRoleUuid,roles),
                                IsDeleted = u.IsDeleted,
                                u.OldCard,
                                Id = u.Id
                            };

                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.LoginName.Contains(payload.Kw.Trim()) || x.RealName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == payload.IsDeleted);
                }
                //if (payload.Status > UserStatus.All)
                //{
                //    query = query.Where(x => x.Status == payload.Status);
                //}

                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                //var data = list.Select(_mapper.Map<SystemUser, UserJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(UserCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.LoginName.Trim().Length <= 0)
            {
                response.SetFailed("请输入登录名称");
                return Ok(response);
            }
            using (_dbContext)
            {
                //if (_dbContext.SystemUser.Count(x => x.LoginName == model.LoginName) > 0)
                //{
                //    response.SetFailed("登录名已存在");
                //    return Ok(response);
                //}
                if (model.UserIdCard != null && model.UserIdCard != "")
                {
                    if (_dbContext.SystemUser.Count(x => x.UserIdCard == model.UserIdCard) > 0)
                    {
                        response.SetFailed("身份证号已存在");
                        return Ok(response);
                    }
                }
                if (model.SystemRoleUuid == null && model.SystemRoleUuid.ToString() == "")
                {
                    response.SetFailed("请选择角色");
                    return Ok(response);
                }

                if (ConfigurationManager.HaikanPassport_IfUse)
                {
                    bool checkregister = false;

                    var obj = SyncInformation.CheckUserName(model.LoginName);
                    if (int.Parse(obj) > 0)
                    {
                        var cum = SyncInformation.CheckUserMail(model.OldCard);
                        if (int.Parse(cum) > 0)
                        {
                            //var cur = SyncInformation.CheckUserRegister(model.LoginName, model.OldCard);
                            var ru = SyncInformation.RegisterUser(model.LoginName, model.PassWord.Trim(), model.OldCard);
                            if (ru == "999")
                            {
                                checkregister = true;
                            }
                            else
                            {
                                if (ru == "-888")
                                {
                                    response.SetFailed("系统禁止注册中文用户名");
                                    return Ok(response);
                                }
                                if (ru == "-999")
                                {
                                    response.SetFailed("当前禁止注册新用户");
                                    return Ok(response);
                                }
                            }

                        }
                        else
                        {
                            if (cum == "-4")
                            {
                                response.SetFailed("Email 格式有误");
                                return Ok(response);
                            }
                            if (cum == "-5")
                            {
                                response.SetFailed("Email 不允许注册");
                                return Ok(response);
                            }
                            if (cum == "-6")
                            {
                                response.SetFailed("Email 已经被注册");
                                return Ok(response);
                            }
                        }
                    }
                    else
                    {
                        if (obj == "-1")
                        {
                            response.SetFailed("用户名不合法");
                            return Ok(response);
                        }
                        if (obj == "-2")
                        {
                            response.SetFailed("用户名包含不允许注册的词语");
                            return Ok(response);
                        }
                        if (obj == "-3")
                        {
                            response.SetFailed("用户名已经存在");
                            return Ok(response);
                        }
                    }
                    if (!checkregister)
                    {
                        response.SetFailed("用户统一身份注册失败");
                        return Ok(response);
                    }
                }
                


                var entity = _mapper.Map<UserCreateViewModel, SystemUser>(model);
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.SystemUserUuid = Guid.NewGuid();
                entity.IsDeleted = 0;
                entity.LoginName = model.LoginName;
                entity.RealName = model.RealName;
                //entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(model.PassWord.Trim(), MdDesEncrypt.SecretKey);
                entity.PassWord = Security.GenerateMD5(model.PassWord.Trim());
                entity.SystemRoleUuid = model.SystemRoleUuid.ToString();
                entity.UserIdCard = model.UserIdCard;
                entity.OldCard = model.OldCard;
                var rolename = _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "超级管理员");
                if (model.SystemRoleUuid.ToString().Contains(rolename.SystemRoleUuid.ToString()))
                {
                    entity.UserType = 0;
                }
                else
                {
                    entity.UserType = 2;
                }
                _dbContext.SystemUser.Add(entity);
                _dbContext.SaveChanges();

                
                //entity.SystemRoleUuid = "";
                //entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(model.PassWord.Trim(), MdDesEncrypt.SecretKey);
                //for (int i = 0; i < model.SystemRoleUuid.Count; i++)
                //{
                //    entity.SystemRoleUuid += model.SystemRoleUuid[i] + ",";
                //}
                //entity.SystemRoleUuid = entity.SystemRoleUuid.TrimEnd(',');
                //entity.OldCard = model.OldCard;
                //entity.Phone = model.Phone;
                //_dbContext.SystemUser.Add(entity);
                //_dbContext.SaveChanges();

                _dbContext.Database.ExecuteSqlCommand("DELETE FROM SystemUserRoleMapping WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                var success = true;
                    if (model.SystemRoleUuid != null)
                    {
                        var roles = new SystemUserRoleMapping();
                        roles.SystemUserUuid = entity.SystemUserUuid;
                        roles.SystemRoleUuid = model.SystemRoleUuid.Value;
                        roles.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                        roles.AddPeople = AuthContextService.CurrentUser.DisplayName;

                        _dbContext.SystemUserRoleMapping.Add(roles);

                    }
                success = _dbContext.SaveChanges() > 0;
                if (success)
                {
                    response.SetSuccess();
                }
                else
                {
                    _dbContext.Database.ExecuteSqlCommand("DELETE FROM SystemUser WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                    response.SetFailed("保存用户角色数据失败");
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var query = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                //var query = _mapper.Map<SystemUser, UserEditViewModel>(entity);
                //query.SystemRoleUuid = _dbContext.SystemUserRoleMapping.FirstOrDefault(x=>x.SystemUserUuid==entity.SystemUserUuid).SystemRoleUuid;
                //query.PassWord = Haikan3.Utils.DesEncrypt.Decrypt(query.PassWord.Trim(), MdDesEncrypt.SecretKey);
                //response.SetData(new { query, Role = query.SystemRoleUuid.ToString().ToLower().Split(',') });
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的用户信息
        /// </summary>
        /// <param name="model">用户视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(UserEditViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == model.SystemUserUuid);
                if (entity == null)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }
                if (entity.LoginName != model.LoginName)
                {
                    response.SetFailed("用户民不可修改");
                    return Ok(response);
                }
                //if (_dbContext.SystemUser.Count(x => x.LoginName == model.LoginName && x.SystemUserUuid != model.SystemUserUuid) > 0)
                //{
                //    response.SetFailed("登录名已存在");
                //    return Ok(response);
                //}
                if (model.UserIdCard != null && model.UserIdCard != "")
                {
                    if (entity.UserIdCard!=model.UserIdCard)
                    {
                        if (_dbContext.SystemUser.Count(x => x.UserIdCard == model.UserIdCard) > 0)
                        {
                            response.SetFailed("身份证号已存在");
                            return Ok(response);
                        }
                    }
                }
                if (model.SystemRoleUuid == null && model.SystemRoleUuid == "")
                {
                    response.SetFailed("请选择角色");
                    return Ok(response);
                }
                if (entity.LoginName == model.LoginName&&ConfigurationManager.HaikanPassport_IfUse)
                {
                    var uu = SyncInformation.UpdateUser(model.LoginName, model.PassWord.Trim(), model.OldCard);
                    if (!(int.Parse(uu) > 0))
                    {
                        response.SetFailed("修改统一身份信息失败");
                        return Ok(response);
                    }
                }
                
                

                entity.LoginName = model.LoginName;
                entity.RealName = model.RealName;
                entity.UserIdCard = model.UserIdCard;
                entity.SystemRoleUuid = model.SystemRoleUuid;
                entity.OldCard = model.OldCard;
                if (Security.GenerateMD5(model.PassWord.Trim()) != entity.PassWord)
                {
                    //entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(model.PassWord.Trim(), MdDesEncrypt.SecretKey);
                    entity.PassWord = Security.GenerateMD5(model.PassWord.Trim());
                }
                var rolename = _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "超级管理员");
                if (model.SystemRoleUuid.ToString().Contains(rolename.SystemRoleUuid.ToString()))
                {
                    entity.UserType = 0;
                }
                else
                {
                    entity.UserType = 2;
                }
                _dbContext.SaveChanges();
                //entity.UserType = model.UserType;
                //entity.ShopUuid = model.ShopUuid;
                //entity.VillageId = model.VillageId;
                //string temp = "";
                //for (int i = 0; i < model.SystemRoleUuid.Count; i++)
                //{
                //    temp += model.SystemRoleUuid[i] +",";
                //}
                //entity.SystemRoleUuid = temp.TrimEnd(',');
                //entity.IsDeleted = model.IsDeleted;
                //entity.OldCard = model.OldCard;
                //entity.Phone = model.Phone;
                _dbContext.Database.ExecuteSqlCommand("DELETE FROM SystemUserRoleMapping WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                var success = true;
                ////循环加权限
                //for (int i = 0; i < model.SystemRoleUuid.Count; i++)
                //{
                if (!string.IsNullOrEmpty(model.SystemRoleUuid))
                {
                    var roles = new SystemUserRoleMapping();
                    roles.SystemUserUuid = entity.SystemUserUuid;
                    roles.SystemRoleUuid = Guid.Parse(model.SystemRoleUuid);
                    roles.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                    roles.AddPeople = AuthContextService.CurrentUser.DisplayName;

                    _dbContext.SystemUserRoleMapping.Add(roles);

                }
                //}
                success = _dbContext.SaveChanges() > 0;
                if (success)
                {
                    response.SetSuccess();
                }
                else
                {
                    _dbContext.Database.ExecuteSqlCommand("DELETE FROM SystemUser WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                    response.SetFailed("保存用户角色数据失败");
                }
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }


       



        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditUserPwd(UserPwdViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == AuthContextService.CurrentUser.Guid);
                if (entity == null)
                {
                    response.SetFailed("用户不存在");
                    return Ok(response);
                }

                if (Security.GenerateMD5(model.oldpwd.Trim()) != entity.PassWord)
                {
                    response.SetFailed("原密码不正确");
                    return Ok(response);
                }
                entity.PassWord = Security.GenerateMD5(model.newpwd.Trim());//model.Password;

                _dbContext.SaveChanges();
                response = ResponseModelFactory.CreateInstance;
                return Ok(response);
            }
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }


        /// <summary>
        /// 恢复用户
        /// </summary>
        /// <param name="ids">用户GUID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Recover(string ids)
        {
            var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">用户ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public ResponseModel Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (command == "recover")
                {

                    var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    foreach (var item in parameters)
                    {
                        var query1 = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == Guid.Parse(item.Value.ToString()));
                        query1.IsDeleted = 0;
                        _dbContext.SaveChanges();
                    }
                    return response;
                }

                if (command == "delete")
                {

                    var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    foreach (var item in parameters)
                    {
                        var query1 = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == Guid.Parse(item.Value.ToString()));
                        query1.IsDeleted = 1;
                        _dbContext.SaveChanges();
                    }
                    return response;
                }

            }
            return response;
            //var response = ResponseModelFactory.CreateInstance;
            //switch (command)
            //{
            //    case "delete":
            //        if (ConfigurationManager.AppSettings.IsTrialVersion)
            //        {
            //            response.SetIsTrial();
            //            return Ok(response);
            //        }
            //        response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            //        break;
            //    case "recover":
            //        response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            //        break;
            //    //case "forbidden":
            //    //    if (ConfigurationManager.AppSettings.IsTrialVersion)
            //    //    {
            //    //        response.SetIsTrial();
            //    //        return Ok(response);
            //    //    }
            //    //    response = UpdateStatus(UserStatus.Forbidden, ids);
            //    //    break;
            //    //case "normal":
            //    //    response = UpdateStatus(UserStatus.Normal, ids);
            //    //    break;
            //    default:
            //        break;
            //}
            //return Ok(response);
        }

        /// <summary>
        /// 绑定身份证号
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SetIDCard(IdCardInfo info)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (string.IsNullOrEmpty(info.Openid) || string.IsNullOrEmpty(info.IdCard))
            {
                response.SetFailed("openid或身份证号为空");
                return Ok(response);
            }
            var entity = _dbContext.SystemUser.FirstOrDefault(x => x.Wechat == info.Openid);
            if (entity != null)
            {
                entity.UserIdCard = info.IdCard;
                entity.IdcardMd5 = YunSendMsg.GenerateMD5(info.IdCard);
                _dbContext.SaveChanges();
                response.SetSuccess("绑定成功");
                return Ok(response);
            }
            else
            {
                response.SetFailed("绑定失败");
                return Ok(response);
            }
           


            
        }

            #region 用户-角色
            /// <summary>
            /// 保存用户-角色的关系映射数据
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
            [HttpPost("/api/v1/rbac/user/save_roles")]
        public IActionResult SaveRoles(SaveUserRolesViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            var roles = model.AssignedRoles.Select(x => new SystemUserRoleMapping
            {
                SystemUserUuid = model.UserGuid,
                AddTime = DateTime.Now.ToString(),

            }).ToList();
            _dbContext.Database.ExecuteSqlCommand("DELETE FROM SystemUserRoleMapping WHERE SystemUserUUID={0}", model.UserGuid);
            var success = true;
            if (roles.Count > 0)
            {
                _dbContext.SystemUserRoleMapping.AddRange(roles);
                success = _dbContext.SaveChanges() > 0;
            }

            if (success)
            {
                response.SetSuccess();
            }
            else
            {
                response.SetFailed("保存用户角色数据失败");
            }
            return Ok(response);
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                //var idList = ids.Split(",").ToList();
                //idList.ForEach(x => {
                //  _dbContext.Database.ExecuteSqlCommand($"UPDATE DncUser SET IsDeleted=1 WHERE Id = {x}");
                //});
                //_dbContext.Database.ExecuteSqlCommand($"UPDATE SystemUser SET IsDeleted={(int)isDeleted} WHERE SystemUserUuid IN ({ids})");
                //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                //var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                //var sql = string.Format("UPDATE SystemUser SET IsDeleted=@IsDeleted WHERE SystemUserUUID IN ({0})", parameterNames);
                //parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                //_dbContext.Database.ExecuteSqlCommand(sql, parameters);
                //var response = ResponseModelFactory.CreateInstance;
                //return response;


                using (_dbContext)
                {
                    var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    foreach (var item in parameters)
                    {
                        var query1 = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == Guid.Parse(item.Value.ToString()));
                        if (ConfigurationManager.HaikanPassport_IfUse)
                        {
                            var du = SyncInformation.DeleteUser(query1.LoginName);
                            if (int.Parse(du) <= 0) continue;
                            query1.IsDeleted = 1;
                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            query1.IsDeleted = 1;
                            _dbContext.SaveChanges();
                        }
                        

                       
                    }
                    var response = ResponseModelFactory.CreateInstance;
                    return response;
                }

            }
        }
        private static string GetRoleName(string ids,List<SystemRole> roles)
        {
            string s = "";

            if (!string.IsNullOrEmpty(ids))
            {
                var index = ids.IndexOf(',');

                if (index != -1 && index < ids.Length - 1)
                {
                    string rn = "";
                    var arr = ids.Split(',');
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (rn.Length > 0)
                        {
                            rn += ",";
                        }
                        rn += roles.Find(x => x.SystemRoleUuid == Guid.Parse(arr[i])).RoleName;
                    }
                    return rn;
                }
                else if(index==ids.Length-1)
                {
                    ids=ids.Remove(index);
                    return roles.Find(x => x.SystemRoleUuid == Guid.Parse(ids)).RoleName;
                }
                else
                {
                    return roles.Find(x => x.SystemRoleUuid == Guid.Parse(ids)).RoleName;
                }
                //ids = ids.ToLower();
                //if (ids.Contains(AuthContextService.CurrentUser.ZYZ))
                //    s += "志愿者，";
                //if (ids.Contains(AuthContextService.CurrentUser.YH))
                //    s += "用户，";
                //if (ids.Contains(AuthContextService.CurrentUser.DDY))
                //    s += "督导员，";
                //if (ids.Contains(AuthContextService.CurrentUser.SJ))
                //    s += "商家，";
            }
            else
            {
                return "无";
            }
            
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="status">用户状态</param>
        /// <param name="ids">用户ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        //private ResponseModel UpdateStatus(UserStatus status, string ids)
        //{
        //    using (_dbContext)
        //    {
        //        var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
        //        var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
        //        var sql = string.Format("UPDATE DncUser SET Status=@Status WHERE Guid IN ({0})", parameterNames);
        //        parameters.Add(new SqlParameter("@Status", (int)status));
        //        _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        //        var response = ResponseModelFactory.CreateInstance;
        //        return response;
        //    }
        //}
        #endregion
    }
}