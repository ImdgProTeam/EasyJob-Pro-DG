using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace CustomControlLib
{
    /// <summary>
    /// ComboBox with TexBox in front of it to change values in the combobox list (items).
    /// Bind ItemsSourse to an ObservableCollection
    /// </summary>
    public class ChangeableComboBox : Control
    {
        static ChangeableComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChangeableComboBox), new FrameworkPropertyMetadata(typeof(ChangeableComboBox)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }


        #region Text Property
        public static readonly DependencyProperty TextValueProperty = DependencyProperty.Register("TextValue", typeof(string), typeof(ChangeableComboBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextValuePropertyChanged)));

        private static void OnTextValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChangeableComboBox comboBox = (ChangeableComboBox)d;
            if (comboBox is null) return;
        }

        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set
            {
                SetValue(TextValueProperty, value);
            }
        }
        #endregion

        #region ItemsSource Property
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ChangeableComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        private static void OnItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChangeableComboBox comboBox = (ChangeableComboBox)d;
            if (comboBox is null) return;
        }

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        #endregion

        #region SelectedIndex Property
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(ChangeableComboBox),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedIndexPropertyChanged)));

        private static void OnSelectedIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChangeableComboBox comboBox = (ChangeableComboBox)d;
            if (comboBox is null) return;
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        #endregion

        #region SelectedItem Property
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(ChangeableComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSelectedItemPropertyChanged)));

        private static void OnSelectedItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChangeableComboBox comboBox = (ChangeableComboBox)d;
            if (comboBox is null) return;
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        #endregion

        #region DisplayMemberPath Property
        public static readonly DependencyProperty DisplayMemberPathProperty = DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(ChangeableComboBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnDisplayMemberPathPropertyChanged)));

        private static void OnDisplayMemberPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChangeableComboBox comboBox = (ChangeableComboBox)d;
            if (comboBox is null) return;
        }

        public string DisplayMemberPath
        {
            get { return (string)GetValue(DisplayMemberPathProperty); }
            set
            {
                SetValue(SelectedIndexProperty, value);
            }
        }
        #endregion
    }
}
