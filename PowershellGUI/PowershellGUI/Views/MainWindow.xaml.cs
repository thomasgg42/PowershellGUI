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

            // Scan antall mapper i ModulePath
            // Opprett antall radio knapper ut ifra dette

            // for hver valgte radio knapp
            // scan antall mapper i mappen tilhørende valgt radio knapp
            // populer dropdown med mappenes navn
            }
        }
    }