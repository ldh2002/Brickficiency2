﻿using System.Collections.Generic;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IItemService : IRefreshable
    {
        IReadOnlyCollection<ItemDetails> GetItems();

        ItemDetails GetItem(string itemTypeCode, string itemId);

        IReadOnlyCollection<ItemDetails> SearchItems(string itemTypeCode, int? categoryId, string filter);

        IReadOnlyCollection<int> GetCategoryIdsForItemType(string itemTypeCode);
    }
}
