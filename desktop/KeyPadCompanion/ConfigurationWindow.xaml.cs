using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;


namespace KeyPadCompanion
{
    /// <summary>
    /// Interaction logic for ConfigurationWindow.xaml
    /// </summary>
    public partial class ConfigurationWindow : Window
    {
        public ConfigurationWindow()
        {
            InitializeComponent();

            // Get all ports
            List<string> ports = new List<string>(SerialPort.GetPortNames());
            portsComboBox.Items.Clear();
            foreach (string port in ports)
            {
                portsComboBox.Items.Add(port);
            }

            // Load
            string savedPortName = Properties.Settings.Default.ComPortName;
            int index = ports.FindIndex(str => (str == savedPortName));
            if (index >= 0)
            {
                portsComboBox.SelectedIndex = index;
                saveButton.IsEnabled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (portsComboBox.SelectedIndex >= 0)
            {
                Properties.Settings.Default.ComPortName = portsComboBox.SelectedItem.ToString();
                Properties.Settings.Default.Save();
                Close();
            }
        }


        private void portsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = (portsComboBox.SelectedIndex >= 0);
        }
    }
}
