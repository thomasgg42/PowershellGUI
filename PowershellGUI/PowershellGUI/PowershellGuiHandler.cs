using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI
    {
    class PowershellGUIHandler
        {

        private ViewModel.DirectoryReader  directoryReader;
        private ViewModel.FileReader       fileReader;
        private ViewModel.PowershellParser psParser;

        public string ModulePath { get; set; }
        /*
         * Constructor
         */
        public PowershellGUIHandler()
            {
            directoryReader = new ViewModel.DirectoryReader();
            fileReader      = new ViewModel.FileReader();
            psParser        = new ViewModel.PowershellParser();
            }
        }
    }
