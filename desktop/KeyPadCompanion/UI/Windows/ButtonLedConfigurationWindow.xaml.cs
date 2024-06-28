using KeyPadCompanion.Data.Model;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using KeyPadCompanion.Data;


namespace KeyPadCompanion.UI.Windows
{
    /// <summary>
    /// Interaction logic for ButtonLedConfigurationWindow.xaml
    /// </summary>
    public partial class ButtonLedConfigurationWindow : Window
    {
        
        private List<ButtonLedConfigurationElement> data = new List<ButtonLedConfigurationElement>();
        private int index;

        public ButtonLedConfigurationWindow(int index)
        {
            this.index = index;

            // Deep copy data
            data = Configuration.Instance.ButtonLedConfiguration[index].ConvertAll(o => o.Clone());

            InitializeComponent();
            ConfigurationListView.ItemsSource = data;
        }

  
        private void ConfigurationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ConfigurationListView.SelectedIndex;
            if (ConfigurationListView.SelectedIndex < 0) return;

            var itemData = data[index];
            ConfigurationListView.SelectedIndex = -1;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                var window = new LedConfigurationWindow((Color)ColorConverter.ConvertFromString(itemData.HexColor), itemData.Speed, itemData.Mode);
                window.ShowDialog();

                data[index].HexColor = window.Color.HexColor();
                data[index].Speed = window.Speed;
                data[index].Mode = window.Mode;
                ConfigurationListView.Items.Refresh();
            }));

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration.Instance.ButtonLedConfiguration[index] = data;
            Configuration.Save();
            DialogResult = true;
            Close();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            data.Add(new ButtonLedConfigurationElement());
            ConfigurationListView.Items.Refresh();
        }

        private void OrderUpButton_Click(object sender, RoutedEventArgs e)
        {
            var rowData = (sender as Button).DataContext as ButtonLedConfigurationElement;
            int index = data.IndexOf(rowData);
            if (index <= 0) return;
            data.Move(rowData, index - 1);
            ConfigurationListView.Items.Refresh();
        }

        private void OrderDownButton_Click(object sender, RoutedEventArgs e)
        {
            var rowData = (sender as Button).DataContext as ButtonLedConfigurationElement;
            int index = data.IndexOf(rowData);
            if (index >= data.Count-1) return;
            data.Move(rowData, index + 1);
            ConfigurationListView.Items.Refresh();
        }

        private void DeketeButton_Click(object sender, RoutedEventArgs e)
        {
            var rowData = (sender as Button).DataContext as ButtonLedConfigurationElement;
            data.Remove(rowData);
            ConfigurationListView.Items.Refresh();
        }

        private void ConditionConfigButton_Click(object sender, RoutedEventArgs e)
        {
            var rowData = (sender as Button).DataContext as ButtonLedConfigurationElement;
            int index = data.IndexOf(rowData);
            if (index > data.Count - 1) return;

            if (rowData.Condition == LedStateConditions.IsInputSelected)
            {
                var window = new AudioInputPickerWindow(rowData.InputDeviceId);
                window.ShowDialog();
                data[index].InputDeviceId = window.SelectedDeviceId;
                ConfigurationListView.Items.Refresh();
            }

            if (!rowData.HasParameters)
            {
                MessageBox.Show("This setting does not have any parameters");
            }
        }
    }
}
