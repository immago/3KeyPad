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
