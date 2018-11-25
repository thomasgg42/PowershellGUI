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
    class KeyValuePair
        {
        private string _inputKey;
        private string _inputValue;
        private string _inputType;

        public KeyValuePair(string key, string value, string type)
            {
            _inputKey   = key;
            _inputValue = value;
            _inputType = type;
            }

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

        public string InputValue
            {
            get
                {
                return _inputValue;
                }
            set
                {
                _inputValue = value;
                }
            }

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
        }
    }
