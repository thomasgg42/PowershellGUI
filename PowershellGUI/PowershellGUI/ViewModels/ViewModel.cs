using PowershellGUI.Models;

namespace PowershellGUI.ViewModels
    {
    class ViewModel
        {
        private DirectoryReader _DirectoryReader;
        private FileReader _FileReader;
        private PowershellParser _PowershellParser;



        public ViewModel()
            {
            _DirectoryReader = new DirectoryReader("DirectoryReaderTest");
            _FileReader = new FileReader("FileReaderTest");
            _PowershellParser = new PowershellParser();
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
        }
    }
