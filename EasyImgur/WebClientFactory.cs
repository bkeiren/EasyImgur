using System.Net;
using EasyImgur.Properties;

namespace EasyImgur
{
    public class WebClientFactory
    {
        public WebClientFactory()
        {
            this.ReloadSetting();
        }

        public IWebProxy Proxy { get; set; }

        public WebClient Create()
        {
            var client = new WebClient();
            if (this.Proxy != null) client.Proxy = this.Proxy;
            return client;
        }

        public void ReloadSetting()
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.ProxyAddress))
            {
                var ap = Settings.Default.ProxyAddress.Split(':');
                this.Proxy = new WebProxy(ap[0], int.Parse(ap[1]));
            }
        }
    }
}