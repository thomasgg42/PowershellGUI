using System.Windows;


namespace PowershellGUI
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            PowershellGUIHandler powershellGUI = new PowershellGUIHandler();
            powershellGUI.ModulePath = @"../../Modules";
            
            // Scan antall mapper i ModulePath
            // Opprett antall radio knapper ut ifra dette
            
            // for hver valgte radio knapp
            // scan antall mapper i mappen tilhørende valgt radio knapp
            // populer dropdown med mappenes navn

            
            }
        }
    }
