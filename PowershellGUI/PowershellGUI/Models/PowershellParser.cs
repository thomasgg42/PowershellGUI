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

        /// <summary>
        /// Constructor
        /// </summary>
        public PowershellParser()
            {
            commandLineArguments = new List<string>();
            // Get file from FileReader.FileURI
            // execute file with input values from gui
            // display output
            }

        private void ExecuteScript(string scriptPath)
            {
            using (PowerShell psInstance = PowerShell.Create())
                {
                psInstance.AddCommand(scriptPath);
                //for every commandLineArgument, psInstance.AddArgument(arg)
                foreach (string arg in commandLineArguments)
                    {
                    psInstance.AddArgument(arg);
                    }
                Collection<PSObject> psOutput = psInstance.Invoke();
                ScriptOutput = psOutput.ToString();

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

        private void GetScriptParameters(ObservableCollection<ScriptArgument> scriptVariables)
            {
            foreach (ScriptArgument obj in scriptVariables)
                {
                string argKey   = obj.InputKey.ToString().ToLower();
                string argValue = obj.InputValue.ToString().ToLower();
               // string argument = "-" + argKey + " \"" + argValue + "\"";
                commandLineArguments.Add(argValue);
                }
            

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
