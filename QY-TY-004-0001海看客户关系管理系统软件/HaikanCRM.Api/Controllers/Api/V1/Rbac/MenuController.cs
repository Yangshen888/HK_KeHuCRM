using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Entities.Enums;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.Extensions.CustomException;
using HaikanCRM.Api.Models.Response;
using HaikanCRM.Api.RequestPayload.Rbac.Menu;
using HaikanCRM.Api.ViewModels.Rbac.SystemMenu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using static HaikanCRM.Api.Entities.SystemUser;

namespace HaikanEnrollmentRegistration.Api.Controllers.Api.V1.Rbac
{
    /// <summary>
    /// 
    /// </summary>
    //[CustomAuthorize]
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class MenuController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public MenuController(HaikanCRMContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(MenuRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.SystemMenu.AsQueryable();
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Name.Contains(payload.Kw.Trim()) || x.Url.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == payload.IsDeleted);
                }
                if (payload.Status > CommonEnum.Status.All)
                {
                    query = query.Where(x => (CommonEnum.Status)x.Status == payload.Status);
                }
                if (payload.ParentGuid.HasValue)
                {
                    query = query.Where(x => x.ParentGuid == payload.ParentGuid);
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var data = list.Select(_mapper.Map<SystemMenu, MenuJsonModel>);
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(data, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="model">菜单视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(MenuCreateViewModel model)
        {
            using (_dbContext)
            {
                var entity = _mapper.Map<MenuCreateViewModel, SystemMenu>(model);
                entity.CreatedOn = DateTime.Now.ToString("yyyy-MM-dd");
                entity.SystemMenuUuid = Guid.NewGuid();
                entity.CreatedByUserGuid = AuthContextService.CurrentUser.Guid;
                entity.CreatedByUserName = AuthContextService.CurrentUser.DisplayName;
                entity.IsDeleted = 0;
                entity.Icon = string.IsNullOrEmpty(entity.Icon) ? "md-menu" : entity.Icon;
                _dbContext.SystemMenu.Add(entity);
                _dbContext.SaveChanges();
                var response = ResponseModelFactory.CreateInstance;
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="guid">菜单ID</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                var model = _mapper.Map<SystemMenu, MenuEditViewModel>(entity);
                //if (model.ParentGuid.HasValue)
                //{
                //    var parent = _dbContext.DncMenu.FirstOrDefault(x => x.Guid == entity.ParentGuid);
                //    if (parent != null)
                //    {
                //        model.ParentName = parent.Name;
                //    }
                //}
                var tree = LoadMenuTree(model.ParentGuid.ToString());
                response.SetData(new { model, tree });
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的菜单信息
        /// </summary>
        /// <param name="model">菜单视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(MenuEditViewModel model)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == model.SystemMenuUuid);
                entity.Name = model.Name;
                entity.Icon = string.IsNullOrEmpty(model.Icon) ? "md-menu" : model.Icon;
                entity.Level = 1;
                entity.ParentGuid = model.ParentGuid;
                entity.Sort = model.Sort;
                entity.Url = model.Url;
                entity.ModifiedByUserGuid = AuthContextService.CurrentUser.Guid;
                entity.ModifiedByUserName = AuthContextService.CurrentUser.DisplayName;
                entity.ModifiedOn = DateTime.Now.ToString("yyyy-MM-dd");
                entity.Description = model.Description;
                entity.ParentName = model.ParentName;
                entity.Component = model.Component;
                entity.HideInMenu = (int)model.HideInMenu;
                entity.NotCache = (int)model.NotCache;
                entity.BeforeCloseFun = model.BeforeCloseFun;
                if (!ConfigurationManager.AppSettings.IsTrialVersion)
                {
                    entity.Alias = model.Alias;
                    entity.IsDeleted = (int)model.IsDeleted;
                    entity.Status = (int)model.Status;
                    entity.IsDefaultRouter = (int)model.IsDefaultRouter;
                }
                _dbContext.SaveChanges();
                var response = ResponseModelFactory.CreateInstance;
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 菜单树
        /// </summary>
        /// <returns></returns>
        [HttpGet("{selected?}")]
        public IActionResult Tree(string selected)
        {
            var response = ResponseModelFactory.CreateInstance;
            var tree = LoadMenuTree(selected?.ToString());
            response.SetData(tree);
            return Ok(response);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids">菜单ID,多个以逗号分隔</param>
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
        /// 恢复菜单
        /// </summary>
        /// <param name="ids">菜单ID,多个以逗号分隔</param>
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
        /// <param name="ids">菜单ID,多个以逗号分隔</param>
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
                        var query1 = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == Guid.Parse(item.Value.ToString()));
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
                        var query1 = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == Guid.Parse(item.Value.ToString()));
                        query1.IsDeleted = 1;
                        _dbContext.SaveChanges();
                    }
                    return response;
                }

                if (command == "forbidden")
                {

                    var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    foreach (var item in parameters)
                    {
                        var query1 = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == Guid.Parse(item.Value.ToString()));
                        query1.Status = 0;
                        _dbContext.SaveChanges();
                    }
                    return response;

                }
                if (command == "normal")
                {
                    var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                    foreach (var item in parameters)
                    {
                        var query1 = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == Guid.Parse(item.Value.ToString()));
                        query1.Status = 1;
                        _dbContext.SaveChanges();
                    }

