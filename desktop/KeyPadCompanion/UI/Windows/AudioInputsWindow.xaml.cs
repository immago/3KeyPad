using CoreAudio;
using KeyPadCompanion.Data.Controllers;
using KeyPadCompanion.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KeyPadCompanion.UI.Windows
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
            var selectedDevices = Configuration.Instance.ActiveAudioInputDevices;

            foreach (var device in devices)
            {
                bool isActive = selectedDevices.Contains(device.ID);
                data.Add(new AudioInputElement() { IsSelected = isActive, Name = device.DeviceFriendlyName, Id = device.ID });
            }

            DevicesListView.ItemsSource = data;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ActiveAudioInputDevices = data.Where(item => (item.IsSelected)).Select(x => x.Id).ToList();
            Configuration.Save();
            Close();
        }
    }
}
