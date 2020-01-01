namespace PsGui.Models.PowershellExecuter
{
    /// <summary>
    /// A class handling checkbox functionality.
    /// @Inherits from ScriptArgument.
    /// </summary>
    public class CheckboxArgument : ScriptArgument
    {
        private bool _isChecked;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        public CheckboxArgument(string key, string description, string type) : base(key, description, type)
        {
            _isChecked = false;

            // Checkboxes are unchecked by default
            base.InputValue = "false";
        }

        /// <summary>
        /// Returns true if the checkbox is checked. False 
        /// otherwise.
        /// </summary>
        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                base.InputValue = _isChecked ? base.InputValue = "true" : base.InputValue = "false";
            }
        }

    }
}