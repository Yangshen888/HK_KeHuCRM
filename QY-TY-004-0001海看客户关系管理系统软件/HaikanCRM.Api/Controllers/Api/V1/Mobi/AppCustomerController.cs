using System;
using System.Collections.Generic;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.ViewModels.Client;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HaikanCRM.Api.Controllers.Api.V1.Mobi
{
    /// <summary>
    /// 钉钉端的操作接口
    /// </summary>
    [Route("api/v1/Mobi/[controller]/[action]")]
    [ApiController]
    public class AppCustomerController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public AppCustomerController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取全部状态
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllStatus()
        {
            var list = _dbContext.CustomerStatus.Select(x => new
            {
                x.StatusName,
                x.CustomerStatusUuid,
            }).ToList();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list);
            return Ok(response);
        }

        /// <summary>
        /// 获取全部类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllTypes()
        {
            var list = _dbContext.CustomerTypeList.Select(x => new
            {
                x.TypeName,
                x.TypeUuid,
            }).ToList();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list);
            return Ok(response);
        }

        /// <summary>
        /// 获取全部行业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AllIndustry()
        {
            var list = _dbContext.CustomerIndustry.Select(x => new
            {
                x.IndustryName,
                x.IndustryUuid,
            }).ToList();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list);
            return Ok(response);
        }

        /// <summary>
        /// 根据uuid查询客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult AppGetCustomerInfo(string id)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from x in _dbContext.Customer
                             where x.ClientUuid.ToString() == id
                             select new
                             {
                                 x.Id,
                                 x.ClientUuid,
                                 x.ClientName,
                                 clientManager = x.ClientManagerNavigation.RealName,
                                 customerStatusUuid = x.ClientStatus,
                                 x.BusinessUuid,
                                 x.ClientPhone,
                                 x.ClientAddress,
                                 x.CallLogUuid,
                                 systemUserUuid = x.SystemUserUuid,
                                 x.ClientArea,
                                 industryUuid = x.ClientIndustry,
                                 typeUuid = x.ClientType,
                                 ClientEmail = x.ClientEmail == null ? "" : x.ClientEmail != null ? x.ClientEmail : "",
                                 ClientFax = x.ClientFax == null ? "" : x.ClientFax != null ? x.ClientFax : "",
                                 ClientPostcode = x.ClientPostcode == null ? "" : x.ClientPostcode != null ? x.ClientPostcode : "",
                                 ClientWebsite = x.ClientWebsite == null ? "" : x.ClientWebsite != null ? x.ClientWebsite : "",
                                 Remarks = x.Remarks == null ? "" : x.Remarks != null ? x.Remarks : "",
                                 x.ClientBirthday,

                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }
        
        /// <summary>
        /// 保存编辑客户信息(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult SavaEditCustomerInfo(AppEditModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == Guid.Parse(model.ClientUuid));
                if (entity != null)
                {
                    entity.ClientName = model.ClienName;
                    if (model.ClientStatus != null)
                    {
                        entity.ClientStatus = Guid.Parse(model.ClientStatus);
                    }

                    //entity.ClientPhone = model.ClientPhone;
                    entity.ClientEmail = model.ClientEmail;
                    entity.ClientFax = model.ClientFax;
                    entity.ClientPostcode = model.ClientPostcode;
                    if (model.ClientType != null)
                    {
                        entity.ClientType = Guid.Parse(model.ClientType);
                    }

                    if (model.ClientIndustry != null)
                    {
                        entity.ClientIndustry = Guid.Parse(model.ClientIndustry);
                    }

                    entity.ClientArea = model.ClientArea;
                    entity.ClientWebsite = model.ClientWebsite;
                    entity.ClientAddress = model.ClientAddress;
                    //entity.SystemUserUuid = model.SystemUserUuid.ToString();
                    entity.Remarks = model.Remarks;
                    if (model.ClientBirthday != null)
                    {
                        entity.ClientBirthday = DateTime.Parse(model.ClientBirthday).ToString("yyyy-MM-dd");
                    }
                }

                if (model.usName != null)
                {
                    var entity6 = new SystemLog();
                    entity6.SystemLogUuid = Guid.NewGuid();
                    entity6.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")编辑了客户信息(客户名称:" + model.ClienName + ")";
                    entity6.UserName = model.usName;
                    entity6.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity6.Type = "钉钉端编辑";
                    entity6.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity6);
                }
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询客户赋值模糊匹配控件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult VagueSelectCustomer()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Customer.Where(x => x.IsDelete == 0);
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
                }
                var list = query.Select(x => new { x.ClientName }).ToList();
                //var query = entity.ToList();
                response.SetData(list);
                return Ok(response);
            }
        }


        /// <summary>
        /// 查询联系人赋值模糊匹配控件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult VagueSelectContact()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var entity = from x in _dbContext.ContactPerson
                             select new
                             {
                                 x.ContactName,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询商机赋值模糊匹配控件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult VagueSelectBusiness()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var entity = from x in _dbContext.Business
                             select new
                             {
                                 x.BusinessName,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

    


  

        /// <summary>
        /// 钉钉端添加客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AppCustomerAdd(AppAddClient model)
        {

            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new HaikanCRM.Api.Entities.Customer();
                var zt = _dbContext.CustomerStatus.FirstOrDefault(x => x.StatusName == model.ClientStatus);
                var lx = _dbContext.CustomerTypeList.FirstOrDefault(x => x.TypeName == model.ClientType);
                var entity3 = new Business();
                entity3.BusinessUuid = Guid.NewGuid();
                entity.BusinessUuid = entity3.BusinessUuid.ToString();
                entity.ClientManager = model.ClientManager;
                entity.ClientUuid = Guid.NewGuid();
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                if (model.ClmoName != null)
                {
                    var query = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClmoName);
                    if (query != null)
                    {
                        entity.SuperiorUuid = query.ClientUuid;
                        entity.SuperiorMenu = query.SuperiorMenu + 1;
                    }
                }
                else
                {
                    entity.SuperiorUuid = Guid.Empty;
                    entity.SuperiorMenu = 0;
                }

                entity.IsDelete = 0;
                if (zt == null)
                {
                    entity.ClientStatus = null;
                }
                else
                {
                    entity.ClientStatus = zt.CustomerStatusUuid;
                }
                if (lx == null)
                {
                    entity.ClientType = null;
                }
                else
                {
                    entity.ClientType = lx.TypeUuid;
                }
                if (model.ClientIndustry != null)
                {
                    entity.ClientIndustry = Guid.Parse(model.ClientIndustry);
                }
                entity.ClientName = model.ClienName;
                //entity.ClientPhone = model.ClientPhone;
                entity.ClientEmail = model.ClientEmail;
                entity.ClientFax = model.ClientFax;
                entity.ClientPostcode = model.ClientPostcode;
                entity.ClientArea = model.ClientArea;
                entity.ClientWebsite = model.ClientWebsite;
                entity.ClientAddress = model.ClientAddress;
                if (model.usName != "")
                {
                    var entity6 = new SystemLog();
                    entity6.SystemLogUuid = Guid.NewGuid();
                    entity6.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")添加了一个客户(客户名称:" + model.ClienName + ")";
                    entity6.UserName = model.usName;
                    entity6.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity6.Type = "钉钉端添加";
                    entity6.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity6);
                }
                entity.Remarks = model.Remarks;
                _dbContext.Customer.Add(entity);
                _dbContext.Business.Add(entity3);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 钉钉端获取客户详情
        /// </summary>
        /// <param name="clientUuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult AppCustomerInfo(string clientUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from x in _dbContext.Customer
                             where x.ClientUuid.ToString() == clientUuid
                             select new
                             {
                                 x.Id,
                                 x.ClientUuid,
                                 x.ClientName,
                                 clientManager = x.ClientManagerNavigation.RealName,
                                 //clientStatus = x.ClientStatusNavigation.StatusName,
                                 clientStatus = x.ClientStatusNavigation.StatusName == null ? "" : x.ClientStatusNavigation.StatusName != null ? x.ClientStatusNavigation.StatusName : "",
                                 x.BusinessUuid,
                                 x.ClientPhone,
                                 x.ClientAddress,
                                 x.CallLogUuid,
                                 //SystemUserUuid = selectusername.Trim(','),
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        ///获取该客户全部联系记录	
        /// </summary>
        /// <param name="clientUuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult AppCustomerCallLog(string clientUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {

                var entity = from c1 in _dbContext.ContactCallLog
                             join c2 in _dbContext.ContactPerson
                             on c1.ContactPersonUuid equals c2.ContactPersonUuid
                             //join c3 in _dbContext.Customer
                             //on c1.ClientUuid equals c3.ClientUuid
                             where c1.IsDelete == 0 && c1.ClientUuid.ToString() == clientUuid
                             select new
                             {
                                 c1.Id,
                                 c1.ClientUuid,
                                 c1.CallContent,
                                 c1.CallTime,
                                 c1.CallLogUuid,
                                 c2.ContactName,
                                 c1.ClientUu.ClientName,
                                 c1.SheBeiId,
                             };
                entity = entity.OrderByDescending(x => x.Id);
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }
        /// <summary>
        /// 获取钉钉端客户列表
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AppCustomerList(AppAddClient mode)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                //var query = _dbContext.Customer.Where(x => x.ClientManager.ToString() == "423968DF-A15D-4B5F-9E25-005486496332" && x.IsDelete == 0);
                var query = from x in _dbContext.Customer
                            where x.SuperiorUuid.ToString() == "00000000-0000-0000-0000-000000000000" && x.IsDelete == 0
                            select new
                            {
                                x.Id,
                                x.ClientUuid,
                                x.ClientName,
                                ClientManageruuid = x.ClientManager,
                                ClientManager = x.ClientManagerNavigation.RealName,
                                x.ClientStatusNavigation.StatusName,
                                x.BusinessUuid,
                                x.ClientPhone,
                                x.ClientPostcode,
                                x.ClientAddress,
                                Manager = x.ClientManager,
                            };
                // 如果是我们指定的角色,可以查看所有的客户,否则只能看自己负责的客户
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.Manager == AuthContextService.CurrentUser.Guid);
                }
                //如果客户名称搜索框不为空,模糊匹配搜索框内容
                if (!string.IsNullOrEmpty(mode.Kk1))
                {
                    query = query.Where(x => x.ClientName.Contains(mode.Kk1));
                }
                //按照ID排序(降序)
                query = query.OrderByDescending(x => x.Id);
                var qu = query.ToList();

                string param2 = "{\"code\":0,\"message\":\"成功\",\"ListData\":[";
                for (int i = 0; i < qu.Count; i++)
                {
                    if (i == 0)
                    {
                        param2 += "{ \"Id\": \"" + qu[i].Id + "\",\"ClientUuid\":\"" + qu[i].ClientUuid + "\",\"ClientName\":\"" + qu[i].ClientName + "\",\"ClientManager\":\"" + qu[i].ClientManager + "\",\"StatusName\":\"" + qu[i].StatusName
                            + "\",\"BusinessUuid\":\"" + qu[i].BusinessUuid + "\",\"ClientPhone\":\"" + qu[i].BusinessUuid + "\",\"ClientPostcode\":\"" + qu[i].BusinessUuid + "\",\"ClientAddress\":\"" + qu[i].BusinessUuid + "\",\"checkbox\":\"" +
                            Gettruefalse(qu[i].ClientManageruuid.ToString(), mode.SystemUserUuid, qu[i].ClientName, mode.Kk1) + "\",\"children\":[" + Geterjicd(qu[i].ClientUuid.ToString(), mode.SystemUserUuid, mode.Kk1) + "]}";
                    }
                    else
                    {
                        param2 += ",{ \"Id\": \"" + qu[i].Id + "\",\"ClientUuid\":\"" + qu[i].ClientUuid + "\",\"ClientName\":\"" + qu[i].ClientName + "\",\"ClientManager\":\"" + qu[i].ClientManager + "\",\"StatusName\":\"" + qu[i].StatusName
                            + "\",\"BusinessUuid\":\"" + qu[i].BusinessUuid + "\",\"ClientPhone\":\"" + qu[i].BusinessUuid + "\",\"ClientPostcode\":\"" + qu[i].BusinessUuid + "\",\"ClientAddress\":\"" + qu[i].BusinessUuid + "\",\"checkbox\":\"" +
                            Gettruefalse(qu[i].ClientManageruuid.ToString(), mode.SystemUserUuid, qu[i].ClientName, mode.Kk1) + "\",\"children\":[" + Geterjicd(qu[i].ClientUuid.ToString(), mode.SystemUserUuid, mode.Kk1) + "]}";
                    }
                }
                param2 += "]}";
                var result = JsonConvert.DeserializeObject<CustomerRoot>(param2);
                response.SetData(result);
                return Ok(response);
            }
        }

        /// <summary>
        /// 递归算法
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        public string Geterjicd(string ids, string name1, string name2)
        {
            string strlist = "";
            var qures = _dbContext.Customer.Where(x => x.SuperiorUuid.ToString() == ids);
            var strlistss = qures.ToList();
            if (strlistss.Count > 0)
            {
                for (int i = 0; i < strlistss.Count; i++)
                {
                    if (i == 0)
                    {
                        strlist += "{ \"Id\": \"" + strlistss[i].Id + "\",\"ClientUuid\":\"" + strlistss[i].ClientUuid + "\",\"ClientName\":\"" + strlistss[i].ClientName + "\",\"ClientManager\":\"" + (_dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == strlistss[i].ClientManager.ToString()) != null ? _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == strlistss[i].ClientManager.ToString())?.RealName : "") + "\",\"StatusName\":\"" + (strlistss[i].ClientStatusNavigation != null ? strlistss[i].ClientStatusNavigation.StatusName : "")
                            + "\",\"BusinessUuid\":\"" + strlistss[i].BusinessUuid + "\",\"ClientPhone\":\"" + strlistss[i].BusinessUuid + "\",\"ClientPostcode\":\"" + strlistss[i].BusinessUuid + "\",\"ClientAddress\":\"" + strlistss[i].BusinessUuid + "\",\"checkbox\":\"" +
                            Gettruefalse(strlistss[i].ClientManager.ToString(), name1, strlistss[i].ClientName, name2) + "\",\"children\":[" + Geterjicd(strlistss[i].ClientUuid.ToString(), name1, name2) + "]}";
                    }
                    else
                    {
                        strlist += ",{ \"Id\": \"" + strlistss[i].Id + "\",\"ClientUuid\":\"" + strlistss[i].ClientUuid + "\",\"ClientName\":\"" + strlistss[i].ClientName + "\",\"ClientManager\":\"" + (_dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == strlistss[i].ClientManager.ToString()) != null ? _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == strlistss[i].ClientManager.ToString())?.RealName : "") + "\",\"StatusName\":\"" + (strlistss[i].ClientStatusNavigation != null ? strlistss[i].ClientStatusNavigation.StatusName : "")
                            + "\",\"BusinessUuid\":\"" + strlistss[i].BusinessUuid + "\",\"ClientPhone\":\"" + strlistss[i].BusinessUuid + "\",\"ClientPostcode\":\"" + strlistss[i].BusinessUuid + "\",\"ClientAddress\":\"" + strlistss[i].BusinessUuid + "\",\"checkbox\":\"" +
                            Gettruefalse(strlistss[i].ClientManager.ToString(), name1, strlistss[i].ClientName, name2) + "\",\"children\":[" + Geterjicd(strlistss[i].ClientUuid.ToString(), name1, name2) + "]}";
                    }
                }
            }
            return strlist;
        }


        /// <summary>
        /// 判断当前人有没有权限看到这个栏目
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        public string Gettruefalse(string id1, string id2, string name1, string name2)
        {
            string strnames = "false";
            if (!string.IsNullOrEmpty(name2))
            {
                if (id1 == id2 && name1.Contains(name2))
                {
                    strnames = "true";
                }
            }
            else
            {
                if (id1 == id2)
                {
                    strnames = "true";
                }
            }
            return strnames;
        }



    }
}
