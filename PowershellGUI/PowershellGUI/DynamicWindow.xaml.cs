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
        public DynamicWindow(string test)
        {
            InitializeComponent();

            if (test == "AD")
            {
                // relativ filsti til psScripts mappe
                string filePath = @"../../psScripts/AD";
                string[] files = System.IO.Directory.GetFiles(filePath);
                
                //For hver filstring i mappen AD, legges til en kanpp via dynamisk brukerkontroll
                foreach(string i in files)
                {
                    StackPanel st = new StackPanel();
                    sp.Children.Add(st);

                    Button btn = new Button();
                    btn.Content = i;
                    btn.Name = test;
                    btn.IsEnabled = true;
                    btn.Visibility = Visibility.Visible;
                    st.Children.Add(btn);
                    

                    //TODO: Finne ut hvordan man inisialiser knapp ????btn.InitializeComponent()????

                
                }
            }

                    
        }

        private void Initialize()
        {
           
        }

    }
   

}
