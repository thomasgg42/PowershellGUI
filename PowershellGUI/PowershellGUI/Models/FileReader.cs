using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class FileReader : ObservableObject
        {
        private string _FilePath;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileReader()
            {
            }

        public string FilePath
            {
            get
                {
                return _FilePath;
                }
            set
                {
                _FilePath = value;
                // Setting a new directory path notifies listeners
                OnPropertyChanged("FilePath");
                }
            }

        }
    }
