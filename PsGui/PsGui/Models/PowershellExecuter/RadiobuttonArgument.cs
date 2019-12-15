namespace PsGui.Models.PowershellExecuter
{
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
