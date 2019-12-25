using System;
using System.IO;

namespace PsGui.Models
{
    public static class PsGuiException
    {
        public static string errorLogName = "error.log";
        public static void WriteErrorToScreen(string error)
        {
            System.Windows.MessageBox.Show(error);
        }

        /// <summary>
        /// Writes the provided error string to error.log in the root
        /// directory. If the file does not exists, it is created.
        /// If the file exists, contents are appended.
        /// </summary>
        /// <param name="error"></param>
        public static void WriteErrorToFile(string error)
        {
            bool appendFileContents = true;
            using (StreamWriter outputFile = new StreamWriter(errorLogName, appendFileContents))
            {
                outputFile.WriteLine(error);
            }
        }

        /// <summary>
        /// Clears error.log in the root directory.
        /// </summary>
        public static void ClearErrorLog()
        {
            FileStream fileStream = File.Open(errorLogName, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();
        }

        public static void CloseApp()
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
