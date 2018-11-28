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

        private void ExecuteScript(string scriptPath)
            {
            using (PowerShell psInstance = PowerShell.Create())
                {
                psInstance.AddCommand(scriptPath);
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
            foreach(ScriptArgument obj in scriptVariables)
                {
                System.Windows.MessageBox.Show(obj.InputValue);
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
            //ExecuteScript(scriptPath);
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
