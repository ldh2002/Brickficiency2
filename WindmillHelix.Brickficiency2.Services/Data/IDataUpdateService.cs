using System;

namespace WindmillHelix.Brickficiency2.Services.Data
{
    public interface IDataUpdateService
    {
        DateTime? GetLastFullUpdate();

        void UpdateData();

        void PrefetchData();
    }
}
