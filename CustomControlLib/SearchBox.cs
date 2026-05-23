using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CustomControlLib
{

    [TemplatePart(Name = buttonClear, Type = typeof(Button))]
    [TemplatePart(Name = buttonSettings, Type = typeof(Button))]
    [TemplatePart(Name = searchTextBox, Type = typeof(TextBox))]
    public class SearchBox : Control
    {
        private const string buttonClear = "PART_buttonClear";
        private const string buttonSettings = "PART_buttonSettings";
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
            ButtonSettings = GetTemplateChild(buttonSettings) as Button;


            this.GotFocus -= SearchBox_GotFocus;
            this.GotFocus += SearchBox_GotFocus;
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

        Button btnSettings;
        protected Button ButtonSettings
        {
            get { return btnSettings; }
            set
            {
                if (btnSettings != null)
                {
                    btnSettings.Click -= new RoutedEventHandler(btnSettings_Click);
                }

                btnSettings = value;

                if (btnSettings != null)
                {
                    btnSettings.Click += new RoutedEventHandler(btnSettings_Click);
                }
            }
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                button.ContextMenu.IsOpen = true;
            }
        }


        #endregion

        #region Text Property
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(SearchBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextPropertyChanged)));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchBox searchBox = (SearchBox)d;
            if (searchBox is null) return;

            searchBox.SetButtonEnabled();
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                SetValue(TextProperty, value);
            }
        }
        #endregion

        #region Settings button Properties

        public string SettingsButtonIcon
        {
            get { return (string)GetValue(SettingsButtonIconProperty); }
            set { SetValue(SettingsButtonIconProperty, value); }
        }

        public static readonly DependencyProperty SettingsButtonIconProperty =
            DependencyProperty.Register(nameof(SettingsButtonIcon), typeof(string), typeof(SearchBox), new PropertyMetadata("o"));


        public FontFamily SettingsButtonFontFamily
        {
            get { return (FontFamily)GetValue(SettingsButtonFontFamilyProperty); }
            set { SetValue(SettingsButtonFontFamilyProperty, value); }
        }

        public static readonly DependencyProperty SettingsButtonFontFamilyProperty =
            DependencyProperty.Register(nameof(SettingsButtonFontFamily), typeof(FontFamily), typeof(SearchBox), new PropertyMetadata(new FontFamily("Segoe UI")));

        #endregion

        #region Settings menu

        public IEnumerable SettingsMenuItemsSource
        {
            get { return (IEnumerable)GetValue(SettingsMenuItemsSourceProperty); }
            set { SetValue(SettingsMenuItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty SettingsMenuItemsSourceProperty =
            DependencyProperty.Register(nameof(SettingsMenuItemsSource), typeof(IEnumerable), typeof(SearchBox), new PropertyMetadata(null));

        #endregion

        #region Methods
        private void OnButtonClick()
        {
            ClearText();
        }

        private void ClearText()
        {
            Text = string.Empty;
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

        #region Override methods

        //protected override void OnKeyDown(KeyEventArgs e)
        //{
        //    base.OnKeyDown(e);

        //    //Clears text box on press Escape
        //    if (e.Key == Key.Escape)
        //    {
        //        ClearText();
        //        e.Handled = true;
        //    }
        //} 

        #endregion
    }
}
