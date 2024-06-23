using KeyPadCompanion.Data.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static KeyPadCompanion.Data.Model.ButtonLedConfigurationElement;
using KeyPadCompanion.Data;

namespace KeyPadCompanion.UI.Windows
{
    /// <summary>
    /// Interaction logic for ButtonLedConfigurationWindow.xaml
    /// </summary>
    public partial class ButtonLedConfigurationWindow : Window
    {
        
        List<ButtonLedConfigurationElement> data = new List<ButtonLedConfigurationElement>();

        public ButtonLedConfigurationWindow()
        {
            InitializeComponent();

            data.Add(new ButtonLedConfigurationElement() { 
                IsEnabled = true, 
                Condition = LedStateConditions.Default, 
                HexColor = "#000000",
                Mode = 0,
                Speed = 1000  
            });

            data.Add(new ButtonLedConfigurationElement()
            {
                IsEnabled = true,
                Condition = LedStateConditions.Default,
                HexColor = "#111111",
                Mode = 0,
                Speed = 1000
            });


            ConfigurationListView.ItemsSource = data;
        }

  
        private void ConfigurationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ConfigurationListView.SelectedIndex;
            if (ConfigurationListView.SelectedIndex < 0) return;

            var itemData = data[index];

            var window = new LedConfigurationWindow((Color)ColorConverter.ConvertFromString(itemData.HexColor), itemData.Speed, itemData.Mode);
            window.ShowDialog();

            data[index].HexColor = window.Color.HexColor();
            data[index].Speed = window.Speed;
            data[index].Mode = window.Mode;
            ConfigurationListView.Items.Refresh();

            ConfigurationListView.SelectedIndex = -1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(data);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            data.Add(new ButtonLedConfigurationElement()
            {
                IsEnabled = true,
                Condition = LedStateConditions.Default,
                HexColor = "#ff0000",
                Mode = 0,
                Speed = 1000
            });
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
    }
}
