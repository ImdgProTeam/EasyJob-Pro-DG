using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CustomControlLib
{

    [TemplatePart(Name = buttonClear, Type = typeof(Button))]
    [TemplatePart(Name = searchTextBox, Type = typeof(TextBox))]
    public class SearchBox : Control
    {
        private const string buttonClear = "PART_buttonClear";
        private const string searchTextBox = "PART_SearchTextBox";

        static SearchBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchBox), new FrameworkPropertyMetadata(typeof(SearchBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var txbSearch = GetTemplateChild(searchTextBox) as TextBox;
            ButtonClear = GetTemplateChild(buttonClear) as Button;
            this.GotFocus += SearchBox_GotFocus;
            this.KeyUp += SearchBox_KeyUp;
        }



        #region Button
        Button btnClear;
        protected Button ButtonClear
        {
            get { return btnClear; }
            set
            {
                if (btnClear != null)
                {
                    btnClear.Click -= new RoutedEventHandler(btnClear_Click);
                }

                btnClear = value;

                if (btnClear != null)
                {
                    btnClear.Click += new RoutedEventHandler(btnClear_Click);
                }

                SetButtonEnabled();
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            OnButtonClick();
        }


        #endregion

        #region Text Property
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SearchBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextPropertyChanged)));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion


        #region Methods
        private void OnButtonClick()
        {
            Text = string.Empty;
            SetButtonEnabled();
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            SetButtonEnabled();
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var txbSearch = GetTemplateChild(searchTextBox) as TextBox;
            if (txbSearch is null) return;
            txbSearch.Focus();
            txbSearch.SelectAll();
        }

        private void SetButtonEnabled()
        {
            if (ButtonClear is null) return;
            ButtonClear.IsEnabled = !string.IsNullOrEmpty(Text);
        } 
        #endregion
    }
}
