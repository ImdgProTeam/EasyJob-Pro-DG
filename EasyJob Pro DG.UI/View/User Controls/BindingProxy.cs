using System.Windows;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Proxy object is used to store <see cref="Window"/> or <see cref="User_Controls"/> DataContext 
    /// for further use in xaml after the scope of context is changed by another element Binding.
    /// Creaty a <see cref="BindingProxy"/> object as a StaticResource, store DataContext in Data property and access it further via object key.
    /// </summary>
    public class BindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = 
            DependencyProperty.Register("Data", typeof(object), typeof(BindingProxy), new UIPropertyMetadata(null));
    }
}
