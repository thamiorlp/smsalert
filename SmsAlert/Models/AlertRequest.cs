using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmsAlert.Models
{
    /// <summary>
    /// 告警发送请求类
    /// </summary>
    public class AlertRequest
    {
        /// <summary>
        /// 告警内容
        /// </summary>
        [Required]
        public string Message { get; set; }

        /// <summary>
        /// 接口告警的手机号码
        /// </summary>
        [Required]
        public IEnumerable<string> Receivers { get; set; }
    }
}
