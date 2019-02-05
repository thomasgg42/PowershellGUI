using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    public class TextArgument : ScriptArgument
        {
        // TODO: SCRIPTARGUMENT CHILDREN FIX
        public TextArgument(string key, string description, string type) : base(key, description, type)
            {
            }

        /*

        /// <summary>
        /// Returns true if a script argument contains information.
        /// </summary>
        /// <returns></returns>
        public bool HasNoInput()
            {
            if (_inputValue == "")
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
                if(IsInputOk(value))
                    {
                    _inputValue = value;
                    }
                }
            }
        */
        }
    }
