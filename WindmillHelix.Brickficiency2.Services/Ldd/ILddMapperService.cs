using WindmillHelix.Brickficiency2.Common.Domain;

namespace WindmillHelix.Brickficiency2.Services.Ldd
{
    public interface ILddMapperService
    {
        MappingResult<MappedPart> MapItem(LddPart source);
    }
}
