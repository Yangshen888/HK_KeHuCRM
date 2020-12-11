using System;
using System.IO;
using System.Linq;
using HaikanCRM.Api.Entities;
using HaikanCRM.Api.Entities.Enums;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Extensions.DataAccess;
using HaikanCRM.Api.Models.Response;
using HaikanCRM.Api.RequestPayload.Bussiness;
using HaikanCRM.Api.ViewModels.Business;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HaikanCRM.Api.Controllers.Api.V1.business
{
    /// <summary>
    /// 有关商机文件的操作接口
    /// </summary>
    [Route("api/v1/business/[controller]/[action]")]
    [ApiController]
    public class BusinessDocumentsController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly HaikanCRMContext _dbContext;
 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="hostingEnvironment"></param>
        public BusinessDocumentsController(HaikanCRMContext dbContext, IWebHostEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 商机文件列表
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(BDocumentsRPayload payload)
        {
            using (_dbContext)
            {
                var query = _dbContext.BusinessDocuments.Where(x => x.BusinessUuid == payload.BusinessUuid && x.IsDelete == 0);

                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.FileName.Contains(payload.Kw));
                }
                if (payload.FirstSort != null)
                {
                    query = query.OrderBy(payload.FirstSort.Field, payload.FirstSort.Direct == "DESC");
                }
                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();

                var response = ResponseModelFactory.CreateResultInstance;
                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        /// <summary>
        /// 添加商机文件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(BDocumentsModels model)
        {
            var response = ResponseModelFactory.CreateInstance;

            using (_dbContext)
            {
                if (_dbContext.BusinessDocuments.Any(x => x.BusinessUuid == model.BusinessUuid && x.FileName == model.FileName && x.IsDelete == 0))
                {
                    response.SetFailed("已存在该名称");
                    return Ok(response);
                }
                var bd = new BusinessDocuments()
                {
                    BusDocumentsUuid = Guid.NewGuid(),
                    FileName = model.FileName,
                    FileUrl = model.FileUrl,
                    FileInfo = model.FileInfo,
                    BusinessUuid = model.BusinessUuid,
                    AddTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    IsDelete = 0,
                };
                _dbContext.BusinessDocuments.Add(bd);
                _dbContext.SaveChanges();

                return Ok(response);
            }
        }

        /// <summary>
        /// 获取编辑的商机文件信息
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.BusinessDocuments.First(x => x.BusDocumentsUuid == guid);
                response.SetData(entity);
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑的商机文件信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(BDocumentsModels model)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var entity = _dbContext.BusinessDocuments.FirstOrDefault(x => x.BusDocumentsUuid == model.BusDocumentsUuid);
                if (entity == null)
                {
                    response.SetFailed("该商机文件不存在");
                    return Ok(response);
                }
                else if (entity.FileName != model.FileName && _dbContext.BusinessDocuments.Any(x => x.FileName == model.FileName && x.BusinessUuid == model.BusinessUuid && x.IsDelete == 0))
                {
                    response.SetFailed("已存在该名称");
                    return Ok(response);
                }
                else
                {
                    entity.FileName = model.FileName;
                    entity.FileInfo = model.FileInfo;
                    entity.FileUrl = model.FileUrl;
                    _dbContext.SaveChanges();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }


        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    break;
                case "recover":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                    break;
            }
            return Ok(response);
        }


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> UpLoadAsync([FromForm] string filename)
        {
            var response = ResponseModelFactory.CreateInstance;
            //var check = System.IO.File.Exists(filename);
            if ((!string.IsNullOrEmpty(filename)) && System.IO.File.Exists(filename))
            {
                FileInfo info = new FileInfo(filename);
                if (info.Attributes != FileAttributes.ReadOnly)
                {
                    System.IO.File.Delete(filename);
                }
            }
            var files = Request.Form.Files;
            if (files == null || files.Count() <= 0)
            {
                response.SetFailed("请上传文件");
                return Ok(response);
            }
            try
            {
                int index = files[0].FileName.LastIndexOf('.');
                string fname = Guid.NewGuid().ToString() + files[0].FileName.Substring(index);
                string allfname = DateTime.Now.ToString("yyyyMMdd_HHmmssfff") + "_" + fname;
                string strpath = Path.Combine("UploadFiles/File", allfname);
                string path = Path.Combine(_hostingEnvironment.WebRootPath, strpath);

                var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);

                await files[0].CopyToAsync(stream);
                stream.Close();

                response.SetData(new { path, DataPath = strpath });
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult DeletetoFile(string filePath)
        {
            var response = ResponseModelFactory.CreateInstance;

            var path = Path.Combine(_hostingEnvironment.WebRootPath + "/UploadFiles/File", filePath);
            if (string.IsNullOrEmpty(path))
            {
                response.SetFailed("无路径");
                return Ok(response);
            }
            try
            {
                if (System.IO.File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    if (info.Attributes != FileAttributes.ReadOnly)
                    {
                        System.IO.File.Delete(path);
                        response.SetSuccess("删除成功");
                    }
                    else
                    {
                        response.SetFailed("只读文件");
                    }
                }
                else
                {
                    response.SetFailed("文件不存在");
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetFailed(ex.Message);
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter($"@p{index}", id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql =
                    $"UPDATE BusinessDocuments SET IsDelete=@IsDelete WHERE BusDocumentsUUID IN ({parameterNames})";
                parameters.Add(new SqlParameter("@IsDelete", (int)isDeleted));
                var num = _dbContext.Database.ExecuteSqlRaw(sql, parameters);

                var response = ResponseModelFactory.CreateInstance;
                if (num <= 0)
                {
                    response.SetFailed("删除失败");
                }
                return response;
            }
        }

    }

}
