using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    class KeyValuePair
        {
        private string _inputKey;
        private string _inputValue;

        public KeyValuePair(string key, string value)
            {
            _inputKey   = key;
            _inputValue = value;
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
        }
    }
