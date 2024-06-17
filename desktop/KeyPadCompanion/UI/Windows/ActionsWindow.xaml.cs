using System.Windows;
using KeyPadCompanion.Data.Model;

namespace KeyPadCompanion.UI.Windows
{
    /// <summary>
    /// Interaction logic for ActionsWindow.xaml
    /// </summary>
    public partial class ActionsWindow : Window
    {
        private int buttonIndex = 0;
        private ButtonType buttonType = ButtonType.SinglePress;

        public ActionsWindow(int buttonIndex, ButtonType buttonType)
        {
            this.buttonIndex = buttonIndex;
            this.buttonType = buttonType;
            InitializeComponent();
        }

        private void SwitchAudioInputButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ButtonActions[buttonIndex].SetValue(Actions.SwitchAudioInput, buttonType);
            Configuration.Save();
            DialogResult = true;
            Close();
        }

        private void AudioInputsButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new AudioInputsWindow();
            window.Owner = this;
            window.ShowDialog();
        }

        private void MuteMicrophoneButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ButtonActions[buttonIndex].SetValue(Actions.MuteMicrophone, buttonType);
            Configuration.Save();
            DialogResult = true;
            Close();
        }

        private void EmulateKeyInputButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ButtonActions[buttonIndex].SetValue(Actions.EmulateKeyboard, buttonType);
            Configuration.Save();
            DialogResult = true;
            Close();
        }

        private void SwitchAudioOutputButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ButtonActions[buttonIndex].SetValue(Actions.SwitchAudioOutput, buttonType);
            Configuration.Save();
            DialogResult = true;
            Close();
        }

        private void AudioOutputsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NoneButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ButtonActions[buttonIndex].SetValue(Actions.None, buttonType);
            Configuration.Save();
            DialogResult = true;
            Close();
        }
    }
}
