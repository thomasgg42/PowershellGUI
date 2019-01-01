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
        private string       _scriptOutput;
        private List<string> commandLineArguments;
        private List<string> commandLineArgKeys;

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
        private void ExecuteScriptCommands(string scriptPath)
            {
            using (PowerShell psInstance = PowerShell.Create())
                {
                // Add command
                psInstance.AddCommand(scriptPath);

                // Add arguments to command
                int argLength = commandLineArguments.Count;
                for (int ii = 0; ii < argLength; ii++)
                    {
                    psInstance.AddParameter(commandLineArgKeys[ii], commandLineArguments[ii]);
                    }

                // Execute script
                Collection<PSObject> psOutput = psInstance.Invoke();

                // Get output from execution
                StringBuilder tmp = new StringBuilder();
                foreach (PSObject output in psOutput)
                    {
                    if (output != null)
                        {
                        tmp.Append(output.ToString());
                        }
                    }
                ScriptExecutionOutput = tmp.ToString();
                }
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public PowershellExecuter()
            {
            _scriptOutput        = "";
            commandLineArguments = new List<string>();
            commandLineArgKeys   = new List<string>();
            }

        /// <summary>
        /// Sets or gets the powershell execution output.
        /// </summary>
        public string ScriptExecutionOutput
            {
            get
                {
                return _scriptOutput;
                }
            set
                {
                if(value != null)
                    {
                    _scriptOutput = value;
                    }
                }
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

        }
    }
