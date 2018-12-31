using PsGui.Models.PowershellExecuter;
using System.IO;
using System.Collections.ObjectModel;

namespace PsGui.Models
    {
    public class DirectoryReader
        {
        private string modulePath;
        private string moduleFolderName;
        private string _selectedCategoryName;
        private bool   _isScriptSelected;
        private string _selectedScript;

        private ObservableCollection<ScriptCategory> _scriptCategories;
        private ObservableCollection<string> _scriptFiles;

        /// <summary>
        /// Constructor
        /// </summary>
        public DirectoryReader(string modulepath, string moduleFolderName)
            {
            this.moduleFolderName = moduleFolderName;
            this.modulePath       = modulepath;
            _scriptCategories     = new ObservableCollection<ScriptCategory>();
            _scriptFiles          = new ObservableCollection<string>();
            _isScriptSelected     = false;
            _selectedScript       = "";
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
            string categoryDirectoryPath = modulePath + "\\" + moduleFolderName + "\\";
            try
                {
                categoryList = Directory.GetDirectories(categoryDirectoryPath);
                foreach (string category in categoryList)
                    {
                    _scriptCategories.Add(new ScriptCategory(category));
                    }
                }
            catch (System.Exception e)
                {
                throw new PsGuiException("Models.DirectoryReader.UpdateScriptCategoriesList()");
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
            _scriptCategories.Clear();
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
        /// Sets or gets a collection of strings representing
        /// the script files in each category. The script files in 
        /// each directory.
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
        /// @Throws PSGuiException
        /// </summary>
        public void UpdateScriptFilesList()
            {
            string[] scriptList;
            string scriptDirectoryPath = modulePath + "\\" + moduleFolderName + "\\" + _selectedCategoryName + "\\";
            // Todo: const
            string fileExtension = ".ps1";
            int fileExtensionLength = fileExtension.Length;
            try
                {
                // TODO: scriptList gir exception?
                scriptList = Directory.GetFiles(scriptDirectoryPath, "*" + fileExtension);
                foreach (string script in scriptList)
                    {
                    string fileNameWithExtension = Path.GetFileName(script);
                    string fileNameWithoutExtension = (Path.GetFileName(script)).Substring(0, fileNameWithExtension.Length - fileExtensionLength);
                    _scriptFiles.Add(Path.GetFileName(fileNameWithoutExtension));
                    }
                }
            catch (System.Exception e)
                {
                throw new PsGuiException("Models.DirectoryReader.UpdateScriptFilesList(): " + e.ToString());
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
        /// Clears the list of script files.
        /// </summary>
        public void ClearScripts()
            {
            _scriptFiles.Clear();
            }




        }
    }
