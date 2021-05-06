using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    public class ButtonLess : Button
    {
        static ButtonLess()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonLess), new FrameworkPropertyMetadata(typeof(ButtonLess)));
        }
    }
}
