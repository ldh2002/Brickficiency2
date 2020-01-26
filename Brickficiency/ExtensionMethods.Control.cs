using System;
using System.Windows.Forms;

namespace Brickficiency
{
    public static partial class ExtensionMethods
    {
        public static void InvokeAction(this Control control, Action action)
        {
            control.Invoke(action);
        }
    }
}
