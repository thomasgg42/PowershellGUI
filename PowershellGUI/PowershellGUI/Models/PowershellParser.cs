using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Text;

namespace PowershellGUI.Models
    {
    class PowershellParser : ObservableObject
        {
        private string _scriptOutput;

        /// <summary>
        /// Constructor
        /// </summary>
        public PowershellParser()
            {
            // Get file from FileReader.FileURI
            // execute file with input values from gui
            // display output
            }

        public string ScriptOutput
            {
            get
                {
                return _scriptOutput;
                }
            set
                {
                _scriptOutput = value;
                OnPropertyChanged("ScriptOutput");
                }
            }

        public void ExecuteScript(string scriptPath)
            {
            using (PowerShell psInstance = PowerShell.Create())
                {
                psInstance.AddScript(scriptPath);
                psInstance.AddCommand("Out-string");
                Collection<PSObject> psOutput = psInstance.Invoke();

                StringBuilder tmp = new StringBuilder();
                foreach(PSObject output in psOutput)
                    {
                    tmp.Append(output.ToString());
                    }
                ScriptOutput = tmp.ToString();
                }
            }
        }
    }
