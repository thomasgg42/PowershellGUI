using PowershellGUI.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PowershellGUI.ViewModels
    {
    class ViewModel
        {
        private string              modulePath;
        private DirectoryReader     _DirectoryReader;
        private FileReader          _FileReader;
        private PowershellParser    _PowershellParser;
        private ComparisonConverter _ComparisonConverter;

        private ICommand            _clickCommand;

        /// <summary>
        /// Clears the script session allowing a new script
        /// to be run without previous input values interfering
        /// with the new script.
        /// </summary>
        private void ClearLastScriptSession()
            {
            DirectoryReader.ClearDirectoryReader();
            FileReader.ClearFileReader();
            PowershellParser.ClearPowershellParser();
            }

        /// <summary>
        /// Is not included in ClearLastScriptSession() 
        /// so that the output is kept a bit longer on screen.
        /// </summary>
        private void ClearScriptOutput()
            {
            PowershellParser.ScriptOutput = "";
            }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modulePath"></param>
        public ViewModel(string modulePath)
            {
            // Run script knappen er inaktiv til et script velges
            // Tror alt som mangler er å få DirectoryReader til å endre på _canExecute
            // Nederste textbox demostrerer dette - alltid false!
            this.modulePath      = modulePath;
            _DirectoryReader     = new DirectoryReader(modulePath);
            _FileReader          = new FileReader();
            _PowershellParser    = new PowershellParser();
            _ComparisonConverter = new ComparisonConverter();
            _clickCommand        = new CommandHandler(ExecutePowershellScript, CanExecute);
            }

        /// <summary>
        /// Handles the click-functionality of the Run script button
        /// </summary>
        public ICommand ClickCommand
            {
            get
                {
                return _clickCommand;
                }
            set
                {
                _clickCommand = value;
                }
            }

        /// <summary>
        /// Gets the DirectoryReader instance
        /// </summary>
        public DirectoryReader DirectoryReader
            {
            get
                {
                return _DirectoryReader;
                }
            }

        /// <summary>
        /// Gets the FileReader instance
        /// </summary>
        public FileReader FileReader
            {
            get
                {
                return _FileReader;
                }
            }

        /// <summary>
        /// Gets the PowershellParser instance
        /// </summary>
        public PowershellParser PowershellParser
            {
            get
                {
                return _PowershellParser;
                }
            set
                {
                _PowershellParser = value;
                }
            }

        /// <summary>
        /// Gets the ComparisonConverter instance
        /// </summary>
        public ComparisonConverter ComparisonConverter
            {
            get
                {
                return _ComparisonConverter;
                }
            }

        /// <summary>
        /// Decides if the execute button is active or not.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
            {
            bool canExecute = true;
            // Button inactive if no scripts selected
            if(DirectoryReader.IsScriptSelected == false)
                {
                canExecute = false;
                }
            // Button inactive if input field empty
            foreach(ScriptArgument arg in FileReader.ScriptVariables)
                {
                if(arg.InputValue == null || arg.InputValue.Equals(""))
                    {
                    canExecute = false;
                    }
                }
            return canExecute;
            }

        /// <summary>
        /// Gets or sets the selected powershell script. Handles
        /// GUI logic related to if a script is selected or not.
        /// This includes clearing fields and values.
        /// </summary>
        public string SelectedPsScript
            {
            get
                {
                FileReader.FileURI = DirectoryReader.SelectedPsScript;
                // If a file has been chosen
                if (FileReader.FileURI != null && !FileReader.FileURI.Equals(""))
                    {
                    FileReader.ReadFile();
                    }
                // Else no file chosen
                else
                    {
                    /*
                    if(FileReader.ScriptVariables.Count > 0)
                        {
                        FileReader.ScriptVariables.Clear();
                        }
                    */
                    }
                return DirectoryReader.SelectedPsScript;
                }
            set
                {
                // Clear the output when selecting a new script
                if(value != null)
                    {
                    ClearScriptOutput();
                    ClearLastScriptSession();
                    DirectoryReader.SelectedPsScript = value;
                    }
                }
            }

        /// <summary>
        /// Executes a Powershell script
        /// Passes data between DirectoryReader, FileReader
        /// and PowershellParser.
        /// </summary>
        public void ExecutePowershellScript(object obj)
            {
            PowershellParser.RunPsScript(FileReader.FileURI, FileReader.ScriptVariables);
            ClearLastScriptSession();
            }
        }
    }
