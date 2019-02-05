﻿using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace PsGui.Models.PowershellExecuter
    {
    public class ScriptReader
        {

        private CompositeCollection                  _scriptVariables;
        private ObservableCollection<TextArgument> _scriptTextVariables;
        private ObservableCollection<PasswordArgument> _scriptPasswordVariables;
        private ObservableCollection<UsernameArgument> _scriptUsernameVariables;
        private ObservableCollection<MultiLineArgument> _scriptMultiLineVariables;

        private string _scriptDescription;
        private string _scriptHeader;

        /// <summary>
        /// Reads the powershell script header's top section.
        /// </summary>
        /// <param name="lineNum"></param>
        /// <param name="line"></param>
        private void ReadScriptHeader(int lineNum, string line)
            {
            const int description = 1;
            const int header = 2;

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
                case "password": _scriptPasswordVariables.Add(new PasswordArgument(inputKey, inputDesc, inputType)); break;
                case "username": _scriptUsernameVariables.Add(new UsernameArgument(inputKey, inputDesc, inputType)); break;
                case "multiline": _scriptMultiLineVariables.Add(new MultiLineArgument(inputKey, inputDesc, inputType)); break;
                default: _scriptTextVariables.Add(new TextArgument(inputKey, inputDesc, inputType)); break;
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
        private bool ContainsVariables(ObservableCollection<ScriptArgument> collection)
            {
            // TODO: SCRIPTARGUMENT CHILDREN FIX
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
            _scriptVariables          = new CompositeCollection();
            _scriptTextVariables      = new ObservableCollection<TextArgument>();
            _scriptUsernameVariables  = new ObservableCollection<UsernameArgument>();
            _scriptPasswordVariables  = new ObservableCollection<PasswordArgument>();
            _scriptMultiLineVariables = new ObservableCollection<MultiLineArgument>();
            _scriptDescription        = "";
            _scriptHeader             = "";
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
            // TODO: SCRIPTARGUMENT CHILDREN FIX
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
            // TODO: SCRIPTARGUMENT CHILDREN FIX
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
            // TODO: SCRIPTARGUMENT CHILDREN FIX
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
        /// script input multi line text values.
        /// </summary>
        public ObservableCollection<MultiLineArgument> ScriptMultiLineVariables
            {
            // TODO: SCRIPTARGUMENT CHILDREN FIX
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
            int lastHeaderLine        = 2;
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

            // TODO: SCRIPTARGUMENT CHILDREN FIX 
            // Add each collection to the main collection
            if (ScriptTextVariables != null && ScriptTextVariables.Count > 0)
                {
                //ScriptVariables.Add(ScriptTextVariables);
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptTextVariables });
                }
           
            if(ScriptUsernameVariables != null && ScriptUsernameVariables.Count > 0)
                {
                //ScriptVariables.Add(ScriptUsernameVariables);
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptUsernameVariables });
                }

            if (ScriptPasswordVariables != null && ScriptPasswordVariables.Count > 0)
               {
                //ScriptVariables.Add(ScriptPasswordVariables);
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptPasswordVariables });
                }

            if (ScriptMultiLineVariables != null && ScriptMultiLineVariables.Count > 0)
               {
                //ScriptVariables.Add(ScriptMultiLineVariables);
                ScriptVariables.Add(new CollectionContainer() { Collection = ScriptMultiLineVariables });
                }
            }


        /// <summary>
        /// Clears all script variables (input fields).
        /// </summary>
        public void ClearScriptVariableInfo()
            {
            // TODO: SCRIPTARGUMENT CHILDREN FIX
            if (_scriptTextVariables != null && _scriptTextVariables.Count > 0)
                {
                foreach (ScriptArgument arg in _scriptTextVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            
            if (_scriptUsernameVariables != null && ScriptUsernameVariables.Count > 0)
                {
                foreach (ScriptArgument arg in _scriptUsernameVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if (_scriptPasswordVariables != null && ScriptUsernameVariables.Count > 0)
                {
                foreach (ScriptArgument arg in _scriptPasswordVariables)
                    {
                    arg.ClearUserInput();
                    }
                }
            if (_scriptMultiLineVariables != null && ScriptUsernameVariables.Count > 0)
                {
                foreach (ScriptArgument arg in _scriptMultiLineVariables)
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
            ScriptVariables.Clear();
            _scriptDescription = "";
            _scriptHeader = "";
            }
        }
    }
