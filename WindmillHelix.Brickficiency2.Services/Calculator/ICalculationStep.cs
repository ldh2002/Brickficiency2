using System.ComponentModel;
using WindmillHelix.Brickficiency2.Services.Calculator.Models;

namespace WindmillHelix.Brickficiency2.Services.Calculator
{
    public interface ICalculationStep : INotifyPropertyChanged
    {
        CalculationStepStatus Status { get; }

        string Name { get; }

        int PercentComplete { get; }

        int StepOrder { get; }
    }
}
