using PsGui.ViewModels;
using PsGui.Models.PowershellExecuter;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace PsGuiTest
{
    /// <summary>
    /// Tests the ViewModels of the PsGui application.
    /// </summary>
    [TestClass]
    public class PowershellExecuterTest
    {
        /// <summary>
        /// When the program launches:
        /// - If the config file does not exist, it is created in the current directory.
        /// - If the config file exists, it is read and the name and path of the script folder is set.
        /// - The Tabs collection of the MainViewModel contains the PsExecViewModel.
        /// - A script directory path leading to the script and module directories shall be set
        ///   in the ModulePath property.
        /// - If the Script directory does not exist, an exception with the string: "No script directory found!"
        ///   is thrown.
        /// - A list of available directories (categories) shall be available in the ScriptCategoryBrowser collection.
        /// - The first found category is set in the SelectedScriptCategory property.
        /// - A list of available scripts from SelectedScriptCategory are found in the ScriptFileBrowser collection.
        /// - The selected script shall be set to an empty string. 
        /// - The Execute button shall be inactive when IsScriptSelected is false.
        /// </summary>
        [TestMethod]
        public void InitialTest()
        {
            // Test Tab collection content
            MainViewModel mainViewModel = new MainViewModel();
            Assert.AreEqual(false, mainViewModel.Tabs[0] == null);
            Assert.AreEqual(true, mainViewModel.Tabs[0].GetType() == typeof(PsExecViewModel));

            // Test config file and directory existance
            PsExecViewModel psExecViewModel = (PsExecViewModel)mainViewModel.Tabs[0];
            Assert.AreEqual(true, psExecViewModel.ModulePath.Equals(".\\Scripts\\"));
            Assert.AreEqual(true, psExecViewModel.ScriptCategoryBrowser.Count > 0);

            // Test exceptions if folders does not exist

            // Test category directories and script files, demands an existing ps script file
            Assert.AreEqual(true, psExecViewModel.ScriptCategoryBrowser.Count > 0);
            Assert.AreEqual(true, psExecViewModel.SelectedScriptCategory.Length > 0);
            Assert.AreEqual(true, psExecViewModel.ScriptFileBrowser.Count > 0);
            Assert.AreEqual("", psExecViewModel.SelectedScriptFile);

            // Test exception if script files does not exist

            // Test execute button
            Assert.AreEqual(false, psExecViewModel.IsScriptSelected);
        }

        /// <summary>
        /// When a new category is chosen without a script having previously run:
        /// - A new list of scripts shall be available from dropdown.
        /// - All input fields defined in the script shall load into ScriptTextVariables.
        /// - Each ScriptTextVariable contains a ScriptArgument from the script file.
        /// - Each ScriptArgument has no value.
        /// </summary>
        [TestMethod]
        public void NewCategoryChosenNoPrevScriptExecuted()
        {
            // todo
        }
    }
 }
