using System;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.RequestPayload.Contact;
using HaikanCRM.Api.ViewModels.CallLog;
using Microsoft.AspNetCore.Mvc;

namespace HaikanCRM.Api.Controllers.Api.V1.Mobi
{
    /// <summary>
    /// 钉钉端相关联系记录操作接口
    /// </summary>
    [Route("api/v1/Mobi/[controller]/[action]")]
    [ApiController]
    public class AppCallLogController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public AppCallLogController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// 钉钉端联系记录列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppCallLogList(CallLogRequstPaload payload)
        {
            using (_dbContext)
            {
                var d5 = DateTime.Now.ToString("yyyy-MM-dd");
                var query = from c1 in _dbContext.ContactCallLog
                            where c1.IsDelete == 0
                            select new
                            {
                                c1.Id,
                                c1.CallContent,
                                c1.CallTime,
                                c1.CallLogUuid,
                                c1.ContactPersonUu.ContactName,
                                c1.ContactPersonUuid,
                                c1.ClientUu.ClientName,
                                BusinessName = c1.BusinessUu.BusinessName == null ? "" : c1.BusinessUu.BusinessName != null ? c1.BusinessUu.BusinessName : "",
                                c1.ClientUu.ClientManager,
                                c1.ClientUuid,
                                c1.SheBeiId,
                            };

                // TODO:不可以硬编码,下个版本解决
                // 如果是我们指定的角色,可以查看所有的联系记录,否则只能看自己的联系记录
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
                }
                //如果联系人名字不为空 则模糊匹配该联系人的联系记录
                if (!string.IsNullOrEmpty(payload.ContactName))
                {
                    query = query.Where(x => x.ContactName.Contains(payload.ContactName));
                }
                //如果客户经理不为空 则查血客户经理是当前登录用户的当天的联系就记录
                if (payload.logUuid != "")
                {
                    query = query.Where(x => x.ClientManager.ToString() == payload.logUuid && x.CallTime.Substring(0, 10) == d5);
                }
                //按照ID排序(降序)
                query = query.OrderByDescending(x => x.Id);
                var query1 = query.ToList();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query1);
                return Ok(response);
            }
        }

        /// <summary>
        /// 钉钉端联系记录详情
        /// </summary>
        /// <returns></returns>
        [HttpGet("{callLogUuid}")]
        public IActionResult AppCallLogInfo(string callLogUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var entity = from c1 in _dbContext.ContactCallLog
                             where c1.CallLogUuid.ToString() == callLogUuid
                             select new
                             {
                                 c1.Id,
                                 c1.CallContent,
                                 c1.CallTime,
                                 c1.CallLogUuid,
                                 c1.ContactPersonUu.ContactName,
                                 c1.ClientUu.ClientName,


                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }


        /// <summary>
        /// 联系人界面联系记录
        /// </summary>
        /// <param name="contactPersonUuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetContactCallLogList(string contactPersonUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var entity = from c1 in _dbContext.ContactCallLog
                                 //join c2 in _dbContext.ContactPerson
                                 //on c1.ContactPersonUuid equals c2.ContactPersonUuid
                                 //join c3 in _dbContext.Customer
                                 //on c1.ClientUuid equals c3.ClientUuid
                             where c1.IsDelete == 0 && c1.ContactPersonUuid.ToString() == contactPersonUuid
                             select new
                             {
                                 c1.Id,
                                 c1.ClientUuid,
                                 c1.CallContent,
                                 c1.CallTime,
                                 c1.CallLogUuid,
                                 c1.ContactPersonUu.ContactName,
                                 c1.ClientUu.ClientName,
                                 c1.BusinessUu.BusinessName,
                                 c1.SheBeiId,
                             };
                entity = entity.OrderByDescending(x => x.Id);
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 查看某个客户的联系记录
        /// </summary>
        /// <param name="clientUuid">客户的uuid</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult CallLogList(string clientUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var entity = from c1 in _dbContext.ContactCallLog
                             where c1.IsDelete == 0 && c1.ClientUuid.ToString() == clientUuid
                             select new
                             {
                                 c1.Id,
                                 c1.CallContent,
                                 c1.CallTime,
                                 c1.CallLogUuid,
                                 c1.ClientUu.ClientName,
                                 c1.ClientUuid,
                                 c1.SheBeiId,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据客户ID获取商机下拉框的值
        /// </summary>
        /// <param name="clientUuid"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetCusIdSelectBus(string clientUuid, string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from c1 in _dbContext.Business
                             where c1.IsDelete == 0 && c1.ClientUuid.ToString() == clientUuid
                             select new
                             {
                                 //c1.SystemUserUuid,
                                 // c1.Id,s
                                 c1.BusinessName,
                                 c1.BusinessUuid,
                             };

                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 根据输入客户获取该客户联系人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[ProducesResponseType(200)]
        public IActionResult PassCusSelectCon(Dataqqq model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var qq = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientNames);
                if (qq != null)
                {
                    var entity = from c1 in _dbContext.ContactPerson
                                 where c1.ClientUuid == qq.ClientUuid
                                 select new
                                 {
                                     c1.ContactPersonUuid,
                                     c1.ContactName,
                                 };
                    var query = entity.ToList();
                    response.SetData(query);
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("该客户无联系人");
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 根据输入客户获取该客户商机
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        //[ProducesResponseType(200)]
        public IActionResult PassCusSelectBus(Dataqqq model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var qq = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientNames);
                if (qq != null)
                {
                    var entity = from c1 in _dbContext.Business
                                 where c1.ClientUuid == qq.ClientUuid
                                 select new
                                 {
                                     c1.BusinessName,
                                     c1.BusinessUuid,
                                 };

                    var query = entity.ToList();
                    response.SetData(query);
                    return Ok(response);
                }
                else
                {
                    response.SetFailed("该客户无商机");
                    return Ok(response);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class Dataqqq
        {
            /// <summary>
            /// 
            /// </summary>
            public string ClientNames { get; set; }
        }

        /// <summary>
        /// 钉钉端修改客户联系记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppEditCallLog(CallLogViewModels model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = new ContactCallLog();
                entity.CallLogUuid = Guid.NewGuid();
                entity.ContactPersonUuid = Guid.Parse(model.ContactPersonUuid);
                entity.ClientUuid = Guid.Parse(model.ClientUuid);
                if (model.BusinessUuid != "")
                {
                    entity.BusinessUuid = Guid.Parse(model.BusinessUuid);
                }
                entity.CallContent = model.CallContent;
                if (model.ContactDetailsUuid != "")
                {
                    if (model.ContactDetailsUuid == "0")
                    {
                        entity.ContactDetailsName = "微信";
                    }
                    if (model.ContactDetailsUuid == "1")
                    {
                        entity.ContactDetailsName = "QQ";
                    }
                    if (model.ContactDetailsUuid == "2")
                    {
                        entity.ContactDetailsName = "电话";
                    }
                    if (model.ContactDetailsUuid == "3")
                    {
                        entity.ContactDetailsName = "面谈";
                    }
                    if (model.ContactDetailsUuid == "4")
                    {
                        entity.ContactDetailsName = "邮件";
                    }
                }
                if (model.CallTime == "")
                {
                    response.SetFailed("请选择联系时间");
                    return Ok(response);
                }
                else
                {
                    var d2 = DateTime.Parse(model.CallTime).ToString("yyyy-MM-dd HH:mm");
                    entity.CallTime = d2;
                }
                if (model.usName != "")
                {
                    var quee = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid.ToString() == model.ClientUuid);
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    if (quee != null)
                        entity3.OperationContent =
                            "钉钉端客户管理系统-用户(" + model.usName + ")添加了一条与客户(" + quee.ClientName + ")的联系记录";
                    entity3.UserName = model.usName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "钉钉端添加";
                    entity3.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity3);
                }
                entity.IsDelete = 0;
                _dbContext.ContactCallLog.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }

        }

        /// <summary>
        /// 添加联系记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppAddCallLog(CallLogViewModels model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = new ContactCallLog();
                entity.CallLogUuid = Guid.NewGuid();
                entity.ContactPersonUuid = Guid.Parse(model.ContactPersonUuid);
                var query = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientName);
                if (query != null)
                {
                    entity.ClientUuid = query.ClientUuid;
                    if (model.BusinessUuid != null)
                    {
                        entity.BusinessUuid = Guid.Parse(model.BusinessUuid);
                    }
                    entity.CallContent = model.CallContent;
                    if (model.ContactDetailsUuid != "")
                    {
                        if (model.ContactDetailsUuid == "0")
                        {
                            entity.ContactDetailsName = "微信";
                        }

                        if (model.ContactDetailsUuid == "1")
                        {
                            entity.ContactDetailsName = "QQ";
                        }

                        if (model.ContactDetailsUuid == "2")
                        {
                            entity.ContactDetailsName = "电话";
                        }

                        if (model.ContactDetailsUuid == "3")
                        {
                            entity.ContactDetailsName = "面谈";
                        }

                        if (model.ContactDetailsUuid == "4")
                        {
                            entity.ContactDetailsName = "邮件";
                        }
                    }

                    if (model.CallTime == null)
                    {
                        response.SetFailed("请选择联系时间");
                        return Ok(response);
                    }
                    else
                    {
                        var d2 = DateTime.Parse(model.CallTime).ToString("yyyy-MM-dd HH:mm");
                        entity.CallTime = d2;
                    }

                    if (model.usName != "")
                    {
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        entity3.OperationContent =
                            "钉钉端客户管理系统-用户(" + model.usName + ")添加了一条与客户(" + query.ClientName + ")的联系记录";
                        entity3.UserName = model.usName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "钉钉端添加";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }
                }

                entity.IsDelete = 0;
                _dbContext.ContactCallLog.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 联系人详情页面跳转添加联系记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppConSkipAddCallLog(CallLogViewModels model)
        {
            {
                var response = ResponseModelFactory.CreateInstance;
                using (_dbContext)
                {
                    var entity = new ContactCallLog();
                    entity.CallLogUuid = Guid.NewGuid();
                    entity.ContactPersonUuid = Guid.Parse(model.ContactPersonUuid);
                    entity.ClientUuid = Guid.Parse(model.ClientUuid);
                    if (model.BusinessUuid != "")
                    {
                        entity.BusinessUuid = Guid.Parse(model.BusinessUuid);
                    }
                    entity.CallContent = model.CallContent;
                    if (model.ContactDetailsUuid != "")
                    {
                        if (model.ContactDetailsUuid == "0")
                        {
                            entity.ContactDetailsName = "微信";
                        }
                        if (model.ContactDetailsUuid == "1")
                        {
                            entity.ContactDetailsName = "QQ";
                        }
                        if (model.ContactDetailsUuid == "2")
                        {
                            entity.ContactDetailsName = "电话";
                        }
                        if (model.ContactDetailsUuid == "3")
                        {
                            entity.ContactDetailsName = "面谈";
                        }
                        if (model.ContactDetailsUuid == "4")
                        {
                            entity.ContactDetailsName = "邮件";
                        }
                    }
                    if (model.CallTime == "")
                    {
                        response.SetFailed("请选择联系时间");
                        return Ok(response);
                    }
                    else
                    {
                        var d2 = DateTime.Parse(model.CallTime).ToString("yyyy-MM-dd HH:mm");
                        entity.CallTime = d2;
                    }
                    if (model.usName != "")
                    {
                        var quert = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        if (quert != null)
                            entity3.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")添加了一条与客户(" +
                                                       quert.ClientName + ")的联系记录";
                        entity3.UserName = model.usName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "钉钉端添加";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }
                    entity.IsDelete = 0;
                    _dbContext.ContactCallLog.Add(entity);
                    _dbContext.SaveChanges();
                    response.SetSuccess();
                    return Ok(response);
                }
            }

        }
    }
}
