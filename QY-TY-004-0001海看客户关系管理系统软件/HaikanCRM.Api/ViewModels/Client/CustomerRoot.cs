using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Client
{
    /// <summary>
    /// 
    /// </summary>
    public class Children
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientUuid { get; set; }
        /// <summary>
        /// 浙工大材料学院
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientManager { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BusinessUuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientPostcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string checkbox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> children { get; set; }
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
        /// 
        /// </summary>
        public string ClientUuid { get; set; }
        /// <summary>
        /// 浙江工业大学
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientManager { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BusinessUuid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientPhone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientPostcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string checkbox { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Children> children { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CustomerRoot
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
}
