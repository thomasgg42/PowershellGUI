namespace PsGui.Models.PowershellExecuter
{
    /// <summary>
    /// A class with currently little functionality. Provides
    /// a scalable solution for input logic.
    /// @Inherits from ScriptArgument.
    /// </summary>
    public class UsernameArgument : ScriptArgument
    {

        /// <summary>
        /// Gets the stored username in the defined
        /// credentials file.
        /// </summary>
        private string GetStoredCredentials()
        {
            int textFileRow = 0;
            string userName = "";
            try
            {
                userName = System.IO.File.ReadAllLines(PsGui.ViewModels.MainViewModel.powershell_script_credentialsfile_path)[textFileRow];
            }
            catch (System.Exception e)
            {
                // Errors are handled silently
                // If the file is not found or unreadable,
                // no credentials are stored.
            }

            return userName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        public UsernameArgument(string key, string description, string type) : base(key, description, type)
        {
            base.InputValue = GetStoredCredentials();
        }
    }
}
