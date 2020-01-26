using System.Collections.Generic;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    public interface ILddFileService
    {
        IReadOnlyCollection<LddPart> ExtractPartsList(string fileName);
    }
}
