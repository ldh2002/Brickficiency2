using System.Net;

namespace WindmillHelix.Brickficiency2.Common.Providers
{
    public interface IBricklinkCredentialProvider
    {
        NetworkCredential GetCredentials();
    }
}
