using System;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using Microsoft.AspNetCore.Mvc;

namespace HaikanCRM.Api.Controllers.Api.V1.Contact
{
    /// <summary>
    /// 联系记录的操作接口
    /// </summary>
    [Route("api/v1/Contact/[controller]/[action]")]
    [ApiController]
    public class CallLogController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public CallLogController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }



        /// <summary>
        /// 根据客户的uuid获取联系人下拉框的值
        /// </summary>
        /// <param name="clientUuid">客户的uuid</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Gealluser(string clientUuid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from c1 in _dbContext.ContactPerson
                             where c1.IsDelete == 0 && c1.ClientUuid.ToString() == clientUuid
                             select new
                             {
                                 // c1.Id,
                                 contactPersonUuid = c1.ContactPersonUuid,
                                 contactName = c1.ContactName,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }




        /// <summary>
        /// 联系人下拉框
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Lianxiren(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                //var query = _dbContext.ContactPerson.Where(x => x.IsDelete == 0);

                //var query = from cp in _dbContext.ContactPerson
                //            join c in _dbContext.Customer
                //                on cp.ClientUuid equals c.ClientUuid
                //            where cp.IsDelete == 0
                //            select new
                //            {
                //                cp.ContactPersonUuid,
                //                cp.ClientUu,
                //                ContactName=cp.ContactName+ "(" + c.ClientName + ")",
                //            };
                var query = _dbContext.ContactPlist.Where(x=>x.IsDelete==0);

                if (AuthContextService.CurrentUser.RoleName != "超级管理员" && AuthContextService.CurrentUser.RoleName != "行业经理")
                {
                    query = query.Where(x => x.ClientManager == AuthContextService.CurrentUser.Guid);
                }
                //var list = query.Select(x => new
                //{
                //    x.ContactPersonUuid,
                //    ContactName = SelectName(x.ContactPersonUuid),
                //}).ToList();

                response.SetData(query.ToList());
                return Ok(response);
            }
        }

        /// <summary>
        /// 通过联系人uuid查询联系人名称
        /// </summary>
        /// <param name="contactPersonUuid"></param>
        /// <returns></returns>
        private static string SelectName(Guid contactPersonUuid)
        {
            string contactName = "";
            //var response = ResponseModelFactory.CreateResultInstance;
            using (HaikanCRMContext c1 = new HaikanCRMContext())
            {
                var participantids = c1.ContactPerson.FirstOrDefault(x => x.ContactPersonUuid == contactPersonUuid);
                var query = c1.Customer.FirstOrDefault(x => x.ClientUuid == participantids.ClientUuid);
                if (query != null)
                {
                    if (participantids != null) contactName = participantids.ContactName + "(" + query.ClientName + ")";
                }
            }
            return contactName;
        }

        /// <summary>
        /// 通过联系人获取客户
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Kehu(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = from c in _dbContext.Customer
                             join cp in _dbContext.ContactPerson
                             on c.ClientUuid equals cp.ClientUuid
                             where c.IsDelete == 0 && cp.ContactPersonUuid == guid
                             select new
                             {

                                 c.ClientUuid,
                                 c.ClientName,
                             };
                var query = entity.ToList();
                response.SetData(query);
                return Ok(response);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetSystemUser(string userid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.Streets == userid);
                response.SetData(entity);
                return Ok(response);
            }
        }

    }
}
