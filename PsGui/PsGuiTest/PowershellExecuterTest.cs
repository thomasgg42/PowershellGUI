using PsGui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PsGuiTest
{
    /// <summary>
    /// Tests the ViewModels of the PsGui application.
    /// </summary>
    [TestClass]
    public class PowershellExecuterTest
    {

    // Returns true if all conditions are met to activate the execute button.
    private bool ExecuteButtonIsActive(PsExecViewModel psExecViewModel)
        {
            if (psExecViewModel.IsScriptSelected == false)
            {
                return false;
            }
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument arg in psExecViewModel.ScriptTextVariables)
            {
                if (arg.InputValue != null && arg.InputValue.Equals(""))
                {
                    return false;
                }
            }
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument arg in psExecViewModel.ScriptUsernameVariables)
            {
                if (arg.InputValue != null && arg.InputValue.Equals(""))
                {
                    return false;
                }
            }
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument arg in psExecViewModel.ScriptPasswordVariables)
            {
                if (arg.InputValue != null && arg.InputValue.Equals(""))
                {
                    return false;
                }
            }
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument arg in psExecViewModel.ScriptMultiLineVariables)
            {
                if (arg.InputValue != null && arg.InputValue.Equals(""))
                {
                    return false;
                }
            }
            return true;
        }


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
            Assert.AreEqual(true, psExecViewModel.ScriptCategoryBrowser[0].FriendlyName.Equals("Category1"));
            Assert.AreEqual(true, psExecViewModel.ScriptFileBrowser[0].Equals("test1"));

            // Test exception if script files does not exist

            Assert.AreEqual(false, psExecViewModel.IsBusy);
            Assert.AreEqual(true, psExecViewModel.CanInteract);

            // Test execute button (input field contents)
            Assert.AreEqual(false, ExecuteButtonIsActive(psExecViewModel));
        }

        /// <summary>
        /// When a script is selected from the current category:
        /// - The IsScriptSelected shall equal true
        /// - The SelectedScriptPath shall equal the relative file path to the selected script
        /// - ScriptExecutionErrorOutput shall be empty
        /// - ScriptExecutionOutput shall be empty
        /// - The number of ScriptVariables shall match the amount of variables defined in the relevant script.
        /// - ScriptVariables shall not contain any information.
        /// </summary>
        [TestMethod]
        public void ScriptChosenInCurrentCategoryTest()
        {
            MainViewModel mainViewModel = new MainViewModel();
            PsExecViewModel psExecViewModel = (PsExecViewModel)mainViewModel.Tabs[0];

            Assert.AreEqual(true, psExecViewModel.ScriptFileBrowser.Count > 0);
            Assert.AreEqual("", psExecViewModel.SelectedScriptFile);

            // Test selecting a new script with two string input fields, 
            // one int input field and one multiline input field
            psExecViewModel.SelectedScriptFile = "test2";
            Assert.AreEqual(true, psExecViewModel.SelectedScriptPath.Equals(".\\Scripts\\Category1\\test2.ps1")); // tenkt feil?
            Assert.AreEqual(true, psExecViewModel.ScriptExecutionErrorException == null);
            Assert.AreEqual(true, psExecViewModel.ScriptExecutionErrorDetails == null);
            Assert.AreEqual(true, psExecViewModel.ScriptExecutionOutputRaw == null);
            Assert.AreEqual(true, psExecViewModel.ScriptExecutionOutputCustom == null);
            Assert.AreEqual(true, psExecViewModel.ScriptUsernameVariables.Count == 0);
            Assert.AreEqual(true, psExecViewModel.ScriptPasswordVariables.Count == 0);
            Assert.AreEqual(true, psExecViewModel.ScriptTextVariables.Count == 3);
            Assert.AreEqual(true, psExecViewModel.ScriptMultiLineVariables.Count == 1);

            Assert.AreEqual(false, psExecViewModel.IsBusy);
            Assert.AreEqual(true, psExecViewModel.CanInteract);

            // Test execute button
            Assert.AreEqual(false, ExecuteButtonIsActive(psExecViewModel));
        }

        /// <summary>
        /// When a script has been selected and execution is in progress:
        /// - The IsScriptSelected shall equal true
        /// - The SelectedScriptPath shall equal the relative file path to the selected script
        /// - 
        /// - IsBusy shall return true.
        /// - CanInteract shall return false.
        /// </summary>
        [TestMethod]
        public void ScriptExecutionstartedTest()
        {

        }

        /// <summary>
        /// When a script has been chosen and executed:
        /// - All saved script information shall be emptied
        /// - Selected script shall be reset to empty string
        /// - ScriptExecutionOutput shall contain regular script output
        /// - ScriotExecutionErrorOutput shall contain script error output
        /// - The execute button shall become inactive
        /// </summary>
        [TestMethod]
        public void ScriptExecutedResetStatus()
        {
            MainViewModel mainViewModel = new MainViewModel();
            PsExecViewModel psExecViewModel = (PsExecViewModel)mainViewModel.Tabs[0];

            psExecViewModel.SelectedScriptFile = "test2";

            // Ensures script input details are deleted when script has been executed
            psExecViewModel.SelectedScriptFile = "";
            psExecViewModel.ClearScriptSession(); // Shall be called after execution
            Assert.AreEqual(true, psExecViewModel.IsScriptSelected == false);
            Assert.AreEqual(true, psExecViewModel.ScriptTextVariables.Count == 0);
            Assert.AreEqual(true, psExecViewModel.ScriptUsernameVariables.Count == 0);
            Assert.AreEqual(true, psExecViewModel.ScriptPasswordVariables.Count == 0);
            Assert.AreEqual(true, psExecViewModel.ScriptMultiLineVariables.Count == 0);
            Assert.AreEqual(true, psExecViewModel.ScriptVariables.Count == 0);


            // Test if output strings are not yet defined
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionOutputRaw);
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionOutputCustom);
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionErrorException);
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionErrorDetails);

            // Test execute button
            Assert.AreEqual(false, ExecuteButtonIsActive(psExecViewModel));

        }

        /// <summary>
        /// When a new category is chosen:
        /// - A new list of scripts shall be available from dropdown.
        /// - Previous script variable data collection shall be cleared so that
        ///   ScriptVariables contains no collections.
        /// - The Execute button shall be inactive while IsScriptSelected is false.
        /// - Script execution output shall not contain any strings.
        /// - Script execution error output shall not contain any strings.
        /// </summary>
        [TestMethod]
        public void NewCategoryChosenTest()
        {
            MainViewModel   mainViewModel   = new MainViewModel();
            PsExecViewModel psExecViewModel = (PsExecViewModel)mainViewModel.Tabs[0];

            // Change category
            psExecViewModel.SelectedScriptCategory = "Category2";

            // Test new script and category data
            Assert.AreEqual(true, psExecViewModel.ScriptVariables.Count == 0);
            Assert.AreEqual("", psExecViewModel.SelectedScriptFile);
            Assert.AreEqual(true, psExecViewModel.ScriptCategoryBrowser[1].FriendlyName.Equals("Category2"));
            Assert.AreEqual(true, psExecViewModel.SelectedScriptCategory.Equals("Category2"));
            Assert.AreEqual(true, psExecViewModel.ScriptFileBrowser[0].Equals("test3"));

            // Test if output strings are not yet defined
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionOutputRaw);
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionOutputCustom);
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionErrorException);
            Assert.AreEqual(null, psExecViewModel.ScriptExecutionErrorDetails);

            Assert.AreEqual(false, psExecViewModel.IsBusy);
            Assert.AreEqual(true, psExecViewModel.CanInteract);

            // Test execute button
            Assert.AreEqual(false, ExecuteButtonIsActive(psExecViewModel));
        }
    }
 }
