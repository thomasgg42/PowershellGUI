using System.Windows;


namespace PsGui.Views
    {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            this.DataContext = new PsGui.ViewModels.MainViewModel();
            }

        }
    }
