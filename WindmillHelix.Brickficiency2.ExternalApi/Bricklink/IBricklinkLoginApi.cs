using System.Net;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkLoginApi
    {
        bool Login(CookieContainer cookies, string userName, string password);
    }
}
