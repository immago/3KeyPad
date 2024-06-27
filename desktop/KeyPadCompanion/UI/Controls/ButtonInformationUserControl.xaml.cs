using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using KeyPadCompanion.Data.Model;
using KeyPadCompanion.UI.Windows;

namespace KeyPadCompanion.UI.Controls
{
    /// <summary>
    /// Interaction logic for ButtonInformationUserControl.xaml
    /// </summary>
    public partial class ButtonInformationUserControl : UserControl
    {

        private int buttonIndex = 0;
        private Color color = Color.FromRgb(0,0,0);
        private int speed = 0;
        private int mode = 0;

        public ButtonInformationUserControl()
        {
            InitializeComponent();
        }

        public void SetLed(Color color, int speed, int mode)
        {
            this.color = color;
            this.speed = speed;
            this.mode = mode;

            LedSpeedLabel.Content = $"S:{speed}";
            LedModeLabel.Content = $"M:{mode}";
            ColorRectangle.Fill = new SolidColorBrush(color);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            string tagString = Tag.ToString() ?? "0";
            buttonIndex = Int32.Parse(tagString);
            LoadActions();
        }

        private void LoadActions()
        {
            var singleClickAction = Configuration.Instance.ButtonActions[buttonIndex].GetValue(ButtonEventType.SinglePress);
            SingleClickLabel.Content = $"Single click: {GetActionName(singleClickAction)}";

            var doubleClickAction = Configuration.Instance.ButtonActions[buttonIndex].GetValue(ButtonEventType.DoublePress);
            DoubleClickLabel.Content = $"Double click: {GetActionName(doubleClickAction)}";

            var longClickAction = Configuration.Instance.ButtonActions[buttonIndex].GetValue(ButtonEventType.LongPress);
            LongClickLabel.Content = $"Long click: {GetActionName(longClickAction)}";
        }

        private void SingleClickConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActionsWindow(buttonIndex, ButtonEventType.SinglePress);
            window.ShowDialog();
            LoadActions();
        }

        private void DoubleClickConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActionsWindow(buttonIndex, ButtonEventType.DoublePress);
            window.ShowDialog();
            LoadActions();
        }

        private void LongClickConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActionsWindow(buttonIndex, ButtonEventType.LongPress);
            window.ShowDialog();
            LoadActions();
        }

        private void ColorRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            /*
            var window = new LedConfigurationWindow(color, speed, mode);
            window.ShowDialog();

            Debug.WriteLine("TODO: Save LED settings");
            Debug.WriteLine(window.Color);
            Debug.WriteLine(window.Speed);
            Debug.WriteLine(window.Mode);
            */
            var window = new ButtonLedConfigurationWindow(buttonIndex);
            window.ShowDialog();
            
        }


        private string GetActionName(Actions action)
        {
            switch (action)
            {
                case Actions.None:
                    return "None";
                case Actions.SwitchAudioInput:
                    return "Mic switch";
                case Actions.SwitchAudioOutput:
                    return "Audio switch";
                case Actions.MuteMicrophone:
                    return "Mute";
                case Actions.EmulateKeyboard:
                    return "Key press";
                default:
                    return "";
            }
        }
    }
}
