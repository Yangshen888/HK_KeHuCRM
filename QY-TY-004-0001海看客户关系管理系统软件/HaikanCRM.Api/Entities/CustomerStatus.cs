using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class CustomerStatus
    {
        public CustomerStatus()
        {
            Customer = new HashSet<Customer>();
        }

        public Guid CustomerStatusUuid { get; set; }
        public int Id { get; set; }
        public string StatusName { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
    }
}
