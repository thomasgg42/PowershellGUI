namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// A class with currently no functionality. Provides
    /// a scalable solution for input logic.
    /// @Inherits from ScriptArgument.
    /// </summary>
    public class PasswordArgument : ScriptArgument
        {

        /// <summary>
        /// Stores the username and hashed password from the personal
        /// credentials file.
        /// </summary>
        private void CachePassword()
        {
            string[] lines  = System.IO.File.ReadAllLines(PsGui.ViewModels.MainViewModel.powershell_script_credentialsfile_path);
            string passwordHash = lines[1];
            base.InputValue = passwordHash;
            
            
            // Create powershell script to handle the 
            // hashing.

            // Silently check if file exists
            // if no file, no warnings or errors.

            // If file exists, read file
            // If file contains 2 lines
            // transform hashed value to normal value

            // Add the values to the ScriptReader.ScriptUsernameVariables and scriptReader.ScriptPasswordVariables collections
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="description"></param>
        /// <param name="type"></param>
        public PasswordArgument(string key, string description, string type) : base(key, description, type)
            {
            CachePassword();
            }
        }
    }
