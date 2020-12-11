using Haikan3.Utils;
using HaikanCRM.Api.Extensions;
using HaikanCRM.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HaikanCRM.Api.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 测试日志
        /// </summary>
        /// <returns></returns>
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Logger()
        {
            _logger.LogDebug(message: "LogDebug()...");
            _logger.LogInformation(message: "LogInformation()...");
            _logger.LogWarning(message: "LogWarning()...");
            _logger.LogError(message: "LogError()...");
            ResponseResultModel response = ResponseModelFactory.CreateResultInstance;
            response.SetSuccess(message: "test logger success");
            return Ok(value: response);
        }

        [HttpGet]
        public string Test1()
        {
            var user = User;
            
            return "xxxxxxxxxxxxxxx";
        }


        [HttpGet]
        public string Test2()
        {
            var obj = SyncInformation.DeleteUser("xxx");
            return JsonConvert.SerializeObject(obj);
        }

    }
}
