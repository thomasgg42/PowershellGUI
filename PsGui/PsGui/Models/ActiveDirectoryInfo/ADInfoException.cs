using PsGui.Models;

namespace PsGui.Models.ActiveDirectoryInfo
    {
    class ADInfoException : PsGuiException
        {
        public ADInfoException(string temp1, string temp2, bool closeApp) : base(temp1, temp2, closeApp)
            {

            }
        public ADInfoException(string temp1) : base(temp1)
            {

            }
        }
    }
