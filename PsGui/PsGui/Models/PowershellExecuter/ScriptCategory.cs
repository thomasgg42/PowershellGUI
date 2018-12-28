using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// Stores each script category with a filepath and 
    /// a friendly name.
    /// </summary>
    public class ScriptCategory
        {
        public string FilePath { get; private set; }
        public string FriendlyName { get; private set; }

        /// <summary>
        /// Removes leading and trailing slashes 
        /// and dots from the category name.
        /// </summary>
        /// <param name="filePath"></param>
        private void ConvertFriendlyName(string filePath)
            {
            string prefix = ".\\Modules\\";
            int prefixLength = prefix.Length;
            FriendlyName = FilePath.Substring(prefixLength);
            }

        public ScriptCategory(string filePath)
            {
            FilePath = filePath;
            ConvertFriendlyName(filePath);
            System.Windows.MessageBox.Show(FriendlyName);
            }
        }
    }
