using System.ComponentModel;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    public interface ICalculatorService : INotifyPropertyChanged
    {
        void Abort();
    }
}
