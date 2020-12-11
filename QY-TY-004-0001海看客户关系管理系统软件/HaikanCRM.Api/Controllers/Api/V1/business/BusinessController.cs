using System;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.Extensions.DataAccess;
using HaikanCRM.Api.RequestPayload.Bussiness;
using HaikanCRM.Api.RequestPayload.Client;
using HaikanCRM.Api.ViewModels.Client;
using Microsoft.AspNetCore.Mvc;

namespace HaikanCRM.Api.Controllers.Api.V1.business
{
    /// <summary>
    /// 相关商机的操作接口
    /// </summary>
    [Route("api/v1/business/[controller]/[action]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public BusinessController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 商机列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BusShow(BussinessModel payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from b in _dbContext.Business
                            where b.IsDelete == 0
                            select new
                            {
                                b.Id,
                                b.BusinessUuid,
                                b.BusinessName,
                                b.BusinessStage,
                                b.SalesAmount,
                                b.Winrate,
                                b.BusinessSource,
                                b.BusinessType,
                                b.CheckTime,
                                b.Remarks,
                                b.SystemUserUuid,
                                b.ClientUuid,
                                clientName = b.ClientUu.ClientName,
                                b.ClientUu.ClientManager,
                                systemUserName = b.BusinessUuid == null ? "" : b.BusinessUuid != null ? SelectName(b.BusinessUuid).Trim(',') : "",
                            };

                // TODO:不可以硬编码,下个版本解决
                // 如果是我们指定的角色,可以查看所有的商机,否则只能看自己的商机
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
                }
                //如果商机搜索框不为空,模糊匹配搜索框内容
                if (!string.IsNullOrEmpty(payload.BusinessName))
                {
                    query = query.Where(x => x.BusinessName.Contains(payload.BusinessName));
                }
                //页面加载自动把已关闭的商机筛选掉
                if (string.IsNullOrEmpty(payload.BusinessName) && string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.BusinessStage.Contains("已关闭") == false);
                }
                //如果选择了状态,筛选处于当前状态的商机
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.BusinessStage.Contains(payload.Kw1));
                }
                //如果客户Uuid不为空,则只查询该客户的商机
                if (payload.ClientUUID != null)
                {
                    query = query.Where(x => x.ClientUuid == payload.ClientUUID);
                }
                query = query.OrderByDescending(x => x.Id);
                var demo = query.ToList();
                response.SetData(demo);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取参与人名字
        /// </summary>
        /// <param name="businessUuid"></param>
        /// <returns></returns>
        private static string SelectName(Guid businessUuid)
        {
            var systemUserName = "";
            using var c1 = new HaikanCRMContext();

            var participantIds = c1.Business.Where(x => x.BusinessUuid == businessUuid).ToList()[0].SystemUserUuid;//参与人uuid

            if (string.IsNullOrEmpty(participantIds)) return systemUserName;

            var patCount = participantIds.Split(',');
            for (var i = 0; i < patCount.Length; i++)
            {
                if (patCount[i] != "")
                {
                    systemUserName += c1.SystemUser.Where(x => x.SystemUserUuid == Guid.Parse(patCount[i])).ToList()[0]
                        .RealName + ",";
                }

            }

            return systemUserName;
        }


        /// <summary>
        /// 商机添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BusAdd(BusAddFrom model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                if (model.businessUuid == "")
                {
                    var entity = new Business
                    {
                        BusinessUuid = Guid.NewGuid(),
                        BusinessStage = model.businessStage,
                        SalesAmount = model.salesAmount,
                        Winrate = model.winrate,
                        BusinessSource = model.businessSource,
                        BusinessType = model.businessType
                    };

                    if (model.checkTime != "")
                    {
                        entity.CheckTime = model.checkTime;
                    }

                    if (model.systemUserUuid != "")
                    {
                        entity.ContactBusName = model.systemUserUuid;
                    }

                    entity.Remarks = model.remarks;
                    entity.IsDelete = 0;
                    entity.BusinessName = model.businessName;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    var query = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.clienName);
                    if (query == null)
                    {
                        response.SetFailed("所输入的客户不存在");
                        return Ok(response);
                    }
                    entity.ClientUuid = query.ClientUuid;

                    var entity1 = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                    if (entity1 != null) entity.CompanyUuid = entity1.CompanyUuid;
                    if (entity.ClientUuid != null)
                    {
                        var entity2 = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                        if (entity2 != null)
                        {
                            entity.CompanyUuid = entity2.CompanyUuid;
                            if (entity2.BusinessUuid == null || entity2.BusinessUuid == entity.BusinessUuid.ToString())
                            {
                                entity2.BusinessUuid = entity.BusinessUuid.ToString();
                            }
                            else if (entity2.BusinessUuid.Contains(entity.BusinessUuid.ToString()))
                            {
                                entity2.BusinessUuid = entity2.BusinessUuid;
                            }
                            else
                            {
                                entity2.BusinessUuid = entity2.BusinessUuid + "," + entity.BusinessUuid;
                            }
                        }
                    }

                    if (model.usName != "")
                    {
                        var entity6 = new SystemLog();
                        entity6.SystemLogUuid = Guid.NewGuid();
                        entity6.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")给客户(客户名称:" + query.ClientName + ")添加了一条商机,商机名称(" + model.businessName + ")";
                        entity6.UserName = model.usName;
                        entity6.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity6.Type = "钉钉端添加";
                        entity6.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity6);
                    }

                    _dbContext.Business.Add(entity);
                    _dbContext.SaveChanges();
                    response.SetSuccess("添加成功");
                }
                else
                {
                    var entity = _dbContext.Business.FirstOrDefault(x => x.BusinessUuid.ToString() == model.businessUuid);
                    if (entity != null)
                    {
                        entity.BusinessStage = model.businessStage;
                        entity.SalesAmount = model.salesAmount;
                        entity.Winrate = model.winrate;
                        entity.BusinessSource = model.businessSource;

                        if (model.systemUserUuid != "")
                        {
                            entity.ContactBusName = model.systemUserUuid;
                        }

                        entity.BusinessType = model.businessType;
                        entity.CheckTime = model.checkTime;
                        entity.Remarks = model.remarks;
                        entity.BusinessName = model.businessName;
                        entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var query = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.clienName);
                        if (query == null)
                        {
                            response.SetFailed("所输入的客户不存在");
                            return Ok(response);
                        }

                        entity.ClientUuid = query.ClientUuid;
                        var entity1 = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                        if (entity1 != null) entity.CompanyUuid = entity1.CompanyUuid;
                        if (entity.ClientUuid != null)
                        {
                            var entity2 = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                            if (entity2 != null)
                            {
                                entity.CompanyUuid = entity2.CompanyUuid;
                                if (entity2.BusinessUuid == null ||
                                    entity2.BusinessUuid == entity.BusinessUuid.ToString())
                                {
                                    entity2.BusinessUuid = entity.BusinessUuid.ToString();
                                }
                                else if (entity2.BusinessUuid.Contains(entity.BusinessUuid.ToString()))
                                {
                                    entity2.BusinessUuid = entity2.BusinessUuid;
                                }
                                else
                                {
                                    entity2.BusinessUuid = entity2.BusinessUuid + "," + entity.BusinessUuid;
                                }
                            }
                        }

                        if (model.usName != "")
                        {
                            var dad = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                            var entity6 = new SystemLog();
                            entity6.SystemLogUuid = Guid.NewGuid();
                            if (dad != null)
                                entity6.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")编辑了客户(客户名称:" +
                                                           dad.ClientName + ")的一条商机,商机名称(" + entity.BusinessName + ")";
                            entity6.UserName = model.usName;
                            entity6.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity6.Type = "钉钉端编辑";
                            entity6.IsDelete = 0;
                            _dbContext.SystemLog.Add(entity6);
                        }
                    }

                    _dbContext.SaveChanges();
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// 商机详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult BusView(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            var participantIds = _dbContext.Business.Where(x => x.BusinessUuid == guid).ToList()[0].SystemUserUuid;//参与人uuid
            var systemUserName = "";
            if (!string.IsNullOrEmpty(participantIds))
            {
                var patCount = participantIds.Split(',');
                for (var i = 0; i < patCount.Length; i++)
                {
                    if (patCount[i] != "")
                    {
                        systemUserName += _dbContext.SystemUser.Where(x => x.SystemUserUuid == Guid.Parse(patCount[i])).ToList()[0].RealName + ",";
                    }

                }
            }
            using (_dbContext)
            {
                var query = from b in _dbContext.Business
                            where b.IsDelete == 0 && b.BusinessUuid == guid
                            select new
                            {
                                b.Id,
                                b.BusinessUuid,
                                b.BusinessName,
                                b.BusinessStage,
                                b.SalesAmount,
                                b.Winrate,
                                b.ContactBusName,
                                b.BusinessSource,
                                b.BusinessType,
                                CheckTime = b.CheckTime == null ? "" : b.CheckTime,
                                b.Remarks,
                                b.ClientUuid,
                                realName = b.ClientUu.ClientManager,
                                clientName = b.ClientUu.ClientName,
                                systemUserName = systemUserName.Trim(','),
                                systemUserUuid = b.SystemUserUuid,
                            };
                var query2 = query.ToList();
                response.SetData(query2);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取商机单独页面
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEdit1Shangji(BussinessModel payload)
        {
            using (_dbContext)
            {
                var query = from b in _dbContext.Business
                            join c in _dbContext.Customer
                            on b.ClientUuid equals c.ClientUuid
                            join su in _dbContext.SystemUser
                            on c.ClientManager equals su.SystemUserUuid
                            where b.IsDelete == 0
                            select new
                            {
                                b.Id,
                                b.BusinessUuid,
                                b.BusinessName,
                                b.BusinessStage,
                                b.SalesAmount,
                                b.Winrate,
                                b.BusinessSource,
                                b.BusinessType,
                                b.CheckTime,
                                b.Remarks,
                                b.SystemUserUuid,
                                b.ClientUuid,
                                Manager = b.ClientUu.ClientManager,
                                clientName = b.ClientUu.ClientName,
                                ClientManager = CooM(b.ClientUuid),
                                //ClientManager = su.RealName,

                                systemUserName = b.ContactBusName == null ? "" : (b.ContactBusName != null && b.ContactBusName != "") ? SelectNameLog(b.ContactBusName, _dbContext).Trim(',') : "",
                            };
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.Manager == AuthContextService.CurrentUser.Guid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.BusinessName.Contains(payload.Kw));
                }

                if (string.IsNullOrEmpty(payload.stasources) && string.IsNullOrEmpty(payload.Kw) && string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.BusinessStage.Contains("已关闭") == false);
                }
                if (!string.IsNullOrEmpty(payload.stasources))
                {
                    query = query.Where(x => x.BusinessStage.Contains(payload.stasources));
                }
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.clientName.Contains(payload.Kw1));
                }
                query = query.OrderByDescending(x => x.Id);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取参与人名字
        /// </summary>
        /// <param name="contactBusName"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static string SelectNameLog(string contactBusName, HaikanCRMContext context)
        {
            string systemUserName;
            //var response = ResponseModelFactory.CreateResultInstance;
            if (contactBusName.Substring(contactBusName.Length - 1) == ",")
            {
                contactBusName = contactBusName.Substring(0, contactBusName.Length - 1);
            }
            var ids = contactBusName.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = "'" + ids[i] + "'";
            }
            var idstr = string.Join(",", ids);

            //select stuff((select ',' + ContactName from ContactPerson where ContactPersonUUID in (
            var sql = string.Format("select stuff((select ',' + ContactName from ContactPerson where ContactPersonUUID in ({0}) for xml path('')),1,1,'') as Names ", idstr);
            var cNames = context.Database.FromSql<CNames>(sql);
            systemUserName = cNames[0].Names;

            return systemUserName;
        }


        /// <summary>
        /// 根据uuid查客户经理的名字
        /// </summary>
        /// <param name="clientUuid"></param>
        /// <returns></returns>
        private static string CooM(Guid? clientUuid)
        {
            string clientManager = "";
            using (HaikanCRMContext c1 = new HaikanCRMContext())
            {
                var query = c1.Customer.FirstOrDefault(x => x.ClientUuid == clientUuid);
                var query2 = c1.SystemUser.FirstOrDefault(x => x.SystemUserUuid == query.ClientManager);
                if (query2 != null) clientManager = query2.RealName;
                return clientManager;
            }
        }

    }



}
