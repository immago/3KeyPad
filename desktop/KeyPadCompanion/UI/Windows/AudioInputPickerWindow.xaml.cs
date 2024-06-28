using CoreAudio;
using KeyPadCompanion.Data.Controllers;
using KeyPadCompanion.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyPadCompanion.UI.Windows
{
    /// <summary>
    /// Interaction logic for AudioInputPickerWindow.xaml
    /// </summary>
    public partial class AudioInputPickerWindow : Window
    {
        private AudioIOController audioIOController = new AudioIOController();
        private List<MMDevice> devices;
        public string? SelectedDeviceId;

        public AudioInputPickerWindow(string? selectedDeviceId)
        {
            this.SelectedDeviceId = selectedDeviceId;
            InitializeComponent();

            devices = audioIOController.GetInputDevices();

            DevicesComboBox.SelectedValuePath = "ID";
            DevicesComboBox.DisplayMemberPath = "DeviceFriendlyName";
            foreach (var device in devices)
            {
                DevicesComboBox.Items.Add(device);
            }
            DevicesComboBox.SelectedValue = SelectedDeviceId;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDeviceId = DevicesComboBox.SelectedValue as string;
            DialogResult = true;
            Close();
        }
    }
}
