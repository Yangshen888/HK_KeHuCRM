using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.RequestPayload.Client
{
    public class MyClientRequestPayload : RequestPayload
    {
        public string Kw { get; set; }
        public string Kw1 { get; set; }
        public string ClientUuid { get; set; }
        public List<string> time { get; set; }
        public Feedback? Feedback
        {
            get
            {
                int num = 0;
                if(int.TryParse(Kw1, out num))
                {
                    return (Feedback?)num;
                }
                else
                {
                    return null;
                }
                
            }
        }
    }



    /// <summary>
    /// 时间反馈
    /// </summary>
    public enum Feedback
    {
        /// <summary>
        /// 1天未联系的客户
        /// </summary>
        dayout,
        /// <summary>
        /// 3天未联系的客户
        /// </summary>
        thrdayout,
        /// <summary>
        /// 1周未联系的客户
        /// </summary>
        weekout,
        /// <summary>
        /// 1个月未联系的客户
        /// </summary>
        monthout,
        /// <summary>
        /// 本周内联系的客户
        /// </summary>
        weekin,
        /// <summary>
        /// 本月内联系的客户
        /// </summary>
        monthin,
        /// <summary>
        /// 无联系的客户
        /// </summary>
        eveout,
    }
}
