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
using System.Windows.Shapes;

namespace PowershellGUI
{
    /// <summary>
    /// Interaction logic for DynamicWindow.xaml
    /// </summary>
    public partial class DynamicWindow : Window
    {
        public DynamicWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // relativ filsti til psScripts mappe
            string filePath = @"../../psScripts";
            string[] files = System.IO.Directory.GetFiles(filePath);
            var comboBox = (ComboBox)sender;
            comboBox.ItemsSource = files;
            int firstItem = 0;
            comboBox.SelectedIndex = firstItem;
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        
    }
}
