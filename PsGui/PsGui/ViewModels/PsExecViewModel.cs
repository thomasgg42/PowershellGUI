using PsGui.Models.PowershellExecuter;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace PsGui.ViewModels
    {
    /// <summary>
    /// Executes powershell scripts with command line arguments
    /// in the form of user input.
    /// </summary>
    public class PsExecViewModel : ObservableObject
        {
        private DirectoryReader    directoryReader;
        private ScriptReader       scriptReader;
        private PowershellExecuter powershellExecuter;


        private bool isBusy = false;
        // Module path should be renamed scriptFolderPath. The word
        // module should not be confused with a Powershell-module.
        // The modulePath contains the relative file path to the scripts folder.
        private string _modulePath;
        private string _scriptOutput;
        private string _scriptErrorOutput;
        private string _scriptProgressOutput;

        public ICommand RadioButtonChecked { get; set; }
        public ICommand ExecuteButtonPushed { get; set; }
        public ICommand ScriptDescriptionButtonPushed { get; set; }

        public string TabName { get; } = "Script Executer";

        /// <summary>
        /// Executes a powershell script at the supplied path with the supplied
        /// input variables. Saves normal and error output from the script.
        /// </summary>
        /// <param name="obj"></param>
        private void ExecutePowershellScript(object obj)
        {
            try
            {
                powershellExecuter.ExecuteScript(SelectedScriptPath, ScriptVariables);
            }
            catch (Exception e)
            {
                throw new PsExecException("Script execution failed due to bad PowerShell script code!", e.ToString(), false);
            }

            ScriptExecutionOutput = powershellExecuter.ScriptOutput;
            ScriptExecutionErrorOutput = powershellExecuter.ScriptErrors;
            ClearScriptSession();
        }

        private async Task ExecutePowershellScriptAsync(object obj)
        {
            isBusy = true;
            powershellExecuter.SetScriptParameters(ScriptVariables);
            await Task.Run(() =>
            {
                using (PowerShell psInstance = PowerShell.Create())
                {
                    psInstance.AddCommand(SelectedScriptPath);
                    int argLength = powershellExecuter.CommandLineArguments.Count;
                    for (int ii = 0; ii < argLength; ii++)
                    {
                        psInstance.AddParameter(powershellExecuter.CommandLineArgumentKeys[ii], powershellExecuter.CommandLineArguments[ii]);
                    }

                    PSDataCollection<PSObject> outputCollection = new PSDataCollection<PSObject>();

                    // Collect output in real time
                    outputCollection.DataAdded += OutputCollection_DataAdded;
                    psInstance.Streams.Error.DataAdded += Error_DataAdded;
                    psInstance.Streams.Progress.DataAdded += Progress_DataAdded;

                    // Begin powershell execution and continue control
                    IAsyncResult result = psInstance.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                    // When wrapping PowerShell instance in a using block, the pipeline will
                    // close and the script execution will abort. Therefore we must wait for
                    // the state of the pipeline to equal Completed
                    while (result.IsCompleted == false)
                    {
                        Thread.Sleep(1000);
                    }

                    // Gather output for the PowershellExecuter object
                    Collection<PSObject> outputObjects = new Collection<PSObject>();
                    foreach (PSObject outputItem in outputCollection)
                    {
                        outputObjects.Add(outputItem);
                    }

                    powershellExecuter.CollectPowershellScriptOutput(outputObjects);
                    powershellExecuter.CollectPowershellScriptErrors(psInstance);

                    //System.Windows.MessageBox.Show(powershellExecuter.ScriptErrors);
                    //System.Windows.MessageBox.Show(ScriptExecutionOutput);
                }
            });
            ClearScriptSession();
            isBusy = false;
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void OutputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            ScriptExecutionOutput += ((PSDataCollection<PSObject>)sender)[e.Index].ToString();
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            ScriptExecutionErrorOutput += ((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString();
        }

        /// <summary>
        /// Event handler for when Data is added to the Progress stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all progress output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void Progress_DataAdded(object sender, DataAddedEventArgs e)
        {
            ScriptExecutionProgressOutput = ((PSDataCollection<ProgressRecord>)sender)[e.Index].PercentComplete.ToString();
        }



        /// <summary>
        /// Returns true if a selected powershell script
        /// is ready to be executed. False otherwise.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CanExecuteScript(object parameter)
            {
            if(isBusy == true)
            {
                return false;
            }
            if (IsScriptSelected == false)
                {
                return false;
                }
            else
                {
                foreach (ScriptArgument arg in ScriptTextVariables)
                    {
                    if (arg.InputValue != null && arg.InputValue.Equals(""))
                        {
                        return false;
                        }
                    }
                foreach (ScriptArgument arg in ScriptUsernameVariables)
                    {
                    if (arg.InputValue != null && arg.InputValue.Equals(""))
                        {
                        return false;
                        }
                    }
                foreach (ScriptArgument arg in ScriptPasswordVariables)
                    {
                    if (arg.InputValue != null && arg.InputValue.Equals(""))
                        {
                        return false;
                        }
                    }
                foreach (ScriptArgument arg in ScriptMultiLineVariables)
                    {
                    if (arg.InputValue != null && arg.InputValue.Equals(""))
                        {
                        return false;
                        }
                    }
                }
            return true;
            }

        /// <summary>
        /// Helper function for the ICommand implementation.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanClickRadiobutton(object param)
            {
            return true;
            }

        /// <summary>
        /// Helper function for the ICommand implementation.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private bool CanClickDescriptionButton(object param)
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
        /// Displays a pop-up box with script information.
        /// </summary>
        private void GetScriptDescription(object descButton)
            {
            if (scriptReader.ScriptDescription != null &&
               !(scriptReader.ScriptDescription.Equals("")))
                {
                System.Windows.MessageBox.Show(scriptReader.ScriptDescription);
                }
            else if (scriptReader.ScriptDescription != null &&
                     scriptReader.ScriptDescription.Equals(""))
                {
                System.Windows.MessageBox.Show("No description provided");
                }
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
                throw new PsExecException("Cannot find any category directories in the script folder!", e.ToString(), true);
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

            RadioButtonChecked  = new PsGui.Converters.CommandHandler(GetSelectedScriptCategoryName, CanClickRadiobutton);
            ScriptDescriptionButtonPushed = new PsGui.Converters.CommandHandler(GetScriptDescription, CanClickDescriptionButton);
            
            //ExecuteButtonPushed = new PsGui.Converters.CommandHandler(ExecutePowershellScript, CanExecuteScript);
            ExecuteButtonPushed = new PsGui.Converters.CommandHandlerAsync(ExecutePowershellScriptAsync, CanExecuteScript);
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
                        // If new script selected, clear previous session
                        ScriptTextVariables.Clear();
                        ScriptUsernameVariables.Clear();
                        ScriptPasswordVariables.Clear();
                        ScriptMultiLineVariables.Clear();
                        ScriptVariables.Clear();

                        IsScriptSelected = true;
                        SelectedScriptPath = _modulePath + directoryReader.SelectedCategoryName + "\\" + value + ".ps1";
                        scriptReader.ReadSelectedScript(SelectedScriptPath);

                        if((ScriptExecutionOutput != null && ScriptExecutionOutput.Length > 0) ||
                           (ScriptExecutionErrorOutput != null && ScriptExecutionErrorOutput.Length > 0))
                            {
                            // If previous session output/errors, clear them
                            ScriptExecutionOutput = "";
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
                OnPropertyChanged("IsScriptSelected");
                }
            }

        /// <summary>
        /// Gets a collection of collections represnting the different
        /// types of script input variables and their values.
        /// </summary>
        public CompositeCollection ScriptVariables
            {
            get
                {
                return scriptReader.ScriptVariables;
                }
            }

        /// <summary>
        /// Gets a collection of strings representing the 
        /// script input text values.
        /// </summary>
        public ObservableCollection<TextArgument> ScriptTextVariables
            {
            get
                {
                return scriptReader.ScriptTextVariables;
                }
            }

        /// <summary>
        /// Gets a collection of strings representing the
        /// script input username values.
        /// </summary>
        public ObservableCollection<UsernameArgument> ScriptUsernameVariables
            {
            get
                {
                return scriptReader.ScriptUsernameVariables;
                }
            }

        /// <summary>
        /// Gets a collection of strings representing the
        /// script input password values.
        /// Security note: This leaves the clear text password in memory
        /// which is considered a security issue. However, if your
        /// RAM is accessible to attackers, you have bigger issues.
        /// </summary>
        public ObservableCollection<PasswordArgument> ScriptPasswordVariables
            {
            get
                {
                return scriptReader.ScriptPasswordVariables;
                }
            }

        /// <summary>
        /// Gets a collection of strings representing
        /// script input multi line text values.
        /// </summary>
        public ObservableCollection<MultiLineArgument> ScriptMultiLineVariables
            {
            get
                {
                return scriptReader.ScriptMultiLineVariables;
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
            IsScriptSelected = false;

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

        public string ScriptExecutionProgressOutput
        {
            get
            {
                return _scriptProgressOutput;
            }
            set
            {
                if(value != null)
                {
                    _scriptProgressOutput = value;
                    OnPropertyChanged("ScriptExecutionProgressOutput");
                }
            }
        }

        }
    }