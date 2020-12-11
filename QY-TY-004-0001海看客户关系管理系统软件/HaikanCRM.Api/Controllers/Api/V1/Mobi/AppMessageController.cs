using System;
using System.Linq;
using AutoMapper;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HaikanCRM.Api.Controllers.Api.V1.Mobi
{
    /// <summary>
    /// 相关消息的操作接口
    /// </summary>
    [Route("api/v1/Mobi/[controller]/[action]")]
    [ApiController]
    public class AppMessageController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AppMessageController(HaikanCRMContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 查看自己的消息列表
        /// </summary>
        /// <param name="guid">systemUser的uuid</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult MessageShow(string guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = from cp in _dbContext.Customer
                            join b in _dbContext.SystemMessage
                            on cp.ClientUuid equals b.ClientUuid
                            join s in _dbContext.SystemUser
                            on cp.ClientManager equals s.SystemUserUuid
                            where cp.IsDelete != 1
                            && b.IsDelete != 1
                            && s.IsDeleted != 1
                            select new
                            {
                                b.Id,
                                cp.ClientManager,
                                cp.ClientName,
                                cp.ClientUuid,
                                cp.SystemUserUuid,
                                s.RealName,
                                b.MessageUuid,
                                b.AddTime,
                            };
                if (guid != null)
                {
                    query = query.Where(x => x.SystemUserUuid.Contains(guid));
                }
                query = query.OrderByDescending(x => x.Id);
                var demo = query.ToList();
                response.SetData(demo);
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除自己的消息列表
        /// </summary>
        /// <param name="guid">systemUser的uuid</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult MesDelete(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query1 = _dbContext.SystemMessage.FirstOrDefault(x => x.MessageUuid == guid);
                
                if (query1 == null) return Ok(response);
                
                query1.IsDelete = 1;
                
                _dbContext.SaveChanges();
                response.SetSuccess("删除成功");
                
                return Ok(response);
            }
        }

        /// <summary>
        /// 钉钉端显示通讯录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserList(string name)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                //var query = from cp in _dbContext.SystemUser
                //            where cp.IsDeleted != 1
                //            select new
                //            {
                //                cp.SystemUserUuid,
                //                cp.RealName,
                //                txlu = Xxdd(cp.SystemUserUuid),
                //            };
                var query = _dbContext.AddressBook.Select(x=>new
                {
                    x.SystemUserUuid,
                    x.RealName,
                    x.IsDeleted,
                    txlu=x.Txlu??0,
                }).Where(x => x.IsDeleted == 0);
                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(x => x.RealName.Contains(name));
                }
                query = query.OrderByDescending(x => x.RealName);
                var demo = query.ToList();
                response.SetData(demo);
                return Ok(response);
            }
        }

        /// <summary>
        /// 计算用户当天的联系记录数量
        /// </summary>
        /// <param name="SystemUserUuid"></param>
        /// <returns></returns>
        private static int Xxdd(Guid SystemUserUuid)
        {
            int txlu = 0;
            using (HaikanCRMContext c1 = new HaikanCRMContext())
            {
                var que = c1.Customer.Where(x => x.ClientManager == SystemUserUuid);

                var d5 = DateTime.Now.ToString("yyyy-MM-dd");
                if (que.ToList().Count > 0)
                {
                    for (int i = 0; i < que.ToList().Count; i++)
                    {
                        txlu += c1.ContactCallLog.Count(x => x.ClientUuid == que.ToList()[i].ClientUuid && x.CallTime.Substring(0, 10) == d5);
                    }
                }
            }

            return txlu;
        }

    }
}
