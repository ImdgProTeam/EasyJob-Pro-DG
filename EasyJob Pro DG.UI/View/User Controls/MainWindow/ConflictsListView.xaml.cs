using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для ListBoxConflicts.xaml
    /// </summary>
    public partial class ConflictsListView : AnimatedUserControl
    {
        
        //Brushes
        Brush _original;
        private Brush uncheckedColor = Brushes.Red;
        private Brush checkedColor = Brushes.GreenYellow;
        readonly Brush _hover;
        
        //Booleans
        bool lockChange;


        public ConflictsListView() 
            : base(Animations.AnimationTypes.SlideAndFadeInFromRight)
        {
            InitializeComponent();
            _hover = (Brush)this.FindResource("Brush.Text.MouseOver");

        }


        //Methods to describe color change on mouse entering / leaving an item
        private void PaintWhenOver(object sender)
        {
            if (lockChange) return;
            var expander = sender as Expander;

            if (expander?.Header is StackPanel panel)
                foreach (var element in panel.Children)
                {
                    if (element.GetType() == typeof(TextBlock))
                    {
                        TextBlock txb = element as TextBlock;
                        _original = txb?.Foreground;
                        if (txb != null) txb.Foreground = _hover;
                    }
                }

            lockChange = true;
        }
        private void RestoreColors(object sender)
        {
            var expander = sender as Expander;
            if (expander?.Header is StackPanel panel)
                foreach (var element in panel.Children)
                {
                    if (element.GetType() == typeof(TextBlock))
                    {
                        if (element is TextBlock txb) txb.Foreground = _original;
                    }
                }

            lockChange = false;
        }
        private void Expander_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            PaintWhenOver(sender);
        }
        private void Expander_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            RestoreColors(sender);
        }

        /// <summary>
        /// Method to change checkbox ellipse color
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ellipse_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Ellipse ellipse = sender as Ellipse;

                if (ellipse == null) return;
                ellipse.Fill = ellipse.Fill == uncheckedColor ? checkedColor : uncheckedColor;

            }
        }


        /// <summary>
        /// Method to collapse conflict description on click on header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeaderTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DockPanel panel = null;
            TextBlock txbText = null;

            if (sender is TextBlock txbSender)
            {
                panel = txbSender.Parent as DockPanel;
            }

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (panel != null)
                {

                    txbText = panel.Children[panel.Children.Count - 1] as TextBlock;

                    if (txbText != null) txbText.Visibility = 
                        txbText.Visibility == Visibility.Visible 
                            ? Visibility.Collapsed 
                            : Visibility.Visible;
                }
            }
        }

    }
}
