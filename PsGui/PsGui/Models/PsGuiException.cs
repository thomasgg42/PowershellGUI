using System;
using System.IO;

namespace PsGui.Models
{
    public class PsGuiException : Exception
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="temp1"></param>
        /// <param name="temp2"></param>
        /// <param name="closeApp">True/false</param>
        public PsGuiException(string temp1, string temp2, bool closeApp)
        {
            System.Windows.MessageBox.Show(temp1);
            WriteErrorToFile(temp2);
            if (closeApp)
            {
                CloseApp();
            }
        }

        public PsGuiException(string temp1)
        {
            // Non-critical errors?
            System.Windows.MessageBox.Show(temp1);
        }

        /// <summary>
        /// Writes the provided error message to an error log in the 
        /// current directory.
        /// </summary>
        /// <param name="error"></param>
        public void WriteErrorToFile(string error)
        {
            using (StreamWriter outputFile = new StreamWriter("error.log"))
            {
                outputFile.WriteLine(error);
            }
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
