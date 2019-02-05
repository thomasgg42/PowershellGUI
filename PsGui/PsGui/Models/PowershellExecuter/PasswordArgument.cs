using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsGui.Models.PowershellExecuter
    {
    public class PasswordArgument : ScriptArgument
        {
        public PasswordArgument(string key, string description, string type) : base(key, description, type)
            {

            }
        }
    }
