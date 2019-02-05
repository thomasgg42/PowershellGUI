namespace PsGui.Models.PowershellExecuter
    {
    /// <summary>
    /// A base class for each of the argument types made by the 
    /// user input for the powershell script.
    /// Inheritance is used to be able to separate different types
    /// of script argument into different WPF Controls in the view
    /// by the use of CompositeCollection and multiple ObservableCollections.
    /// a friendly name. 
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
        /// Removes folder prefix from the file path.
        /// </summary>
        /// <param name="filePath"></param>
        private void ConvertFriendlyName(string filePath)
            {
            string prefix = ".\\Modules\\";
            int prefixLength = prefix.Length;
            FriendlyName = FilePath.Substring(prefixLength);
            }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="filePath">Filepath including the modulePath and moduleFOlder.</param>
        public ScriptCategory(string filePath)
            {
            FilePath = filePath;
            ConvertFriendlyName(filePath);
            }
        }
    }
