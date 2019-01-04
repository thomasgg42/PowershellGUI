using PsGui.Models.PowershellExecuter;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace PsGui.ViewModels
    {
    /// <summary>
    /// Executes powershell scripts with command line arguments
    /// in the form of user input.
    /// </summary>
    public class PsExecViewModel : ObservableObject
        {
        public string TabName { get; } = "Script Executer";

        private DirectoryReader    directoryReader;
        private ScriptReader       scriptReader;
        private PowershellExecuter powershellExecuter;

        public ICommand RadioButtonChecked  { get; set; }
        public ICommand ExecuteButtonPushed { get; set; }

        private string _modulePath;
        private string _scriptOutput;
        private string _scriptErrorOutput;

        /// <summary>
        /// Executes a powershell script at the supplied path with the supplied
        /// input variables. Saves normal and error output from the script.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecutePowershellScript(object obj)
            {
            powershellExecuter.ExecuteScript(SelectedScriptPath, ScriptVariables);
            ScriptExecutionOutput      = powershellExecuter.ScriptOutput;
            ScriptExecutionErrorOutput = powershellExecuter.ScriptErrors;
            ClearScriptSession();
            }

        /// <summary>
        /// Returns true if a selected powershell script
        /// is ready to be executed. False otherwise.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CanExecuteScript(object parameter)
            {
            bool canExec = true;
            if (IsScriptSelected == false)
                {
                canExec = false;
                }
            else
                {
                foreach (ScriptArgument arg in ScriptVariables)
                    {
                    if (arg.InputValue.Equals(""))
                        {
                        canExec = false;
                        }
                    }
                }

            return canExec;
            }

        /// <summary>
        /// Helper function for the ICommand implementation.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanExecuteRadioButtonCheck(object param)
            {
            return true;
            }

        /// <summary>
        /// Helper function for the ICommand implementation. Ensures
        /// that INotifyPropertyChanged executes upon using radio buttons.
        /// </summary>
        /// <param name="radioBtnContent"></param>
        private void GetSelectedScriptCategoryName(object radioBtnContent)
            {
            SelectedScriptCategory = radioBtnContent.ToString();
            }

        /// <summary>
        /// Sets the initial script category on program startup.
        /// </summary>
        private void SetInitialScriptCategory()
            {
            int firstCategory = 0;
            try
                {
                ScriptCategoryBrowser[firstCategory].IsSelectedCategory = true;
                SelectedScriptCategory = ScriptCategoryBrowser[firstCategory].FriendlyName;
                }
            catch(Exception e)
                {
                throw new PsExecException("Finner ingen kategorimapper i Modules-mappen!", e.ToString());
                }
            }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modulePath"></param>
        public PsExecViewModel(string modulePath, string moduleFolderName)
            {
            _modulePath         = modulePath + "\\" + moduleFolderName + "\\";
            directoryReader     = new DirectoryReader(_modulePath);
            scriptReader        = new ScriptReader();
            UpdateScriptCategoriesList();
            SetInitialScriptCategory();
            powershellExecuter  = new PowershellExecuter();

            RadioButtonChecked  = new PsGui.Converters.CommandHandler(GetSelectedScriptCategoryName, CanExecuteRadioButtonCheck);
            ExecuteButtonPushed = new PsGui.Converters.CommandHandler(ExecutePowershellScript, CanExecuteScript);
            }

        /// <summary>
        /// Sets or gets the filepath to the "Module" folder containing
        /// the categories for all the powershell scripts.
        /// </summary>
        public string ModulePath
            {
            get
                {
                return _modulePath;
                }
            set
                {
                if (value != null)
                    {
                    _modulePath = value;
                    }
                }
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the directory file paths, the script categories.
        /// </summary>
        public ObservableCollection<ScriptCategory> ScriptCategoryBrowser
            {
            get
                {
                return directoryReader.ScriptCategories;
                }
            set
                {
                if (value != null)
                    {
                    directoryReader.ScriptCategories = value;
                    }
                }
            }

        /// <summary>
        /// Sets or gets the selected category in form of a 
        /// ScriptCategory.FriendlyName. Clears existing 
        /// input field values upon setting a new category.
        /// Does not clear the fields themselves.
        /// </summary>
        public string SelectedScriptCategory
            {
            get
                {
                return directoryReader.SelectedCategoryName;
                }
            set
                {
                if (value != null)
                    {
                    scriptReader.ClearScriptVariableInfo();
                    directoryReader.SelectedCategoryName = value;
                    directoryReader.ClearScripts();
                    directoryReader.UpdateScriptFilesList();
                    }
                }
            }

        /// <summary>
        /// Fills the list of script categories based on 
        /// the directories found in the Module-folder. Uses
        /// the SelectedScriptCategory property value to set the
        /// selected script boolean value in the list object.
        /// @Throws PsGuiException
        /// </summary>
        public void UpdateScriptCategoriesList()
            {
            directoryReader.UpdateScriptCategoriesList();
            foreach (ScriptCategory cat in ScriptCategoryBrowser)
                {
                if (cat.FriendlyName.Equals(directoryReader.SelectedCategoryName)
                    && cat.IsSelectedCategory != true)
                    {
                    cat.IsSelectedCategory = true;
                    }
                }
            }


        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script files in each category.
        /// </summary>
        public ObservableCollection<string> ScriptFileBrowser
            {
            get
                {
                return directoryReader.ScriptFiles;
                }
            set
                {
                if (value != null)
                    {
                    directoryReader.ScriptFiles = value;
                    }
                else
                    {
                    System.Windows.MessageBox.Show("ScriptFileBrowser null");
                    }
                }
            }

        /// <summary>
        /// Sets or gets the selected powershell script and its path. Also
        /// calls functions responsible to read contents of a 
        /// selected powershell script and functions responsible
        /// of cleaning up previous script input fields.
        /// </summary>
        public string SelectedScriptFile
            {
            get
                {
                return directoryReader.SelectedScript;
                }
            set
                {
                if(value != null)
                    {
                    // The setter runs when set to empty string as well as a script name
                    if(value != "")
                        {
                        ScriptVariables.Clear();
                        IsScriptSelected = true;
                        SelectedScriptPath = _modulePath + directoryReader.SelectedCategoryName + "\\" + value + ".ps1";
                        scriptReader.ReadSelectedScript(SelectedScriptPath);
                        if((ScriptExecutionOutput != null && ScriptExecutionOutput.Length > 0) ||
                           (ScriptExecutionErrorOutput != null && ScriptExecutionErrorOutput.Length > 0))
                            {
                            ScriptExecutionOutput      = "";
                            ScriptExecutionErrorOutput = "";
                            }
                        }
                    else
                        {
                        IsScriptSelected = false;
                        }
                    }
                }
            }

        /// <summary>
        /// Sets or gets the file path to the selected
        /// powershell script.
        /// </summary>
        public string SelectedScriptPath
            {
            get
                {
                return directoryReader.SelectedScriptPath;
                }
            set
                {
                if(value != null)
                    {
                    directoryReader.SelectedScriptPath = value;
                    }
                }
            }

        /// Updates the list of scripts based on the currently
        /// selected category.
        /// Finds all files with a .ps1 file extension in the 
        /// selected category folder and stores each file name
        /// excluding the file extension.
        public void UpdateScriptFilesList()
            {
            directoryReader.UpdateScriptFilesList();
            }

        /// <summary>
        /// Returns true if a powershell script has been selected.
        /// </summary>
        public bool IsScriptSelected 
            {
            get
                {
                return directoryReader.IsScriptSelected;
                }
            set
                {
                directoryReader.IsScriptSelected = value;
                }
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script command line input variables.
        /// </summary>
        public ObservableCollection<ScriptArgument> ScriptVariables
            {
            get
                {
                return scriptReader.ScriptVariables;
                }
            }

        /// <summary>
        /// Clears the script session, and resets the program state 
        /// back to the initial state.
        /// </summary>
        public void ClearScriptSession()
            {
            directoryReader.ClearSesssion();
            scriptReader.ClearSession();
            powershellExecuter.ClearSession();
            UpdateScriptCategoriesList();
            SetInitialScriptCategory();
            }

        /// <summary>
        /// Gets the output from the executed powershell script.
        /// </summary>
        public string ScriptExecutionOutput
            {
            get
                {
                return _scriptOutput;
                }
            set
                {
                if(value != null)
                    {
                    _scriptOutput = value;
                    OnPropertyChanged("ScriptExecutionOutput");
                    }
                }
            }

        /// <summary>
        /// Gets the error output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorOutput
            {
            get
                {
                return _scriptErrorOutput;
                }
            set
                {
                if (value != null)
                    { 
                    _scriptErrorOutput = value;
                    OnPropertyChanged("ScriptExecutionErrorOutput");
                    }
                }
            }
        }
    }
