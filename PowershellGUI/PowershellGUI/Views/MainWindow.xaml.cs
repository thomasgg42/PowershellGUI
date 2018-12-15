using System.Windows;
using PowershellGUI.ViewModels;
using PowershellGUI.Views;

namespace PowershellGUI.Views
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            string devModulePath = @".\..\..\..\Modules"; // during developement
            string releaseModulePath = "Modules";         // release
            DataContext = new ViewModel(devModulePath);
            }
        }
    }