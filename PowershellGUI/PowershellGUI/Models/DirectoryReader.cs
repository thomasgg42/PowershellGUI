using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;

namespace PowershellGUI.Models
    {
    /// <summary>
    /// Radio buttons corresponding to each module-folder. For every new
    /// folder, this list must be updated as well as the XML-file's stack panel.
    /// </summary>
    public enum RadioButtons
        {
        ActiveDirectory,
        Exchange,
        Skype,
        FutureCategory // Example of a new category
        }

    /// <summary>
    /// Stores information about existing script directories and the selected
    /// scripts files.
    /// </summary>
    public class DirectoryReader : ObservableObject
        {
        private bool   _isScriptSelected;
        private string _modulePath;
        private string _selectedPsScript;
        private string directoryName;
        private ObservableCollection<string> _directoryNameBrowser;
        private RadioButtons _activeButton;

        /// <summary>
        /// Returns the provided radio button as a string.
        /// </summary>
        /// <param name="btn">RadioButton corresponding to a script directory</param>
        /// <returns></returns>
        private string getSelectedDirectory(RadioButtons btn)
            {
            return btn.ToString();
            }

        /// <summary>
        /// Fills the _directoryNameBrowser collection with powershell
        /// script names. Also stores the full filepath to the given script
        /// for later use.
        /// </summary>
        private void UpdateDirectoryBrowser()
            {
            _directoryNameBrowser.Clear();

            // Gets the directory corresponding to selected radio button
            foreach(RadioButtons btn in (RadioButtons[])System.Enum.GetValues(typeof(RadioButtons)))
                {
                if(_activeButton == btn)
                    {
                    directoryName = getSelectedDirectory(btn);
                    }
                }

            // Gets all the .ps1 scripts in the active directory
            string[] scripts;
            string directoryFilePath = _modulePath + "\\" + directoryName + "\\";
            int fileExtensionLength = 4;
            try
                {
                scripts = System.IO.Directory.GetFiles(directoryFilePath, "*.ps1");
                foreach (string script in scripts)
                    {
                    string fileNameWithExtension = Path.GetFileName(script);
                    string fileNameWithoutExtension = (Path.GetFileName(script)).Substring(0, fileNameWithExtension.Length - fileExtensionLength);
                    DirectoryNameBrowser.Add(Path.GetFileName(fileNameWithoutExtension));
                    }
                }
            catch (System.Exception e)
                {
                // TODO: exception klasser?
                System.Windows.MessageBox.Show("No \"Modules\" directory found. This application must\n be placed together with the \"Modules\" directory.");
                }
            OnPropertyChanged("DirectoryBrowser");
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public DirectoryReader(string modulePath)
            {
            _isScriptSelected = false;
            _modulePath = modulePath;
            directoryName = "";
            _activeButton = RadioButtons.ActiveDirectory;
            _directoryNameBrowser = new ObservableCollection<string>();
            UpdateDirectoryBrowser();
            }

        /// <summary>
        /// Resets the selected script back to none and
        /// empties the list of scripts.
        /// </summary>
        public void ClearDirectoryReader()
            {
            SelectedPsScript = "";
            UpdateDirectoryBrowser();
            }

        /// <summary>
        /// Sets the script's full file path.
        /// Gets only the script's name, excluding the file path, as shown in the ComboBox.
        /// </summary>
        public string SelectedPsScript
            {
            get
                {
                return _selectedPsScript;
                }
            set
                {
                if(value != null)
                    {
                    _selectedPsScript = _modulePath + "\\" + directoryName + "\\" + value + ".ps1"; // + .ps1 når jeg fjerner denne..
                    OnPropertyChanged("SelectedPsScript");
                    }
                if(value != null && value.Equals(""))
                    {
                    _selectedPsScript = "";
                    IsScriptSelected = false;
                    }
                else
                    {
                    IsScriptSelected = true;
                    }
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
        /// Gets or sets the directoryPath to the directory to read
        /// </summary>
        public ObservableCollection<string> DirectoryNameBrowser
            {
            get
                {
                return _directoryNameBrowser;
                }
            set
                {
                if(value != null)
                    {
                    _directoryNameBrowser = value;
                 //   OnPropertyChanged("DirectoryPathBrowser");
                    }
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
                UpdateDirectoryBrowser();
                IsScriptSelected = false;
                }
            }

        }
    }
