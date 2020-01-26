using System.Collections.Generic;
using WindmillHelix.Brickficiency2.ExternalApi.Bricklink.Models;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkStoreApi
    {
        IReadOnlyCollection<BricklinkStore> GetStoresByCountry(string countryCode);
    }
}
