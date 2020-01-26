using System.Net;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkSessionService
    {
        CookieContainer GetCookieContainer();

        void EnsureAuthenticated();
    }
}
