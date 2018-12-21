using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    public class ScriptArgument
        {
        ArgumentChecker inputCheck;
        private string _inputKey;
        private string _inputDescription;
        private string _inputType;
        private string _inputValue;

        public ScriptArgument(string key, string description, string type)
            {
            _inputKey         = key;
            _inputDescription = description;
            _inputType        = type;
            _inputValue       = "";
            }

        /// <summary>
        /// Checks input format based on the stored input type.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private bool IsInputOk(string input)
            {
            bool inputOk = false;
            switch (_inputType)
                {
                case "string": inputOk = inputCheck.IsString(input); break;
                case "int": inputOk    = inputCheck.IsInt(input); break;
                case "bool": inputOk   = inputCheck.IsBool(input); break;
                }
            return inputOk;
            }

        /// <summary>
        /// Sets or gets the script argument's variable name.
        /// </summary>
        public string InputKey
            {
            get
                {
                return _inputKey;
                }
            set
                {
                _inputKey = value;
                }
            }

        /// <summary>
        /// Sets or gets the script argument's description.
        /// </summary>
        public string InputDescription
            {
            get
                {
                return _inputDescription;
                }
            set
                {
                _inputDescription = value;
                }
            }

        /// <summary>
        /// Sets or gets the script argument's type.
        /// </summary>
        public string InputType
            {
            get
                {
                return _inputType;
                }
            set
                {
                _inputType = value;
                }
            }

        /// <summary>
        /// Sets or gets the script argument's value.
        /// </summary>
        public string InputValue
            {
            get
                {
                return _inputValue;
                }
            set
                {
                _inputValue = value;
                if(IsInputOk(value))
                    {
                    _inputValue = value;
                    }
                }
            }

        /// <summary>
        /// Returns true if a script argument contains information.
        /// </summary>
        /// <returns></returns>
        public bool HasNoInput()
            {
            if(_inputValue == "")
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        /// <summary>
        /// Clears input from the user in the object.
        /// </summary>
        public void ClearUserInput()
            {
            _inputValue = "";
            }

        }
    }
