using System.IO;
using System.Collections.ObjectModel;

namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// Handles reading of directories to find
    /// powershell scripts to be executed.
    /// </summary>
    public class DirectoryReader
        {
        private string modulePath;
        private int    modulePathLength;
        private string _selectedCategoryName;
        private bool   _isScriptSelected;
        private string _selectedScript;
        private string _selectedScriptPath;

        private ObservableCollection<ScriptCategory> _scriptCategories;
        private ObservableCollection<string>         _scriptFiles;

        /// <summary>
        /// Constructor
        /// </summary>
        public DirectoryReader(string modulepath)
            {
            this.modulePath       = modulepath;
            modulePathLength      = this.modulePath.Length;
            _scriptCategories     = new ObservableCollection<ScriptCategory>();
            _scriptFiles          = new ObservableCollection<string>();
            _isScriptSelected     = false;
            _selectedScript       = "";
            _selectedScriptPath   = "";
            _selectedCategoryName = "";
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script categories, the script directories.
        /// </summary>
        public ObservableCollection<ScriptCategory> ScriptCategories
            {
            get
                {
                return _scriptCategories;
                }
            set
                {
                _scriptCategories = value;
                }
            }

        /// <summary>
        /// Fills the list of categories with the folders found 
        /// in the given module path.
        /// @Throws PSGuiException
        /// </summary>
        public void UpdateScriptCategoriesList()
            {
            string[] categoryList;
            try
                {
                categoryList = Directory.GetDirectories(modulePath);
                foreach (string category in categoryList)
                    {
                    _scriptCategories.Add(new ScriptCategory(category, modulePathLength));
                    }
                }
            catch (System.Exception e)
                {
                throw new PsExecException("No script directory found!", e.ToString(), true);
                }
            }

        /// <summary>
        /// Sets or gets the selected category in form of a 
        /// ScriptCategory.FriendlyName.
        /// </summary>
        public string SelectedCategoryName
            {
            get
                {
                return _selectedCategoryName;
                }
            set
                {
                _selectedCategoryName = value;
                }
            }

        /// <summary>
        /// Removes all stored items in the script categories
        /// list.
        /// </summary>
        public void ClearCategories()
            {
            if(_scriptCategories.Count > 0)
                {
                _scriptCategories.Clear();
                _selectedCategoryName = "";
                }
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script files in each category.
        /// </summary>
        public ObservableCollection<string> ScriptFiles
            {
            get
                {
                return _scriptFiles;
                }
            set
                {
                _scriptFiles = value;
                }
            }

        /// <summary>
        /// Updates the list of scripts based on the currently
        /// selected category.
        /// Finds all files with a .ps1 file extension in the 
        /// selected category folder and stores each file name
        /// excluding the file extension.
        /// </summary>
        public void UpdateScriptFilesList()
            {
            string[] scriptList;
            string scriptDirectoryPath = modulePath + _selectedCategoryName + "\\";
            // Todo: const
            string fileExtension = ".ps1";
            int fileExtensionLength = fileExtension.Length;
            scriptList = Directory.GetFiles(scriptDirectoryPath, "*" + fileExtension);
            foreach (string script in scriptList)
                {
                string fileNameWithExtension = Path.GetFileName(script);
                string fileNameWithoutExtension = (Path.GetFileName(script)).Substring(0, fileNameWithExtension.Length - fileExtensionLength);
                _scriptFiles.Add(Path.GetFileName(fileNameWithoutExtension));
                }
            }

        /// <summary>
        /// Sets or gets the selected powershell script
        /// in the selected category.
        /// </summary>
        public string SelectedScript
            {
            get
                {
                return _selectedScript;
                }
            set
                {
                _selectedScript = value;
                }
            }

        /// <summary>
        /// Sets or gets the full filepath
        /// to the selected script.
        /// </summary>
        public string SelectedScriptPath
            {
            get
                {
                return _selectedScriptPath;
                }
            set
                {
                _selectedScriptPath = value;
                }
            }

        /// <summary>
        /// Clears the list of script files.
        /// </summary>
        public void ClearScripts()
            {
            if(_scriptFiles.Count > 0)
                {
                _scriptFiles.Clear();
                _selectedScript   = "";
                _isScriptSelected = false;
                }
            }

        /// <summary>
        /// Returns true if a script is selected.
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
                }
            }

        /// <summary>
        /// Clears the stored data.
        /// </summary>
        public void ClearSesssion()
            {
            ClearCategories();
            ClearScripts();
            }

        }
    }
