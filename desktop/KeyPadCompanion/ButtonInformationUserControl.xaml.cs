using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

namespace KeyPadCompanion
{
    /// <summary>
    /// Interaction logic for ButtonInformationUserControl.xaml
    /// </summary>
    public partial class ButtonInformationUserControl : UserControl
    {

        private int buttonIndex = 0;

        public ButtonInformationUserControl()
        {
            InitializeComponent();
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
            var singleClickAction = Configuration.Instance.ButtonActions[buttonIndex].GetValue(ButtonType.SinglePress);
            SingleClickLabel.Content = $"Single click: {GetActionName(singleClickAction)}";

            var doubleClickAction = Configuration.Instance.ButtonActions[buttonIndex].GetValue(ButtonType.DoublePress);
            DoubleClickLabel.Content = $"Double click: {GetActionName(doubleClickAction)}";

            var longClickAction = Configuration.Instance.ButtonActions[buttonIndex].GetValue(ButtonType.LongPress);
            LongClickLabel.Content = $"Long click: {GetActionName(longClickAction)}";
        }

        private void SingleClickConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActionsWindow(buttonIndex, ButtonType.SinglePress);
            window.ShowDialog();
            LoadActions();
        }

        private void DoubleClickConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActionsWindow(buttonIndex, ButtonType.DoublePress);
            window.ShowDialog();
            LoadActions();
        }

        private void LongClickConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ActionsWindow(buttonIndex, ButtonType.LongPress);
            window.ShowDialog();
            LoadActions();
        }

        private void ColorRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("TODO");
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
