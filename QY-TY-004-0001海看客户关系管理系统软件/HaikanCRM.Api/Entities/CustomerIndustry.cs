using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class CustomerIndustry
    {
        public CustomerIndustry()
        {
            Customer = new HashSet<Customer>();
        }

        public Guid IndustryUuid { get; set; }
        public int Id { get; set; }
        public string IndustryName { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
    }
}
