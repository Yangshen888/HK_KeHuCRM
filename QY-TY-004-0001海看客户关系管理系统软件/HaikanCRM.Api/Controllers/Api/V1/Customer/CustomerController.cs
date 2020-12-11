using System;
using System.Collections.Generic;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Entities.Enums;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.Extensions.DataAccess;
using HaikanCRM.Api.Models.Response;
using HaikanCRM.Api.RequestPayload.Bussiness;
using HaikanCRM.Api.RequestPayload.Client;
using HaikanCRM.Api.RequestPayload.Contact;
using HaikanCRM.Api.ViewModels.Client;
using HaikanCRM.Api.ViewModels.Rbac.SystemMenu;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HaikanCRM.Api.Controllers.Api.V1.Customer
{
    /// <summary>
    /// 相关客户的操作接口
    /// </summary>
    [Route("api/v1/Customer/[controller]/[action]")]
    [ApiController]
    //[CustomAuthorize]
    public class CustomerController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public CustomerController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 客户列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(MyClientRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from x in _dbContext.Customer
                            where x.IsDelete == 0 && x.ClientManager.ToString() == payload.Kw1
                            select new
                            {
                                x.Id,
                                x.ClientUuid,
                                x.ClientName,
                                x.ClientManagerNavigation.RealName,
                                x.ClientStatusNavigation.StatusName,
                                x.ClientIndustryNavigation.IndustryName,
                                cpName = SelectcpName(x.ClientUuid).Trim(','),
                                x.ClientArea,
                                x.BusinessUuid,
                                x.SystemUserUuid,
                                //systemUserName = SelectName2(x.ClientUuid).Trim(','),
                                x.SuperiorMenu
                            };

                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.ClientName.Contains(payload.Kw.Trim()));
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
        /// 根据uuid获取联系人
        /// </summary>
        /// <param name="clientUuid"></param>
        /// <returns></returns>
        private static string SelectcpName(Guid clientUuid)
        {
            string cpName = "";
            using (HaikanCRMContext c1 = new HaikanCRMContext())
            {
                var participantids = c1.ContactPerson.Where(x => x.ClientUuid == clientUuid);//联系人名字
                if (participantids.ToList().Count > 0)
                {
                    for (int i = 0; i < participantids.ToList().Count; i++)
                    {
                        cpName += participantids.ToList()[i].ContactName + ",";
                    }
                }
            }
            if (cpName == "")
            {
                cpName = "";
            }
            return cpName;
        }

        /// <summary>
        /// 树形控件 客户列表
        /// </summary>
        /// <param name="tc"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetList(TreeClient tc)
        {
            using (_dbContext)
            {
                //var query = from x in _dbContext.Customer
                //            where x.IsDelete == 0
                //            select new
                //            {
                //                x.Id,
                //                x.ClientUuid,
                //                x.ClientName,
                //                x.ClientManager,
                //                x.ClientManagerNavigation.RealName,
                //                x.ClientStatusNavigation.StatusName,
                //                x.BusinessUuid,
                //                x.SystemUserUuid,
                //                x.SuperiorUuid,
                //                x.ClientIndustryNavigation.IndustryName,
                //                cpName = SelectcpName(x.ClientUuid).Trim(','),
                //                x.ClientArea,
                //                x.ClientBirthday,
                //                //systemUserName = SelectName2(x.ClientUuid).Trim(','),
                //                x.SuperiorMenu
                //            };
                var query = _dbContext.CustomerTree.Where(x => x.IsDelete == 0);

                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == Guid.Parse(tc.Kw1));
                }
                if (!string.IsNullOrEmpty(tc.Kw))
                {
                    query = query.Where(x => x.ClientName.Contains(tc.Kw.Trim()));
                }

                query = query.OrderByDescending(x => x.Id);
                var data = query.Paged(tc.CurrentPage, tc.PageSize).ToList();
                var totalCount = query.Count();

                //拼接json字符串
                var returndata = "[";
                for (int i = 0; i < data.Count; i++)
                {
                    //int shuju = _dbContext.Customer.Count(x => x.ClientUuid == data[i].SuperiorUuid);
                    int shuju = data[i].SpCount.Value;
                    var lxrlist = "";
                    if (data[i].CpName != "")
                    {
                        string[] lists = data[i].CpName?.Split(',');
                        if (lists != null && lists.Length > 0)
                        {
                            for (int i1 = 0; i1 < lists.Length; i1++)
                            {
                                if (lists[i1] != "")
                                {
                                    if (lxrlist != "")
                                    {
                                        lxrlist += ",\"" + lists[i1] + "\"";
                                    }
                                    else
                                    {
                                        lxrlist = "\"" + lists[i1] + "\"";
                                    }
                                }
                            }
                        }
                    }

                    var datatttt = lxrlist;
                    if (data[i].SuperiorMenu == 0 || shuju == 0)
                    {
                        //var cplist = data[i].cpName.Split(',');
                        returndata = returndata + "{clientuuid:'" + data[i].ClientUuid + "',clientName:'" + data[i].ClientName + "',clientManager:'" + data[i].ClientManager + "',realName:'" + data[i].RealName + "',clientBirthday:'" + data[i].ClientBirthday + "',statusName:'" + data[i].StatusName + "',industryName:'" + data[i].IndustryName + "',cpName:[" + datatttt + "],clientArea:'" + data[i].ClientArea + "',children:" + ChildrenData(data[i].ClientUuid) + "},";
                    }
                }

                returndata = returndata.Trim(',') + "]";
                var obj = JsonConvert.DeserializeObject(returndata);

                return Ok(new { obj, totalCount });
            }
        }

        /// <summary>
        /// 获取子级方法(拼接到json字符串)
        /// </summary>
        /// <param name="clientuuid"></param>
        /// <returns></returns>
        public string ChildrenData(Guid clientuuid)
        {

            //var shujudata = (from x in _dbContext.Customer
            //                 where x.IsDelete == 0 && x.SuperiorUuid == clientuuid
            //                 select new
            //                 {
            //                     x.Id,
            //                     x.ClientUuid,
            //                     x.ClientName,
            //                     x.ClientManagerNavigation.RealName,
            //                     x.ClientStatusNavigation.StatusName,
            //                     x.BusinessUuid,
            //                     x.SystemUserUuid,
            //                     x.SuperiorUuid,
            //                     x.ClientManager,
            //                     x.ClientBirthday,
            //                     x.ClientIndustryNavigation.IndustryName,
            //                     cpName = SelectcpName(x.ClientUuid).Trim(','),
            //                     x.ClientArea,
            //                     //systemUserName = SelectName2(x.ClientUuid).Trim(','),
            //                     x.SuperiorMenu
            //                 }).ToList();
            var shujudata = _dbContext.CustomerTree.Where(x => x.SuperiorUuid == clientuuid && x.IsDelete == 0)
                .ToList();
            if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
            {
                shujudata = shujudata.FindAll(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
            }
            var childendata = "[";
            //如果还有子集则一直调用此ChildrenData()方法
            if (shujudata.Count > 0)
            {

                for (int i = 0; i < shujudata.Count; i++)
                {
                    //var shuju = _dbContext.Customer.Count(x => x.ClientUuid == shujudata[i].SuperiorUuid);
                    var lxrlist = "";
                    if (shujudata[i].CpName != "")
                    {
                        string[] lists = shujudata[i].CpName?.Split(',');
                        if (lists != null && lists.Length > 0)
                        {
                            for (int i1 = 0; i1 < lists.Length; i1++)
                            {
                                if (lists[i1] != "")
                                {
                                    if (lxrlist != "")
                                    {
                                        lxrlist += ",\"" + lists[i1] + "\"";
                                    }
                                    else
                                    {
                                        lxrlist = "\"" + lists[i1] + "\"";
                                    }
                                }
                            }
                        }
                    }

                    var datatttt = lxrlist;
                    childendata = childendata + "{clientuuid:'" + shujudata[i].ClientUuid + "',clientName:'" + shujudata[i].ClientName + "',clientManager:'" + shujudata[i].ClientManager + "',clientBirthday:'" + shujudata[i].ClientBirthday + "',realName:'" + shujudata[i].RealName + "',statusName:'" + shujudata[i].StatusName + "',industryName:'" + shujudata[i].IndustryName + "',cpName:[" + datatttt + "],clientArea:'" + shujudata[i].ClientArea + "',children:" + ChildrenData(shujudata[i].ClientUuid) + "},";
                }
            }

            return childendata.Trim(',') + "]";
        }

        /// <summary>
        /// 用户穿梭框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Allsystemuser()
        {
            using (_dbContext)
            {
                var query = from s in _dbContext.SystemUser
                            where s.IsDeleted == 0
                            select new
                            {
                                key = s.SystemUserUuid,
                                key1 = s.Id,
                                loginName = s.LoginName,//登录名
                                label = s.RealName,//真实姓名
                                isDeleted = s.IsDeleted,
                                disabled = false
                            };

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 已选择用户
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Selectsystemuser(int id)
        {
            using (_dbContext)
            {
                var participantids = _dbContext.Customer.Where(x => x.Id == id).ToList()[0].SystemUserUuid;//参与人id

                List<studentyibangmodel> stuyibangmodel = new List<studentyibangmodel>();
                if (participantids != "" && participantids != null)
                {
                    var patcount = participantids.Trim(',').Split(',');

                    foreach (var item in patcount)
                    {
                        var chaxun = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == item);
                        if (chaxun != null)
                        {
                            studentyibangmodel stuyibang = new studentyibangmodel();

                            stuyibang.key = item;
                            stuyibang.label = chaxun.RealName;
                            stuyibang.disabled = false;
                            stuyibangmodel.Add(stuyibang);
                        }
                    }

                }

                var list = stuyibangmodel.ToArray();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 已选择用户
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Selectsystemuser1(int id)
        {
            using (_dbContext)
            {
                var participantids = _dbContext.Business.Where(x => x.Id == id).ToList()[0].SystemUserUuid;//参与人id

                List<studentyibangmodel> stuyibangmodel = new List<studentyibangmodel>();
                if (participantids != "" && participantids != null)
                {
                    var patcount = participantids.Trim(',').Split(',');

                    foreach (var item in patcount)
                    {
                        var chaxun = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid.ToString() == item);
                        if (chaxun != null)
                        {
                            studentyibangmodel stuyibang = new studentyibangmodel();

                            stuyibang.key = item;
                            stuyibang.label = chaxun.RealName;
                            stuyibang.disabled = false;
                            stuyibangmodel.Add(stuyibang);
                        }
                    }

                }

                var list = stuyibangmodel.ToArray();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Tree()
        {
            var response = ResponseModelFactory.CreateInstance;
            var tree = LoadMenuTree();
            response.SetData(tree);
            return Ok(response);
        }

        /// <summary>
        /// 树形结构
        /// </summary>
        /// <returns></returns>
        private List<MenuTree> LoadMenuTree()
        {
            var temp = _dbContext.Customer.Where(x => x.IsDelete == (int)CommonEnum.IsDeleted.No).ToList().Select(x => new MenuTree
            {
                Guid = x.ClientUuid.ToString(),
                ParentGuid = x.SuperiorUuid,
                Title = x.ClientName
            }).ToList();
            var root = new MenuTree
            {
                Title = "全部",
                Guid = Guid.Empty.ToString(),
                ParentGuid = null
            };
            temp.Insert(0, root);
            var tree = temp.BuildTree();
            return tree;
        }

        /// <summary>
        /// 树形结构
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SupTree(string guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            var tree = SupLoadMenuTree(guid);
            response.SetData(tree);
            return Ok(response);
        }

        /// <summary>
        /// 树形结构
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private List<MenuTree> SupLoadMenuTree(string guid)
        {
            var super = _dbContext.Customer.FirstOrDefault(x => x.IsDelete == (int)CommonEnum.IsDeleted.No && x.ClientUuid == Guid.Parse(guid));
            if (super != null && super.SuperiorUuid == Guid.Empty)
            {
                var temp = _dbContext.Customer.Where(x => x.IsDelete == (int)CommonEnum.IsDeleted.No && x.SuperiorUuid == super.ClientUuid).ToList().Select(x => new MenuTree
                {
                    Guid = x.ClientUuid.ToString(),
                    ParentGuid = x.SuperiorUuid,
                    Title = x.ClientName
                }).ToList();
                //var roots = new MenuTree
                //{
                //    Title = super.ClientName,
                //    Guid = super.ClientUuid.ToString(),
                //    ParentGuid = null
                //};
                var tree = temp.BuildTree();
                return tree;
            }
            else
            {
                var supers = _dbContext.Customer.FirstOrDefault(x => x.IsDelete == (int)CommonEnum.IsDeleted.No && x.ClientUuid == super.SuperiorUuid);
                var temp = _dbContext.Customer.Where(x => x.IsDelete == (int)CommonEnum.IsDeleted.No && x.SuperiorUuid == supers.ClientUuid).ToList().Select(x => new MenuTree
                {
                    Guid = x.ClientUuid.ToString(),
                    ParentGuid = x.SuperiorUuid,
                    Title = x.ClientName
                }).ToList();
                //var root = new MenuTree
                //{
                //    Title = supers.ClientName,
                //    Guid = supers.ClientUuid.ToString(),
                //    ParentGuid = null
                //};
                var tree = temp.BuildTree();
                return tree;
            }
        }

        /// <summary>
        /// 后台创建客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(MyClientViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new HaikanCRM.Api.Entities.Customer();
                //var entity2 = new HaikanCRM.Api.Entities.ContactPerson();
                entity.ClientManager = model.UserGuid;
                if (_dbContext.Customer.Count(x => x.ClientName == model.ClientName && x.IsDelete == 0) > 0)
                {
                    response.SetFailed("客户已存在");
                    return Ok(response);
                }
                entity.ClientName = model.ClientName;
                entity.ClientUuid = Guid.NewGuid();
                entity.ClientStatus = model.ClientStatus;
                entity.ClientPhone = model.ClientPhone;
                entity.ClientEmail = model.ClientEmail;
                entity.ClientFax = model.ClientFax;
                entity.ClientPostcode = model.ClientPostcode;
                entity.ClientType = model.ClientType;
                entity.ClientIndustry = model.ClientIndustry;
                entity.ClientBirthday = model.birthTime;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.ClientArea = model.ClientArea;
                entity.ClientWebsite = model.ClientWebsite;
                entity.ClientAddress = model.ClientAddress;
                entity.Remarks = model.Remarks;
                entity.IsDelete = 0;
                if (model.SuperiorUuid == null || model.SuperiorUuid == Guid.Empty)
                {
                    entity.SuperiorUuid = Guid.Empty;
                    entity.SuperiorMenu = 0;
                }
                else
                {
                    Guid? newuuid = model.SuperiorUuid;
                    var supentity = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == newuuid);
                    if (supentity != null)
                    {
                        entity.SuperiorMenu = supentity.SuperiorMenu + 1;
                        entity.SuperiorUuid = newuuid;
                        supentity.SystemUserUuid = supentity.SystemUserUuid + entity.SystemUserUuid;
                    }
                }
                if (model.SystemUserUuid == "")
                {
                    entity.SystemUserUuid = entity.ClientManager.ToString() + ",";
                }
                else if (model.SystemUserUuid.Contains(entity.ClientManager.ToString()))
                {
                    entity.SystemUserUuid = model.SystemUserUuid;
                }
                else
                {
                    entity.SystemUserUuid = model.SystemUserUuid + entity.ClientManager.ToString() + ",";
                }
                if (model.SystemUserUuid != "")
                {
                    var entity2 = new SystemMessage();
                    entity2.MessageUuid = Guid.NewGuid();
                    entity2.ClientUuid = entity.ClientUuid;
                    entity2.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity2.IsDelete = 0;
                    _dbContext.SystemMessage.Add(entity2);
                }
                if (model.UserName != "")
                {
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    entity3.OperationContent = "用户" + model.UserName + "添加了一个客户(客户名称:" + model.ClientName + ")";
                    entity3.UserName = model.UserName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "添加";
                    entity3.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity3);
                }
                _dbContext.Customer.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取客户编辑数据
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetEdit(string guid)
        {
            using (_dbContext)
            {
                var participantids = _dbContext.Customer.Where(x => x.ClientUuid.ToString() == guid).ToList()[0].SystemUserUuid;//参与人uuid
                var systemUserName = "";
                if (participantids != "" && participantids != null)
                {
                    var patcount = participantids.Split(',');
                    for (int i = 0; i < patcount.Length; i++)
                    {
                        if (patcount[i] != "")
                        {
                            systemUserName += _dbContext.SystemUser.Where(x => x.SystemUserUuid == Guid.Parse(patcount[i])).ToList()[0].RealName + ",";
                        }

                    }
                }
                var query = (from x in _dbContext.Customer
                             where x.IsDelete == 0 && x.ClientUuid.ToString() == guid
                             select new
                             {
                                 x.Id,
                                 x.ClientUuid,
                                 x.ClientName,
                                 x.ClientManager,
                                 x.ClientStatus,
                                 x.ClientPhone,
                                 x.ClientEmail,
                                 x.ClientBirthday,
                                 x.ClientFax,
                                 x.ClientPostcode,
                                 x.ClientType,
                                 x.ClientIndustry,
                                 x.ClientArea,
                                 x.ClientWebsite,
                                 x.ClientAddress,
                                 x.Remarks,
                                 x.BusinessUuid,
                                 systemUserName = systemUserName.Trim(','),
                                 systemUserUuid = x.SystemUserUuid,
                                 x.SuperiorUuid

                             }).FirstOrDefault();
                var response = ResponseModelFactory.CreateInstance;
                var supname = "";
                if (query != null && query.SuperiorUuid != Guid.Empty && query.SuperiorUuid != null)
                {
                    supname = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == query.SuperiorUuid)?.ClientName;
                    response.SetData(new { query, supname });
                }
                response.SetData(new { query, supname });
                return Ok(response);
            }
        }

        /// <summary>
        /// 商机联系记录详情
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BusView2(CallLogRequstPaload payload)
        {
            using (_dbContext)
            {
                var query = from c1 in _dbContext.ContactCallLog
                            where c1.IsDelete == 0 && c1.BusinessUuid.ToString() == payload.guid
                            select new
                            {
                                c1.Id,
                                c1.CallContent,
                                c1.CallTime,
                                c1.CallLogUuid,
                                contactName = c1.ContactPersonUu.ContactName,
                                clientName = c1.ClientUu.ClientName,
                                c1.ClientUuid,
                                c1.SheBeiId,
                                c1.BusinessUuid,
                                c1.ContactDetailsUuid,
                                c1.ContactDetailsName,
                                businessName = c1.BusinessUu.BusinessName,
                                cdName = c1.ContactDetailsUuid == null ? "" : c1.ContactDetailsUuid != null ? SelectName(c1.ContactDetailsUuid) : "",
                            };
                query = query.OrderByDescending(x => x.Id);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 商机联系记录详情
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BusViewlxrLog(CallLogRequstPaload payload)
        {
            using (_dbContext)
            {
                var query = from c1 in _dbContext.ContactCallLog
                            where c1.IsDelete == 0 && c1.ContactPersonUuid.ToString() == payload.guid
                            select new
                            {
                                c1.Id,
                                c1.CallContent,
                                c1.CallTime,
                                c1.CallLogUuid,
                                contactName = c1.ContactPersonUu.ContactName,
                                clientName = c1.ClientUu.ClientName,
                                c1.ClientUuid,
                                c1.SheBeiId,
                                c1.BusinessUuid,
                                c1.ContactDetailsName,
                                c1.ContactDetailsUuid,
                                businessName = c1.BusinessUu.BusinessName,
                                cdName = c1.ContactDetailsUuid == null ? "" : c1.ContactDetailsUuid != null ? SelectName(c1.ContactDetailsUuid) : "",
                            };
                query = query.OrderByDescending(x => x.Id);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }
        /// <summary>
        /// 当前联系记录的联系方式
        /// </summary>
        /// <param name="contactDetailsUuid"></param>
        /// <returns></returns>
        private static string SelectName(Guid? contactDetailsUuid)
        {
            string cdName = "";
            using (HaikanCRMContext c1 = new HaikanCRMContext())
            {
                var query = c1.ContactDetails.FirstOrDefault(x => x.ContactDetailsUuid == contactDetailsUuid);
                if (query != null && query.Phone != null)
                {
                    cdName = "电话";
                }
                if (query != null && query.WeChat != null)
                {
                    cdName = "微信";
                }
                if (query != null && query.TencentQq != null)
                {
                    cdName = "QQ";
                }
                return cdName;
            }
        }

        /// <summary>
        /// 商机联系记录基础信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult BusView3(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from b in _dbContext.ContactCallLog
                            where b.IsDelete == 0 && b.BusinessUuid == guid
                            select new
                            {
                                b.Id,
                                b.BusinessUuid,
                                b.ClientUuid,
                                b.ContactPersonUuid,
                                b.CallLogUuid,
                                clientName = b.ClientUu.ClientName,
                                businessName = b.BusinessUu.BusinessName,
                                b.ContactPersonUu.ContactName,


                            };
                var query2 = query.ToList();
                response.SetData(query2);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑客户(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(MyClientViewModel model)//MyClientViewModel
        {
            Guid? uuid = model.ClientUuid;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == uuid);
                if (entity != null && entity.ClientName != model.ClientName && _dbContext.Customer.Any(x => x.ClientName == model.ClientName && x.IsDelete == 0))
                {
                    response.SetFailed("客户已存在");
                    return Ok(response);
                }

                if (entity != null)
                {
                    entity.ClientName = model.ClientName;
                    entity.ClientBirthday = model.birthTime;
                    entity.ClientStatus = model.ClientStatus;
                    entity.ClientPhone = model.ClientPhone;
                    entity.ClientEmail = model.ClientEmail;
                    entity.ClientFax = model.ClientFax;
                    entity.ClientPostcode = model.ClientPostcode;
                    entity.ClientType = model.ClientType;
                    entity.ClientIndustry = model.ClientIndustry;
                    entity.ClientArea = model.ClientArea;
                    entity.ClientWebsite = model.ClientWebsite;
                    entity.ClientAddress = model.ClientAddress;
                    entity.Remarks = model.Remarks;
                    string cc = model.SystemUserUuid;
                    if (model.SuperiorUuid == Guid.Empty || model.SuperiorUuid == null)
                    {
                        entity.SuperiorUuid = Guid.Empty;
                    }
                    else
                    {
                        Guid? newuuid = model.SuperiorUuid;
                        if (newuuid == Guid.Empty)
                        {
                            entity.SuperiorUuid = newuuid;
                            entity.SuperiorMenu = 0;
                        }
                        else
                        {
                            if (newuuid != entity.SuperiorUuid)
                            {
                                var supentity = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == newuuid);
                                if (supentity != null) entity.SuperiorMenu = supentity.SuperiorMenu + 1;
                                entity.SuperiorUuid = newuuid;
                                //supentity.SystemUserUuid = supentity.SystemUserUuid + entity.SystemUserUuid;
                            }
                        }
                    }

                    if (entity.SystemUserUuid != cc)
                    {
                        var entity2 = new SystemMessage();
                        entity2.MessageUuid = Guid.NewGuid();
                        entity2.ClientUuid = entity.ClientUuid;
                        entity2.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        entity2.IsDelete = 0;
                        _dbContext.SystemMessage.Add(entity2);
                    }

                    if (model.UserName != "")
                    {
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        entity3.OperationContent = "用户" + model.UserName + "编辑了一个客户(客户名称:" + model.ClientName + ")";
                        entity3.UserName = model.UserName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "编辑";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }

                    entity.SystemUserUuid = model.SystemUserUuid;
                }

                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取商机
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetEdit1(BussinessModel payload)
        {
            using (_dbContext)
            {
                var query = from b in _dbContext.Business
                            where b.IsDelete == 0 && b.ClientUuid == payload.ClientUUID
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
                                b.ContactBusName,
                                clientName = b.ClientUu.ClientName,
                                systemUserName = b.ContactBusName == null ? "" : (b.ContactBusName != null && b.ContactBusName != "") ? SelectNameLog(b.ContactBusName, _dbContext).Trim(',') : "",
                            };
                query = query.OrderByDescending(x => x.Id);
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }



        /// <summary>
        /// 修改商机
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult BusEdit(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                Guid guid = model.businessUuid;
                var entity = _dbContext.Business.FirstOrDefault(x => x.BusinessUuid == guid);
                if (entity != null)
                {
                    entity.BusinessStage = model.businessStage;
                    entity.SalesAmount = model.salesAmount;
                    if (model.contactBusName != null)
                    {
                        entity.ContactBusName = model.contactBusName;
                    }

                    entity.Winrate = model.winrate;
                    entity.BusinessSource = model.businessSource;
                    entity.BusinessType = model.businessType;

                    if (model.systemUserUuid != "")
                    {
                        entity.SystemUserUuid = model.systemUserUuid;
                    }
                    else
                    {
                        entity.SystemUserUuid = model.userGuid.ToString() + ",";
                    }

                    if (model.checkTime == null)
                    {
                        entity.CheckTime = null;
                    }
                    else
                    {
                        entity.CheckTime = model.checkTime;
                    }

                    entity.Remarks = model.remarks;
                    entity.BusinessName = model.businessName;
                    if (entity.ClientUuid != null)
                    {
                        var entity1 = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                        if (entity1 != null && entity1.BusinessUuid == null)
                        {
                            entity1.BusinessUuid = "";
                            if (entity1.BusinessUuid.Contains("," + entity.BusinessUuid.ToString() + ","))
                            {
                                var bid1 = entity1.BusinessUuid.Split("," + entity.BusinessUuid.ToString() + ",")[0];
                                var bid2 = entity1.BusinessUuid.Split("," + entity.BusinessUuid.ToString() + ",")[1];
                                entity1.BusinessUuid = bid1 + "," + bid2;
                            }
                            else if (entity1.BusinessUuid.Contains("," + entity.BusinessUuid.ToString()))
                            {
                                entity1.BusinessUuid =
                                    entity1.BusinessUuid.Split("," + entity.BusinessUuid.ToString())[0];
                            }
                            else if (entity1.BusinessUuid.Contains(entity.BusinessUuid.ToString() + ","))
                            {
                                entity1.BusinessUuid =
                                    entity1.BusinessUuid.Split(entity.BusinessUuid.ToString() + ",")[1];
                            }
                            else if (entity1.BusinessUuid.Contains(entity.BusinessUuid.ToString()))
                            {
                                entity1.BusinessUuid = null;
                            }
                        }
                        else
                        {
                            if (entity1 != null && entity1.BusinessUuid.Contains("," + entity.BusinessUuid.ToString() + ","))
                            {
                                var bid1 = entity1.BusinessUuid.Split("," + entity.BusinessUuid.ToString() + ",")[0];
                                var bid2 = entity1.BusinessUuid.Split("," + entity.BusinessUuid.ToString() + ",")[1];
                                entity1.BusinessUuid = bid1 + "," + bid2;
                            }
                            else if (entity1 != null && entity1.BusinessUuid.Contains("," + entity.BusinessUuid.ToString()))
                            {
                                entity1.BusinessUuid =
                                    entity1.BusinessUuid.Split("," + entity.BusinessUuid.ToString())[0];
                            }
                            else if (entity1 != null && entity1.BusinessUuid.Contains(entity.BusinessUuid.ToString() + ","))
                            {
                                entity1.BusinessUuid =
                                    entity1.BusinessUuid.Split(entity.BusinessUuid.ToString() + ",")[1];
                            }
                            else if (entity1 != null && entity1.BusinessUuid.Contains(entity.BusinessUuid.ToString()))
                            {
                                entity1.BusinessUuid = null;
                            }
                        }
                    }

                    entity.ClientUuid = model.clientUuid;
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

                    {
                        var quee = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        if (quee != null)
                            entity3.OperationContent =
                                "用户" + model.userName + "编辑了客户" + quee.ClientName + "的一个商机(商机名称:" +
                                model.businessName + ")";
                        entity3.UserName = model.userName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "编辑";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }
                }

                _dbContext.SaveChanges();
            }
            return Ok(response);
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
                var entity = new Business();
                entity.BusinessUuid = Guid.NewGuid();
                entity.BusinessStage = model.businessStage;
                entity.SalesAmount = model.salesAmount;
                entity.Winrate = model.winrate;
                entity.BusinessSource = model.businessSource;
                entity.BusinessType = model.businessType;
                //string sys = model.systemUserUuid;
                if (model.checkTime != "")
                {
                    entity.CheckTime = model.checkTime;
                }

                if (model.contactBusName != null && model.contactBusName.Length > 0)
                {
                    var dd = "";
                    for (int i = 0; i < model.contactBusName.Length; i++)
                    {
                        dd += model.contactBusName[i] + ",";
                    }
                    entity.ContactBusName = dd;
                }

                entity.Remarks = model.remarks;
                entity.IsDelete = 0;
                entity.BusinessName = model.businessName;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.ClientUuid = Guid.Parse(model.clientUuid);
                var entity1 = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                if (entity1 != null)
                {
                    entity.CompanyUuid = entity1.CompanyUuid;
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

                    if (model.userName != "")
                    {
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        entity3.OperationContent = "用户" + model.userName + "给客户" + entity1.ClientName +
                                                   "添加了一个商机(商机名称:" + model.businessName + ")";
                        entity3.UserName = model.userName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "添加";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }
                }

                _dbContext.Business.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");

                return Ok(response);
            }
        }

        /// <summary>
        /// 删除商机
        /// </summary>
        /// <param name="ids">uuID字符串,多个以逗号隔开</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(string ids, string userName)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                foreach (var item in parameters)
                {
                    var query1 = _dbContext.Business.FirstOrDefault(x => x.BusinessUuid == Guid.Parse(item.Value.ToString()));
                    if (userName != "")
                    {
                        var que = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == query1.ClientUuid);
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        if (que != null)
                            if (query1 != null)
                                entity3.OperationContent = "用户" + userName + "删除了客户" + que.ClientName + "的一个商机(商机名称:" +
                                                           query1.BusinessName + ")";
                        entity3.UserName = userName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "删除";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }

                    if (query1 != null) query1.IsDelete = 1;
                    _dbContext.SaveChanges();
                }
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 删除多条批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">商机ID,多个以逗号分隔</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids, string userName)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(ids, userName);
                    break;
                case "recover":
                    response = UpdateIsDelete(ids, userName);
                    break;
            }
            return Ok(response);
        }

        /// <summary>
        /// 保存编辑(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditBusiness(BuiiViewModel model)
        {
            var uuid = model.BusinessUuid;
            string datatime2;
            if (model.CheckTime == null)
            {
                datatime2 = null;
            }
            else
            {
                var datatime = model.CheckTime.Split("T")[0];
                var datatime1 = DateTime.Parse(datatime);
                datatime1 = datatime1.AddDays(1);
                datatime2 = datatime1.ToString("yyyy-MM-dd");
            }
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Business.FirstOrDefault(x => x.BusinessUuid == uuid);
                if (entity != null)
                {
                    entity.BusinessStage = model.BusinessStage;
                    entity.SalesAmount = model.SalesAmount;
                    entity.Winrate = model.Winrate;
                    entity.BusinessSource = model.BusinessSource;
                    entity.BusinessType = model.BusinessType;
                    entity.CheckTime = datatime2;
                    entity.Remarks = model.Remarks;
                }

                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取联系人数据
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult GetTravel(MyClientRequestPayload payload)
        {
            using (_dbContext)
            {
                //var entity = _dbContext.ContactPerson.Where(x => x.ClientUuid.ToString() == guid);
                var entity = from x in _dbContext.ContactPerson
                             where x.IsDelete == 0 && x.ClientUuid.ToString() == payload.ClientUuid
                             select new
                             {
                                 x.Id,
                                 x.ClientUuid,
                                 clientName = x.ClientUu.ClientName,
                                 x.ContactName,
                                 x.Title,
                                 x.Call,
                                 x.Phone,
                                 x.Email,
                             };

                entity = entity.OrderByDescending(x => x.Id);
                var list = entity.Paged(payload.CurrentPage, payload.PageSize).ToList();
                /*                var list = entity.ToList();*/

                var totalCount = list.Count();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加联系人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult EditContactPerson(MyClientViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = new ContactPerson();
                entity.ContactPersonUuid = Guid.NewGuid();
                entity.SystemUserUuid = model.UserGuid;
                entity.ContactName = model.ContactName;
                entity.ClientUuid = model.ClientUuid;
                //entity.Title = model.Title;
                entity.Call = model.Call;
                entity.Phone = model.Phone;
                entity.Email = model.Email;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.IsDelete = 0;
                if (model.UserName != "")
                {
                    var query = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == model.ClientUuid);
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    if (query != null)
                        entity3.OperationContent = "用户" + model.UserName + "给客户" + query.ClientName +
                                                   "添加了一个联系人(联系人姓名:" + model.ContactName + ")";
                    entity3.UserName = model.UserName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "添加";
                    entity3.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity3);
                }
                _dbContext.ContactPerson.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids">ID,多个以逗号分隔</param>
        /// <param name="usName"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids, string usName)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.Customer.Single(x => x.ClientUuid.ToString() == ids);
                if (usName != "")
                {
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    entity3.OperationContent = "用户" + usName + "删除了一个客户(客户名称:" + entity.ClientName + ")";
                    entity3.UserName = usName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "删除";
                    entity3.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity3);
                }
                entity.IsDelete = 1;
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
            //response = UpdateIsDelete(ids);

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
                Label = x.StatusName,
                Value = x.CustomerStatusUuid.ToString().ToLower(),
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
                Label = x.TypeName,
                Value = x.TypeUuid.ToString().ToLower(),
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
        public IActionResult AllIndustry()
        {
            var list = _dbContext.CustomerIndustry.Select(x => new
            {
                Label = x.IndustryName,
                Value = x.IndustryUuid.ToString().ToLower(),
            }).ToList();
            var response = ResponseModelFactory.CreateResultInstance;
            response.SetData(list);
            return Ok(response);
        }


        /// <summary>
        /// 行业列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ClientIndustryList(MyClientRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from x in _dbContext.CustomerIndustry
                            select new
                            {
                                x.Id,
                                x.IndustryUuid,
                                x.IndustryName,
                            };

                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.IndustryName.Contains(payload.Kw.Trim()));
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
        /// 行业添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult IdunesyAdd(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = new CustomerIndustry();
                entity.IndustryUuid = Guid.NewGuid();
                entity.IndustryName = model.industryName;
                _dbContext.CustomerIndustry.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess("添加成功");
                return Ok(response);
            }
        }


        /// <summary>
        /// 行业详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult IdunesyView(Guid guid)
        {
            using (_dbContext)
            {
                var query = from x in _dbContext.CustomerIndustry
                            where x.IndustryUuid == guid
                            select new
                            {
                                x.Id,
                                x.IndustryUuid,
                                x.IndustryName,
                            };
                var query2 = query.ToList();
                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(query2);
                return Ok(response);
            }
        }


        /// <summary>
        /// 修改行业
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult IdunesyEdit(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                Guid guid = model.industryUuid;
                var entity = _dbContext.CustomerIndustry.FirstOrDefault(x => x.IndustryUuid == guid);
                if (entity != null) entity.IndustryName = model.industryName;
                _dbContext.SaveChanges();
            }
            return Ok(response);
        }

        /// <summary>
        /// 行业删除多条批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">行业ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult IndustryBatch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(ids);
                    break;
            }
            return Ok(response);
        }

        /// <summary>
        /// 删除行业
        /// </summary>
        /// <param name="ids">厢房ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                foreach (var item in parameters)
                {
                    string sql = "DELETE FROM ClientIndustry WHERE IndustryUUID='" + item.Value + "'";
                    _dbContext.Database.ExecuteSqlRaw(sql);
                    //var query1 = _dbContext.CustomerIndustry.FirstOrDefault(x => x.IndustryUuid == Guid.Parse(item.Value.ToString()));
                    //_dbContext.SaveChanges();
                }
                var response = ResponseModelFactory.CreateInstance;
                return response;
            }
        }

        /// <summary>
        /// 操作日志列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LogList(MyClientRequestPayload payload)
        {
            using (_dbContext)
            {
                var query = from x in _dbContext.SystemLog
                            select new
                            {
                                x.Id,
                                x.SystemLogUuid,
                                x.UserId,
                                x.UserName,
                                x.OperationTime,
                                x.OperationContent,
                                x.Ipaddress,
                                x.AddTime,
                                x.Type,

                            };

                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.Type.Contains(payload.Kw.Trim()));
                }
                if (!string.IsNullOrEmpty(payload.Kw1))
                {
                    query = query.Where(x => x.OperationTime.Contains(payload.Kw1.Trim()));
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
            var ids = contactBusName.Split(",");
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = "'" + ids[i] + "'";
            }
            var idstr = string.Join(",", ids);

            //select stuff((select ',' + ContactName from ContactPerson where ContactPersonUUID in (
            var sql = string.Format("select stuff((select ',' + ContactName from ContactPerson where ContactPersonUUID in ({0}) for xml path('')),1,1,'') as Names ", idstr);
            var cNames = context.Database.FromSql<HaikanCRM.Api.RequestPayload.Client.CNames>(sql);
            systemUserName = cNames[0].Names;

            return systemUserName;
        }


    }

    /// <summary>
    /// 树工具类
    /// </summary>
    public static class MenuTreeHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        public static List<MenuTree> BuildTree(this List<MenuTree> menus)
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
                     Selected = null == x.Guid,
                     Children = build(new Guid(x.Guid ?? string.Empty)),
                 })
                 .ToList();
            };
            var result = build(null);
            return result;
        }
    }


}
