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
        private bool _isSelected;
        public bool IsSelectedCategory
            {
            get
                {
                return _isSelected;
                }
            set
                {
                _isSelected = value;
                }
            }

        /// <summary>
        /// Removes folder prefix from the file path.
        /// </summary>
        /// <param name="filePath"></param>
        private void ConvertFriendlyName(string filePath)
            {
            string prefix = ".\\Modules\\";
            int prefixLength = prefix.Length;
            FriendlyName = FilePath.Substring(prefixLength);
            }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath"></param>
        public ScriptCategory(string filePath)
            {
            FilePath = filePath;
            ConvertFriendlyName(filePath);
            }
        }
    }
