using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HaikanCRM.Api.ViewModels.DIngDing
{

    //部门信息
    public class departmentAlldata
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }

        public List<department> department { get; set; }


    }

    public class department
    {
        public string name { get; set; }
        public int id { get; set; }
        public string parentid { get; set; }
        public bool createDeptGroup { get; set; }
        public bool autoAddUser { get; set; }
    }


    //人员信息
    public class depauser
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public bool hasMore { get; set; }
        public List<userlist> userlist { get; set; }



    }

    public class userlist
    {
        public string userid { get; set; }
        public string name { get; set; }

    }



    public class PersistentCodeResult
    {
        public string errcode { get; set; }
        public string access_token { get; set; }
        public string errmsg { get; set; }
        public int expires_in { get; set; }

    }
    public class PersistentCodeResult12
    {
        /// <summary>
        /// 
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sys_level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string is_sys { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string errmsg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deviceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userid { get; set; }

    }

    public class usersdata
    {
        public int errcode { get; set; }
        public string errmsg { get; set; }
        public string name { get; set; }
        public string unionid { get; set; }
        public string userid { get; set; }
        public string isLeaderInDepts { get; set; }
        public bool isBoss { get; set; }
        public string hiredDate { get; set; }
        public bool isSenior { get; set; }
        public int[] department { get; set; }
        public string orderInDepts { get; set; }
        public bool active { get; set; }
        public string avatar { get; set; }
        public bool isAdmin { get; set; }
        public bool isHide { get; set; }
        public string jobnumber { get; set; }
        public string position { get; set; }
        public List<roles> roles { get; set; }
    }


    public class roles
    {
        public string id { get; set; }
        public string name { get; set; }
        public string groupName { get; set; }
    }
}
