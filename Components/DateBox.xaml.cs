using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyApp.Components
{
    public partial class DateBox : UserControl
    {
        public DateBox()
        {
            InitializeComponent();
            _segments.Add(DaySegment);
            _segments.Add(MonthSegment);
            _segments.Add(YearSegment);
        }

        private static readonly List<Key> DigitKeys = new List<Key> { Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9,
                                                                        Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, 
                                                                        Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9 };
        private static readonly List<Key> MoveForwardKeys = new List<Key> { Key.Right };
        private static readonly List<Key> MoveBackwardKeys = new List<Key> { Key.Left };
        private static readonly List<Key> OtherAllowedKeys = new List<Key> { Key.Tab, Key.Delete };

        private readonly List<TextBox> _segments = new List<TextBox>();

        private bool _suppressDateUpdate = false;

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            "Date", typeof (string), typeof (DateBox), new FrameworkPropertyMetadata(default(string), DateChanged)
            {
                BindsTwoWayByDefault = true
            });

        private static void DateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var dateBox = dependencyObject as DateBox;
            var text = e.NewValue as string;

            if (text != null && dateBox != null)
            {
                dateBox._suppressDateUpdate = true;
                var i = 0;
                foreach (var segment in text.Split('-'))
                {
                    dateBox._segments[i].Text = segment;
                    i++;
                }
                dateBox._suppressDateUpdate = false;
            }
        }

        public string Date
        {
            get { return (string) GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        private void UIElement_OnPreviewKeyDown(object sender, KeyEventArgs e)
       {
            if (DigitKeys.Contains(e.Key))
            {
                e.Handled = ShouldCancelDigitKeyPress();
                HandleDigitPress();
            }
            else if(MoveBackwardKeys.Contains(e.Key))
            {
                e.Handled = ShouldCancelBackwardKeyPress();
                HandleBackwardKeyPress();
            }
            else if (MoveForwardKeys.Contains(e.Key))
            {
                e.Handled = ShouldCancelForwardKeyPress();
                HandleForwardKeyPress();
            } 
            else if (e.Key == Key.Back)
            {
                HandleBackspaceKeyPress();
            }
            else if (e.Key == Key.Tab)
            {
                e.Handled = true;
                HandleTabKeyPress();
            }
            else
            {
                e.Handled = !AreOtherAllowedKeysPressed(e);
            }
        }

        private bool AreOtherAllowedKeysPressed(KeyEventArgs e)
        {
            return e.Key == Key.C && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   e.Key == Key.V && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   e.Key == Key.A && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   e.Key == Key.X && ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) != 0) ||
                   OtherAllowedKeys.Contains(e.Key);
        }

        private void HandleDigitPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox!= null && currentTextBox.Text.Length == 4 &&
                currentTextBox.CaretIndex == 4 && currentTextBox.SelectedText.Length == 0)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
            else if (currentTextBox != null && !ReferenceEquals(currentTextBox, YearSegment) && currentTextBox.Text.Length == 2 &&
                currentTextBox.CaretIndex == 2 && currentTextBox.SelectedText.Length == 0)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private bool ShouldCancelDigitKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null &&
                   currentTextBox.Text.Length == 4 &&
                   currentTextBox.CaretIndex == 4 &&
                   currentTextBox.SelectedText.Length == 0)
            {
                return true;
            }
            else if (currentTextBox != null &&
                   !ReferenceEquals(currentTextBox, YearSegment) &&
                   currentTextBox.Text.Length == 2 &&
                   currentTextBox.CaretIndex == 2 &&
                   currentTextBox.SelectedText.Length == 0)
            {
                return true;
            }
            else return false;
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_suppressDateUpdate)
            {
                Date = string.Format("{0}-{1}-{2}", DaySegment.Text, MonthSegment.Text, YearSegment.Text);
            }

            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.Text.Length == 4 && currentTextBox.CaretIndex == 4)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
            else if (currentTextBox != null && !ReferenceEquals(currentTextBox, YearSegment) && currentTextBox.Text.Length == 2 && currentTextBox.CaretIndex == 2)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private bool ShouldCancelBackwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;
            return currentTextBox != null && currentTextBox.CaretIndex == 0;
        }

        private void HandleBackspaceKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.CaretIndex == 0 && currentTextBox.SelectedText.Length == 0)
            {
                MoveFocusToPreviousSegment(currentTextBox);
            }
        }

        private void HandleBackwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.CaretIndex == 0)
            {
                MoveFocusToPreviousSegment(currentTextBox);
            }
        }

        private bool ShouldCancelForwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;
            return (
                currentTextBox != null && currentTextBox.CaretIndex == 2 && !ReferenceEquals(currentTextBox, YearSegment) ||
                currentTextBox != null && currentTextBox.CaretIndex == 4 && ReferenceEquals(currentTextBox, YearSegment)
                );
        }

        private void HandleForwardKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null && currentTextBox.CaretIndex == currentTextBox.Text.Length)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private void HandleTabKeyPress()
        {
            var currentTextBox = FocusManager.GetFocusedElement(this) as TextBox;

            if (currentTextBox != null)
            {
                MoveFocusToNextSegment(currentTextBox);
            }
        }

        private void MoveFocusToPreviousSegment(TextBox currentTextBox)
        {
            if (!ReferenceEquals(currentTextBox, DaySegment))
            {
                var previousSegmentIndex = _segments.FindIndex(box => ReferenceEquals(box, currentTextBox)) - 1;
                currentTextBox.SelectionLength = 0;
                currentTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                _segments[previousSegmentIndex].CaretIndex = _segments[previousSegmentIndex].Text.Length;
            }
        }

        private void MoveFocusToNextSegment(TextBox currentTextBox)
        {
            /*if (!ReferenceEquals(currentTextBox, YearSegment))
            {
                currentTextBox.SelectionLength = 0;
                currentTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }*/
            currentTextBox.SelectionLength = 0;
            currentTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
    }
}
