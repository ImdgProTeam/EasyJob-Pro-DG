using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace CustomControlLib
{
    [TemplatePart (Name =buttonLess, Type = typeof(RepeatButton))]
    [TemplatePart(Name = buttonMore, Type = typeof(RepeatButton))]
    public class NumericalTextBoxWithButtons : Control
    {
        private const string textBoxNumber = "PART_TextBoxNumber";
        private const string buttonLess = "PART_ButtonLess";
        private const string buttonMore = "PART_ButtonMore";
        private const sbyte minValue = sbyte.MinValue;
        private const sbyte maxValue = sbyte.MaxValue;
        private const sbyte defaultValue = 1;

        RepeatButton btnLess;
        protected RepeatButton ButtonLess
        {
            get { return btnLess; }
            set
            {
                if (btnLess != null)
                {
                    btnLess.Click -= new RoutedEventHandler(btnLess_Click);
                }

                btnLess = value;

                if (btnLess != null)
                {
                    btnLess.Click += new RoutedEventHandler(btnLess_Click);
                }

                SetButtonsEnabled();
            }
        }

        RepeatButton btnMore;
        protected RepeatButton ButtonMore
        {
            get { return btnMore; }
            set
            {
                if (btnMore != null)
                {
                    btnMore.Click -= new RoutedEventHandler(btnMore_Click);
                }

                btnMore = value;

                if (btnMore != null)
                {
                    btnMore.Click += new RoutedEventHandler(btnMore_Click);
                }

                SetButtonsEnabled();

            }
        }

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            IncreaseValue();
        }

        private void btnLess_Click(object sender, EventArgs e)
        {
            ReduceValue();
        }


        static NumericalTextBoxWithButtons()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericalTextBoxWithButtons), 
                new FrameworkPropertyMetadata(typeof(NumericalTextBoxWithButtons)));
        }


        // ------------------- Properties ----------------------------------------

        #region Text Property
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NumericalTextBoxWithButtons), 
            new FrameworkPropertyMetadata(defaultValue.ToString(), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnTextPropertyChanged)));

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericalTextBoxWithButtons control)
                control.OnTextPropertyChanged((string)e.OldValue, (string)e.NewValue);
        }

        protected virtual void OnTextPropertyChanged(string oldValue, string newValue)
        {
            sbyte parsedNewValue;
            bool isEnteredByteValue = sbyte.TryParse(newValue, out parsedNewValue);
            bool isNewValueAboveMaxValue = parsedNewValue > MaxValue;
            bool isNewValueBelowMinValue = parsedNewValue < MinValue;
            
            if(isEnteredByteValue)
            {
                if (isNewValueAboveMaxValue)
                {
                    parsedNewValue = MaxValue;
                    Text = parsedNewValue.ToString();
                }

                if (isNewValueBelowMinValue)
                {
                    parsedNewValue = MinValue;
                    Text = parsedNewValue.ToString();
                }

                SetButtonsEnabled(parsedNewValue);

                return;
            }
            Text = oldValue;
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region MaxMinValue Properties
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(sbyte), typeof(NumericalTextBoxWithButtons), 
            new FrameworkPropertyMetadata(maxValue));

        public sbyte MaxValue
        {
            get { return (sbyte)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(sbyte), typeof(NumericalTextBoxWithButtons), 
            new FrameworkPropertyMetadata(minValue));

        public sbyte MinValue
        {
            get { return (sbyte)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }
        #endregion

        public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register("Interval", typeof(int), typeof(NumericalTextBoxWithButtons),
            new FrameworkPropertyMetadata(400));
        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }


        // ------------------ Methods ------------------------------------------------
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var txbNumber = GetTemplateChild(textBoxNumber) as TextBox;
            ButtonLess = GetTemplateChild(buttonLess) as RepeatButton;
            ButtonMore = GetTemplateChild(buttonMore) as RepeatButton;
            this.GotFocus += NumericalTextBoxWithButtons_GotFocus;
            this.KeyDown += NumericalTextBoxWithButtons_KeyDown;
            this.KeyUp += NumericalTextBoxWithButtons_KeyUp;
            this.MouseWheel += NumericalTextBoxWithButtons_MouseWheel;
        }

        private void NumericalTextBoxWithButtons_KeyUp(object sender, KeyEventArgs e)
        {
            if (!char.IsDigit((char)e.Key)) return;
        }

        private void NumericalTextBoxWithButtons_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0) IncreaseValue();
            if (e.Delta < 0) ReduceValue();
        }

        private void NumericalTextBoxWithButtons_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Add) IncreaseValue();
            if (e.Key == System.Windows.Input.Key.Subtract) ReduceValue();
        }

        private void NumericalTextBoxWithButtons_GotFocus(object sender, RoutedEventArgs e)
        {
            var txbNumber = GetTemplateChild(textBoxNumber) as TextBox;
            txbNumber.Focus();
            //txbNumber.SelectAll();
        }

        private void IncreaseValue()
        {
            sbyte value;
            bool isValueInteger = sbyte.TryParse(Text, out value);

            if (isValueInteger) 
            {
                Text = (++value).ToString();
            }
        }

        private void ReduceValue()
        {
            sbyte value;
            bool isValueInteger = sbyte.TryParse(Text, out value);

            if (isValueInteger)
            {
                Text = (--value).ToString();
            }
        }

        private void SetButtonsEnabled(sbyte parsedNewValue)
        {
            if (ButtonLess == null || ButtonMore == null)
                return;
            if (parsedNewValue == MinValue)
                ButtonLess.IsEnabled = false;
            else
                ButtonLess.IsEnabled = true;

            if (parsedNewValue == MaxValue)
                ButtonMore.IsEnabled = false;
            else
                ButtonMore.IsEnabled = true;
        }

        private void SetButtonsEnabled()
        {
            sbyte parsedValue;
            bool isEnteredByteValue = sbyte.TryParse(Text, out parsedValue);

            if (isEnteredByteValue)
            {
                SetButtonsEnabled(parsedValue);
            }
        }


        // --------------- Events ------------------------------------------------------

        //public new static readonly RoutedEvent GotFocusEvent =
        //    EventManager.RegisterRoutedEvent("GotFocus", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumericalTextBoxWithButtons));

        //public new event RoutedEventHandler GotFocus
        //{
        //    add { AddHandler(GotFocusEvent, value); }
        //    remove { RemoveHandler(GotFocusEvent, value); }
        //}

        //protected virtual void RaiseGotFocusEvent()
        //{
        //    RoutedEventArgs args = new RoutedEventArgs(NumericalTextBoxWithButtons.GotFocusEvent);
        //    RaiseEvent(args);
        //}
    }
}
