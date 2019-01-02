using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    class PowershellExecuter
        {
        private List<string> commandLineArguments;
        private List<string> commandLineArgKeys;

        /// <summary>
        /// Gets the script execution output.
        /// </summary>
        public string ScriptOutput { get; private set; }

        /// <summary>
        /// Gets the script argument data from the supplied collection.
        /// </summary>
        /// <param name="scriptVariables"></param>
        private void GetScriptParameters(ObservableCollection<ScriptArgument> scriptVariables)
            {
            foreach (ScriptArgument var in scriptVariables)
                {
                string argKey = var.InputKey.ToString().ToLower();
                string argValue = var.InputValue.ToString().ToLower();
                commandLineArgKeys.Add(argKey);
                commandLineArguments.Add(argValue);
                }
            }

        /// <summary>
        /// Executes the powershell script in the provided
        /// file path. Saves the output.
        /// </summary>
        /// <param name="scriptPath"></param>
        public void ExecuteScriptCommands(string scriptPath)
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
                ScriptOutput = tmp.ToString();
                }
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public PowershellExecuter()
            {
            ScriptOutput         = "";
            commandLineArguments = new List<string>();
            commandLineArgKeys   = new List<string>();
            }

        /// <summary>
        /// Executes a provided scripts and handles cleanup
        /// for the next script execution.
        /// </summary>
        /// <param name="scriptPath"></param>
        public void ExecuteScript(string scriptPath, ObservableCollection<ScriptArgument> scriptVars)
            {
            GetScriptParameters(scriptVars);
            ExecuteScriptCommands(scriptPath);
            }

        /// <summary>
        /// Clears the script output.
        /// </summary>
        public void ClearScriptOutput()
            {
            ScriptOutput = "";
            }

        /// <summary>
        /// Clears stored data, excludes the scriptoutput.
        /// </summary>
        public void ClearSession()
            {
            commandLineArgKeys.Clear();
            commandLineArguments.Clear();
            }


        }
    }
