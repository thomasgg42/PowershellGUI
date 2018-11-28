using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class FileReader : ObservableObject
        {
        public string FileURI { get; set; }

        private string scriptDescription;
        private string scriptHeader;
        private string scriptOutput;
        private string _fileContent;

        // Ønsket oprinnelig Dictionary, men denne er ikke "observable"
        // IDictionary interfacet var tungvint å implementere
        // Ser ut til at man egentlig skal ha denne i ViewModel 
        private ObservableCollection<ScriptArgument> _scriptVariables;

        /// <summary>
        /// Constructor
        /// </summary>
        public FileReader()
            {
            _scriptVariables = new ObservableCollection<ScriptArgument>();
            _fileContent = "";
            scriptDescription = "";
            scriptHeader = "";
            scriptOutput = "";
            }

        public string FileContent
            {
            get
                {
                return _fileContent;
                }
            set
                {
                _fileContent = value;
                OnPropertyChanged("FileContent");
                }
            }

        public ObservableCollection<ScriptArgument> ScriptVariables
            {
            get
                {
                return _scriptVariables;
                }
            set
                {
                _scriptVariables = value;
                //OnPropertyChanged("ScriptVariables");
                }
            }

        /// <summary>
        /// Parses the powershell script header's top area.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="line"></param>
        private void ParseScriptHeader(int lineNum, string line)
            {
            // logic to separate variable names from variable content
            const int description = 1;
            const int header      = 2;
            const int output      = 3;

            switch (lineNum)
                {
                case description:
                    {
                    scriptDescription = line;
                    break;
                    }
                case header:
                    {
                    scriptHeader = line;
                    break;
                    }
                case output:
                    {
                    scriptOutput = line;
                    break;
                    }
                }
            }

        /// <summary>
        /// Parses the Powershell script header's bottom area.
        /// </summary>
        /// <param name="line"></param>
        private void GetScriptVariables(string line)
            {
            string varName    = ParseVariableName(line);
            string varType    = ParseVariableType(line);
            string varDescription = ParseQuotationContent(line);
            SaveInputField(varName, varDescription, varType);
            //OnPropertyChanged("ScriptVariables");
            }

        private void SaveInputField(string inputKey, string inputDesc, string inputType)
            {
            ScriptArgument scriptInput = new ScriptArgument(inputKey, inputDesc, inputType);
            _scriptVariables.Add(scriptInput);
            }

        /// <summary>
        /// Gets the variable name from a string from the 
        /// script header's input variables section.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseVariableName(string line)
            {
            string tmp  = line.Split(' ')[0];
            string varName = tmp.Split(']')[1];
            return varName;
            }

        /// <summary>
        /// Gets the variable types defined by brackets.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseVariableType(string line)
            {
            string tmp     = line.Split(']')[0];
            string varType = tmp.Substring(1);
            return varType.ToLower();
            }

        /// <summary>
        /// Gets the content inside quotation marks in a string.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseQuotationContent(string line)
            {
            Regex regex    = new Regex("\".*?\"");
            var match      = regex.Match(line);
            string content = match.ToString();
            int firstChar  = 1;
            int lastChar   = content.Length;
            content = content.Substring(firstChar);
            content = content.Substring(0, lastChar - 2);
            return content;
            }


        /// <summary>
        /// Parses a powerscript file and saves the header contents. 
        /// Requires the FileURI to be set.
        /// </summary>
        public void ReadFile()
            {
            string[] lines         = System.IO.File.ReadAllLines(FileURI);
            string scriptHeaderEnd = "#>";
            int lineNum            = 0;
            int lastHeaderLine     = 3;

            foreach (string line in lines)
                {
                // if header is completely parsed
                if (line == scriptHeaderEnd)   { break; }
                // If description, header or output
                if (lineNum <= lastHeaderLine) { ParseScriptHeader(lineNum, line); }
                // else if input variables to script
                else { GetScriptVariables(line); }
                lineNum++;
                }
            }
        }
    }
