using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для TabDimensions.xaml
    /// </summary>
    public partial class TabDimensions : UserControl
    {
        public TabDimensions()
        {
            InitializeComponent();
        }

        //private void SetUpAccommodationTable()
        //{
        //    int i = 1;
        //    foreach (var entry in tempShip.AccommodationBays)
        //    {
        //        TextBlock txbNumber = new TextBlock();
        //        txbNumber.Text = "Accommodation " + i;
        //        txbNumber.VerticalAlignment = VerticalAlignment.Bottom;
        //        Grid.SetColumn(txbNumber, 0);
        //        Grid.SetRow(txbNumber, i);

        //        TextBox txbBayNumber = new TextBox();
        //        //Binding bind = new Binding("AccommodationBays")
        //        txbBayNumber.Text = entry.ToString();
        //        txbBayNumber.Tag = "Enter the last bay before accommodation";
        //        txbBayNumber.VerticalAlignment = VerticalAlignment.Bottom;

        //        Grid.SetColumn(txbBayNumber, 1);
        //        Grid.SetRow(txbBayNumber, i);

        //        if (gridAccommodations.RowDefinitions.Count <= i)
        //        {
        //            RowDefinition row = new RowDefinition();
        //            row.Height = new GridLength(20);
        //            gridAccommodations.RowDefinitions.Add(row);
        //        }

        //        gridAccommodations.Children.Add(txbNumber);
        //        gridAccommodations.Children.Add(txbBayNumber);

        //        i++;
        //    }
        //}
    }
}
