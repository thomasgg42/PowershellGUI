using PsGui.Models;
using PsGui.Models.PowershellExecuter;
using PsGui.Views;
using System.Collections.ObjectModel;
using System.IO;

namespace PsGui.ViewModels
    {
    /// <summary>
    /// Executes powershell scripts with command line arguments
    /// in the form of user input.
    /// </summary>
    public class PsExecViewModel
        {
        public string TabName { get; } = "Script Executer";

        private DirectoryReader    directoryReader;
        private ScriptReader       scriptReader;
        private PowershellExecuter powershellExecuter;
        private PsExecException    powershellExecptions;
        private ArgumentChecker    argumentChecker;
        private ScriptArgument     scriptArgument;

        private string _modulePath;
        private bool  _isScriptSelected;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modulePath"></param>
        public PsExecViewModel(string modulePath, string moduleFolderName)
            {
            _modulePath = modulePath;
            scriptReader = new ScriptReader();
            directoryReader = new DirectoryReader(modulePath, moduleFolderName);
            UpdateScriptCategoriesList();
            /*
            for(int ii = 0; ii < 9; ii++)
                {
                ScriptCategoryBrowser.Add("ActiveDirectory" + ii);
                }
                */
            // Nå skal dropdown menu kunne populeres, bekreft før fortsettelse
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
                else
                    {
                    System.Windows.MessageBox.Show("ScriptCategoryBrowser null");
                    }
                }
            }

        /// <summary>
        /// Sets or gets the selected category in form of a 
        /// script directory and a radio button in the GUI.
        /// </summary>
        public string SelectedScriptCategory
            {
            get
                {
                return directoryReader.SelectedCategory;
                }
            set
                {
                if(value != null)
                    {
                    // When a new script is selected, remove values from previous input fields
                    // but keep the fields
                    scriptReader.ClearScriptVariableInfo();
                    directoryReader.ClearCategories();
                    directoryReader.SelectedCategory = value;
                    //    directoryReader.UpdateScriptCategories();
                    UpdateScriptCategoriesList();
                    }
                }
            }

        /// <summary>
        /// Fills the list of script categories based on
        /// the currently selected category.
        /// @Throws PsGuiException
        /// </summary>
        public void UpdateScriptCategoriesList()
            {
            directoryReader.UpdateScriptCategoriesList();
            }


        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script files in each category. The script files in 
        /// each directory.
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
        /// Sets or gets the selected powershell script.
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
                        IsScriptSelected = true;
                        } 
                    directoryReader.SelectedScript = value;
                    }
                else
                    {
                    System.Windows.MessageBox.Show("SelectedScriptFile null");
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
                directoryReader.IsScriptSelected = value; // bool never null
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
            set
                {
                if(value != null)
                    {
                    scriptReader.ScriptVariables = value;
                    }
                }
            }

        /// <summary>
        /// Returns true if a selected powershell script
        /// is ready to be executed. False otherwise.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
            {
            bool canExec = true;
            if(IsScriptSelected == false)
                {
                canExec = false;
                }
            foreach(ScriptArgument arg in ScriptVariables)
                {
                if (arg.InputValue.Equals(""))
                    {
                    canExec = false;
                    }
                }
            return canExec;
            }

        }
    }
