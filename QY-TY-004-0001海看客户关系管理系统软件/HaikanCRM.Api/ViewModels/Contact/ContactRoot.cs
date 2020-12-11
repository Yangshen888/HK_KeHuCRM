using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Contact
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public int code { get; set; }
        /// <summary>
        /// 成功
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ListData> ListData { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ListData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 李四
        /// </summary>
        public string ContactName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContactPersonUuid { get; set; }
        /// <summary>
        /// 湖南卫视
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> phone { get; set; }
    }


}
