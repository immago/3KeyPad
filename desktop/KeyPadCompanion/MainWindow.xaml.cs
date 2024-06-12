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
using System.Diagnostics;

namespace KeyPadCompanion
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CommunicationController? communicationController;

        public MainWindow()
        {
            InitializeComponent();
            Restart();

            AudioIOController a = new AudioIOController();
            //a.GetInputDevices();
            //a.test();

            /*
            var list = a.GetInputDevices();
            foreach (var device in list)
            {
                Debug.WriteLine($"{device.DeviceFriendlyName} {device.ID}");
            }*/

            /*
                Fifine (2- USB PnP Audio Device) {0.0.1.00000000}.{7350d454-5177-485c-8be3-0697eef0cc3a}
                Barracuda X (Razer Barracuda X 2.4) {0.0.1.00000000}.{5ff58714-e131-40a0-a1cf-2edead9fb889}

                Voicemeeter Out A4 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{001279dd-5baa-4816-a853-a78e217e579e}
                Микрофон (Steam Streaming Microphone) {0.0.1.00000000}.{35b33186-dfe7-464d-96e5-7acb3a0ddf8f}
                Voicemeeter Out B3 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{3f84283b-9695-447a-87ba-8be18b47f162}
                Voicemeeter Out B1 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{42bbdbe2-a35c-413b-a4e5-05c6a4519820}
                Voicemeeter Out A5 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{695d5c98-f741-47c6-8849-0f7f73de799a}
                Voicemeeter Out A3 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{88eed62c-b7d3-4a64-95d6-7a233ef22dac}
                Quest 2 Headset Microphone (Oculus Virtual Audio Device) {0.0.1.00000000}.{8b8b8cd7-079b-418a-9701-47f5e2831272}
                Voicemeeter Out A1 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{941a19ab-97da-4aeb-8d41-f747816fef59}
                Voicemeeter Out A2 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{c73c70e6-9760-44b3-b25c-9d4da19fe050}
                Voicemeeter Out B2 (VB-Audio Voicemeeter VAIO) {0.0.1.00000000}.{dc8c193a-1388-498a-9ca4-e8f0cac5b919}
            */
            //a.setDefaultAudioDevice("{0.0.1.00000000}.{5ff58714-e131-40a0-a1cf-2edead9fb889}");
            //a.setDefaultAudioDevice("{0.0.1.00000000}.{7350d454-5177-485c-8be3-0697eef0cc3a}");
        }

        private void ConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfigurationWindow();
            window.Owner = this;
            window.ShowDialog();
            Restart();
        }

        void Restart()
        {
            string port = Properties.Settings.Default.ComPortName;
            if (port.Length == 0) { return; }

            communicationController?.Stop();
            communicationController = new CommunicationController(port);
            communicationController.OnVersionResponse += CommunicationController_OnVersionResponse;
            communicationController.OnLedResponse += CommunicationController_OnLedResponse;
            communicationController.OnButtonClick += CommunicationController_OnButtonClick;
            communicationController.Start();
            communicationController.GetVersion();
            communicationController.GetLed(0);

            // TODO;
            communicationController.SetLed(0, 0, 255, 50, 0, 1000);
            communicationController.SetLed(1, 0, 50, 255, 0, 1000);
            communicationController.SetLed(2, 0, 0, 50, 255, 1000);
        }

        private void CommunicationController_OnButtonClick(CommunicationController.ButtonClickType type, int index)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                string text = $"[{DateTime.Now.ToString("dd/MM/yy HH:mm")}] ";
                switch (type)
                {
                    case CommunicationController.ButtonClickType.Single:
                        text += $"Button #{index} single click";
                        break;
                    case CommunicationController.ButtonClickType.Double:
                        text += $"Button #{index} double click";
                        break;
                    case CommunicationController.ButtonClickType.Long:
                        text += $"Button #{index} long click";
                        break;
                }
                lastReceivedCommandLabel.Content = text;
            });
        }

        private void CommunicationController_OnLedResponse(int index, int mode, byte r, byte g, byte b, int speed)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Button 1
                if (index == 0)
                {
                    button1ColorRqctangle.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            });
        }

        private void CommunicationController_OnVersionResponse(string version)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                versionLabel.Content = $"ver: {version}";
            });
        }

        private void button1ColorRqctangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("TODO");
        }
    }
}
