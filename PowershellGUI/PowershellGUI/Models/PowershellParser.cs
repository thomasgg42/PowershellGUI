using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections.Generic;

namespace PowershellGUI.Models
    {
    class PowershellParser : ObservableObject
        {
        private string _scriptOutput;
        private List<string> commandLineArguments;
        private List<string> commandLineArgKeys;

        /// <summary>
        /// Sets the script output which is to be shown
        /// to the user.
        /// </summary>
        /// <param name="output">String output from the PS-script.</param>
        private void SetScriptOutput(string output)
            {
            ScriptOutput = output;
            }

        private void ExecuteScript(string scriptPath)
            {
            using (PowerShell psInstance = PowerShell.Create())
                {
                psInstance.AddCommand(scriptPath);
                int argLength = commandLineArguments.Count;
                for (int ii = 0; ii < argLength; ii++)
                    {
                    psInstance.AddParameter(commandLineArgKeys[ii], commandLineArguments[ii]);
                    }

                Collection<PSObject> psOutput = psInstance.Invoke();

                StringBuilder tmp = new StringBuilder();
                foreach (PSObject output in psOutput)
                    {
                    if (output != null)
                        {
                        tmp.Append(output.ToString());
                        }
                    }
                SetScriptOutput(tmp.ToString());
                }
            }

        private void GetScriptParameters(ObservableCollection<ScriptArgument> scriptVariables)
            {
            foreach (ScriptArgument obj in scriptVariables)
                {
                string argKey = obj.InputKey.ToString().ToLower();
                string argValue = obj.InputValue.ToString().ToLower();
                // string argument = "-" + argKey + " \"" + argValue + "\"";
                commandLineArgKeys.Add(argKey);
                commandLineArguments.Add(argValue);
                }
            }


        /// <summary>
        /// Constructor
        /// </summary>
        public PowershellParser()
            {
            commandLineArguments = new List<string>();
            commandLineArgKeys = new List<string>();
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

        /// <summary>
        /// Gets the script input arguments
        /// and runs the script.
        /// </summary>
        /// <param name="scriptPath">Filepath to script.</param>
        /// <param name="scriptVariables">Collection of input arguments</param>
        public void RunPsScript(string scriptPath, ObservableCollection<ScriptArgument> scriptVariables)
            {
            GetScriptParameters(scriptVariables);
            ExecuteScript(scriptPath);
            }

        }
    }

/*
 *                 PowerShellInstance.AddScript("start-sleep -s 7; get-service");
                IAsyncResult result = PowerShellInstance.BeginInvoke();
                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    Thread.Sleep(1000);
                }
                Console.WriteLine("Finished!");
 * 
 */
