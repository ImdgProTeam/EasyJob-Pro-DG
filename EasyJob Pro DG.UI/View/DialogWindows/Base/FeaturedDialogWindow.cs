using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Window with additional common functionality.
    /// </summary>
    public class FeaturedDialogWindow : Window
    {
        /// <summary>
        /// Method to DragMove the window while left mouse button pressed.
        /// MouseLeftButtonDown="Window_MouseLeftButtonDown" shall be defined in xaml.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }

        /// <summary>
        /// Reference method to define Close button Click event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
