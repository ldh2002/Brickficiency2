using System.Collections.Generic;
using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface IWantedListService
    {
        IReadOnlyCollection<WantedList> GetWantedLists();

        IReadOnlyCollection<WantedListItem> GetWantedListItems(int wantedListId);
    }
}
