using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для AddContainerGrid.xaml
    /// </summary>
    public partial class AddContainerToDataGrid : UserControl
    {
        public AddContainerToDataGrid()
        {
            InitializeComponent();
        }

        #region Dependency Properties
        /// <summary>
        /// Sets label text in the right part of the control
        /// </summary>
        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title", typeof(string), typeof(AddContainerToDataGrid));

        /// <summary>
        /// Property to choose between adding a Dg or just a Container
        /// </summary>
        public bool IsAddDg
        {
            get { return (bool)this.GetValue(IsAddDgProperty); }
            set { SetValue(IsAddDgProperty, value); }
        }

        public static readonly DependencyProperty IsAddDgProperty = DependencyProperty.Register(
            "IsAddDg", typeof(bool), typeof(AddContainerToDataGrid));

        public string ContainerNumber
        {
            get { return (string)this.GetValue(ContainerNumberProperty); }
            set { SetValue(ContainerNumberProperty, value); }
        }
        public static readonly DependencyProperty ContainerNumberProperty = DependencyProperty.Register(
            "ContainerNumber", typeof(string), typeof(AddContainerToDataGrid), new PropertyMetadata(null));

        public ushort UNNO
        {
            get { return (ushort)this.GetValue(UNNOProperty); }
            set { SetValue(UNNOProperty, value); }
        }
        public static readonly DependencyProperty UNNOProperty = DependencyProperty.Register(
            "UNNO", typeof(ushort), typeof(AddContainerToDataGrid), new PropertyMetadata((ushort)0));

        public string Location
        {
            get { return (string)this.GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }
        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register(
            "Location", typeof(string), typeof(AddContainerToDataGrid), new PropertyMetadata(null));


        /// <summary>
        /// Enables Add button
        /// </summary>
        public bool CanUserAdd
        {
            get { return (bool)this.GetValue(CanUserAddProperty); }
            set { this.SetValue(CanUserAddProperty, value); }
        }
        public static readonly DependencyProperty CanUserAddProperty =
            DependencyProperty.Register("CanUserAdd", typeof(bool), typeof(AddContainerToDataGrid), new PropertyMetadata(false));


        public static readonly RoutedEvent AddItemEvent
            = EventManager.RegisterRoutedEvent("AddItem",
                                                RoutingStrategy.Bubble,
                                                typeof(RoutedEventHandler),
                                                typeof(AddContainerToDataGrid)); 
        #endregion

        /// <summary>
        /// Routed Event to be Raised on press 'Add' button
        /// </summary>
        public event RoutedEventHandler AddItem
        {
            add { AddHandler(AddItemEvent, value); }
            remove { RemoveHandler(AddItemEvent, value); }
        }

        #region Controls events
        /// <summary>
        /// Closes UserControl on press 'x' Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// Raises Event on pressing 'Add' Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddItemEvent));
        }

        /// <summary>
        /// Handles TextInput limitations for ContainerNumber TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == "/r") return;
            if (!char.IsLetterOrDigit(e.Text[0])) e.Handled = true;

            var txb = sender as TextBox;
            var length = txb.Text.Replace(" ", "").Length;
            var selection = txb.SelectedText;
            if (length > 10 && string.IsNullOrEmpty(selection)) e.Handled = true;
        }

        /// <summary>
        /// Handles TextInput limitations for Location TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Location_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == "/r") return;
            if (!char.IsDigit(e.Text[0])) e.Handled = true;

            var txb = sender as TextBox;
            var length = txb.Text.Replace(" ", "").Length;
            var selection = txb.SelectedText;
            if (length > 6 && string.IsNullOrEmpty(selection)) e.Handled = true;
        }

        /// <summary>
        /// Handles TextInput limitations for UNNO TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UNNO_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (e.Text == "/r") return;
            if (!char.IsDigit(e.Text[0])) e.Handled = true;

            var txb = sender as TextBox;
            var selection = txb.SelectedText;
            var firstSymbol = txb.Text[0];
            var caretIndex = txb.CaretIndex;

            if (string.IsNullOrEmpty(selection) && (firstSymbol != '0' || caretIndex == 0)) e.Handled = true;
        }

        /// <summary>
        /// Handles spaces for UNNO TextBox - space will be ignorred
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UNNO_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        /// <summary>
        /// Selects all text in UNNO TextBox on GotFocus if it is not 0000
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txb = sender as TextBox;
            if (txb.Text != "0000") txb.SelectAll();
        }

        /// <summary>
        /// Returns cursor to the last symbol on every selection change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var txb = sender as TextBox;
            if (txb.SelectedText != "") return;
            if (txb.CaretIndex == txb.Text.Length) return;
            txb.CaretIndex = txb.Text.Length;
            e.Handled = true;
        } 
        #endregion
    }
}
