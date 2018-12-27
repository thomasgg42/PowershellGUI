using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    public class ScriptReader
        {
        private ObservableCollection<ScriptArgument> _scriptVariables;
        private string _fileUri;
        private string scriptDescription;
        private string scriptHeader;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScriptReader()
            {
            _scriptVariables = new ObservableCollection<ScriptArgument>();
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script command line input variables.
        /// </summary>
        public ObservableCollection<ScriptArgument> ScriptVariables
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

        /// <summary>
        /// Returns true if there are no registered script variables.
        /// </summary>
        /// <returns></returns>
        public bool ContainsVariables()
            {
            if(_scriptVariables != null && _scriptVariables.Count != 0)
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        /// <summary>
        /// Clears all script variables (input fields) for defined
        /// information.
        /// </summary>
        public void ClearScriptVariableInfo()
            {
            foreach(ScriptArgument arg in _scriptVariables)
                {
                arg.ClearUserInput();
                }
            }

        }
    }
