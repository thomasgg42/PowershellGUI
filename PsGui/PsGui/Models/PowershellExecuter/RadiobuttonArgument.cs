namespace PsGui.Models.PowershellExecuter
{
    /// <summary>
    /// A class handling radio button functionality.
    /// @Inherits from ScriptArgument.
    /// </summary>
    public class RadiobuttonArgument : ScriptArgument
    {
        private bool _isChecked;

        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        public RadiobuttonArgument(string key, string description, string type) : base(key, description, type)
        {
            // Checkboxes are unchecked by default
            _isChecked = false;
            base.InputValue = "false";
        }

        /// <summary>
        /// Returns true if the radio button is checked.
        /// False otherwise.
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
