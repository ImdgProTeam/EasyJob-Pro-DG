using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace EasyJob_ProDG.UI.Behaviors
{
    public class SelectTextOnFocusBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += (o, e) =>
            {
                Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    AssociatedObject.SelectAll();
                }));
            };
        }

    }
}
