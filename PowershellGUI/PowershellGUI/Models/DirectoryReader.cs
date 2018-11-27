using System.Collections.ObjectModel;
using System.Windows.Input;

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
        private bool   _isScriptSelected;
        private string _modulePath;
        private string _selectedPsScript;
        private ObservableCollection<string> directoryBrowser;
        private RadioButtons _activeButton;

        /// <summary>
        /// Returns the selected Powershell script's filepath
        /// </summary>
        public string SelectedPsScript
            {
            get
                {
                return _selectedPsScript;
                }
            set
                {
                _selectedPsScript = value;
                OnPropertyChanged("SelectedPsScript");
                IsScriptSelected = true;
                }
            }

        /// <summary>
        /// Gets or sets the boolean value true if a script is selected.
        /// </summary>
        public bool IsScriptSelected
            {
            get
                {
                return _isScriptSelected;
                }
            set
                {
                _isScriptSelected = value;
                OnPropertyChanged("IsScriptSelected");
                }
            }

        /// <summary>
        /// Updates the directory browser to match the 
        /// selected category.
        /// </summary>
        private void UpdateDirectoryBrowser()
            {
            // @SIMEN
            // funksjonen har som mål å fylle directoryBrowser med powershellscriptene som kan velges

            // if/else dårlig mtp økning av antall kategorier i fremtiden
            // modulePath skal lede til topp-dir hvor AD/Exchange/Skype ligger
            directoryBrowser.Clear();

            /*
            if (_activeButton == RadioButtons.ActiveDirectory)
                {
                directoryBrowser.Add("ad valgt");
                }
            else if (_activeButton == RadioButtons.Exchange)
                {
                directoryBrowser.Add("exchange valgt");
                }
            else
                {
                directoryBrowser.Add("noe annet");
                }

            */
            OnPropertyChanged("DirectoryBrowser");
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public DirectoryReader(string modulePath)
            {
            _isScriptSelected = false;
            _modulePath      = modulePath;
            _activeButton    = RadioButtons.ActiveDirectory;
            directoryBrowser = new ObservableCollection<string>();
            //UpdateDirectoryBrowser();
            TmpUpdateDirectoryBrowser(); // for testing
            }

        /// <summary>
        /// Gets or sets the directoryPath to the directory to read
        /// </summary>
        public ObservableCollection<string> DirectoryBrowser
            {
            get
                {
                return directoryBrowser;
                }
            set
                {
                directoryBrowser = value;
                OnPropertyChanged("DirectoryBrowser");
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
                //UpdateDirectoryBrowser();
                TmpUpdateDirectoryBrowser();
                IsScriptSelected = false;
                }
            }


        /// <summary>
        /// Used for testing to build other functions
        /// </summary>
        public void TmpUpdateDirectoryBrowser()
            {
            // Denne kan ikke sette SelectedScript
            // Den skal kun fylle ComboBox med tilgjengelige scripts
            // SelectedScript skal settes i det et script er valgt!
            directoryBrowser.Clear();
            string script = "";
            if(_activeButton == RadioButtons.ActiveDirectory)
                {
                script = _modulePath + "\\Active Directory\\psfile1.ps1";
                }
            else if(_activeButton == RadioButtons.Exchange)
                {
                script = _modulePath + "\\Exchange\\psfile2.txt";
                }
            else if(_activeButton == RadioButtons.Skype)
                {
                script = _modulePath + "\\Skype\\psfile3.txt";
                }
            DirectoryBrowser.Add(script);
            }
        }
    }
