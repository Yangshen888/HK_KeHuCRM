using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Client
{
    public class BuiiViewModel
    {
        public string BusinessStage { get; set; }
        public string SalesAmount { get; set; }
        public string Winrate { get; set; }
        public string BusinessSource { get; set; }
        public string BusinessType { get; set; }
        public string CheckTime { get; set; }
        public string Remarks { get; set; }
        public string UserName { get; set; }
        public string ClientName { get; set; }
        public Guid? BusinessUuid { get; set; }
        
    }
}
