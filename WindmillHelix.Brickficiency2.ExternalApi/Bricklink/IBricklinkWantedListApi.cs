using System.Collections.Generic;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkWantedListApi
    {
        IReadOnlyCollection<WantedList> GetWantedLists();

        IReadOnlyCollection<WantedListItem> GetWantedListItems(int wantedListId);
    }
}
