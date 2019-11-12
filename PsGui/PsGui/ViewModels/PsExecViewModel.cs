using PsGui.Models.PowershellExecuter;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
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

        private const string newLine = "\r\n";

        private bool   _isBusy;
        private bool   _canInteract;
        // Module path should be renamed scriptFolderPath. The word
        // module should not be confused with a Powershell-module.
        // The modulePath contains the relative file path to the scripts folder.
        private string _modulePath;

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

            // Outdated since async implemented
            // ScriptExecutionOutputStandard = powershellExecuter.ScriptExecutionOutput;
            ScriptExecutionErrorException = powershellExecuter.ScriptExecutionErrorException;
            ClearScriptSession();
        }

        /// <summary>
        /// Executes a powershell script at the supplied path asynchronously.
        /// Output stream data is saved in real time as the script executes.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private async Task ExecutePowershellScriptAsync(object obj)
        {
            // Disable UI elements
            IsBusy = true;
            scriptReader.SetArgumentsEnabled(false);

            // Add user input to script parameters
            powershellExecuter.SetScriptParameters(ScriptVariables);

            // Execute script asynchronously
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
                    outputCollection.DataAdded            += OutputCollection_DataAdded;
                    psInstance.Streams.Error.DataAdded    += Error_DataAdded;
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
                }
            });

            IsBusy = false;
            scriptReader.SetArgumentsEnabled(true);
            ClearScriptSession();
        }

        /// <summary>
        /// Event handler for when data is added to the output stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void OutputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            FilterScriptExecutionOutput(((PSDataCollection<PSObject>)sender)[e.Index].ToString());
        }

        /// <summary>
        /// Event handler for when Data is added to the Error stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all error output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            //   ScriptExecutionErrorCustom                     = ((PSDataCollection<ErrorRecord>)sender)[e.Index].Exception.Message;
            ScriptExecutionOutputCustom.Add(new CustomOutput(((PSDataCollection<ErrorRecord>)sender)[e.Index].Exception.Message, CustomOutput.Types.Error));

            ScriptExecutionErrorTargetObject               = ((PSDataCollection<ErrorRecord>)sender)[e.Index].TargetObject.ToString();
            ScriptExecutionErrorScriptStackTrace           = ((PSDataCollection<ErrorRecord>)sender)[e.Index].ScriptStackTrace;
            ScriptExecutionErrorFullyQualifiedErrorId      = ((PSDataCollection<ErrorRecord>)sender)[e.Index].FullyQualifiedErrorId;
            //   ScriptExecutionErrorCategoryInfo          = ((PSDataCollection<ErrorRecord>)sender)[e.Index].CategoryInfo.ToString();
            //   ScriptExecutionErrorException             = ((PSDataCollection<ErrorRecord>)sender)[e.Index].Exception.ToString();
            //   ScriptExecutionErrorDetails               = ((PSDataCollection<ErrorRecord>)sender)[e.Index].ErrorDetails.ToString();
            //   ScriptExecutionErrorInvocationInfo        = ((PSDataCollection<ErrorRecord>)sender)[e.Index].InvocationInfo.ToString();
            //   ScriptExecutionErrorPipelineIterationInfo = ((PSDataCollection<ErrorRecord>)sender)[e.Index].PipelineIterationInfo.ToString();
            string errorSeparator = "=======================================================" + newLine;
            ScriptExecutionErrorRaw += errorSeparator;
        }

        /// <summary>
        /// Event handler for when Data is added to the Progress stream.
        /// </summary>
        /// <param name="sender">Contains the complete PSDataCollection of all progress output items.</param>
        /// <param name="e">Contains the index ID of the added collection item and the ID of the PowerShell instance this event belongs to.</param>
        private void Progress_DataAdded(object sender, DataAddedEventArgs e)
        {
            ScriptExecutionProgressPercentComplete = ((PSDataCollection<ProgressRecord>)sender)[e.Index].PercentComplete.ToString();
            ScriptExecutionProgressCurrentOperation = ((PSDataCollection<ProgressRecord>)sender)[e.Index].StatusDescription.ToString();
        }

        /// <summary>
        /// Gets the script output and filters it into the different types
        /// of script output. Calls the properties responsible for each of
        /// the output types.
        /// </summary>
        private void FilterScriptExecutionOutput(string output)
        {
            // OutputStreamsContainsData and Clear-functions
            // must be updated upon updating this function

            int custPrefixLength = powershellExecuter.CustomOutputPrefix.Length;
            output += newLine;

            if (output.Substring(0, custPrefixLength).Equals(powershellExecuter.CustomOutputPrefix))
            {
                // If Write-Output is custom output

                // Custom output should contain Write-Error too!
                // ScriptExecutionOutputCustom += output.Substring(custPrefixLength);
                ScriptExecutionOutputCustom.Add(new CustomOutput(output.Substring(0, custPrefixLength), CustomOutput.Types.Output));
            }
            else
            {
                // Miss-typed and unspecified Write-Output contents are not filtered
                ScriptExecutionOutputRaw += output;
            }
        }

        /// <summary>
        /// Returns true if a selected powershell script
        /// is ready to be executed. False otherwise.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CanExecuteScript(object parameter)
            {
            if(IsBusy == true)
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

                // Her må vi sjekke for noe annet
                /*
                foreach (ScriptArgument arg in ScriptCheckboxVariables)
                {
                    if (arg.InputValue != null && arg.InputValue.Equals(""))
                    {
                        return false;
                    }
                }
                */
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
        /// Provides a replacement for the Windows.MessageBox by using
        /// Material Design and DialogHost. The DialogHost uses its own
        /// View and ViewModel to define the looks and feels.
        /// The RootDialog is defined in MainWindow.xaml and acts as a 
        /// container of the contents that is to be set inactive while
        /// the box is active.
        /// </summary>
        /// <param name="descButton">Sender</param>
        /// <returns>void task</returns>
        private async Task GetScriptDescriptionAsync(object descButton)
        {
            string msg   = "No script description provided.";
            string title = SelectedScriptFile;

            if (scriptReader.ScriptDescription != null &&
               !(scriptReader.ScriptDescription.Equals("")))
            {
                msg = scriptReader.ScriptDescription;
            }

            var view = new Views.DialogBoxView
            {
                DataContext = new DialogBoxViewModel(title, msg)
            };

            await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
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
            IsBusy                = false;
            CanInteract           = true;
            _modulePath           = modulePath + "\\" + moduleFolderName + "\\";
            directoryReader       = new DirectoryReader(_modulePath);
            scriptReader          = new ScriptReader();
            powershellExecuter    = new PowershellExecuter();

            UpdateScriptCategoriesList();
            SetInitialScriptCategory();

            RadioButtonChecked            = new PsGui.Converters.CommandHandler(GetSelectedScriptCategoryName, CanClickRadiobutton);
            ScriptDescriptionButtonPushed = new PsGui.Converters.CommandHandlerAsync(GetScriptDescriptionAsync, CanClickDescriptionButton);
            ExecuteButtonPushed           = new PsGui.Converters.CommandHandlerAsync(ExecutePowershellScriptAsync, CanExecuteScript);

            // ScriptDescriptionButtonPushed = new PsGui.Converters.CommandHandler(GetScriptDescription, CanClickDescriptionButton);
            //ExecuteButtonPushed = new PsGui.Converters.CommandHandler(ExecutePowershellScript, CanExecuteScript);

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
                if (value != null)
                {
                    // The setter runs when set to empty string as well as a script name
                    if (value != "")
                    {
                        // If new script selected, clear previous session
                        ScriptTextVariables.Clear();
                        ScriptUsernameVariables.Clear();
                        ScriptPasswordVariables.Clear();
                        ScriptMultiLineVariables.Clear();
                        ScriptCheckboxVariables.Clear();
                        ScriptVariables.Clear();

                        directoryReader.SelectedScript = value;
                        IsScriptSelected = true;
                        SelectedScriptPath = _modulePath + directoryReader.SelectedCategoryName + "\\" + value + ".ps1";
                        scriptReader.ReadSelectedScript(SelectedScriptPath);
                        ScriptExecutionProgressPercentComplete  = "0";
                        ScriptExecutionProgressCurrentOperation = "";
                        // If previous session output/errors, clear them
                        if (StreamsContainsData())
                        {
                            ClearStreams();
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
                if (value != null)
                {
                    directoryReader.SelectedScriptPath = value;
                }
            }
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
        /// Sets or gets the busy state. Enables or disables 
        /// script argument changes.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                CanInteract = !_isBusy;
            }
        }

        /// <summary>
        /// Sets or gets the UI element interaction state.
        /// </summary>
        public bool CanInteract
        {
            get
            {
                return _canInteract;
            }
            set
            {
                _canInteract = value;
                OnPropertyChanged("CanInteract");
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
        /// Gets a collection of strings representing
        /// script input checkbox values.
        /// </summary>
        public ObservableCollection<CheckboxArgument> ScriptCheckboxVariables
        {
            get
            {
                return scriptReader.ScriptCheckboxVariables;
            }
        }

        /// <summary>
        /// Set or get a collection of CustomOutput containing either script output
        /// or script error output defined by the user.
        /// </summary>
        public ObservableCollection<CustomOutput> ScriptExecutionOutputCustom
        {
            get
            {
                return powershellExecuter.ScriptExecutionOutputCustom;
            }
            set
            {
                if(value != null)
                {
                    powershellExecuter.ScriptExecutionOutputCustom = value;
                }
            }
        }


        /// <summary>
        /// Gets the standard output generated by the executed
        /// powershell script.
        /// </summary>
        /*  public string ScriptExecutionOutputStandard
          {
              get
              {
                  return powershellExecuter.ScriptExecutionOutputStandard;
              }
              set
              {
                  if (value != null)
                  {
                      powershellExecuter.ScriptExecutionOutputStandard = value;
                      OnPropertyChanged("ScriptExecutionOutputStandard");
                  }
              }
          }
          */

        /// <summary>
        /// Gets the raw output generated by the executed
        /// powershell script.
        /// </summary>
        public string ScriptExecutionOutputRaw
        {
            get
            {
                return powershellExecuter.ScriptExecutionOutputRaw;
            }
            set
            {
                if(value != null)
                {
                    powershellExecuter.ScriptExecutionOutputRaw = value;
                    OnPropertyChanged("ScriptExecutionOutputRaw");
                }
            }
        }

        /// <summary>
        /// Gets the custom output, written by the developer
        /// while scripts are executed in powershell.
        /// </summary>

       /*
        public string ScriptExecutionOutputCustom
        {
            get
            {
                return powershellExecuter.ScriptExecutionOutputCustom;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptCustomOutput.Add(new CustomOutput(value, CustomOutput.Types.Output));
                  //  powershellExecuter.ScriptExecutionOutputCustom = value;
                    OnPropertyChanged("ScriptExecutionOutputCustom");
                }
            }
        }
        */

        /// <summary>
        /// Gets the Percent Completed progress status
        /// output generated by the executed powershell script.
        /// </summary>
        public string ScriptExecutionProgressPercentComplete
        {
            get
            {
                return powershellExecuter.ScriptExecutionProgressPercentComplete;
            }
            set
            {
                if(value != null)
                {
                    powershellExecuter.ScriptExecutionProgressPercentComplete = value;
                    OnPropertyChanged("ScriptExecutionProgressPercentComplete");
                }
            }
        }

        /// <summary>
        /// Gets the Current Operation progress status
        /// output generated by the executed powershell script.
        /// </summary>
        public string ScriptExecutionProgressCurrentOperation
        {
            get
            {
                return powershellExecuter.ScriptExecutionProgressCurrentOperation;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionProgressCurrentOperation = value;
                    OnPropertyChanged("ScriptExecutionProgressCurrentOperation");
                }
            }
        }

        /// <summary>
        /// Gets the error custom output, written by the developer
        /// while scripts are executed in powershell.
        /// </summary>
     /*   public string ScriptExecutionErrorCustom
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorCustom;
            }
            set
            {
                if (value != null)
                {
                  //  powershellExecuter.ScriptCustomOutput.Add(new CustomOutput(value, CustomOutput.Types.Error));
                   // powershellExecuter.ScriptExecutionErrorCustom = value;
                    OnPropertyChanged("ScriptExecutionErrorCustom");
                }
            }
        }
        */

        /// <summary>
        /// Gets the error output in its raw form, consisting
        /// of all the different properties.
        /// </summary>
        public string ScriptExecutionErrorRaw
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorRaw;
            }
            set
            {
                if(value != null)
                {
                    if(!value.Equals(""))
                    {
                        // If adding additional error text
                        powershellExecuter.ScriptExecutionErrorRaw = value;
                        powershellExecuter.ScriptExecutionErrorRaw += newLine;
                    }
                    else
                    {
                        // If clearing the error text
                        powershellExecuter.ScriptExecutionErrorRaw = value;
                    }
                }
                OnPropertyChanged("ScriptExecutionErrorRaw");
            }
        }

        /// <summary>
        /// Gets the error TargetObject output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorTargetObject
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorTargetObject;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorTargetObject = value;
                    ScriptExecutionErrorRaw += value;
                    OnPropertyChanged("ScriptExecutionErrorTargetObject");
                }
            }
        }

        /// <summary>
        /// Gets the error ScriptStackTrace output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorScriptStackTrace
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorScriptStackTrace;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorScriptStackTrace = value;
                    ScriptExecutionErrorRaw                                += value;
                    OnPropertyChanged("ScriptExecutionErrorScriptStackTrace");
                }
            }
        }

        /// <summary>
        /// Gets the error FullyQualifiedErrorId output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorFullyQualifiedErrorId
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorFullyQualifiedErrorId;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorFullyQualifiedErrorId = value;
                    ScriptExecutionErrorRaw                                     += value;
                    OnPropertyChanged("ScriptExecutionErrorFullyQualifiedErrorId");
                }
            }
        }

        /// <summary>
        /// Gets the error Exception output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorException
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorException;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorException = value;
                    ScriptExecutionErrorRaw                          += value;
                    OnPropertyChanged("ScriptExecutionErrorOutput");
                }
            }
        }

        /// <summary>
        /// Gets the error Details output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorDetails
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorDetails;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorDetails = value;
                    ScriptExecutionErrorRaw                       += value;
                    OnPropertyChanged("ScriptExecutionProgressCurrentOperation");
                }
            }
        }

        /// <summary>
        /// Gets the error CategoryInfo output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorCategoryInfo
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorCategoryInfo;
            }
            set
            {
                if(value != null)
                {
                    powershellExecuter.ScriptExecutionErrorCategoryInfo = value;
                    ScriptExecutionErrorRaw                            += value;
                    OnPropertyChanged("ScriptExecutionErrorCategoryInfo");
                }
            }
        }

        /// <summary>
        /// Gets the error InvocationInfo output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorInvocationInfo
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorInvocationInfo;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorInvocationInfo  = value;
                    ScriptExecutionErrorRaw                               += value;
                    OnPropertyChanged("ScriptExecutionErrorInvocationInfo");
                }
            }
        }

        /// <summary>
        /// Gets the error PipelineIterationInfo output generated by the executed 
        /// powershell script.
        /// </summary>
        public string ScriptExecutionErrorPipelineIterationInfo
        {
            get
            {
                return powershellExecuter.ScriptExecutionErrorPipelineIterationInfo;
            }
            set
            {
                if (value != null)
                {
                    powershellExecuter.ScriptExecutionErrorPipelineIterationInfo  = value;
                    ScriptExecutionErrorRaw                                      += value;
                    OnPropertyChanged("ScriptExecutionErrorPipelineIterationInfo");

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
        /// Returns true if any of the output streams contains data.
        /// </summary>
        /// <returns>True/false</returns>
        public bool StreamsContainsData()
        {
            // Only checking ErrorRaw because this property will be updated
            // for each of the specific error properties
            if((ScriptExecutionOutputRaw                != null && ScriptExecutionOutputRaw.Length > 0) ||
               (ScriptExecutionOutputCustom             != null && ScriptExecutionOutputCustom.Count > 0) ||
               (ScriptExecutionProgressPercentComplete  != null && !ScriptExecutionProgressPercentComplete.Equals("0")) ||
               (ScriptExecutionProgressCurrentOperation != null && ScriptExecutionProgressCurrentOperation.Length > 0) ||
               (ScriptExecutionErrorRaw                 != null && ScriptExecutionErrorRaw.Length > 0))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Clears the the different output streams for data. Setting them to their default values.
        /// </summary>
        public void ClearStreams()
        {
            ScriptExecutionOutputCustom.Clear();
            ClearOutputStreams();
            ClearProgressStreams();
            ClearErrorStreams();
        }

        /// <summary>
        /// Clears the output streams. Setting them to an empty string.
        /// </summary>
        public void ClearOutputStreams()
        {
            ScriptExecutionOutputRaw = "";
            // ScriptExecutionOutputCustom = "";
        }

        /// <summary>
        /// Clears the progress streams. Reseting the percentage stream back to 0
        /// and the rest of the progress streams back to an empty string.
        /// </summary>
        public void ClearProgressStreams()
        {
            ScriptExecutionProgressCurrentOperation = "";
            ScriptExecutionProgressPercentComplete = "0";
        }

        /// <summary>
        /// Clears the output error streams for data. Setting them to an empty string.
        /// </summary>
        public void ClearErrorStreams()
        {
            //  ScriptExecutionErrorCustom                = "";
            ScriptExecutionErrorRaw                   = "";
            ScriptExecutionErrorTargetObject          = "";
            ScriptExecutionErrorScriptStackTrace      = "";
            ScriptExecutionErrorFullyQualifiedErrorId = "";
            ScriptExecutionErrorFullyQualifiedErrorId = "";
            ScriptExecutionErrorDetails               = "";
            ScriptExecutionErrorException             = "";
            ScriptExecutionErrorCategoryInfo          = "";
            ScriptExecutionErrorInvocationInfo        = "";
            ScriptExecutionErrorPipelineIterationInfo = "";
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
            IsBusy           = false;
        }

    }
}