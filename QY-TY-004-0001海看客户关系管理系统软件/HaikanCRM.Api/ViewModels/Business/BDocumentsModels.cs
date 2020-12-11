using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.Business
{
    public class BDocumentsModels
    {
        public Guid? BusDocumentsUuid { get; set; }
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileInfo { get; set; }
        public string FileUrl { get; set; }
        public Guid? BusinessUuid { get; set; }
        public Guid? ClientUuid { get; set; }
        public Guid? ContactPersonUuid { get; set; }
        public string AddTime { get; set; }
        public int? IsDelete { get; set; }

    }
}
