using Autofac;
using Brickficiency.UI;
using System;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.DependencyInjection;

namespace Brickficiency
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = DependencyInjectionConfig.Setup();

            var initializationForm = container.Resolve<InitializationForm>();

            Application.Run(initializationForm);
        }
    }
}
