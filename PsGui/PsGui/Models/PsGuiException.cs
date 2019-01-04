using System;
using System.IO;

namespace PsGui.Models.PowershellExecuter
    {
    public class PsGuiException : Exception
        {
        public PsGuiException(string temp1, string temp2)
            {
            System.Windows.MessageBox.Show(temp1);
            using (StreamWriter outputFile = new StreamWriter("error.log"))
                {
                outputFile.WriteLine(temp2);
                }
            //skriv temp2 til fil
            }
        public PsGuiException(string temp1)
            {
            System.Windows.MessageBox.Show(temp1);
            }
        }
    }
