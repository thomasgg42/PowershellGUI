using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PsGui.ViewModels
{
    /// <summary>
    /// The main view model originaly thought to contain
    /// a collection of tabs each having their own view model, 
    /// views and models. Each tab was to provide different types
    /// of purpose. However PsGui resulted in only being used for
    /// running Powershell-scripts and the tabs were removed from
    /// the user interface.
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        private const string relConfigFilePath = @".\config.ini";

        private static string powershell_script_folder_path;
        private static string powershell_script_folder_name;

        public static string powershell_script_credentialsfile_path;

        public ObservableCollection<object> Tabs { get; private set; }

        public MainViewModel()
        {
            GetConfig();
            Tabs = new ObservableCollection<object>();
            Tabs.Add(new PsExecViewModel(powershell_script_folder_path, powershell_script_folder_name));
        }

        /// <summary>
        /// Gets the config.ini contents and calls the function
        /// responsible for parsing. If no config.ini file is found
        /// a new one is generated with default values.
        /// </summary>
        private void GetConfig()
        {
            if (!(System.IO.File.Exists(relConfigFilePath)))
            {
                GenerateNewConfigFile();
            }

            string[] lines = System.IO.File.ReadAllLines(relConfigFilePath);
            foreach (string line in lines)
            {
                // if not blank line or comment
                if ((line != null && line != "") && !(line.Trim().StartsWith("#")))
                {
                    ParseConfig(line);
                }
            }
        }

        /// <summary>
        /// Parses the configuration file contents provided
        /// as an argument.
        /// </summary>
        /// <param name="line"></param>
        private void ParseConfig(string line)
        {
            string key = line.Split('=')[0].Trim();
            string value = ParseQuotationContent(line);

            switch (key)
            {
                case "psgui_scriptfolder_relpath": powershell_script_folder_path = value; break;
                case "psgui_scriptfolder_name": powershell_script_folder_name = value; break;
                case "psgui_credentials_path": powershell_script_credentialsfile_path = value.Replace("AAAAA", GetCurrentScriptUser()); break;
                default: break;
            }
        }

        private string GetCurrentScriptUser()
        {
            //return System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            return System.Environment.UserName;
        }

        /// <summary>
        /// Gets the content inside quotation marks in a string.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseQuotationContent(string line)
        {
            Regex regex = new Regex("\".*?\"");
            var match = regex.Match(line);
            string content = match.ToString();
            int firstChar = 1;
            int lastChar = content.Length;
            content = content.Substring(firstChar);
            return content.Substring(0, lastChar - 2);
        }

        /// <summary>
        /// Generates a config.ini file with default values.
        /// </summary>
        private void GenerateNewConfigFile()
        {
            string relativeFilePath = ".\\config.ini";
            string[] contents =
                {
                "# This file MUST be placed at the same directory level",
                "# as the executable. If not, the executable will",
                "# generate a new, empty config.ini file.",
                "",
                "# PSGUI",
                "psgui_rel_scriptfolder_path = \".\"",
                "psgui_scriptfolder_name = \"Scripts\"",
                };
            try
            {
                System.IO.File.WriteAllLines(relativeFilePath, contents);
            }
            catch (System.Exception e)
            {
                Models.PsGuiException.WriteErrorToFile(e.ToString());
                Models.PsGuiException.WriteErrorToScreen("PsGui is unable to create the required config file. Likely due to lack of write permissions. Closing PsGui.");
                Models.PsGuiException.CloseApp();
            }

        }
    }
}
