using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OpenPOS.Controls
{
    public class Keypad : Control
    {
        #region IncrementCommand

        public static readonly DependencyProperty IncrementCommandProperty =
            DependencyProperty.Register("IncrementCommand",
                typeof(ICommand), typeof(Keypad));

        public ICommand IncrementCommand
        {
            get { return (ICommand)GetValue(IncrementCommandProperty); }
            set { SetValue(IncrementCommandProperty, value); }
        }

        public static readonly DependencyProperty IncrementCommandParameterProperty =
            DependencyProperty.Register("IncrementCommandParameter",
                typeof(object), typeof(Keypad));

        public object IncrementCommandParameter
        {
            get { return (object)GetValue(IncrementCommandParameterProperty); }
            set { SetValue(IncrementCommandParameterProperty, value); }
        }

        #endregion

        #region DecrementCommand

        public static readonly DependencyProperty DecrementCommandProperty =
            DependencyProperty.Register("DecrementCommand",
                typeof(ICommand), typeof(Keypad));

        public ICommand DecrementCommand
        {
            get { return (ICommand)GetValue(DecrementCommandProperty); }
            set { SetValue(DecrementCommandProperty, value); }
        }

        public static readonly DependencyProperty DecrementCommandParameterProperty =
            DependencyProperty.Register("DecrementCommandParameter",
                typeof(object), typeof(Keypad));

        public object DecrementCommandParameter
        {
            get { return (object)GetValue(DecrementCommandParameterProperty); }
            set { SetValue(DecrementCommandParameterProperty, value); }
        }

        #endregion

        #region ProcessCommand

        public static readonly DependencyProperty ProcessCommandProperty =
            DependencyProperty.Register("ProcessCommand",
                typeof(ICommand), typeof(Keypad));

        public ICommand ProcessCommand
        {
            get { return (ICommand)GetValue(ProcessCommandProperty); }
            set { SetValue(ProcessCommandProperty, value); }
        }

        public static readonly DependencyProperty ProcessCommandParameterProperty =
            DependencyProperty.Register("ProcessCommandParameter",
                typeof(object), typeof(Keypad));

        public object ProcessCommandParameter
        {
            get { return (object)GetValue(ProcessCommandParameterProperty); }
            set { SetValue(ProcessCommandParameterProperty, value); }
        }

        #endregion

        #region BarcodeCommand

        public static readonly DependencyProperty BarcodeCommandProperty =
            DependencyProperty.Register("BarcodeCommand",
                typeof(ICommand), typeof(Keypad));

        public ICommand BarcodeCommand
        {
            get { return (ICommand)GetValue(BarcodeCommandProperty); }
            set { SetValue(BarcodeCommandProperty, value); }
        }

        public static readonly DependencyProperty BarcodeCommandParameterProperty =
            DependencyProperty.Register("BarcodeCommandParameter",
                typeof(object), typeof(Keypad));

        public object BarcodeCommandParameter
        {
            get { return (object)GetValue(BarcodeCommandParameterProperty); }
            set { SetValue(BarcodeCommandParameterProperty, value); }
        }

        #endregion

        static Keypad()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Keypad), new FrameworkPropertyMetadata(typeof(Keypad)));
        }

        public Keypad()
        {
            //Application.Current.MainWindow.PreviewKeyDown += WindowPreviewKeyDown;
        }

        private Canvas keypadLayoutRoot;
        private TextBox barcodeTextBox;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            keypadLayoutRoot = (Canvas)GetTemplateChild("PART_KEYPAD");
            barcodeTextBox = (TextBox)GetTemplateChild("PART_BARCODE");

            CreateKaypadButtons();
        }

        Button CreateButton(int x, int y, int spanX, int spanY, string content, RoutedEventHandler handler)
        {
            double width = keypadLayoutRoot.Width;
            double height = keypadLayoutRoot.Height;

            double buttonWidth = width / 4;
            double buttonHeight = height / 5;

            Button btn = new Button();
            btn.Content = "?";
            btn.Margin = new Thickness(2);

            btn.Width = buttonWidth * spanX - 4;
            btn.Height = buttonHeight * spanY - 4;

            double left = x * buttonWidth;
            double top = y * buttonHeight;

            btn.SetValue(Canvas.LeftProperty, left);
            btn.SetValue(Canvas.TopProperty, top);

            btn.Content = content;

            btn.Click += handler;

            return btn;
        }

        void KeypadNumButtonClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            barcodeTextBox.Text += (string)btn.Content;
        }

        void KeypadCEButtonClick(object sender, RoutedEventArgs e)
        {
            barcodeTextBox.Text = string.Empty;
        }

        void CreateKaypadButtons()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    int value = ((y * 3) + x) + 1;
                    keypadLayoutRoot.Children.Add(CreateButton(x, y + 1, 1, 1, value.ToString(), new RoutedEventHandler(KeypadNumButtonClick)));
                }
            }

            keypadLayoutRoot.Children.Add(CreateButton(0, 0, 3, 1, "CE", KeypadCEButtonClick));
            keypadLayoutRoot.Children.Add(CreateButton(0, 4, 3, 1, "0", KeypadNumButtonClick));
        }

        #region Keypad hack to get all the keys

        private void WindowPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string key = e.Key.ToString();
            if (key.StartsWith("D"))
            {
                barcodeTextBox.Text += key.Remove(0, 1);
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (BarcodeCommand != null)
                    BarcodeCommand.Execute(barcodeTextBox);
                e.Handled = true;
            }
            else if (e.Key == Key.OemPlus)
            {
                if (IncrementCommand != null)
                    IncrementCommand.Execute(IncrementCommandParameter);
                e.Handled = true;
            }
            else if (e.Key == Key.OemMinus)
            {
                if (DecrementCommand != null)
                    DecrementCommand.Execute(DecrementCommandParameter);
                e.Handled = true;
            }
        }

        #endregion
    }
}
