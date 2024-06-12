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
            var savedPortName = Configuration.instance.ComPortName;
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
                Configuration.instance.ComPortName = portsComboBox.SelectedItem.ToString();
                Configuration.Save();
                Close();
            }
        }


        private void portsComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            saveButton.IsEnabled = (portsComboBox.SelectedIndex >= 0);
        }
    }
}
