using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models
    {
    public class DirectoryReader
        {
        private string _categorySelected;
        private bool   _isScriptSelected;
        private string _selectedScript;

        private ObservableCollection<string> _scriptCategories;
        private ObservableCollection<string> _scriptFiles;

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
                return _categorySelected;
                }
            set
                {
                _categorySelected = value;
                }
            }



        }
    }
