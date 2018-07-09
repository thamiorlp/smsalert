using System.Threading.Tasks;

namespace SmsAlert.Sms
{
    /// <summary>
    /// ISms测试实现，不实际发送短信
    /// </summary>
    public class MockSms : ISms
    {
        public async Task<string> SendMessageAsync(string phone, string message)
        {
            await Task.Yield();
            return "OK";
        }
    }
}
