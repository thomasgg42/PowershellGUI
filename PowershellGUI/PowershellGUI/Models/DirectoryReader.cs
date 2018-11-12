using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {

    enum RadioButtons
        {
        ActiveDirectory,
        Exchange,
        Skype
        }

    class DirectoryReader : ObservableObject
        {
        private string       _directoryPath;
        private RadioButtons _activeButton;

        /*
         *  Constructor
         *  Temporary solution
         */
        public DirectoryReader(string directoryPath)
            {
            _directoryPath = directoryPath;
            _activeButton  = RadioButtons.ActiveDirectory;
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
                // Name of Binding Path name
                OnPropertyChanged("DirectoryPath");
                }
            }

        public RadioButtons ActiveButton
            {
            get
                {
                return _activeButton;
                }
            set
                {
                _activeButton = value;
                OnPropertyChanged("ActiveButton");
                }
            }


        }
    }
