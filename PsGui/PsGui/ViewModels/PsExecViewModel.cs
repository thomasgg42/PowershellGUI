using PsGui.Models;
using PsGui.Models.PowershellExecuter;
using PsGui.Views;
using System.Collections.ObjectModel;
using System.IO;
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
        private PsExecException    powershellExecptions;
        private ArgumentChecker    argumentChecker;
        private ScriptArgument     scriptArgument;

        public ICommand RadioButtonChecked { get; set; } 

        private string _modulePath;
        private bool   _isScriptSelected;

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
        /// that INotifyPropertyChanged executes.
        /// </summary>
        /// <param name="radioBtnContent"></param>
        private void SetSelectedCategoryAsRadioButtonName(object radioBtnContent)
            {
            SelectedScriptCategory = radioBtnContent.ToString();
            // denne må kalle en bool funksjon for å kunne bindes mot
            // radio knapper
            }

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

            int firstCategory = 0;
            ScriptCategoryBrowser[firstCategory].IsSelectedCategory = true;
            SelectedScriptCategory = ScriptCategoryBrowser[firstCategory].FriendlyName;
            RadioButtonChecked = new PsGui.Converters.CommandHandler(SetSelectedCategoryAsRadioButtonName, CanExecuteRadioButtonCheck);
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
        /// ScriptCategory.FriendlyName.
        /// </summary>
        public string SelectedScriptCategory
            {
            get
                {
                return directoryReader.SelectedScript;
                }
            set
                {
                System.Windows.MessageBox.Show(value);
                if (value != null)
                    {
                    // When a new script is selected, remove values from previous input fields
                    // but keep the fields
                    scriptReader.ClearScriptVariableInfo();
                   // directoryReader.ClearCategories();
                    directoryReader.SelectedCategoryName = value;
                    UpdateScriptCategoriesList();
                    }
                }
            
            /*   
             get
                 {
                 foreach(ScriptCategory cat in ScriptCategoryBrowser)
                     {
                     if(cat.IsSelectedCategory)
                         {
                         return cat.FriendlyName;
                         }
                     }
                 throw new PsExecException("ViewModels.SelectedScriptCategory: No category selected");
                 }
             set
                 {
                 if(value != null)
                     {
                     // lag en Command som sender radio button sin content (friendlyName) hit
                     // sjekk deretter foreach category for matchende content
                     // når match, sett active

                     // Pdd. så sendes "true"/"false" til ScriptCategory IsSelectedCategory
                     // men har ingen måte å varsle (inotify) når dette utføres

                     foreach(ScriptCategory cat in ScriptCategoryBrowser)
                         {
                         if(cat.FriendlyName.Equals(value))
                             {
                             cat.IsSelectedCategory = true;
                             }
                         }

                     // When a new script is selected, remove values from previous input fields
                     // but keep the fields
                     scriptReader.ClearScriptVariableInfo();
                     directoryReader.ClearCategories();
                     directoryReader.SelectedCategoryName = value;
                     UpdateScriptCategoriesList();
                     }
                 }
             */
            }

        /// <summary>
        /// When a radio button is checked, this function
        /// checks if the radio button content (friendlyName)
        /// matches the registered selected category name and
        /// returns true if that is so.
        /// </summary>
        public bool IsSelectedScriptCategory
            {
            get
                {
                foreach (ScriptCategory cat in ScriptCategoryBrowser)
                    {
                    if (cat.FriendlyName.Equals(directoryReader.SelectedCategoryName)
                        && cat.IsSelectedCategory)
                        {
                        return true;
                        }
                    }
                return false;
                }
            set
                {
                foreach (ScriptCategory cat in ScriptCategoryBrowser)
                    {
                    if (cat.FriendlyName.Equals(directoryReader.SelectedCategoryName))
                        {
                        cat.IsSelectedCategory = value;
                        }
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
         //   OnPropertyChanged("ScriptCategoryBrowser");
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
        public bool ScriptCanExecute(object parameter)
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
