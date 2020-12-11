using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HaikanCRM.Api.Controllers.Api.V1.IndexStatistics
{
    [Route("api/v1/IndexStatistics/[controller]/[action]")]
    [ApiController]
    public class IndexStatisticsController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        public IndexStatisticsController(HaikanCRMContext dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        /// 首页基础数据统计
        /// </summary>
        [HttpGet]
        public IActionResult GetBaseInfo()
        {
            using (_dbContext)
            {
                var d5 = DateTime.Now.ToString("yyyy-MM");
                var response = ResponseModelFactory.CreateResultInstance;
                List<dynamic> data = new List<dynamic>();
                int CusCount = _dbContext.Customer.Where(x => x.IsDelete == 0).Count();         //客户总数
                int ConCount = _dbContext.ContactPerson.Where(x => x.IsDelete == 0).Count();     //联系人总数
                int BusCount = _dbContext.Business.Where(x => x.IsDelete == 0).Count();     //商机总数
                int BusAddCount = _dbContext.Business.Where(x => x.IsDelete == 0 && x.AddTime.Substring(0,7) == d5).Count();     //当月新增商机总数
                int CallAddCount = _dbContext.ContactCallLog.Where(x => x.IsDelete == 0 && x.CallTime.Substring(0, 7) == d5).Count();     //联系记录当月总数
                int ConBirthCount = _dbContext.ContactPerson.Where(x => x.IsDelete == 0 && x.DateBirth.Substring(0, 7) == d5).Count();     //当月客户生日总数
                data.Add(new { CusCount, ConCount, BusCount, BusAddCount, CallAddCount, ConBirthCount });
                response.SetData(data);
                return Ok(response);
            }
        }


        /// <summary>
        /// 首页统计客户地区饼图
        /// </summary>
        [HttpGet]
        public IActionResult GetCircle()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.Customer.Where(x => x.IsDelete == 0).ToList().GroupBy(x => x.ClientArea).Select(x => new { name=x.Key??"未知" , value = x.AsEnumerable().Count() });
                var query1 = _dbContext.Customer.Where(x => x.IsDelete == 0).GroupBy(x => x.ClientArea).Select(x => x.Key).ToList();
                response.SetData(new { query ,query1});
                return Ok(response);
            }
        }

        /// <summary>
        /// 首也柱形统计图
        /// </summary>
        [HttpGet]
        public IActionResult GetBarChart()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                //Dictionary<string, int> sumlist = new Dictionary<string, int>();
                List<dynamic> timelist = new List<dynamic>();
                List<dynamic> sumlist = new List<dynamic>();
                int lnow = Convert.ToInt32(DateTime.Now.DayOfWeek);
                if (lnow == 0)
                {
                    lnow = 7;
                }
                DateTime sdate1 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 7);
                DateTime edate1 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 7).AddDays(6);
                DateTime sdate2 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 14);
                DateTime edate2 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 14).AddDays(6);
                DateTime sdate3 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 21);
                DateTime edate3 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 21).AddDays(6);
                DateTime sdate4 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 28);
                DateTime edate4 = DateTime.Now.AddDays(Convert.ToInt32(1 - lnow) - 28).AddDays(6);
                string date1 = sdate1.ToString("yyyy-MM-dd") + "至" + edate1.ToString("yyyy-MM-dd");
                string date2 = sdate2.ToString("yyyy-MM-dd") + "至" + edate2.ToString("yyyy-MM-dd");
                string date3 = sdate3.ToString("yyyy-MM-dd") + "至" + edate3.ToString("yyyy-MM-dd");
                string date4 = sdate4.ToString("yyyy-MM-dd") + "至" + edate4.ToString("yyyy-MM-dd");
                int query1 = _dbContext.ContactCallLogView.Where(x => x.IsDelete == 0 && x.CallTime > sdate1 && x.CallTime < edate1).Count();
                int query2 = _dbContext.ContactCallLogView.Where(x => x.IsDelete == 0 && x.CallTime > sdate2 && x.CallTime < edate2).Count();
                int query3 = _dbContext.ContactCallLogView.Where(x => x.IsDelete == 0 && x.CallTime > sdate3 && x.CallTime < edate3).Count();
                int query4 = _dbContext.ContactCallLogView.Where(x => x.IsDelete == 0 && x.CallTime > sdate4 && x.CallTime < edate4).Count();
                DateTime dt = DateTime.Now;  //当前时间  
                int now = Convert.ToInt32(dt.DayOfWeek.ToString("d"));
                if (now == 0)
                {
                    now = 7;
                }
                DateTime startWeek = dt.AddDays(1 - now);  //本周周一  
                DateTime endWeek = startWeek.AddDays(6);  //本周周日 
                string tdate = startWeek.ToString("yyyy-MM-dd") + "至" + endWeek.ToString("yyyy-MM-dd");
                int query = _dbContext.ContactCallLogView.Where(x => x.IsDelete == 0 && x.CallTime > startWeek && x.CallTime < endWeek).Count();
                timelist.Add(tdate);
                timelist.Add(date1);
                timelist.Add(date2);
                timelist.Add(date3);
                timelist.Add(date4);
                sumlist.Add(query);
                sumlist.Add(query1);
                sumlist.Add(query2);
                sumlist.Add(query3);
                sumlist.Add(query4);
                response.SetData(new { timelist, sumlist });
            }
            return Ok(response);
        }


        /// <summary>
        /// 首页统计折线图
        /// </summary>
        [HttpGet]
        public IActionResult GetLineChart()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.AddressBook.Where(x => x.IsDeleted == 0).OrderBy(x => x.SystemUserUuid).Select(x => x.RealName).ToList();
                var query2 = _dbContext.AddressBook.Where(x => x.IsDeleted == 0).OrderBy(x => x.SystemUserUuid).Select(x => x.Txlu ?? 0 ).ToList();
                response.SetData(new { query , query2});
                return Ok(response);
            }
        }
    }
}
