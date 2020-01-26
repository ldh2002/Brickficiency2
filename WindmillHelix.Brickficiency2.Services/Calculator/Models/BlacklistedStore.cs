namespace WindmillHelix.Brickficiency2.Services.Calculator.Models
{
    public class BlacklistedStore
    {
        public BlacklistedStore(StoreType storeType, string id)
        {
            StoreType = storeType;
            Id = id;
        }

        public StoreType StoreType { get; private set; }

        public string Id { get; private set; }
    }
}
