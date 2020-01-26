namespace WindmillHelix.Brickficiency2.Services
{
    public interface IAppDataService
    {
        string GetAppData(string key);

        void WriteAppData(string key, string value);
    }
}
