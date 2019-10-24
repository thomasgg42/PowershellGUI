using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PsGui.ViewModels
    {
    public class MainViewModel : ObservableObject
        {
        private const string relConfigFilePath = @".\config.ini";

        private static string powershellScriptFolderPath;
        private static string powershellScriptModuleFolderName;

        public ObservableCollection<object> Tabs { get; private set; }

        public MainViewModel()
            {
            GetConfig();
            Tabs = new ObservableCollection<object>();
            Tabs.Add(new PsExecViewModel(powershellScriptFolderPath, powershellScriptModuleFolderName));
            }

        /// <summary>
        /// Gets the config.ini contents and calls the function
        /// responsible for parsing. If no config.ini file is found
        /// a new one is generated with default values.
        /// </summary>
        private void GetConfig()
            {
            if(!(System.IO.File.Exists(relConfigFilePath)))
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
                case "psgui_rel_scriptfolder_path": powershellScriptFolderPath = value; break;
                case "psgui_scriptfolder_name": powershellScriptModuleFolderName = value; break;
                default: break;
                }
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
                throw new PsGui.Models.PsGuiException("Could not create new config file. Check write permissions.", e.ToString(), true);
                }
            
            }
        }
    }
