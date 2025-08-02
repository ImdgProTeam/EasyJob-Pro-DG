using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries.UserControls
{
    /// <summary>
    /// Interaction logic for UpdateReportBlock.xaml
    /// </summary>
    public partial class UpdateReportBlock : UserControl
    {
        public UpdateReportBlock()
        {
            InitializeComponent();
        }



        public string BlockLabel
        {
            get { return (string)GetValue(BlockLabelProperty); }
            set { SetValue(BlockLabelProperty, value); }
        }

        public static readonly DependencyProperty BlockLabelProperty =
            DependencyProperty.Register("BlockLabel", typeof(string), typeof(UpdateReportBlock), new PropertyMetadata(""));


    }
}
