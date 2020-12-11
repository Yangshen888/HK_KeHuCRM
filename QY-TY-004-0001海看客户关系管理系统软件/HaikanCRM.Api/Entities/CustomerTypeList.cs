using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class CustomerTypeList
    {
        public CustomerTypeList()
        {
            Customer = new HashSet<Customer>();
        }

        public Guid TypeUuid { get; set; }
        public string TypeName { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
    }
}
