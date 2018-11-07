using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI
{
    public class RoutedEventAddRemoveHandler
    {
        public void OnClickMainWindow(string test)
        {
            //Hvis ad-kanppen trykkes, sett string for mappe med ad-skript
            if(test == "AD")
            {
                // relativ filsti til psScripts mappe
                string filePath = @"../../psScripts/AD";
                string[] files = System.IO.Directory.GetFiles(filePath);

                for (int i = 0; i < files.Length; i++)
                {
                    DynamicUserControl btn = new DynamicUserControl();
                    btn.Content = files[i];
                    btn.IsEnabled = true;
                    
                }
            }
            

        }

        void test(object sender, RoutedEventArgs e)
        {

        }

    }
}

