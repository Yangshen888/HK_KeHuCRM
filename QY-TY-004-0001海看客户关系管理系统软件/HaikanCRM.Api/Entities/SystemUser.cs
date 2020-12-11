using System;
using System.Collections.Generic;

namespace HaikanCRM.Api.Entities
{
    public partial class SystemUser
    {
        public SystemUser()
        {
            ContactPerson = new HashSet<ContactPerson>();
            Customer = new HashSet<Customer>();
        }

        public Guid SystemUserUuid { get; set; }
        public string LoginName { get; set; }
        public string RealName { get; set; }
        public string UserIdCard { get; set; }
        public string PassWord { get; set; }
        public int? UserType { get; set; }
        public Guid? CompanyUuid { get; set; }
        public string AddTime { get; set; }
        public string AddPeople { get; set; }
        public int? IsDeleted { get; set; }
        public string ManageDepartment { get; set; }
        public int Id { get; set; }
        public Guid? GrabageRoomId { get; set; }
        public Guid? VillageId { get; set; }
        public string ZaiGang { get; set; }
        public string Wechat { get; set; }
        public string Phone { get; set; }
        public string OldCard { get; set; }
        public string Sex { get; set; }
        public string Streets { get; set; }
        public string InTime { get; set; }
        public string Types { get; set; }
        public string Address { get; set; }
        public string SystemRoleUuid { get; set; }
        public string ProblemContent { get; set; }
        public string ProblemType { get; set; }
        public string Ewm { get; set; }
        public string Remarks { get; set; }
        public Guid? ShopUuid { get; set; }
        public Guid? HomeAddressUuid { get; set; }
        public string IdcardMd5 { get; set; }
        public Guid? DepartmentUuid { get; set; }

        public virtual SystemDepartment DepartmentUu { get; set; }
        public virtual ICollection<ContactPerson> ContactPerson { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
    }
}
