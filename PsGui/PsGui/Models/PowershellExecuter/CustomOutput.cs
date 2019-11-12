using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private Types outputType;

        public CustomOutput(string text, Types type)
        {
            Text = "";
            outputType = type;
        }

        public string Text { get; set; }

    }
}
