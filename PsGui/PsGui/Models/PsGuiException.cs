using System;
using System.IO;

namespace PsGui.Models.PowershellExecuter
    {
    public class PsGuiException : Exception
        {
        public PsGuiException(string temp1, string temp2)
            {
            System.Windows.MessageBox.Show(temp1);
            //skriv temp2 til fil
            using (StreamWriter outputFile = new StreamWriter("error.log"))
                {
                outputFile.WriteLine(temp2);
                }
            CloseApp();
            }
        public PsGuiException(string temp1)
            {
            System.Windows.MessageBox.Show(temp1);
            CloseApp();
            }

        public void CloseApp()
            {
            if (System.Windows.Forms.Application.MessageLoop)
                {
                // WinForms app
                System.Windows.Forms.Application.Exit();
                }
            else
                {
                // Console app
                System.Environment.Exit(1);
                }
            }
        }
    }
