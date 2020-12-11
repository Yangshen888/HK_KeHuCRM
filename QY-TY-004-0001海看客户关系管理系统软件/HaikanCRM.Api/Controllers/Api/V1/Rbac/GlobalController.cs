using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.ViewModels.Rbac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace HaikanCRM.Api.Controllers.Api.V1.Rbac
{
    /// <summary>
    /// 有关全局设置的操作接口
    /// </summary>
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    public class GlobalController : ControllerBase
    {
        private readonly HaikanCRMContext _dbContext;
        //private readonly IMapper _mapper;
        private IWebHostEnvironment _hostEnv;


        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="iWebHostEnvironment"></param>
        public GlobalController(HaikanCRMContext dbContext, IWebHostEnvironment iWebHostEnvironment)
        {
            _dbContext = dbContext;
            //_mapper = mapper;
            _hostEnv = iWebHostEnvironment;
        }
        #region 查看列表
        /// <summary>
        /// 查看数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult List()
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.SystemSetting.AsQueryable().ToList();
                response.SetData(query);
                return Ok(response);
            }
        }
        #endregion

        #region 修改
        /// <summary>
        /// 保存编辑后的角色信息
        /// </summary>
        /// <param name="model">角色视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(GlobalViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var query = _dbContext.SystemSetting.FirstOrDefault(x => x.ClobalUuid == model.ClobalUuid);
                query.ClobalName = model.ClobalName;
                query.GlobalLogo = model.GlobalLogo;
                query.Globalurl = model.Globalurl;
                query.AddTime = DateTime.Now;
                _dbContext.SaveChanges();
                response.SetSuccess();
                return Ok(response);
            }

        }

        #endregion

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> UpLoadAsync()
        {
            var response = ResponseModelFactory.CreateInstance;
            //var check = System.IO.File.Exists(filename);

            var files = Request.Form.Files;
            if (files == null || files.Count() <= 0)
            {
                response.SetFailed("请上图片文件");
                return Ok(response);
            }
            //var paths = new List<string>();
            //var dataPaths = new List<string>();
            var allowType = new string[] { "image/jpeg", "image/png", "image/jpg" };
            var message = "";
            try
            {
                if (files.Any(c => allowType.Contains(c.ContentType)))
                {

                    //foreach (var file in files)
                    //{
                    if (files[0].Length > 1024 * 1024 * 5)
                    {
                        message += files[0].FileName + "大小超过5M";
                        response.SetFailed(message);
                        return Ok(response);
                    }
                    int index = files[0].FileName.LastIndexOf('.');
                    string prefix = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + Guid.NewGuid().ToString();
                    string fname = prefix + files[0].FileName.Substring(index);
                    string strpath = Path.Combine("UploadFiles", fname);
                    string path = Path.Combine(_hostEnv.WebRootPath, strpath);
                    //System.IO.File.Create(path);
                    var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    await files[0].CopyToAsync(stream);

                    stream.Close();
                    bool toCompression = false;
                    string fname2 = "";
                    string strpath2 = "";

                    if (files[0].Length > (1024 * 1024 * 1))
                    {
                        fname2 = prefix + "_cp" + files[0].FileName.Substring(index);
                        strpath2 = Path.Combine("UploadFiles", fname2);
                        var dFile = Path.Combine(_hostEnv.WebRootPath, strpath2);
                        var num = 1024 * 1024 * 1.0 / files[0].Length;
                        int flag = (int)(num * 100);
                        if (flag < 50)
                        {
                            flag = (int)(flag * 1.7);
                        }
                        toCompression = PhotoCompression.CompressImage(path, dFile, flag, 1024);
                        if (toCompression)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Attributes != FileAttributes.ReadOnly)
                                {
                                    System.IO.File.Delete(path);

                                }

                            }
                        }
                    }
                    else if (files[0].Length > (1024 * 1024 * 0.5) && files[0].Length <= (1024 * 1024 * 1))
                    {
                        fname2 = prefix + "_cp" + files[0].FileName.Substring(index);
                        strpath2 = Path.Combine("UploadFiles", fname2);
                        var dFile = Path.Combine(_hostEnv.WebRootPath, strpath2);

                        int flag = 90;

                        toCompression = PhotoCompression.CompressImage(path, dFile, flag, 500);
                        if (toCompression)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                FileInfo info = new FileInfo(path);
                                if (info.Attributes != FileAttributes.ReadOnly)
                                {
                                    System.IO.File.Delete(path);

                                }

                            }
                        }
                    }
                    if (toCompression)
                    {
                        response.SetData(new { strpath = strpath2, fname = fname2 });

                    }
                    else
                    {
                        response.SetData(new { strpath, fname });
                    }

                }
                if (message.Length > 0)
                {
                    response.SetFailed(message);
                }
                //response.SetData(new { paths, dataPaths });
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }
        }
        #endregion

    }
}






