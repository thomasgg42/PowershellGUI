using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PsGui.Models.PowershellExecuter
    {
    public class PsExecException : PsGuiException
        {
        public PsExecException(string temp) : base(temp)
            {

            }
        }
    }
