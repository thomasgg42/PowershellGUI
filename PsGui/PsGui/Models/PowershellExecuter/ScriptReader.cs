using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    class ScriptReader
        {
        private ObservableCollection<string> _scriptVariables;

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script command line input variables.
        /// </summary>
        public ObservableCollection<string> ScriptVariables
            {
            get
                {
                return _scriptVariables;
                }
            set
                {
                _scriptVariables = value;
                }
            }
        }
    }
