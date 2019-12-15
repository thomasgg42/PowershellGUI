namespace PsGui.Models.PowershellExecuter
{
    /// <summary>
    /// A class representing each of the script categories 
    /// which forms radio buttons in the graphical interface.
    /// </summary>
    public class ScriptCategory
    {
        public string FilePath { get; private set; }
        public string FriendlyName { get; private set; }
        private bool _isSelected;

        /// <summary>
        /// Sets or gets true or false if the category
        /// is considered the selected category.
        /// </summary>
        public bool IsSelectedCategory
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
            }
        }

        /// <summary>
        /// Removes folder prefix from the file path. So the 
        /// remaining string can be shown to the user as the 
        /// script category.
        /// </summary>
        /// <param name="filePath"></param
        private void ConvertFriendlyName(string filePath, int modulePathLength)
        {
            FriendlyName = FilePath.Substring(modulePathLength);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">Filepath including the modulePath and moduleFOlder.</param>
        public ScriptCategory(string filePath, int modulePathLength)
        {
            FilePath = filePath;
            ConvertFriendlyName(filePath, modulePathLength);
        }
    }
}
