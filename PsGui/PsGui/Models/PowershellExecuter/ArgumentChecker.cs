using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// Validates the user input to the command line arguments.
    /// </summary>
    class ArgumentChecker
        {

        public ArgumentChecker()
            {}

        /// <summary>
        /// Returns true if input can be translated to a string value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsString(string input)
            {
            return true;
            }

        /// <summary>
        /// Returns true if input can be translated to an integer value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsInt(string input)
            {
            // Empty string must be allowed to erase input numbers
            // due to this function being evaluated on each keyboard push
            if (int.TryParse(input, out int n) || input.Equals(""))
                {
                return true;
                }
            else
                {
                return false;
                }
            }

        /// <summary>
        /// Returns true if input can be translated to a boolean value.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IsBool(string input)
            {
            return true;
            }
        }
    }
