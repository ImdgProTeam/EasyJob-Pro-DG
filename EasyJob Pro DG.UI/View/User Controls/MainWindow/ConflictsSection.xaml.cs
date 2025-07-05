using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Interaction logic for ConflictsSection.xaml
    /// </summary>
    public partial class ConflictsSection : UserControl
    {
        public ConflictsSection()
        {
            InitializeComponent();
        }


        #region Black block height

        public double BlackBlockHeight
        {
            get { return (double)GetValue(BlackBlockHeightProperty); }
            set { SetValue(BlackBlockHeightProperty, value); }
        }

        public static readonly DependencyProperty BlackBlockHeightProperty =
            DependencyProperty.Register("BlackBlockHeight", typeof(double), typeof(ConflictsSection), new PropertyMetadata(0.0d));


        #endregion
    }
}
