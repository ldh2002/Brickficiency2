using System.Collections.Generic;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    public interface ICalculationSettings
    {
        IReadOnlyCollection<BlacklistedStore> BlacklistedStores { get; }

        bool AreAllCountriesAllowed { get; }

        IReadOnlyCollection<string> AllowedCountryCodes { get; }

        IReadOnlyCollection<string> AllowedRegionCodes { get; }
    }
}
