using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class DirectoryReader : ObservableObject
        {

        private string _directoryPath;

        /*
         *  Constructor
         */
        public DirectoryReader(string directoryPath)
            {
            _directoryPath = directoryPath;
            }

        /// <summary>
        /// Gets or sets the directoryPath to the directory to read
        /// </summary>
        public String DirectoryPath
            {
            get
                {
                return _directoryPath;
                }
            set
                {
                _directoryPath = value;
                // Setting a new directoryPath notifies listeners
                OnPropertyChanged("DirectoryPath");
                }
            }

        }
    }
