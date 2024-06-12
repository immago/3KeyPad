using CoreAudio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KeyPadCompanion
{
    public class AudioInputElement
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }

    /// <summary>
    /// Interaction logic for AudioInputsWindow.xaml
    /// </summary>
    public partial class AudioInputsWindow : Window
    {
        List<AudioInputElement> data = new List<AudioInputElement>();

        private AudioIOController audioIOController = new AudioIOController();
        private List<MMDevice> devices;

        public AudioInputsWindow()
        {
            InitializeComponent();

            devices = audioIOController.GetInputDevices();
            var selectedDevices = Configuration.instance.ActiveAudioInputDevices;

            foreach (var device in devices)
            {
                bool isActive = selectedDevices.Contains(device.ID);
                data.Add(new AudioInputElement() { IsSelected = isActive, Name = device.DeviceFriendlyName, Id = device.ID });
            }

            DevicesListView.ItemsSource = data;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.instance.ActiveAudioInputDevices = data.Where(item => (item.IsSelected)).Select(x => x.Id).ToList();
            Configuration.Save();
            Close();
        }
    }
}
