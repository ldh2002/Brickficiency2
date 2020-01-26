using System.Net;
using WindmillHelix.Brickficiency2.Common.Providers;

namespace WindmillHelix.Brickficiency2.ExternalApi.Tests
{
    public class AppConfigBricklinkCredentialProvider : IBricklinkCredentialProvider
    {
        public NetworkCredential GetCredentials()
        {
            return new NetworkCredential(AppConfig.BricklinkUserName, AppConfig.BricklinkPassword);
        }
    }
}
