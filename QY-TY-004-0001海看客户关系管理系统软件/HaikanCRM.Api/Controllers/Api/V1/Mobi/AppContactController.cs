using System;
using System.Collections.Generic;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.RequestPayload.Contact;
using HaikanCRM.Api.ViewModels.Contact;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HaikanCRM.Api.Controllers.Api.V1.Mobi
{
    [Route("api/v1/Mobi/[controller]/[action]")]
    [ApiController]
    public class AppContactController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public AppContactController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// 钉钉端联系人添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AppAddContact(ContactViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = new ContactPerson();
                //var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid.ToString() == model.ContactPersonUuid);
                entity.ContactPersonUuid = Guid.NewGuid();
                entity.ContactName = model.ContactName;
                var query = _dbContext.Customer.FirstOrDefault(x => x.ClientName == model.ClientName);
                if (query != null) entity.ClientUuid = query.ClientUuid;
                entity.Title = model.Title;
                entity.Call = model.Call;
                entity.Phone = model.Phone;
                entity.Cellphone = model.Cellphone;
                entity.Email = model.Email;
                if (model.Sex != "")
                {
                    entity.Sex = (Int32.Parse(model.Sex) + 1).ToString();
                }
                entity.Remarks = model.Remarks;
                entity.WeChat = model.WeChat;
                entity.DateBirth = model.DateBirth;
                entity.TencentQq = model.TencentQq;
                entity.BgPhone = model.BgPhone;
                entity.ZjPhone = model.ZjPhone;
                entity.OfficeAddress = model.OfficeAddress;
                entity.FamilyAddress = model.FamilyAddress;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.IsDelete = 0;
                entity.SystemUserUuid = model.SystemUserUuid;
                if (model.usName != "")
                {
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    entity3.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")添加了一个联系人(联系人姓名:" + model.ContactName + ")";
                    entity3.UserName = model.usName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "钉钉端添加";
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
        /// 钉钉端联系人编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AppEditContact(ContactViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid.ToString() == model.ContactPersonUuid);
                if (entity != null)
                {
                    entity.ContactName = model.ContactName;
                    entity.ClientUuid = Guid.Parse(model.ClientUuid);
                    entity.Title = model.Title;
                    entity.Call = model.Call;
                    entity.Phone = model.Phone;
                    entity.Cellphone = model.Cellphone;
                    entity.Email = model.Email;
                    if (model.Sex != "")
                    {
                        entity.Sex = (Int32.Parse(model.Sex) + 1).ToString();
                    }

                    entity.Remarks = model.Remarks;
                    entity.WeChat = model.WeChat;
                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity.TencentQq = model.TencentQq;
                    entity.BgPhone = model.BgPhone;
                    entity.ZjPhone = model.ZjPhone;
                    entity.OfficeAddress = model.OfficeAddress;
                    entity.FamilyAddress = model.FamilyAddress;
                }

                if (model.usName != "")
                {
                    var entity3 = new SystemLog();
                    entity3.SystemLogUuid = Guid.NewGuid();
                    entity3.OperationContent = "钉钉端客户管理系统-用户(" + model.usName + ")编辑了联系人(联系人姓名:" + model.ContactName + ")信息";
                    entity3.UserName = model.usName;
                    entity3.OperationTime = DateTime.Now.ToString("yyyy-MM-dd");
                    entity3.Type = "钉钉端编辑";
                    entity3.IsDelete = 0;
                    _dbContext.SystemLog.Add(entity3);
                }
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }
        }



        /// <summary>
        /// 钉钉端联系人列表展示
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult AppContectList(ContactRequstPaload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from cp in _dbContext.ContactPerson
                            where cp.IsDelete == 0
                            select new
                            {
                                cp.Id,
                                cp.ContactName,
                                cp.ContactPersonUuid,
                                cp.Phone,
                                cp.ClientUu.ClientManager,
                                cp.ClientUu.ClientName,
                            };
                // 如果是我们指定的角色,可以查看所有的联系人,否则只能看自己负责的客户的联系人
                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
                }
                //如果联系人名字搜索框不为空,模糊匹配搜索框内容
                if (!string.IsNullOrEmpty(payload.Kk1))
                {
                    query = query.Where(x => x.ContactName.Contains(payload.Kk1));
                }
                //按照ID排序(降序)
                query = query.OrderByDescending(x => x.Id);
                var qu = query.ToList();
                string param2 = "{\"code\":0,\"message\":\"成功\",\"ListData\":[";
                //var strlist = query.ToList();                
                for (int i = 0; i < qu.Count; i++)
                {
                    var phonelist = "";
                    if (qu[i].Phone != "")
                    {
                        string[] lists = qu[i].Phone.Split(' ');
                        if (lists.Length > 0)
                        {
                            for (int i1 = 0; i1 < lists.Length; i1++)
                            {
                                if (lists[i1] != "")
                                {
                                    if (phonelist != "")
                                    {
                                        phonelist += ",\"" + lists[i1] + "\"";
                                    }
                                    else
                                    {
                                        phonelist = "\"" + lists[i1] + "\"";
                                    }
                                }
                            }
                        }
                    }
                    if (i == 0)
                    {
                        param2 += "{ \"Id\": \"" + qu[i].Id + "\",\"ContactName\":\"" + qu[i].ContactName + "\",\"ContactPersonUuid\":\"" + qu[i].ContactPersonUuid + "\",\"ClientName\":\"" + qu[i].ClientName +
                            "\",\"phone\":[" + phonelist + "]}";
                    }
                    else
                    {
                        param2 += ",{ \"Id\": \"" + qu[i].Id + "\",\"ContactName\":\"" + qu[i].ContactName + "\",\"ContactPersonUuid\":\"" + qu[i].ContactPersonUuid + "\",\"ClientName\":\"" + qu[i].ClientName +
                            "\",\"phone\":[" + phonelist + "]}";
                    }
                    //phonelist = "";
                }
                param2 += "]}";

                var result = JsonConvert.DeserializeObject<ContactRoot>(param2);
                response.SetData(result);
                return Ok(response);
            }
        }

        /// <summary>
        ///查询联系人详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult AppShowContactIId(int id)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from x in _dbContext.ContactPerson
                             where x.Id == id
                             select new
                             {
                                 x.ContactPersonUuid,
                                 x.ClientUuid,
                                 x.Id,
                                 clientName = x.ClientUu.ClientName,
                                 x.ContactName,
                                 x.DateBirth,
                                 //x.Call,
                                 //x.Phone,
                                 //x.Email,
                                 //x.WeChat,
                                 //x.TencentQq,
                                 //x.OfficeAddress,
                                 //x.FamilyAddress,
                                 //x.Remarks,
                                 Call = x.Call == null ? "" : x.Call != null ? x.Call : "",
                                 Phone = x.Phone == null ? "" : x.Phone != null ? x.Phone : "",
                                 Email = x.Email == null ? "" : x.Email != null ? x.Email : "",
                                 WeChat = x.WeChat == null ? "" : x.WeChat != null ? x.WeChat : "",
                                 TencentQq = x.TencentQq == null ? "" : x.TencentQq != null ? x.TencentQq : "",
                                 OfficeAddress = x.OfficeAddress == null ? "" : x.OfficeAddress != null ? x.OfficeAddress : "",
                                 FamilyAddress = x.FamilyAddress == null ? "" : x.FamilyAddress != null ? x.FamilyAddress : "",
                                 Remarks = x.Remarks == null ? "" : x.Remarks != null ? x.Remarks : "",
                                 sex = x.Sex == null ? "3" : x.Sex != null ? x.Sex : "",
                                 sex2 = x.Sex,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询联系人详情
        /// </summary>
        /// <param name="contactPersonUuid"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult AppShowContactGuid(string contactPersonUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from x in _dbContext.ContactPerson
                             where x.ContactPersonUuid.ToString() == contactPersonUuid
                             select new
                             {
                                 x.ContactPersonUuid,
                                 x.ClientUuid,
                                 x.Id,
                                 clientName = x.ClientUu.ClientName,
                                 x.ContactName,
                                 x.Title,
                                 x.DateBirth,
                                 x.Call,
                                 x.Phone,
                                 x.Email,
                                 sex = x.Sex == "1" ? "男" : x.Sex == "2" ? "女" : x.Sex == "3" ? "未知" : "",
                                 x.WeChat,
                                 x.TencentQq,
                                 x.BgPhone,
                                 x.ZjPhone,
                                 x.OfficeAddress,
                                 x.FamilyAddress,
                                 x.Remarks,
                                 sex2 = x.Sex,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }


    }
}
