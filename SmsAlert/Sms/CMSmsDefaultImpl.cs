using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmsAlert.Sms
{
    /// <summary>
    /// 使用移动信息机发送短信的ISms默认实现
    /// </summary>
    public class CMSmsDefaultImpl : ISms
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<SmsAlertConfig> _config;
        private readonly ILogger<CMSmsDefaultImpl> _logger;

        public CMSmsDefaultImpl(HttpClient httpClient, IOptions<SmsAlertConfig> config, ILogger<CMSmsDefaultImpl> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// 向指定手机号码发送短信
        /// </summary>
        /// <param name="phone">接收号码</param>
        /// <param name="message">短信内容</param>
        /// <returns></returns>
        public async Task<string> SendMessageAsync(string phone, string message)
        {
            try
            {
                //短信接口要求短信内容以gb2312编码进行发送，先进行转码
                var content = HttpUtility.UrlEncode(message, Encoding.GetEncoding("gb2312"));
                //参数以querystring传递
                var url = $"{_config.Value.SmsApiUrl}?phone={phone}&content={content}&svcid={_config.Value.SmsSvcId}&smstype={_config.Value.SmsType}&longMessage=1&extent=";
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))  //POST方式请求
                {
                    var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                    var resposeContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    _logger.LogInformation($"message [{message}] sent to [{phone}] with result [{resposeContent}]");
                    return resposeContent;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"message [{message}] falied to send to [{phone}]");
                return null;
            }
        }
    }
}
