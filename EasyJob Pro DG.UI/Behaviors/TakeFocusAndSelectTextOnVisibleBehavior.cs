using Microsoft.Xaml.Behaviors;
using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.Behaviors
{
    public class TakeFocusAndSelectTextOnVisibleBehavior : TriggerAction<TextBox>
    {
        protected override void Invoke(object parameter)
        {
            Dispatcher.BeginInvoke(
                DispatcherPriority.Loaded,
                new Action(() =>
                {
                    AssociatedObject.Focus();
                    AssociatedObject.SelectAll();
                }));
        }
    }
}
