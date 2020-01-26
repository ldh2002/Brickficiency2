using System.Collections.Generic;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IColorService : IRefreshable
    {
        IReadOnlyCollection<ItemColor> GetColors();
    }
}
