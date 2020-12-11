using System;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using Haikan3.Utils;
using HaikanCRM.Api.Auth;
using HaikanCRM.Api.Configurations;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.AuthContext;
using HaikanCRM.Api.ViewModels.Rbac.SystemUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HaikanCRM.Api.Controllers.Api.V1.DingDing
{
    /// <summary>
    /// 钉钉端的操作接口
    /// </summary>
    [Route("api/v1/Mobi/[controller]/[action]")]
    [ApiController]
    public class DingDingController : ControllerBase
    {
        private readonly AppAuthenticationSettings _appSettings;
        private readonly HaikanCRMContext _dbContext;
        private readonly MdDesEncrypt MdDesEncrypt;
 
        public DingDingController(IOptions<AppAuthenticationSettings> appSettings, HaikanCRMContext dbContext, IOptions<MdDesEncrypt> mdDesEncrypt)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            MdDesEncrypt = mdDesEncrypt.Value;
        }
        /// <summary>
        /// 获取(企业内部应用)
        /// </summary>
        /// <returns></returns>
        [HttpGet("{strlist}")]
        public IActionResult Getuserinfo(string strlist)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var code = strlist;
                //TODO:钉钉相关的配置信息都要放到配置文件中
                string suiteKey = "dinga7xg5vjb2lwwvicu";
                string suiteSecret = "pUiI0xvN0ZEbsFavSbuaLqctwHL2p9cIRlQ4HU5GS7y-TmYngcTjJGuI309ZLR_h";
                string timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
                string suiteTicket = "TestSuiteTicket";
                string signature1 = timestamp + "\n" + suiteTicket;
                string signature2 = HmacSHA256(signature1, suiteSecret);
                string signature = System.Web.HttpUtility.UrlEncode(signature2, System.Text.Encoding.UTF8);
                string auth_corpid = strlist;
                string url = "https://oapi.dingtalk.com/gettoken?appkey=" + suiteKey + "&appsecret=" + suiteSecret;

                try
                {
                    var response11 = Haikan3.Utils.DingDingHelper.HttpGet(url);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<HaikanCRM.Api.ViewModels.DIngDing.PersistentCodeResult>(response11);
                    if (result != null && result.errcode == "0")
                    {
                        string url11 = "https://oapi.dingtalk.com/user/getuserinfo?access_token=" + result.access_token + "&code=" + code;
                        var response12 = Haikan3.Utils.DingDingHelper.HttpGet(url11);
                        var result12 = Newtonsoft.Json.JsonConvert.DeserializeObject<HaikanCRM.Api.ViewModels.DIngDing.PersistentCodeResult12>(response12);
                        if (result12 != null && result12.errcode == 0)
                        {
                            //获取人员信息
                            //var results = Haikan3.Utils.DingDingHelper.GetUserDetail(result.access_token, result12.userid);
                            var roiduuid = _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "客户经理");
                            var userdata = _dbContext.SystemUser.Where(x => x.Streets == result12.userid).ToList().Count;
                            if (userdata == 0)
                            {
                                UserEditViewModel model = new UserEditViewModel();
                                string pas = "123456";
                                var entity = new HaikanCRM.Api.Entities.SystemUser();
                                entity.SystemUserUuid = Guid.NewGuid();
                                entity.Streets = result12.userid;
                                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                entity.RealName = result12.name;
                                entity.LoginName = result12.name;
                                //entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(pas.Trim(), MdDesEncrypt.SecretKey);
                                entity.PassWord = Security.GenerateMD5(pas.Trim());
                                entity.SystemRoleUuid = roiduuid.SystemRoleUuid.ToString();
                                entity.IsDeleted = 0;
                                entity.ZaiGang = "在岗";
                                entity.UserType = 2;
                                _dbContext.SystemUser.Add(entity);
                                _dbContext.SaveChanges();
                                _dbContext.Database.ExecuteSqlRaw("DELETE FROM SystemUserRoleMapping WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                                var success = true;
                                ////循环加权限
                                //for (int i = 0; i < model.SystemRoleUuid.Count; i++)
                                //{
                                if (entity.SystemRoleUuid != null)
                                {
                                    var roles = new SystemUserRoleMapping();
                                    roles.SystemUserUuid = entity.SystemUserUuid;
                                    roles.SystemRoleUuid = Guid.Parse(entity.SystemRoleUuid);
                                    roles.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                                    roles.AddPeople = AuthContextService.CurrentUser.DisplayName;

                                    _dbContext.SystemUserRoleMapping.Add(roles);

                                }
                                //}
                                success = _dbContext.SaveChanges() > 0;
                                if (success)
                                {
                                    response.SetSuccess();
                                }
                                else
                                {
                                    _dbContext.Database.ExecuteSqlRaw("DELETE FROM SystemUser WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                                    response.SetFailed("保存用户角色数据失败");
                                }
                            }
                            var user = _dbContext.SystemUser.FirstOrDefault(x => x.IsDeleted == 0 && x.Streets == result12.userid);
                            var role = _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == Guid.Parse(user.SystemRoleUuid));
                            var claimsIdentity = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, result12.userid),
                    new Claim("guid",user.SystemUserUuid.ToString()),
                    new Claim("avatar",""),
                    new Claim("displayName",user.RealName),
                    new Claim("loginName",user.LoginName),
                    new Claim("emailAddress",""),
                    //new Claim("guid",user.SystemUserUuid.ToString()),
                    //new Claim("userType",usertype.ToString()),
                    new Claim("userType",user.UserType.Value.ToString()),
                    new Claim("roleid",user.SystemRoleUuid.TrimEnd(',')),
                    new Claim("roleName",role.RoleName.TrimEnd(',')),
                    new Claim("ZYZ",""),
                    new Claim("YH",""),
                    new Claim("DDY",""),
                    new Claim("SJ","")
                });
                            var token = JwtBearerAuthenticationExtension.GetJwtAccessToken(_appSettings, claimsIdentity);

                            response.SetData(new { user, token });

                            return Ok(response);

                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{strlist}")]
        protected IActionResult Tongxunlu()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            string strs = "开始";
            //1、获取access_token
            string access_token = DingDingHelper.GetAccessToken0().access_token;
            try
            {
                //2、获取所有的通讯录有权限的部门
                string url = "https://oapi.dingtalk.com/auth/scopes?access_token=" + access_token;
                var rell = Haikan3.Utils.DingDingHelper.HttpGet(url);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<sqdepaetRoot>(rell);
                strs = "1、" + result.errmsg;
                if (result != null)
                {
                    if (result.errcode == 0)
                    {
                        var deplist = result.auth_org_scopes.authed_dept;
                        if (deplist.Count > 0)
                        {
                            for (int i1 = 0; i1 < deplist.Count; i1++)
                            {
                                var depid = deplist[i1].ToString();
                                var departid = "0";
                                //2、获取部门的详细信息
                                string urlss1 = "https://oapi.dingtalk.com/department/get?access_token=" + access_token + "&id=" + depid;
                                var responsess1 = Haikan3.Utils.DingDingHelper.HttpGet(urlss1);
                                var resultss1 = Newtonsoft.Json.JsonConvert.DeserializeObject<pasrtDesultRoot>(responsess1);
                                if (resultss1 != null)
                                {

                                }
                                GetDepart(access_token, depid, departid);
                                strs = "钉钉通讯录同步成功";
                            }
                        }
                    }
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="depid"></param>
        /// <param name="departid"></param>
        public void GetDepart(string access_token, string depid, string departid)
        {
            //3、获取部门下面所有的人员信息
            string url1 = "https://oapi.dingtalk.com/user/simplelist?access_token=" + access_token + "&department_id=" + depid;
            var res = Haikan3.Utils.DingDingHelper.HttpGet(url1);
            var result1 = Newtonsoft.Json.JsonConvert.DeserializeObject<useridRoot>(res);
            //strs = "2、" + result1.errmsg + "acctoken:" + access_token + " partid:" + depid;
            if (result1 != null)
            {
                if (result1.errcode == 0)
                {
                    var useridlist = result1.userlist;
                    if (useridlist.Count > 0)
                    {
                        for (int i2 = 0; i2 < useridlist.Count; i2++)
                        {
                            var userid = useridlist[i2].userid;
                            //4、获取所有人员的详细详细
                            string url2 = "https://oapi.dingtalk.com/user/get?access_token=" + access_token + "&userid=" + userid;
                            var response2 = Haikan3.Utils.DingDingHelper.HttpGet(url2);
                            var result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<UserDetailRoot>(response2);
                            //strs = "3、" + result2.errmsg;
                            if (result2 != null)
                            {

                            }
                        }
                    }
                }
            }
            //2、获取所有的通讯录有权限的部门的下级部门，直到没有下级部门。
            string urls1 = "https://oapi.dingtalk.com/department/list_ids?access_token=" + access_token + "&id=" + depid;
            var reer = Haikan3.Utils.DingDingHelper.HttpGet(urls1);
            var results1 = Newtonsoft.Json.JsonConvert.DeserializeObject<departRoot>(reer);
            if (results1 != null)
            {
                if (results1.errcode == 0)
                {
                    if (results1.sub_dept_id_list != null && results1.sub_dept_id_list.Count > 0)
                    {
                        for (int i = 0; i < results1.sub_dept_id_list.Count; i++)
                        {
                            string depsid = results1.sub_dept_id_list[i].ToString();
                            var departsid = "0";
                            //2、获取部门的详细信息
                            string urlss1 = "https://oapi.dingtalk.com/department/get?access_token=" + access_token + "&id=" + depsid;
                            var responsess1 = Haikan3.Utils.DingDingHelper.HttpGet(urlss1);
                            var resultss1 = Newtonsoft.Json.JsonConvert.DeserializeObject<pasrtDesultRoot>(responsess1);
                            if (resultss1 != null)
                            {

                            }
                            GetDepart(access_token, depsid, departsid);
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private string HmacSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        //通过手机号码查询用户信息
        [HttpGet("{phone}")]
        [ProducesResponseType(200)]
        public IActionResult GetSystemUser(string phone)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.Phone == phone);


                response.SetSuccess();
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 后台同步部门和人员信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Getalluseranddep()
        {
            //1、获取access_token
            string access_token = Haikan3.Utils.DingDingHelper.GetAccessToken0().access_token;
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                try
                {


                    UserEditViewModel model = new UserEditViewModel();

                    string pas = "123456";
                    var code = access_token;
                    string suiteKey = "dinga7xg5vjb2lwwvicu";
                    string suiteSecret = "pUiI0xvN0ZEbsFavSbuaLqctwHL2p9cIRlQ4HU5GS7y-TmYngcTjJGuI309ZLR_h";
                    string timestamp = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000).ToString();
                    string suiteTicket = "TestSuiteTicket";
                    string signature1 = timestamp + "\n" + suiteTicket;
                    string signature2 = HmacSHA256(signature1, suiteSecret);
                    string signature = System.Web.HttpUtility.UrlEncode(signature2, System.Text.Encoding.UTF8);
                    string auth_corpid = access_token;
                    //string url = "https://oapi.dingtalk.com/service/get_corp_token?signature=" + signature + "&timestamp=" + timestamp + "&suiteTicket=" + suiteTicket + "&accessKey=" + suiteKey;
                    string url = "https://oapi.dingtalk.com/gettoken?appkey=" + suiteKey + "&appsecret=" + suiteSecret;
                    //string param = "{ \"auth_corpid\": \"ding5998aa137739c847bc961a6cb783455b\"}";      
                    //var response11 = Haikan3.Utils.DingDingHelper.HttpPost(url, param);
                    var response11 = Haikan3.Utils.DingDingHelper.HttpGet(url);
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<HaikanCRM.Api.ViewModels.DIngDing.PersistentCodeResult>(response11);
                    if (result != null && result.errcode == "0")
                    {

                        //获取部门列表
                        string urldep = "https://oapi.dingtalk.com/department/list?access_token=" + result.access_token;
                        var responseldep = Haikan3.Utils.DingDingHelper.HttpGet(urldep);
                        var resultdep = Newtonsoft.Json.JsonConvert.DeserializeObject<HaikanCRM.Api.ViewModels.DIngDing.departmentAlldata>(responseldep);
                        //将获取的部门信息保存到数据库
                        //if (resultdep.department == null)
                        //{
                        //    response.SetFailed(resultdep.errmsg);
                        //    return Ok(response);
                        //}
                        for (int i = 0; i < resultdep.department.Count; i++)
                        {
                            var depid = _dbContext.SystemDepartment.Count(x => x.Dingid == resultdep.department[i].id);
                            //数据库中没有查到此部门--将数据添加到数据库中
                            if (depid == 0)
                            {
                                var entity = new SystemDepartment();
                                entity.Name = resultdep.department[i].name;//部门名称
                                entity.Dingid = resultdep.department[i].id;//部门钉钉id
                                entity.IsDeleted = 0;//未删除
                                entity.EstablishTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//添加时间
                                entity.EstablishName = "钉钉同步";//添加人
                                entity.Remark = ""; //备注
                                entity.DepartmentUuid = Guid.NewGuid();//部门guid
                                _dbContext.SystemDepartment.Add(entity);//添加
                                _dbContext.SaveChanges();
                            }
                            else//此部门已存在---更新部门数据
                            {
                                var entity = _dbContext.SystemDepartment.FirstOrDefault(x => x.Dingid == resultdep.department[i].id);
                                entity.Name = resultdep.department[i].name;//更新部门名称
                                _dbContext.SaveChanges();
                            }

                            //获取该部门的所有用户
                            string urldepuser = "https://oapi.dingtalk.com/user/simplelist?access_token=" + result.access_token + "&department_id=" + resultdep.department[i].id;

                            //获取部门uuid
                            var depuuid = _dbContext.SystemDepartment.FirstOrDefault(x => x.Dingid == resultdep.department[i].id).DepartmentUuid;

                            var responsedepuser = Haikan3.Utils.DingDingHelper.HttpGet(urldepuser);
                            var resdepuser = Newtonsoft.Json.JsonConvert.DeserializeObject<HaikanCRM.Api.ViewModels.DIngDing.depauser>(responsedepuser);
                            //将获取到的人员信息保存到数据库中
                            for (int j = 0; j < resdepuser.userlist.Count; j++)
                            {
                                var userid = _dbContext.SystemUser.Count(x => x.Streets == resdepuser.userlist[j].userid);

                                //获取人员信息
                                var results = Haikan3.Utils.DingDingHelper.HttpGet("https://oapi.dingtalk.com/user/get?access_token=" + result.access_token + "&userid=" + resdepuser.userlist[j].userid);
                                var usersxinxi = Newtonsoft.Json.JsonConvert.DeserializeObject<HaikanCRM.Api.ViewModels.DIngDing.usersdata>(results);
                                var roiduuid = _dbContext.SystemRole.FirstOrDefault(x => x.RoleName == "客户经理");
                                //数据库中没有该人员信息--添加到数据库中
                                if (userid == 0)
                                {
                                    var entity = new SystemUser();
                                    entity.SystemUserUuid = Guid.NewGuid();
                                    entity.LoginName = resdepuser.userlist[j].name;
                                    entity.RealName = resdepuser.userlist[j].name;
                                    entity.Streets = resdepuser.userlist[j].userid;
                                    entity.DepartmentUuid = depuuid;//部门uuid
                                    entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                    //entity.PassWord = Haikan3.Utils.DesEncrypt.Encrypt(pas.Trim(), MdDesEncrypt.SecretKey);
                                    entity.PassWord= Security.GenerateMD5(pas.Trim());;
                                    entity.SystemRoleUuid = roiduuid.SystemRoleUuid.ToString();
                                    entity.IsDeleted = 0;
                                    entity.ZaiGang = "在岗";
                                    entity.UserType = 2;
                                    _dbContext.SystemUser.Add(entity);
                                    _dbContext.SaveChanges();

                                    _dbContext.Database.ExecuteSqlRaw("DELETE FROM SystemUserRoleMapping WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                                    var success = true;
                                    ////循环加权限
                                    //for (int i = 0; i < model.SystemRoleUuid.Count; i++)
                                    //{
                                    if (entity.SystemRoleUuid != null)
                                    {
                                        var roles = new SystemUserRoleMapping();
                                        roles.SystemUserUuid = entity.SystemUserUuid;
                                        roles.SystemRoleUuid = Guid.Parse(entity.SystemRoleUuid);
                                        roles.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                                        roles.AddPeople = AuthContextService.CurrentUser.DisplayName;

                                        _dbContext.SystemUserRoleMapping.Add(roles);

                                    }
                                    //}
                                    success = _dbContext.SaveChanges() > 0;
                                    if (success)
                                    {
                                        response.SetSuccess();
                                    }
                                    else
                                    {
                                        _dbContext.Database.ExecuteSqlRaw("DELETE FROM SystemUser WHERE SystemUserUUID={0}", entity.SystemUserUuid);
                                        response.SetFailed("保存用户角色数据失败");
                                    }


                                }
                                else
                                { //数据库中存在该人员--修改信息
                                    var entity = _dbContext.SystemUser.FirstOrDefault(x => x.Streets == resdepuser.userlist[j].userid);
                                    entity.RealName = resdepuser.userlist[j].name;
                                    entity.LoginName = resdepuser.userlist[j].name;
                                    entity.DepartmentUuid = depuuid;//部门uuid
                                    _dbContext.SaveChanges();
                                }

                            }

                            ////获取子部门id列表
                            //string urlzidep = "https://oapi.dingtalk.com/department/list_ids?access_token=" + result.access_token + "&id=" + resultdep.department[i].id;
                            //    var responselzidep = Haikan3.Utils.DingDingHelper.HttpGet(urlzidep);

                        }

                    }

                    //var response = ResponseModelFactory.CreateInstance;
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    response.SetFailed(ex.Message);
                    return Ok(response);
                }
            }

        }

        /// <summary>
        /// 钉钉显示个人信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PersonalList(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = (from cp in _dbContext.SystemUser
                             where cp.IsDeleted != 1
                             && cp.SystemUserUuid == guid
                             select new
                             {
                                 cp.SystemUserUuid,
                                 cp.RealName,
                                 cp.Phone,
                                 cp.AddTime,
                                 cp.ZaiGang,
                                 cp.Sex,
                                 cp.Types,
                                 cp.OldCard,
                                 bumenName = cp.DepartmentUu.Name,
                             }).FirstOrDefault();
                var demo = _dbContext.Customer.Where(x => x.ClientManager == guid && x.IsDelete != 1);
                var clientnum = demo.Count();
                int businessnum = 0;
                foreach (var item in demo)
                {
                    var entity = _dbContext.Business.Where(x => x.ClientUuid == item.ClientUuid && x.IsDelete != 1);
                    businessnum += entity.Count();
                }

                response.SetData(new { query, clientnum, businessnum });
                return Ok(response);
            }
        }

        /// <summary>
        /// 显示个人信息详情
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Showmantion(Guid guid)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                response.SetData(entity);
            }
            return Ok(response);
        }

        /// <summary>
        /// 编辑个人信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Addmantion(dynamic model)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                Guid guid = model.systemUserUuid;
                var entity = _dbContext.SystemUser.FirstOrDefault(x => x.SystemUserUuid == guid);
                if (entity != null)
                {
                    entity.RealName = model.realName;
                    entity.ZaiGang = model.zaiGang;
                    entity.Phone = model.phone;
                    entity.OldCard = model.oldCard;
                    entity.Sex = model.sex;
                    entity.Types = model.types;
                }

                _dbContext.SaveChanges();
                response.SetSuccess("操作成功");
            }
            return Ok(response);
        }

    }
}
