using System;
using System.Net;

namespace WindmillHelix.Brickficiency2.Common.Net
{
    public class ExtendedWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);

            var webRequest = request as HttpWebRequest;
            if (webRequest == null)
            {
                return request;
            }

            webRequest.UserAgent = UserAgent;
            webRequest.CookieContainer = Cookies;

            return webRequest;
        }

        public string UserAgent { get; set; }

        public CookieContainer Cookies { get; set; }
    }
}
