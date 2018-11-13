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
        private List<string> _directoryPath;
        private RadioButtons _activeButton;

        /// <summary>
        /// Constructor
        /// </summary>
        public DirectoryReader()
            {
            _activeButton  = RadioButtons.ActiveDirectory;
            _directoryPath = new List<string>();
            _directoryPath.Add("test/test/test");
            }

        /// <summary>
        /// Gets or sets the directoryPath to the directory to read
        /// </summary>
        public List<string> DirectoryPath
            {
            get
                {
                return _directoryPath;
                }
            set
                {
                _directoryPath = value;
                OnPropertyChanged("DirectoryPath");
                }
            }

        /// <summary>
        /// Gets or sets the current active radio button
        /// @see ComparisonConverter for Enum -> boolean transition
        /// </summary>
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
