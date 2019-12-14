using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace PsGui.Models.PowershellExecuter
    {
    public class ScriptReader
        {

        private CompositeCollection                       _scriptVariables;
        private ObservableCollection<TextArgument>        _scriptTextVariables;
        private ObservableCollection<PasswordArgument>    _scriptPasswordVariables;
        private ObservableCollection<UsernameArgument>    _scriptUsernameVariables;
        private ObservableCollection<MultiLineArgument>   _scriptMultiLineVariables;
        private ObservableCollection<CheckboxArgument>    _scriptCheckboxVariables;
        private ObservableCollection<RadiobuttonArgument> _scriptRadiobuttonVariables;

        private string _scriptDescription;

        /// <summary>
        /// Reads the powershell script header's top section.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="line"></param>
        private void ReadScriptHeader(int lineNum, string line)
            {
            const int description = 1;
           // const int header = 2;

            switch (lineNum)
                {
                case description:
                    {
                    _scriptDescription = ParseQuotationContent(line);
                    break;
                    }
                /*
                case header:
                    {
                    _scriptHeader = ParseQuotationContent(line);
                    break;
                    }
                */
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
        /// Adds each of the stored collections of input objects
        /// to the main collection of input objects.
        /// </summary>
        private void AddScriptArgumentsToMainCollection()
        {
            if (ScriptTextVariables != null && ScriptTextVariables.Count > 0)
                {
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptTextVariables });
                }

            if (ScriptUsernameVariables != null && ScriptUsernameVariables.Count > 0)
                {
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptUsernameVariables });
                }

            if (ScriptPasswordVariables != null && ScriptPasswordVariables.Count > 0)
                {
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptPasswordVariables });
                }

            if (ScriptMultiLineVariables != null && ScriptMultiLineVariables.Count > 0)
                {
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptMultiLineVariables });
                }

            if (ScriptCheckboxVariables != null && ScriptCheckboxVariables.Count > 0)
                {
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptCheckboxVariables });
                }

            if (ScriptRadiobuttonVariables != null && ScriptRadiobuttonVariables.Count > 0)
            {
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptRadiobuttonVariables });
            }

        }

        /// <summary>
        /// Adds a new script argument to the collection matching the argument's
        /// type.
        /// </summary>
        /// <param name="inputKey">Name of the key, displayed for user.</param>
        /// <param name="inputDesc">Description of the key, displayed for user.</param>
        /// <param name="inputType">Value of the key, set by user.</param>
        private void SaveInputField(string inputKey, string inputDesc, string inputType)
            {
            inputType = inputType.ToLower();
            switch (inputType)
                {
                case "password":    _scriptPasswordVariables.Add(new PasswordArgument(inputKey, inputDesc, inputType));       break;
                case "username":    _scriptUsernameVariables.Add(new UsernameArgument(inputKey, inputDesc, inputType));       break;
                case "multiline":   _scriptMultiLineVariables.Add(new MultiLineArgument(inputKey, inputDesc, inputType));     break;
                case "checkbox":    _scriptCheckboxVariables.Add(new CheckboxArgument(inputKey, inputDesc, inputType));       break;
                case "radiobutton": _scriptRadiobuttonVariables.Add(new RadiobuttonArgument(inputKey, inputDesc, inputType)); break;
                default:            _scriptTextVariables.Add(new TextArgument(inputKey, inputDesc, inputType));               break;
                }
            }

        /// <summary>
        /// Parses the Powershell script header's variable area and
        /// saves each input field to the member collections.
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
        /// Returns true if there are no registered script variables.
        /// </summary>
        /// <returns></returns>
        private bool ContainsVariables(ObservableCollection<object> collection)
            {
            if (collection != null && collection.Count > 0)
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScriptReader()
            {
            _scriptVariables            = new CompositeCollection();
            _scriptTextVariables        = new ObservableCollection<TextArgument>();
            _scriptUsernameVariables    = new ObservableCollection<UsernameArgument>();
            _scriptPasswordVariables    = new ObservableCollection<PasswordArgument>();
            _scriptMultiLineVariables   = new ObservableCollection<MultiLineArgument>();
            _scriptCheckboxVariables    = new ObservableCollection<CheckboxArgument>();
            _scriptRadiobuttonVariables = new ObservableCollection<RadiobuttonArgument>();
            _scriptDescription          = "";
            }

        /// <summary>
        /// Gets a collection of collections represnting the different
        /// types of script input variables and their values.
        /// </summary>
        public CompositeCollection ScriptVariables
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
        /// Sets or gets a collection of strings representing the 
        /// script input text values.
        /// </summary>
        public ObservableCollection<TextArgument> ScriptTextVariables
            {
            get
                {
                return _scriptTextVariables;
                }
            private set
                {
                _scriptTextVariables = value;
                }
            }

        /// <summary>
        /// Gets a collection of strings representing the
        /// script input username values.
        /// </summary>
        public ObservableCollection<UsernameArgument> ScriptUsernameVariables
            {
            get
                {
                return _scriptUsernameVariables;
                }
            }

        /// <summary>
        /// Sets or gets a collection of strings representing the
        /// script input password values.
        /// Security note: This leaves the clear text password in memory
        /// which is considered a security issue. However, if your
        /// RAM is accessible to attackers, you have bigger issues.
        /// </summary>
        public ObservableCollection<PasswordArgument> ScriptPasswordVariables
            {
            get
                {
                return _scriptPasswordVariables;
                }
            private set
                {
                _scriptPasswordVariables = value;
                }
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// script input multiline text values.
        /// </summary>
        public ObservableCollection<MultiLineArgument> ScriptMultiLineVariables
            {
            get
                {
                return _scriptMultiLineVariables;
                }
            private set
                {
                _scriptMultiLineVariables = value;
                }
            }

        /// <summary>
        /// Sets or gets a collection of strings representing
        /// script input textbox values.
        /// </summary>
        public ObservableCollection<CheckboxArgument> ScriptCheckboxVariables
        {
            get
            {
                return _scriptCheckboxVariables;
            }
            private set
            {
                _scriptCheckboxVariables = value;
            }
        }
        /// <summary>
        /// Sets or gets a collection of strings representing
        /// script radiobutton values.
        /// </summary>
        public ObservableCollection<RadiobuttonArgument> ScriptRadiobuttonVariables
        {
            get
            {
                return _scriptRadiobuttonVariables;
            }
            set
            {
                _scriptRadiobuttonVariables = value;
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
        /// Reads the selected powershell script and calls the functions
        /// responsible to parse and store the information. Adds 
        /// all the gathered information to the ScriptVariables collection.
        /// </summary>
        /// <param name="script"></param>
        public void ReadSelectedScript(string scriptPath)
            {
            string[] lines            = System.IO.File.ReadAllLines(scriptPath);
            string scriptHeaderEndTag = "#>";
            int lineNum               = 0;
            int lastHeaderLine        = 1; // Must match number of meta data lines
            try
                {
                foreach (string line in lines)
                    {
                    // If header is completely parsed
                    if (line.Equals(scriptHeaderEndTag)) { break; }
                    // If script meta data (description...)
                    if (lineNum <= lastHeaderLine)       { ReadScriptHeader(lineNum, line); }
                    // Else if input variables to script
                    else                                 { ReadScriptVariables(line); }
                    lineNum++;
                    }
                }
            catch (Exception e)
                {
                throw new PsExecException("Cannot read script header. Bad structure!", e.ToString(), true);
                }

            AddScriptArgumentsToMainCollection();
            }

        /// <summary>
        /// Clears all script variables (input fields).
        /// </summary>
        public void ClearScriptVariableInfo()
            {
            // TODO: create function to check if conditoins
            if (_scriptTextVariables != null && _scriptTextVariables.Count > 0)
                {
                foreach (ScriptArgument arg in _scriptTextVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if (_scriptUsernameVariables != null && _scriptUsernameVariables.Count > 0)
                {
                foreach (UsernameArgument arg in _scriptUsernameVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if (_scriptPasswordVariables != null && _scriptPasswordVariables.Count > 0)
                {
                foreach (PasswordArgument arg in _scriptPasswordVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if (_scriptMultiLineVariables != null && _scriptMultiLineVariables.Count > 0)
                {
                foreach (MultiLineArgument arg in _scriptMultiLineVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if(_scriptCheckboxVariables != null && _scriptCheckboxVariables.Count > 0)
                {
                foreach (CheckboxArgument arg in _scriptCheckboxVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if(_scriptRadiobuttonVariables != null && _scriptRadiobuttonVariables.Count > 0)
            {
                foreach(RadiobuttonArgument arg in _scriptRadiobuttonVariables)
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
            ScriptTextVariables.Clear();
            ScriptUsernameVariables.Clear();
            ScriptPasswordVariables.Clear();
            ScriptMultiLineVariables.Clear();
            ScriptCheckboxVariables.Clear();
            ScriptRadiobuttonVariables.Clear();
            ScriptVariables.Clear();
            _scriptDescription = "";            }

        /// <summary>
        /// Locks all the script arguments, setting their lock out value to true
        /// </summary>
        /// <param name="enabled">true/false</param>
        public void SetArgumentsEnabled(bool enabled)
        {
            foreach(CollectionContainer container in _scriptVariables)
            {
                foreach (ScriptArgument arg in container.Collection)
                {
                    arg.IsEnabled = enabled;
                }
            }
        }

        }
    }