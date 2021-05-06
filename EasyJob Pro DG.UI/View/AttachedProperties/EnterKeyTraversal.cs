using System.Windows;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.AttachedProperties
{
    public class EnterKeyTraversal
    {
        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(EnterKeyTraversal),
                new UIPropertyMetadata(false, IsEnabledChanged));

        private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ue = d as UIElement;
            if (ue == null) return;

            if ((bool)e.NewValue)
            {
                ue.PreviewKeyDown += Keydown;
            }
            else
            {
                ue.PreviewKeyDown -= Keydown;
            }
        }

        private static void Keydown(object sender, KeyEventArgs e)
        {
            if (!e.Key.Equals(Key.Enter)) return;
            if (sender is UIElement ue)
                ue.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
