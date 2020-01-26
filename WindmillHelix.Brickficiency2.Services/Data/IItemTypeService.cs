using System.Collections.Generic;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IItemTypeService : IRefreshable
    {
        IReadOnlyCollection<ItemType> GetItemTypes();
    }
}
