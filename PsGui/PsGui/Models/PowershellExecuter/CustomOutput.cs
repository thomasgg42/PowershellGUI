namespace PsGui.Models.PowershellExecuter
{
    /// <summary>
    /// Allows custom error output and custom regular output
    /// in the same collection while one still have control
    /// over the different types.
    /// </summary>
    public class CustomOutput
    {
        public enum Types
        {
            Output,
            Error
        }

        private string _outputColor;
        private const string typeColorOutput = "Red";
        private const string typeColorError = "Black";

        /// <summary>
        /// Regular or error output.
        /// </summary>
        private Types OutputType { get; set; }

        private void SetCurrentTextColor(Types type)
        {
            switch (type)
            {
                case Types.Error: OutputColor = typeColorOutput; break;
                case Types.Output: OutputColor = typeColorError; break;
                default: OutputColor = typeColorError; break;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">Output text</param>
        /// <param name="type">Type of text: Outpur/Error</param>
        public CustomOutput(string text, Types type)
        {
            this.Text = text;
            this.OutputType = type;
            SetCurrentTextColor(type);
        }

        /// <summary>
        /// Used instead of a IValueConverter for Enum -> String
        /// Returns a predefined color corresponding to the active
        /// output type.
        /// </summary>
        /// <returns>String of a color name</returns>
        public string OutputColor
        {
            get
            {
                return _outputColor;
            }

            private set
            {
                _outputColor = value;
            }
        }

        /// <summary>
        /// The output contents.
        /// </summary>
        public string Text { get; set; }

    }
}
