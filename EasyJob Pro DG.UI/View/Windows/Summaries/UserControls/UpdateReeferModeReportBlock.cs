using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries.UserControls
{
    /// <summary>
    /// Interaction logic for UpdateReportBlock.xaml
    /// </summary>
    public partial class UpdateReeferModeReportBlock : UserControl
    {
        public UpdateReeferModeReportBlock()
        {
            InitializeComponent();
        }



        public string BlockLabel
        {
            get { return (string)GetValue(BlockLabelProperty); }
            set { SetValue(BlockLabelProperty, value); }
        }

        public static readonly DependencyProperty BlockLabelProperty =
            DependencyProperty.Register("BlockLabel", typeof(string), typeof(UpdateReeferModeReportBlock), new PropertyMetadata(""));


    }
}
