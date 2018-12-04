using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellGUI.Models
    {
    /// <summary>
    /// Validates the user input to the command line arguments.
    /// </summary>
    class ArgumentChecker
        {

        public ArgumentChecker()
            {

            }

        public bool IsString(string input)
            {
            return true;
            }

        public bool IsInt(string input)
            {
            return int.TryParse(input, out int n);
            }

        public bool IsBool(string input)
            {
            if(String.Equals(input, "true"))
                {
                return true;
                }
            else if(String.Equals(input, "false"))
                {
                return false;
                }
            else
                {
                return false;
                }
            }

        }
    }
