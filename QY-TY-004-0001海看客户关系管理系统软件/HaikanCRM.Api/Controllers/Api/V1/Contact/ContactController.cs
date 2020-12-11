using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.RequestPayload.Contact;
using HaikanCRM.Api.ViewModels.Contact;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;

namespace HaikanCRM.Api.Controllers.Api.V1.Contact
{
    /// <summary>
    /// 相关联系人的操作接口
    /// </summary>
    [Route("api/v1/Contact/[controller]/[action]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="hostingEnvironment"></param>
        public ContactController(HaikanCRMContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 联系人显示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(ContactRequstPaload payload)
        {
            using (_dbContext)
            {
                using (_dbContext)
                {
                    //var query = from cp in _dbContext.ContactPerson
                    //            where cp.IsDelete == 0 
                    //            select new
                    //            {
                    //                cp.Id,
                    //                cp.ContactName,
                    //                cp.AddTime,
                    //                cp.Call,
                    //                cp.ContactPersonUuid,
                    //                cp.Title,
                    //                cp.Phone,
                    //                cp.Cellphone,
                    //                sex = cp.Sex == "1" ? "男" : cp.Sex == "2" ? "女" : cp.Sex == "3" ? "未知" : "",
                    //                cp.WeChat,
                    //                cp.BgPhone,
                    //                cp.ZjPhone,
                    //                cp.DateBirth,
                    //                cp.TencentQq,
                    //                cp.OfficeAddress,
                    //                cp.FamilyAddress,
                    //                realName = cp.SystemUserUu.RealName,
                    //                cp.ClientUu.ClientUuid,
                    //                cp.ClientUu.ClientName,
                    //                cp.ClientUu.ClientPhone,
                    //                Manager=cp.ClientUu.ClientManager,
                    //                ClientManager = CooM(cp.ClientUuid),
                    //                cp.Remarks,

                    //            };
                    
                    var query = _dbContext.ContactPersonView.Select(x => new
                    {
                        x.Id,
                        x.ContactName,
                        x.AddTime,
                        x.Call,
                        x.IsDelete,
                        x.ContactPersonUuid,
                        x.Title,
                        x.Phone,
                        x.Cellphone,
                        sex = x.Sex == "1" ? "男" : x.Sex == "2" ? "女" : x.Sex == "3" ? "未知" : "",
                        x.WeChat,
                        x.BgPhone,
                        x.ZjPhone,
                        x.DateBirth,
                        x.TencentQq,
                        x.OfficeAddress,
                        x.FamilyAddress,
                        x.RealName,
                        x.ClientUuid,
                        x.ClientName,
                        x.ClientPhone,
                        x.Manager,
                        x.ClientManager,
                        x.Remarks,
                    }).Where(x => x.IsDelete == 0);

                    // TODO:不可以硬编码,下个版本解决
                    // 如果是我们指定的角色,可以查看所有的联系人,否则只能看自己的联系人
                    if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                    {
                        query = query.Where(x => x.Manager == AuthContextService.CurrentUser.Guid);
                    }

                    if (!string.IsNullOrEmpty(payload.Kw))
                    {
                        query = query.Where(x => x.ContactName.Contains(payload.Kw));
                    }

                    if (!string.IsNullOrEmpty(payload.Kw1))
                    {
                        query = query.Where(x => x.ClientName.Contains(payload.Kw1));
                    }

                    query = query.OrderByDescending(x => x.Id);
                    var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                    var totalCount = query.Count();
                    var response = ResponseModelFactory.CreateResultInstance;
                    response.SetData(list, totalCount);
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 联系记录显示
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List1(CallLogRequstPaload payload)
        {
            using (_dbContext)
            {
                using (_dbContext)
                {
                    var query = from c1 in _dbContext.ContactCallLog
                                join c2 in _dbContext.ContactPerson
                                on c1.ContactPersonUuid equals c2.ContactPersonUuid
                                where c1.IsDelete == 0
                                //& c2.SystemUserUuid == Guid.Parse(payload.Kw)
                                
                                select new
                                {
                                    c1.Id,
                                    c1.CallContent,
                                    c1.CallTime,
                                    c1.CallLogUuid,
                                    c2.ContactName,
                                    Manager=c2.ClientUu.ClientManager,
                                    c1.ClientUu.ClientName,
                                    ClientManager = CooM(c1.ClientUuid),
                                    c1.ClientUuid,
                                    c1.SheBeiId,
                                    c1.BusinessUuid,
                                    c1.ContactDetailsUuid,
                                    businessName = c1.BusinessUu.BusinessName,
                                    cdName = c1.ContactDetailsName,
                                    //cdText = c1.ContactDetailsUuid == null ? "" : c1.ContactDetailsUuid != null ? SleectText(c1.ContactDetailsUuid) : "",
                                };
                    if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                    {
                        query = query.Where(x => x.Manager == Guid.Parse(payload.Kw));
                    }
                    if (!string.IsNullOrEmpty(payload.Kw1))
                    {
                        query = query.Where(x => x.ContactName.Contains(payload.Kw1));
                    }
                    if (!string.IsNullOrEmpty(payload.Kw2))
                    {
                        query = query.Where(x => x.ClientName.Contains(payload.Kw2));
                    }
                    query = query.OrderByDescending(x => x.Id);
                    var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                    var totalCount = query.Count();
                    var response = ResponseModelFactory.CreateResultInstance;
                    response.SetData(list, totalCount);
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientUuid"></param>
        /// <returns></returns>
        private static string CooM(Guid? clientUuid)
        {
            string clientManager="";
            using (HaikanCRMContext c1 = new HaikanCRMContext())
            {
                var query = c1.Customer.FirstOrDefault(x => x.ClientUuid == clientUuid);
                var query2 = c1.SystemUser.FirstOrDefault(x => x.SystemUserUuid == query.ClientManager);
                if (query2 != null) clientManager = query2.RealName;
                return clientManager;
            }
        }

        /// <summary>
        /// 添加联系人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(ContactViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new ContactPerson();
                entity.SystemUserUuid = model.UserGuid;
                entity.ContactPersonUuid = Guid.NewGuid();

                entity.ContactName = model.ContactName;
                entity.Title = model.Title;
                entity.Call = model.Call;
                entity.Phone = model.Phone;
                entity.Cellphone = model.Cellphone;
                entity.Email = model.Email;
                entity.Sex = model.Sex;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.Remarks = model.Remarks;
                entity.WeChat = model.WeChat;
                //entity.DateBirth = DateTime.Parse(model.DateBirth).ToString("yyyy-MM-dd");
                entity.DateBirth = DateTime.TryParse(model.DateBirth, out var dateBirth) ? dateBirth.ToString("yyyy-MM-dd") : null;
                entity.TencentQq = model.TencentQq;
                entity.BgPhone = model.BgPhone;
                entity.ZjPhone = model.ZjPhone;
                entity.OfficeAddress = model.OfficeAddress;
                entity.FamilyAddress = model.FamilyAddress;
                entity.IsDelete = 0;

                if (model.ClientUuid != "")
                {
                    entity.ClientUuid = Guid.Parse(model.ClientUuid);
                }
                if (model.UserName != "")
                {
                    var quee = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == Guid.Parse(model.ClientUuid));
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    if (quee != null)
                        entity3.OperationContent = "用户" + model.UserName + "给客户" + quee.ClientName + "添加了一个联系人(联系人姓名:" +
                                                   model.ContactName + ")";
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
        /// 添加联系记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create2(CallLogViewModel model)
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
                    entity.ContactDetailsName = model.ContactDetailsUuid;
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
                if (model.userName != "")
                {
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    entity3.OperationContent = "用户" + model.userName + "给客户" + model.ClientName + "添加了一条联系记录";
                    entity3.UserName = model.userName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "添加";
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create1(CallLogViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new ContactCallLog();
                entity.CallLogUuid = Guid.NewGuid();
                entity.ContactPersonUuid = Guid.Parse(model.ContactPersonUuid);
                if (model.ClientName != "")
                {
                    var dd = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientName);
                    if (dd != null) entity.ClientUuid = dd.ClientUuid;
                }
                if (model.BusinessUuid != null)
                {
                    entity.BusinessUuid = Guid.Parse(model.BusinessUuid);
                }
                entity.CallContent = model.CallContent;
                entity.ContactDetailsName = model.ContactDetailsName;
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
                    var quee = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientName);
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    if (quee != null)
                        entity3.OperationContent = "用户" + model.usName + "给客户" + quee.ClientName + "添加了一条联系记录";
                    entity3.UserName = model.usName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "添加";
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
        /// 获取当前客户的所有商机
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Shangji(string shangji)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (shangji != null)
            {
                using (_dbContext)
                {
                    var cp = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid.ToString() == shangji);
                    var query = from b in _dbContext.Business
                                where b.IsDelete == 0 && b.ClientUuid == cp.ClientUuid
                                select new
                                {
                                    value = b.BusinessUuid.ToString().ToLower(),
                                    label = b.BusinessName,
                                };
                    var entiy = query.ToList();
                    response.SetData(entiy);
                    return Ok(response);
                }

            }
            return Ok(response);
        }

        /// <summary>
        /// 获取所有的客户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Huoqukehu()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.Customer.Where(x => x.IsDelete==0);

                // TODO:不可以硬编码,下个版本解决
                // 如果是我们指定的角色,可以查看所有的客户,否则只能看自己的客户
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
                }
                var list=query.Select(x => new {
                    value = x.ClientUuid.ToString().ToLower(),
                    label = x.ClientName,
                }).ToList();
                response.SetData(list);
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取联系人编辑数据
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetEdit(string guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid.ToString() == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 点击客户列表的联系人获取详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult GetEdit2(string guid,string name)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ClientUuid == Guid.Parse(guid)&&x.ContactName==name);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 点击客户列表的联系人获取详情
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult LoadEditDataXq(string guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid == Guid.Parse(guid));
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑联系人(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(ContactViewModel model)
        {
            string uuid = model.ContactPersonUuid;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid == Guid.Parse(uuid));
                if (model.ClientUuid == null)
                {
                    response.SetFailed("客户不能为空");
                    return Ok(response);
                }

                if (entity != null)
                {
                    entity.ContactName = model.ContactName;
                    entity.ClientUuid = Guid.Parse(model.ClientUuid);
                    entity.Title = model.Title;
                    entity.Call = model.Call;
                    entity.Phone = model.Phone;
                    entity.Cellphone = model.Cellphone;
                    entity.DateBirth = DateTime.TryParse(model.DateBirth, out var dateBirth) ? dateBirth.ToString("yyyy-MM-dd") : null;

                    entity.Email = model.Email;
                    entity.Sex = model.Sex;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity.Remarks = model.Remarks;
                    entity.WeChat = model.WeChat;
                    entity.TencentQq = model.TencentQq;
                    entity.BgPhone = model.BgPhone;
                    entity.ZjPhone = model.ZjPhone;
                    entity.OfficeAddress = model.OfficeAddress;
                    entity.FamilyAddress = model.FamilyAddress;
                    if (model.usName != "")
                    {
                        var quee = _dbContext.Customer.FirstOrDefault(x =>
                            x.ClientUuid == Guid.Parse(model.ClientUuid));
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        if (quee != null)
                            entity3.OperationContent =
                                "用户" + model.usName + "给客户" + quee.ClientName + "的联系人做了编辑(联系人姓名:" +
                                model.ContactName + ")";
                        entity3.UserName = model.usName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "编辑";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }

                    //var ds = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientName);
                    entity.ClientUuid = Guid.Parse(model.ClientUuid);
                }

                //_dbContext.ContactPerson.Add(entity);
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 获取联系记录编辑数据
        /// </summary>
        /// <param name="guid">申请惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult GetEdit1(string guid)
        {
            using (_dbContext)
            {
                var query = from c1 in _dbContext.ContactCallLog
                            join c2 in _dbContext.ContactPerson
                            on c1.ContactPersonUuid equals c2.ContactPersonUuid
                            where c1.IsDelete == 0
                            && c1.CallLogUuid.ToString() == guid
                            select new
                            {
                                c1.Id,
                                c1.CallContent,
                                c1.CallTime,
                                c1.CallLogUuid,
                                c2.ContactName,
                                c1.ClientUu.ClientName,
                                c1.ContactPersonUuid,
                                c1.ClientUuid,
                                c1.SheBeiId,
                                c1.BusinessUuid,
                                c1.ContactDetailsUuid,
                                businessName = c1.BusinessUu.BusinessName,
                                cdName = c1.ContactDetailsName,
   
                            };
                var entity = query.ToList();
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑联系记录(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit1(CallLogViewModel model)
        {
            string uuid = model.CallLogUuid;
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.ContactCallLog.FirstOrDefault(x => x.CallLogUuid == Guid.Parse(uuid));
                if (entity != null)
                {
                    entity.CallContent = model.CallContent;
                    if (model.CallTime != "")
                    {
                        var d2 = DateTime.Parse(model.CallTime).ToString("yyyy-MM-dd HH:mm");
                        entity.CallTime = d2;
                    }

                    entity.ContactDetailsName = model.cdName;
                    if (model.usName != "")
                    {
                        var quee = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                        var entity3 = new SystemLog();
                        entity3.SystemLogUuid = Guid.NewGuid();
                        if (quee != null)
                            entity3.OperationContent = "用户" + model.usName + "编辑了客户" + quee.ClientName + "的联系记录";
                        entity3.UserName = model.usName;
                        entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                        entity3.Type = "编辑";
                        entity3.IsDelete = 0;
                        _dbContext.SystemLog.Add(entity3);
                    }
                }

                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ShopInfoImport(IFormFile excelfile)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                DateTime beginTime = DateTime.Now;

                string sWebRootFolder = _webHostEnvironment.WebRootPath + "\\UploadFiles\\ImportUserInfoExcel";
                string uploadtitle = "联系人基础信息导入" + DateTime.Now.ToString("yyyyMMddHHmmss");
                string sFileName = $"{uploadtitle}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                string responsemsgsuccess = "";
                string responsemsgrepeat = "";
                string responsemsgdefault = "";
                int successcount = 0;
                int repeatcount = 0;
                int defaultcount = 0;
                try
                {
                    //把excelfile中的数据复制到file中
                    using (FileStream fs = new FileStream(file.ToString(), FileMode.Create)) //初始化一个指定路径和创建模式的FileStream
                    {
                        excelfile.CopyTo(fs);
                        fs.Flush();  //清空stream的缓存，并且把缓存中的数据输出到file
                    }
                    System.Data.DataTable dt = Haikan3.Utils.ExcelTools.ExcelToDataTable(file.ToString(), "Sheet1", true);

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        response.SetFailed("表格无数据");
                        return Ok(response);
                    }
                    else
                    {
                        if (!dt.Columns.Contains("联系人名称"))
                        {
                            response.SetFailed("无‘联系人名称’列");
                            return Ok(response);
                        }
                        if (!dt.Columns.Contains("所属客户"))
                        {
                            response.SetFailed("无‘所属客户’列");
                            return Ok(response);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            var entity = new ContactPerson();
                            entity.ContactPersonUuid = Guid.NewGuid();
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            if (!string.IsNullOrEmpty(dt.Rows[i]["联系人名称"].ToString()))
                            {
                                //var a = dt.Rows[i]["联系人名称"].ToString();
                                var user = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactName == dt.Rows[i]["联系人名称"].ToString());
                                if (user == null)
                                {
                                    entity.ContactName = dt.Rows[i]["联系人名称"].ToString();
                                }
                                else
                                {
                                    responsemsgrepeat += "<p style='color:orange'>" + "第" + (i + 2) + "行联系人名称已存在" + "</p></br>";
                                    repeatcount++;
                                    continue;
                                }

                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行联系人名称为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["所属客户"].ToString()))
                            {
                                //var a = dt.Rows[i]["所属客户"].ToString();
                                var userkehu = _dbContext.Customer.FirstOrDefault(x => x.ClientName == dt.Rows[i]["所属客户"].ToString());
                                if (userkehu != null)
                                {
                                    entity.ClientUuid = userkehu.ClientUuid;
                                }
                                else
                                {
                                    response.SetFailed("第" + (i + 2) + "行" + "行客户不存在");
                                    return Ok(response);
                                }
                            }
                            else
                            {
                                responsemsgdefault += "<p style='color:red'>" + "第" + (i + 2) + "行所属客户为空" + "</p></br>";
                                defaultcount++;
                                continue;
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["职务"].ToString()))
                            {
                                entity.Call = dt.Rows[i]["职务"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["电话"].ToString()))
                            {
                                entity.Phone = dt.Rows[i]["电话"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["邮箱"].ToString()))
                            {
                                entity.Email = dt.Rows[i]["邮箱"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["QQ"].ToString()))
                            {
                                entity.TencentQq = dt.Rows[i]["QQ"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["微信"].ToString()))
                            {
                                entity.WeChat = dt.Rows[i]["微信"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["办公地址"].ToString()))
                            {
                                entity.OfficeAddress = dt.Rows[i]["办公地址"].ToString();
                            }
                            if (!string.IsNullOrEmpty(dt.Rows[i]["家庭地址"].ToString()))
                            {
                                entity.FamilyAddress = dt.Rows[i]["家庭地址"].ToString();
                            }

                            entity.IsDelete = 0;
                            entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                            _dbContext.ContactPerson.Add(entity);
                            _dbContext.SaveChanges();
                            successcount++;
                        }
                    }
                    responsemsgsuccess = "<p style='color:green'>导入成功:" + successcount + "条</p></br>" + responsemsgsuccess;
                    responsemsgrepeat = "<p style='color:orange'>重复需手动修改数据:" + repeatcount + "条</p></br>" + responsemsgrepeat;
                    responsemsgdefault = "<p style='color:red'>导入失败:" + defaultcount + "条</p></br>" + responsemsgdefault;


                    DateTime endTime = DateTime.Now;
                    TimeSpan useTime = endTime - beginTime;
                    string taketime = "导入时间" + useTime.TotalSeconds.ToString(CultureInfo.InvariantCulture) + "秒  ";
                    response.SetData(JsonConvert.DeserializeObject(JsonConvert.SerializeObject(new
                    {
                        time = taketime,
                        successmsg = responsemsgsuccess
                        ,
                        repeatmsg = responsemsgrepeat,
                        defaultmsg = responsemsgdefault
                    })));
                    return Ok(response);
                }
                catch (Exception ex)
                {

                    response.SetFailed(ex.Message);
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 删除客户的联系人
        /// </summary>
        /// <param name="ids">uuID,多个以逗号分隔</param>
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
                var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid.ToString() == ids);
                if (usName != "")
                {
                    var que = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    if (que != null)
                    {
                        if (entity != null)
                        {
                            entity3.OperationContent = "用户" + usName + "删除了客户" + que.ClientName + "的联系人(联系人姓名:" + entity.ContactName + ")";
                            entity3.UserName = usName;
                            entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                            entity3.Type = "删除";
                            entity3.IsDelete = 0;
                            _dbContext.SystemLog.Add(entity3);
                        }
                    }
                       
                            
                }

                if (entity != null) entity.IsDelete = 1;
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除联系记录
        /// </summary>
        /// <param name="ids">ID,多个以逗号分隔</param>
        /// <param name="usName"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Delete1(string ids, string usName)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var entity = _dbContext.ContactCallLog.Single(x => x.CallLogUuid.ToString() == ids);
                if (usName != "")
                {
                    var que = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == entity.ClientUuid);
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    if (que != null)
                        entity3.OperationContent =
                            "用户" + usName + "删除了客户" + que.ClientName + "的一条联系记录" + entity.ContactName;
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
        }

        /// <summary>
        /// 查询出所有客户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSelectCustomer(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.Customer
                            where s.IsDelete == 0
                            //&& s.SystemUserUuid.Contains(guid)
                            select new
                            {
                                s.ClientUuid,
                                s.ClientName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }


        /// <summary>
        /// 查询出所有客户的下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCustomer()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.Customer
                            where s.IsDelete == 0
                            select new
                            {
                                value = s.ClientUuid,
                                label = s.ClientName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        ///获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GeallUser()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.SystemUser
                            where s.IsDeleted == 0
                            select new
                            {
                                value = s.SystemUserUuid,
                                name = s.RealName,
                                checkeds = false,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 钉钉端添加或编辑商机页面根据输入客户查询该相关联系人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppbusRelatedConName(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                string uuid = model.guid;
                var entity = _dbContext.Customer.FirstOrDefault(x => x.ClientName == uuid);
                var query = from s in _dbContext.ContactPerson
                            where s.IsDelete == 0 && s.ClientUuid == entity.ClientUuid
                            select new
                            {
                                value = s.ContactPersonUuid,
                                name = s.ContactName,
                                checkeds = false,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 钉钉端商机页面根据当前商机详情客户查询相关联系人(页面加载查询赋值)
        /// </summary>
        /// <param name="model2"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AppbusRelatedConGuid(dynamic model2)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                Guid uuid = model2.guid;
                var entity = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == uuid);
                var query = from s in _dbContext.ContactPerson
                            where s.IsDelete == 0 && s.ClientUuid == entity.ClientUuid
                            select new
                            {
                                value = s.ContactPersonUuid,
                                name = s.ContactName,
                                checkeds = false,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询全部客户赋值下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCustomerDropdown()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.Customer
                            where s.IsDelete == 0
                            select new
                            {
                                superiorUuid = s.ClientUuid,
                                superiorName = s.ClientName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询全部行业赋值下拉框
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Gethangye()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.CustomerIndustry
                            select new
                            {
                                s.IndustryUuid,
                                s.IndustryName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询出当前商机所属客户的全部联系人
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Jiekoulxr(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.ContactPerson
                            where s.IsDelete == 0 && s.ClientUuid == guid
                            //&& s.SystemUserUuid.Contains(guid)
                            select new
                            {
                                value = s.ContactPersonUuid,
                                label = s.ContactName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询出当前客户所属得全部商机
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Jiekoushangji(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.Business
                            where s.IsDelete == 0 && s.ClientUuid == guid
                            //&& s.SystemUserUuid.Contains(guid)
                            select new
                            {
                                value = s.BusinessUuid,
                                label = s.BusinessName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询出当前客户所属得全部商机
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LoadSimpleList(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.ContactPerson
                            where s.IsDelete == 0 && s.ClientUuid == guid
                            //&& s.SystemUserUuid.Contains(guid)
                            select new
                            {
                                s.ContactPersonUuid,
                                s.ContactName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑经理权限(保存)
        /// </summary>
        /// <param name="model">申请视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Save_manger(dynamic model)
        {
            string uuid = model.rowUuid;
            string clientUuid = model.clientUuid;
            //string username = model.fields.userName;
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                var entity = _dbContext.Customer.FirstOrDefault(x => x.ClientUuid == Guid.Parse(clientUuid));

                if (entity != null) entity.ClientManager = Guid.Parse(uuid);


                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询出所有客户经理
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Geallmanager()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from s in _dbContext.SystemUser
                            where s.IsDeleted == 0
                            //&& s.SystemUserUuid.Contains(guid)
                            select new
                            {
                                value = s.SystemUserUuid,
                                label = s.RealName,
                            };

                response.SetData(query.ToList());
                return Ok(response);
            }
        }
    }
}
