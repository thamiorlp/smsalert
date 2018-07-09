using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SmsAlert.Models;
using SmsAlert.Sms;

namespace SmsAlert.Controllers
{
    /// <summary>
    /// 告警控制器，处理告警消息相关逻辑
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        private readonly ISms _sms;
        private readonly IOptions<SmsAlertConfig> _config;

        public AlertController(ISms sms, IOptions<SmsAlertConfig> config)
        {
            _sms = sms;
            _config = config;
        }

        /// <summary>
        /// 向指定的手机号码发送告警消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns>返回HTTP状态码200表示处理完成</returns>
        /// <example>
        /// {
        ///   "message": "测试消息",
        ///   "receivers": [
        ///     "13632256155",
        ///     "13602485302"
        ///   ]
        /// }
        /// </example>
        /// <remarks>
        /// {
        ///   "message": "测试消息",
        ///   "receivers": [
        ///     "13632256155",
        ///     "13602485302"
        ///   ]
        /// }
        /// </remarks>
        [HttpPost]
        public async Task SendAlertAsync([FromBody]AlertRequest request)
        {
            foreach (var phone in request?.Receivers)
            {
                //发送短信，忽略发送结果
                await _sms.SendMessageAsync(phone, request.Message).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// 展示当前短信接口的配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetConfig()
        {
            return new JsonResult(_config.Value);
        }
    }
}