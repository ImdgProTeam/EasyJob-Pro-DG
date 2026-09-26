
// Template is in Themes/Generic.xaml

using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.WindowBase;
using System.Windows;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.Windows
{
    public class ToolWindowTemplate : AnimatedToolWindow
    {
        public static readonly DependencyProperty CaptureTextProperty =
        DependencyProperty.Register(nameof(CaptureText), typeof(string), typeof(ToolWindowTemplate),
            new PropertyMetadata("Capture text"));

        public static readonly DependencyProperty ApplyButtonTextProperty =
        DependencyProperty.Register(nameof(ApplyButtonText), typeof(string), typeof(ToolWindowTemplate),
            new PropertyMetadata("Apply"));

        public static readonly DependencyProperty ApplyCommandProperty =
            DependencyProperty.Register(nameof(ApplyCommand), typeof(ICommand), typeof(ToolWindowTemplate));
        public static readonly DependencyProperty OnCloseButtonPressedCommandProperty =
            DependencyProperty.Register(nameof(OnCloseButtonPressedCommand), typeof(ICommand), typeof(ToolWindowTemplate));
        public static readonly DependencyProperty WindowCloseCommandProperty =
            DependencyProperty.Register(nameof(WindowCloseCommand), typeof(ICommand), typeof(ToolWindowTemplate));


        public string CaptureText
        {
            get => (string)GetValue(CaptureTextProperty);
            set => SetValue(CaptureTextProperty, value);
        }

        public string ApplyButtonText
        {
            get => (string)GetValue(ApplyButtonTextProperty);
            set => SetValue(ApplyButtonTextProperty, value);
        }

        public ICommand ApplyCommand
        {
            get => (ICommand)GetValue(ApplyCommandProperty);
            set => SetValue(ApplyCommandProperty, value);
        }
        public ICommand OnCloseButtonPressedCommand
        {
            get => (ICommand)GetValue(OnCloseButtonPressedCommandProperty);
            set => SetValue(OnCloseButtonPressedCommandProperty, value);
        }
        public ICommand WindowCloseCommand
        {
            get => (ICommand)GetValue(WindowCloseCommandProperty);
            set => SetValue(WindowCloseCommandProperty, value);
        }


        // Static constructor for application of template.
        static ToolWindowTemplate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolWindowTemplate),
                new FrameworkPropertyMetadata(typeof(ToolWindowTemplate)));
        }

        public ToolWindowTemplate()
        {
            OnCloseButtonPressedCommand = new DelegateCommand(OnClose);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetDragMove();
        }

        /// <summary>
        /// Method finds element named PART_DragBody and registers MouseLeftButtonDown method from base class <see cref="FeaturedWindow"/>
        /// </summary>
        private void SetDragMove()
        {
            var titleBar = GetTemplateChild("PART_DragBody") as UIElement;
            if (titleBar != null)
            {
                titleBar.MouseLeftButtonDown += (s, e) => Window_MouseLeftButtonDown(s, e);
            }
        }

        /// <summary>
        /// Calls CloseWindow method from base class <see cref="FeaturedWindow"/>
        /// </summary>
        /// <param name="obj"></param>
        private void OnClose(object obj)
        {
            WindowCloseCommand?.Execute(null);
            CloseWindow(this, null);
        }

    }
}
