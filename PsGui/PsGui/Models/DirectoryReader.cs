using PsGui.Models.PowershellExecuter;
using System.IO;
using System.Collections.ObjectModel;

namespace PsGui.Models
    {
    public class DirectoryReader
        {
        private string modulePath;
        private string _selectedCategory;
        private bool   _isScriptSelected;
        private string _selectedScript;

        private ObservableCollection<string> _scriptCategories;
        private ObservableCollection<string> _scriptFiles;

        /// <summary>
        /// Constructor
        /// </summary>
        public DirectoryReader(string modulepath)
            {
            this.modulePath = modulepath;
            _scriptCategories = new ObservableCollection<string>();
            _scriptFiles      = new ObservableCollection<string>();
            _isScriptSelected = false;
            _selectedScript   = "";
            _selectedCategory = "";
            UpdateScriptCategories();
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
        /// Sets or gets a collection of strings representing
        /// the script categories, the script directories.
        /// </summary>
        public ObservableCollection<string> ScriptCategories
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
        /// Sets or gets the selected category in form of a 
        /// script directory and a radio button in the GUI.
        /// </summary>
        public string SelectedCategory
            {
            get
                {
                return _selectedCategory;
                }
            set
                {
                _selectedCategory = value;
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
        /// Fills the list of script categories based on
        /// the currently selected category.
        /// @Throws PsGuiException
        /// </summary>
        public void UpdateScriptCategories()
            {
            string[] scriptList;
            string scriptDirectoryPath = modulePath + "\\" + _selectedCategory + "\\";
            // Todo: const
            string fileExtension = ".ps1";
            int fileExtensionLength = fileExtension.Length;
            try
                {
                scriptList = Directory.GetFiles(scriptDirectoryPath, "*" + fileExtension);
                foreach (string script in scriptList)
                    {
                    string fileNameWithExtension = Path.GetFileName(script);
                    string fileNameWithoutExtension = (Path.GetFileName(script)).Substring(0, fileNameWithExtension.Length - fileExtensionLength);
                    ScriptCategories.Add(Path.GetFileName(fileNameWithoutExtension));
                    }
                }
            catch (System.Exception e)
                {
                throw new PsGuiException("Models.DirectoryReader.UpdateScriptcategories()");
                }
            }

        }
    }
