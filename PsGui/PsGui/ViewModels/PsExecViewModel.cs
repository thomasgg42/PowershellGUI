using PsGui.Models;
using PsGui.Models.PowershellExecuter;
using PsGui.Views;

namespace PsGui.ViewModels
    {
    /// <summary>
    /// Executes powershell scripts with command line arguments
    /// in the form of user input.
    /// </summary>
    public class PsExecViewModel : ObservableObject
        {
        private DirectoryReader    directoryReader;
        private FileReader         fileReader;
        private PowershellExecuter powershellExecuter;
        private PsExecException    powershellExecptions;
        private ArgumentChecker    argumentChecker;
        private ScriptArgument     scriptArgument;

        private string _modulePath;
        private string _selectedPsScript;
        private bool   _isScriptSelected;

        public PsExecViewModel(string modulePath)
            {
            _modulePath = modulePath;
            }

        /// <summary>
        /// Returns true if a selected powershell script
        /// is ready to be executed. False otherwise.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
            {
            return false;
            }

        /// <summary>
        /// Sets or gets the selected powershell script.
        /// </summary>
        public string SelectedScript
            {
            get
                {
                return _selectedPsScript;
                }
            set
                {
                _selectedPsScript = value;
                }

            }

        /// <summary>
        /// Returns true if a powershell script has been selected.
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

        public string ModulePath
            {
            get
                {
                return _modulePath;
                }
            set
                {
                _modulePath = value;
                }
            }

        }
    }
