﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// A class responsible for executing a powershell 
    /// script at a provided path with provided arguments.
    /// </summary>
    class PowershellExecuter
        {
        private List<string> commandLineArguments;
        private List<string> commandLineArgKeys;

        /// <summary>
        /// Gets the script execution output.
        /// </summary>
        public string ScriptOutput { get; private set; }

        /// <summary>
        /// Gets or sets the script execution error messages.
        /// </summary>
        public string ScriptErrors { get; private set; }

        /// <summary>
        /// Collects and saves error messages generated by the
        /// powershell script instance.
        /// </summary>
        /// <param name="instance"></param>
        private void CollectPowershellScriptErrors(PowerShell instance)
            {
            if (instance.HadErrors)
                {
                StringBuilder tmp = new StringBuilder();
                foreach (ErrorRecord err in instance.Streams.Error)
                    {
                    tmp.Append("**General message**");
                    tmp.Append('\n' + err.ToString());
                    tmp.Append("\n\n**Fully Qualified Error ID**");
                    tmp.Append('\n' + err.FullyQualifiedErrorId.ToString());
                    tmp.Append("\n\n**Stack trace**");
                    tmp.Append('\n' + err.ScriptStackTrace.ToString());
                    tmp.Append('\n' + "------------------------------\n\n");
                    }
                ScriptErrors = tmp.ToString();
                }
            else
                {
                // If a previous script has generated errors
                // But a new script does not generate errors
                // Then remove the previously recorded error
                if(ScriptErrors != null && ScriptErrors.Length > 0)
                    {
                    ScriptErrors = "";
                    }
                }
            }

        /// <summary>
        /// Collects and saves the output generated by the
        /// powershell script instance.
        /// </summary>
        /// <param name="instance"></param>
        private void CollectPowershellScriptoutput(Collection<PSObject> instanceOutput)
            {
            StringBuilder tmp = new StringBuilder();
            foreach (PSObject output in instanceOutput)
                {
                if (output != null)
                    {
                    tmp.Append(output.ToString());
                    }
                }
            ScriptOutput = tmp.ToString();
            }

        /// <summary>
        /// Gets the script argument data from the supplied composite collection
        /// consisting of multiple collection containers consisting of 
        /// observable objects.
        /// </summary>
        /// <param name="scriptVariables"></param>
        private void GetScriptParameters(CompositeCollection scriptVariables)
            {
            foreach(CollectionContainer collection in scriptVariables)
                {
                foreach(ScriptArgument arg in collection.Collection)
                    {
                    string argKey = arg.InputKey.ToString().ToLower();
                    string argValue = arg.InputValue.ToString();
                    commandLineArgKeys.Add(argKey);
                    commandLineArguments.Add(argValue);
                    }
                }
            }

        /// <summary>
        /// Executes the powershell script in the provided
        /// file path. Calls functions responsible to collect output.
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
                
                // Prevents displaying objects as objects
                psInstance.AddCommand("Out-String");

                Collection<PSObject> psOutput = psInstance.Invoke();

                CollectPowershellScriptoutput(psOutput);
                CollectPowershellScriptErrors(psInstance);
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
        /// Executes the powershell script at the provided file path
        /// with the provided Composite collection consiting of the
        /// different types of input values.
        /// </summary>
        /// <param name="scriptPath"></param>
        /// <param name="scriptVars"></param>

        public void ExecuteScript(string scriptPath, CompositeCollection scriptVars)
            {
            GetScriptParameters(scriptVars);
            ExecuteScriptCommands(scriptPath);
            }

        public void ExecuteScriptAsync(string scriptPath, CompositeCollection scriptVars)
        {
            GetScriptParameters(scriptVars);
            ExecuteScriptCommandsAsync(scriptPath);
        }

        public void ExecuteScriptCommandsAsync(string scriptPath)
        {
            using (PowerShell psInstance = PowerShell.Create())
            {
                psInstance.AddCommand(scriptPath);
                int argLength = commandLineArguments.Count;
                for (int ii = 0; ii < argLength; ii++)
                {
                    psInstance.AddParameter(commandLineArgKeys[ii], commandLineArguments[ii]);
                }

                // Prevents displaying objects as objects
                psInstance.AddCommand("Out-String");

                Collection<PSObject> psOutput = psInstance.Invoke();

                CollectPowershellScriptoutput(psOutput);
                CollectPowershellScriptErrors(psInstance);
            }
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
