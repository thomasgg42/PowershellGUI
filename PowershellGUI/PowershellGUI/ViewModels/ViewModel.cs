using PowershellGUI.Models;
using System.Windows.Input;

namespace PowershellGUI.ViewModels
    {
    class ViewModel
        {
        private DirectoryReader     _DirectoryReader;
        private FileReader          _FileReader;
        private PowershellParser    _PowershellParser;
        private ComparisonConverter _ComparisonConverter;

        private ICommand _clickCommand;
        private bool     _canExecute;

        /// <summary>
        /// Handles the click-functionality of the Run script button
        /// </summary>
        public ICommand ClickCommand
            {
            get
                {
                // "Coalescing operator"
                return _clickCommand ?? (_clickCommand = new CommandHandler(() => ExecutePowershellScript(), _canExecute));
                }
            }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modulePath"></param>
        public ViewModel(string modulePath)
            {
            // Run script knappen er aktiv
            _canExecute = true;

            _DirectoryReader = new DirectoryReader(modulePath);
            _FileReader = new FileReader();
            _PowershellParser = new PowershellParser();
            _ComparisonConverter = new ComparisonConverter();
            }

        /// <summary>
        /// Gets the DirectoryReader instance
        /// </summary>
        public DirectoryReader DirectoryReader
            {
            get
                {
                return _DirectoryReader;
                }
            }

        /// <summary>
        /// Gets the FileReader instance
        /// </summary>
        public FileReader FileReader
            {
            get
                {
                return _FileReader;
                }
            }

        /// <summary>
        /// Gets the PowershellParser instance
        /// </summary>
        public PowershellParser PowershellParser
            {
            get
                {
                return _PowershellParser;
                }
            }

        /// <summary>
        /// Gets the ComparisonConverter instance
        /// </summary>
        public ComparisonConverter ComparisonConverter
            {
            get
                {
                return _ComparisonConverter;
                }
            }

        /// <summary>
        /// Executes a Powershell script
        /// Passes data between DirectoryReader, FileReader
        /// and PowershellParser.
        /// </summary>
        public void ExecutePowershellScript()
            {
            // button click kaller på executePowershellScript()
            FileReader.FileURI = DirectoryReader.SelectedPsScript;
            FileReader.ReadFile();
            PowershellParser.ExecuteScript();
            }
        }
    }
