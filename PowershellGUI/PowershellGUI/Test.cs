using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;

namespace PowershellGUI
    {
    class Test
        {
        public StringBuilder GetPowershellScript()
            {
            StringBuilder outputList = new StringBuilder();
            using (PowerShell psInstance = PowerShell.Create())
                {
                string scriptPath = @"../../psScripts/whoami.ps1";
                psInstance.AddScript(scriptPath);
                psInstance.AddCommand("Out-String");
                Collection<PSObject> psOutput = psInstance.Invoke();
                foreach(PSObject obj in psOutput)
                    {
                    outputList.Append(obj.ToString());
                    }
                }
            return outputList;
            }
        }
    }
