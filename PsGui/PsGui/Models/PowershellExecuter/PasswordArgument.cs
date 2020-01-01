namespace PsGui.Models.PowershellExecuter
{
    /// <summary>
    /// A class handling password box functionality.
    /// @Inherits from ScriptArgument.
    /// </summary>
    public class PasswordArgument : ScriptArgument
    {
        /// <summary>
        /// Gets the stored password in the defined
        /// credentials file. Saves the password in
        /// the input variable, freeing the user of 
        /// typing in username and password manually.
        /// Base64 is used only to obscure the password
        /// from co-workers looking over your shoulder.
        /// The security must be handled by the environment
        /// where the application runs.
        /// </summary>
        public string GetStoredCredentials()
        {
            int textFileRow = 1;
            string passwordHash = "";
            try
            {
                passwordHash = System.IO.File.ReadAllLines(PsGui.ViewModels.MainViewModel.powershell_script_credentialsfile_path)[textFileRow];
            }
            catch (System.Exception e)
            {
                // Errors are handled silently
                // If the file is not found or unreadable,
                // no credentials are stored.
            }
            if (passwordHash != "")
            {
                return base.Base64Decode(passwordHash);
            }
            return passwordHash;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        public PasswordArgument(string key, string description, string type) : base(key, description, type)
        {
            base.InputValue = GetStoredCredentials();
        }
    }
}
