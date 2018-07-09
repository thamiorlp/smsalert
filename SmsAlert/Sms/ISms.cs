using System.Threading.Tasks;

namespace SmsAlert.Sms
{
    /// <summary>
    /// 短信接口
    /// </summary>
    public interface ISms
    {
        /// <summary>
        /// 向指定手机号码发送短信
        /// </summary>
        /// <param name="phone">接收号码</param>
        /// <param name="message">短信内容</param>
        /// <returns></returns>
        Task<string> SendMessageAsync(string phone, string message);
    }
}
