using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.ExternalApi.Bricklink
{
    public interface IBricklinkImageApi
    {
        Task<byte[]> GetImageBytesAsync(string itemTypeCode, string itemId, int colorId, ImageFormat imageFormat);

        Task<byte[]> GetLargeImageBytesAsync(string itemTypeCode, string itemId, ImageFormat imageFormat);
    }
}
