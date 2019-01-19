using PsGui.Models;

namespace PsGui.Models.ActiveDirectoryInfo
    {
    class ADInfoException : PsGuiException
        {
        public ADInfoException(string temp1, string temp2) : base(temp1, temp2)
            {

            }
        public ADInfoException(string temp1) : base(temp1)
            {

            }
        }
    }