                    return response;
                }

            }
            return response;
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
            //    default:
            //        break;
            //}
            //return Ok(response);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">菜单ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                //var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                //var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                //var sql = string.Format("UPDATE SystemMenu SET IsDeleted=@IsDeleted WHERE SystemMenuUUID IN ({0})", parameterNames);
                //parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                //_dbContext.Database.ExecuteSqlCommand(sql, parameters);
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                foreach (var item in parameters)
                {
                    var query1 = _dbContext.SystemMenu.FirstOrDefault(x => x.SystemMenuUuid == Guid.Parse(item.Value.ToString()));
                    query1.IsDeleted = 1;
                    _dbContext.SaveChanges();
                }
                return response;

            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="status">菜单状态</param>
        /// <param name="ids">菜单ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateStatus( string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE SystemMenu SET Status=@Status WHERE SystemMenuUUID IN ({0})", parameterNames);
                _dbContext.Database.ExecuteSqlCommand(sql, parameters);
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        private List<MenuTree> LoadMenuTree(string selectedGuid = null)
        {
            var temp = _dbContext.SystemMenu.Where(x => x.IsDeleted == (int)CommonEnum.IsDeleted.No && x.Status == (int)CommonEnum.Status.Normal).ToList().Select(x => new MenuTree
            {
                Guid = x.SystemMenuUuid.ToString(),
                ParentGuid = x.ParentGuid,
                Title = x.Name
            }).ToList();
            var root = new MenuTree
            {
                Title = "顶级菜单",
                Guid = Guid.Empty.ToString(),
                ParentGuid = null
            };
            temp.Insert(0, root);
            var tree = temp.BuildTree(selectedGuid);
            return tree;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MenuTreeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menus"></param>
        /// <param name="selectedGuid"></param>
        /// <returns></returns>
        public static List<MenuTree> BuildTree(this List<MenuTree> menus, string selectedGuid = null)
        {
            var lookup = menus.ToLookup(x => x.ParentGuid);
            Func<Guid?, List<MenuTree>> build = null;
            build = pid =>
            {
                return lookup[pid]
                 .Select(x => new MenuTree()
                 {
                     Guid = x.Guid,
                     ParentGuid = x.ParentGuid,
                     Title = x.Title,
                     Expand = (x.ParentGuid == null || x.ParentGuid == Guid.Empty),
                     Selected = selectedGuid == x.Guid,
                     Children = build(new Guid(x.Guid)),
                 })
                 .ToList();
            };
            var result = build(null);
            return result;
        }
    }
}