namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// A base class for each of the argument types made by the 
    /// user input for the powershell script.
    /// Inheritance is used to be able to separate different types
    /// of script argument into different WPF Controls in the view
    /// by the use of CompositeCollection and multiple ObservableCollections.
    /// a friendly name. 
    /// </summary>
    public class ScriptArgument
        {
        private ArgumentChecker inputCheck;
        private string _inputKey;
        private string _inputDescription;
        private string _inputType;
        private string _inputValue;

        public ScriptArgument(string key, string description, string type)
            {
            inputCheck        = new ArgumentChecker();
            _inputKey         = key;
            _inputDescription = description;
            _inputType        = type;
            ClearUserInput();
            }

        /// <summary>
        ///  Removes leading and trailing spaces from the input
        ///  and returns it.
        /// </summary>
        private string TrimInput(string input)
            {
            return input.Trim();
            }

        /// <summary>
        /// Checks input format based on the stored input type.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected bool IsInputOk(string input)
            {
            bool inputOk = false;
            switch (_inputType)
                {
                case "string": inputOk    = inputCheck.IsString(input); break;
                case "int": inputOk       = inputCheck.IsInt(input);    break;
                case "bool": inputOk      = inputCheck.IsBool(input);   break;
                case "username": inputOk  = inputCheck.IsString(input); break;
                case "password": inputOk  = inputCheck.IsString(input); break;
                case "multiline": inputOk = inputCheck.IsString(input); break;
                default: throw new PsExecException("Bad script file variable type definition: " + _inputType);
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
                if (IsInputOk(value))
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
            if (_inputValue.Equals(""))
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
