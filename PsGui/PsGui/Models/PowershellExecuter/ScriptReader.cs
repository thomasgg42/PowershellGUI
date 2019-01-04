using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PsGui.Models.PowershellExecuter
    {
    public class ScriptReader
        {
        private ObservableCollection<ScriptArgument> _scriptVariables;
        private string _scriptDescription;
        private string _scriptHeader;
        private string _scriptOutput;

        /// <summary>
        /// Reads the powershell script header's top section.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="line"></param>
        private void ReadScriptHeader(int lineNum, string line)
            {
            // logic to separate variable names from variable content
            const int description = 1;
            const int header = 2;
            const int output = 3;

            switch (lineNum)
                {
                case description:
                    {
                    _scriptDescription = line;
                    break;
                    }
                case header:
                    {
                    _scriptHeader = line;
                    break;
                    }
                case output:
                    {
                    _scriptOutput = line;
                    break;
                    }
                }
            }

        /// <summary>
        /// Gets the variable name from a string in the 
        /// script header's input variables section.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseVariableName(string line)
            {
            string tmp = line.Split(' ')[0];
            return tmp.Split(']')[1];
            }

        /// <summary>
        /// Gets the variable types defined by brackets.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseVariableType(string line)
            {
            string tmp = line.Split(']')[0];
            return (tmp.Substring(1)).ToLower();
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
            content        = content.Substring(firstChar);
            return content.Substring(0, lastChar - 2);
            }

        /// <summary>
        /// Fills the object corresponding to each command line
        /// argument input field.
        /// </summary>
        /// <param name="inputKey">Name of the key, displayed for user.</param>
        /// <param name="inputDesc">Description of the key, displayed for user.</param>
        /// <param name="inputType">Value of the key, set by user.</param>
        private void SaveInputField(string inputKey, string inputDesc, string inputType)
            {
            _scriptVariables.Add(new ScriptArgument(inputKey, inputDesc, inputType));
            }

        /// <summary>
        /// Parses the Powershell script header's variable area. The variable values
        /// are bound directly to the View.
        /// </summary>
        /// <param name="line"></param>
        private void ReadScriptVariables(string line)
            {
            string varName = ParseVariableName(line);
            string varType = ParseVariableType(line);
            string varDescription = ParseQuotationContent(line);
            SaveInputField(varName, varDescription, varType);
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScriptReader()
            {
            _scriptVariables   = new ObservableCollection<ScriptArgument>();
            _scriptDescription = "";
            _scriptHeader      = "";
            _scriptOutput      = "";
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// the script command line input variables.
        /// </summary>
        public ObservableCollection<ScriptArgument> ScriptVariables
            {
            get
                {
                return _scriptVariables;
                }
            private set
                {
                _scriptVariables = value;
                }
            }

        /// <summary>
        /// Sets or gets the script description from the 
        /// header section.
        /// </summary>
        public string ScriptDescription
            {
            get
                {
                return _scriptDescription;
                }
            set
                {
                _scriptDescription = value;
                }
            }

        /// <summary>
        /// Sets or gets the script header from the
        /// header section.
        /// </summary>
        public string ScriptHeader
            {
            get
                {
                return _scriptHeader;
                }
            set
                {
                _scriptHeader = value;
                }
            }


        /// <summary>
        /// Reads the selected powershell script and stores
        /// relevant information.
        /// </summary>
        /// <param name="script"></param>
        public void ReadSelectedScript(string scriptPath)
            {
            string[] lines            = System.IO.File.ReadAllLines(scriptPath);
            string scriptHeaderEndTag = "#>";
            int lineNum               = 0;
            int lastHeaderLine        = 3;
            try
                {
                foreach (string line in lines)
                    {
                    // if header is completely parsed
                    if (line.Equals(scriptHeaderEndTag)) { break; }
                    // If description, header or output
                    if (lineNum <= lastHeaderLine) { ReadScriptHeader(lineNum, line); }
                    // else if input variables to script
                    else { ReadScriptVariables(line); }
                    lineNum++;
                    }
                }
            catch (Exception e)
                {
                throw new PsExecException("Cannot read script header. Bad structure!", e.ToString());
                }
            }

        /// <summary>
        /// Returns true if there are no registered script variables.
        /// </summary>
        /// <returns></returns>
        public bool ContainsVariables()
            {
            if(_scriptVariables != null && _scriptVariables.Count != 0)
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        /// <summary>
        /// Clears all script variables (input fields) for defined
        /// information.
        /// </summary>
        public void ClearScriptVariableInfo()
            {
            if(_scriptVariables.Count > 0)
                {
                foreach (ScriptArgument arg in _scriptVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            }

        /// <summary>
        /// Clears the stored data.
        /// </summary>
        public void ClearSession()
            {
            ScriptVariables.Clear();
            _scriptDescription = "";
            _scriptHeader = "";
            }
        }
    }
