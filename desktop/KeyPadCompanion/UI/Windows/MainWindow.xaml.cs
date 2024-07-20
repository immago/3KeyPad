using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using KeyPadCompanion.Data.Controllers;
using KeyPadCompanion.Data.Model;
using Microsoft.Toolkit.Uwp.Notifications;

namespace KeyPadCompanion.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CommunicationController? communicationController;
        private ActionsController actionsController = new ActionsController();
        private LEDController ledController = new LEDController();

        public MainWindow()
        {
            InitializeComponent();

            // Tray icon
            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();
            ni.Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            ni.Visible = true;
            ni.DoubleClick += ShowFromTray;
            ni.Click += ShowFromTray;

            Hide();

            // Initial logic
            Restart();
        }

        void Restart()
        {
            var port = Configuration.Instance.ComPortName;
            if (port != null && port.Length == 0) { return; }

            // LED contitions controller
            ledController.OnStateChangedHandler += LedController_OnStateChangedHandler;
            ledController.Start();

            // Serial communication
            communicationController?.Stop();
            communicationController = new CommunicationController(port!);
            communicationController.OnVersionResponse += CommunicationController_OnVersionResponse;
            communicationController.OnLedResponse += CommunicationController_OnLedResponse;
            communicationController.OnButtonClick += CommunicationController_OnButtonClick;
            communicationController.OnErrorOccured += CommunicationController_OnErrorOccured;
            communicationController.Start();
            communicationController.GetVersion();
            communicationController.GetLed(0);


            // Set leds for current state
            LedController_OnStateChangedHandler(0, ledController.StateFor(0));
            LedController_OnStateChangedHandler(1, ledController.StateFor(1));
            LedController_OnStateChangedHandler(2, ledController.StateFor(2));

        }

        private void CommunicationController_OnErrorOccured(Exception ex)
        {
            new ToastContentBuilder()
                .AddText($"Connection error\n {ex.ToString()}")
                .Show();
        }


        // Minimize, close
        private void ShowFromTray(object? sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        // Buttons
        private void ConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new ConfigurationWindow();
            window.Owner = this;
            window.ShowDialog();
            Restart();
        }


        // Led controller events
        private void LedController_OnStateChangedHandler(int index, ButtonLedConfigurationElement state)
        {
            var color = (Color)ColorConverter.ConvertFromString(state.HexColor);
            communicationController?.SetLed(index, state.Mode, color.R, color.G, color.B, state.Speed);

            // Update UI
            CommunicationController_OnLedResponse(index, state.Mode, color.R, color.G, color.B, state.Speed);
        }

        // Communication controller events
        private void CommunicationController_OnButtonClick(ButtonEventType type, int index)
        {

            // Perform action
            actionsController.PerformAction(type, index);

            // Show log
            Application.Current.Dispatcher.Invoke(() =>
            {
                string text = $"[{DateTime.Now.ToString("dd/MM/yy HH:mm")}] ";
                switch (type)
                {
                    case ButtonEventType.SinglePress:
                        text += $"Button #{index} single click";
                        break;
                    case ButtonEventType.DoublePress:
                        text += $"Button #{index} double click";
                        break;
                    case ButtonEventType.LongPress:
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
                    Button1Control.SetLed(Color.FromRgb(r, g, b), speed, mode);
                }

                // Button 2
                if (index == 1)
                {
                    Button2Control.SetLed(Color.FromRgb(r, g, b), speed, mode); ;
                }

                // Button 3
                if (index == 2)
                {
                    Button3Control.SetLed(Color.FromRgb(r, g, b), speed, mode);
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

    }
}
