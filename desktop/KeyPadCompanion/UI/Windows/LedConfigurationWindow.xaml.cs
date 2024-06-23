using System;
using KeyPadCompanion.Data.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using KeyPadCompanion.Data;


namespace KeyPadCompanion.UI.Windows
{
    /// <summary>
    /// Interaction logic for LedConfigurationWindow.xaml
    /// </summary>
    public partial class LedConfigurationWindow : Window
    {
       
        public Color Color { get; set; }
        public int Speed { get; set; }
        public int Mode { get; set; }


        public LedConfigurationWindow(Color color, int speed, int mode)
        {
            InitializeComponent();
            HexColorTextBox.Text = color.HexColor();
            SpeedTextBox.Text = $"{speed}";
            UpdateColorPreview();

            foreach (string name in Configuration.LedModes.Values)
            {
                ModeComboBox.Items.Add(name);
            }
            ModeComboBox.SelectedValue = Configuration.LedModes[mode];
        }

        private void UpdateColorPreview()
        {
            try
            {
                Color = (Color)ColorConverter.ConvertFromString(HexColorTextBox.Text);
                ColorPreviewRectangle.Fill = new SolidColorBrush(Color);
                SaveButton.IsEnabled = true;
            }
            catch
            {
                SaveButton.IsEnabled = false;
                ColorPreviewRectangle.Fill = new SolidColorBrush(Color.FromArgb(0,0,0,0));
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void HexColorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateColorPreview();
        }

        private void SpeedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Speed = Int32.Parse(SpeedTextBox.Text);
        }

        private void ModeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Mode = Configuration.LedModes.FirstOrDefault(x => x.Value == (string)ModeComboBox.SelectedValue).Key;
        }
    }
}
