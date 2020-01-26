using System.Drawing;
using System.Threading.Tasks;

namespace WindmillHelix.Brickficiency2.Services
{
    public interface IImageService
    {

        Task<Image> GetImageAsync(string itemTypeCode, string itemId, int colorId);

        Task<Image> GetLargeImageAsync(string itemTypeCode, string itemId);
    }
}
