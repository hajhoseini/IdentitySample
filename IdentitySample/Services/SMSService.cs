using System.Net;

namespace IdentitySample.Services
{
    public class SMSService
    {
        public void Send(string phoneNumber, string code)
        {
            var client = new WebClient();
            string url = $"http://panel.kavenegar.com/v1/apikey/verify/lookup.json?receptor={phoneNumber}&token={code}&template=VerifyTestAccount"; // تمپلیت چیزی است که از پنل پیامک مشخص می شود
            var content = client.DownloadString(url);
        }
    }
}
