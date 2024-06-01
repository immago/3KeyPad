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
            communicationController.Start();
            communicationController.GetVersion();
        }

        private void CommunicationController_OnVersionResponse(string version)
        {
            Application.Current.Dispatcher.Invoke(() => 
            {
                versionLabel.Content = $"ver: {version}";
            });
        }
    }
}
