using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace Wpf_Calculator
{
    /// <summary>
    /// class for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Button> ButtonList;
        public MainWindow()
        {

            InitializeComponent();

            ButtonList = new List<Button>
            {
                button1,
                Button2// Replace Button1 with the name of your button in XAM //Replace Button2 with the name of your button in XAML
                // Add references to all your buttons here
            };
        }

        /// <summary>
        /// Clicking on numeric buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button btnObject = (Button)sender;
            TaskHandlerClass.UserNumberInputHandler(btnObject.Content.ToString());
            UpdateUI();
        }

        /// <summary>
        /// KeyBoard Input Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown_1(object sender, KeyEventArgs e)
        {
            int digit = 0;
            //Handles input with keyboard modifiers
            if (Keyboard.Modifiers == ModifierKeys.Shift && e.Key != Key.None)
            {
                switch (e.Key)
                {
                    case Key.D8:
                        TaskHandlerClass.KeyboardInput(Key.Multiply);
                        break;
                    case Key.OemPlus:
                        TaskHandlerClass.KeyboardInput(Key.Add);
                        break;
                }
            }
            //Handles number inputs
            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                digit = e.Key - Key.D0;
                TaskHandlerClass.UserNumberInputHandler(digit.ToString());
            }
            //Handles Numpad inputs
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                digit = e.Key - Key.NumPad0;
                TaskHandlerClass.UserNumberInputHandler(digit.ToString());
            }
            //Handles other keyboard inputs
            else
            {
                Key keyValue = e.Key;
                TaskHandlerClass.KeyboardInput(keyValue);
            }
            UpdateUI();
        }

        /// <summary>
        /// Handles Operator button 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Operand_Click(object sender, RoutedEventArgs e)
        {
            Button btnObject = (Button)sender;
            TaskHandlerClass.OperatorHandler(btnObject.Content.ToString());
            UpdateUI();
        }

        /// <summary>
        /// Handle Clear button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAllData_Click(object sender, RoutedEventArgs e)
        {
            TaskHandlerClass.ClearAllDataHandler();
            UpdateUI();
        }

        /// <summary>
        /// Handling the calculation (equal) button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            TaskHandlerClass.CalculateHandler();
            UpdateUI();
        }

        /// <summary>
        /// Performing a backspace operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            TaskHandlerClass.BackSpaceHandler();
            MainDisplay.Text = TaskHandlerClass.DisplayData();
        }

        /// <summary>
        /// Updates the user interface with current data and history.
        /// </summary>
        private void UpdateUI()
        {
            MainDisplay.Text = TaskHandlerClass.DisplayData();
            HistoryLabel.Content = TaskHandlerClass.DisplayHistory();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ////HandleKeyPressColorEffect(e.Key);
            Key keyPressed = e.Key;
            SetButtonBackgroundColor(keyPressed, Brushes.Gray);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Key keyReleased = e.Key;

            // Find the corresponding button and revert its background color
            SetButtonBackgroundColor(keyReleased, Brushes.Blue);
        }

        private void SetButtonBackgroundColor(Key keyReleased, SolidColorBrush transparent)
        {
            foreach (var child in MainGrid.Children)
            {
                if (child is Button button)
                {
                    if (button.Tag != null && button.Tag.ToString() == keyReleased.ToString())
                    {
                        button.Background = transparent;
                        return; // Exit loop once color is set
                    }
                }
            }
        }
    }
}