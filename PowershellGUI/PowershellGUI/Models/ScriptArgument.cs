using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    /// <summary>
    /// Dictionary-alike class providing a key and a value pair.
    /// </summary>
    class ScriptArgument
        {
        private string _inputKey;
        private string _inputDescription;
        private string _inputType;
        private string _inputValue;

        public ScriptArgument(string key, string desc, string type)
            {
            _inputKey         = key;
            _inputDescription = desc;
            _inputType        = type;
            _inputValue       = "";
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
                // om InputType == bool then if(value != alphanummeric) then inputvalue = value; else block
                if(value != "k")
                    {
                    _inputValue = value;
                    }
                }
            }


        }
    }
