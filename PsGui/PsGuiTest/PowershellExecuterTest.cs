using PsGui.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
///  DirectoryReader.SelectedScriptCategory skal bestemme kategori
///  DirectoryReader.ScriptFiles skal knyttes til dropdownmeny med ps-filer
///  DirectoryReader.SelectedScriptFile bestemmer nåværende valgt script
///  FileReader.ScriptVariables gir en liste over input felt
///  FileReader.ScriptArgument skal gi tilgang til hvert input felt sine egenskaper
///  ClickCommand skal kjøres ved trykk på Run Script knapp
///  PowershellExecuter.ScriptOutput skal vise output fra powershsell
/// </summary>


namespace PsGuiTest
    {
    [TestClass]
    public class PowershellExecuterTest
        {
        /// <summary>
        /// When the program launches. A list of available categories 
        /// (directories) shall be saved. One shall not be able to 
        /// execute a script. The selected script shall be set 
        /// to an empty string. A modulepath shall be set to a existing
        /// folder structure at the same directory level as the program.
        /// The script browser list shall be empty and the IsScriptSelected
        /// flag shall be set to false.
        /// </summary>
        [TestMethod]
        public void InitialTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);

            Assert.IsNotNull(psExec.ModulePath);
            Assert.AreEqual(true, psExec.ScriptCategoryBrowser.Count == 0);
            Assert.AreEqual(true, psExec.ScriptFileBrowser.Count == 0);
            Assert.AreEqual("", psExec.SelectedScriptCategory);
            Assert.AreEqual(false, psExec.IsScriptSelected);
            Assert.AreEqual("", psExec.SelectedScriptFile);
            Assert.AreEqual(false, psExec.CanExecute(this));
            }

        /// <summary>
        /// When a category is chosen. The current script shall be cleared from
        /// the dropdown menu. A new list of scripts shall then be available from
        /// the dropdown menu.
        /// </summary>
        [TestMethod]
        public void NewCategoryChosenTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);
            psExec.ScriptCategoryBrowser.Add("Active Directory");
            psExec.ScriptCategoryBrowser.Add("Exchange Server");
            psExec.ScriptCategoryBrowser.Add("Skype");
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0];

            Assert.AreEqual(true, psExec.ScriptCategoryBrowser.Count == 3);
            Assert.AreEqual(true, psExec.SelectedScriptFile == "");
            Assert.AreEqual(false, psExec.IsScriptSelected);

            // I tilfellet hvor man tidligere har valgt et script og hentet inn script variabler
            // Script variabler skal beholdes (input felt), men innholdet i variablene
            // (felt input) skal tømmes
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument arg in psExec.ScriptVariables)
                {
                Assert.AreEqual(true, arg.HasNoInput());
                }
            }

        /// <summary>
        /// When a script file is chosen in a selected category. All input fields 
        /// defined in the script shall be loaded into the ScriptVariables collection.
        /// Each collection object shall contain a ScriptArgument containing information
        /// gathered from the script file. The script argument's input field shall be empty.
        /// </summary>
        [TestMethod]
        public void NewScriptFileChosenInCategoryTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);
            psExec.ScriptCategoryBrowser.Add("Active Directory");
            psExec.ScriptCategoryBrowser.Add("Exchange Server");
            psExec.ScriptCategoryBrowser.Add("Skype");
            // category is selected
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0];

            // dummy script input variables
            string key = "TestKey";
            string description = "TestDescription";
            string type = "TestType";

            // Create argument aka input field
            PsGui.Models.PowershellExecuter.ScriptArgument arg = 
                new PsGui.Models.PowershellExecuter.ScriptArgument(key, description, type);
            // Add argument to ScriptVariables collection
            psExec.ScriptVariables.Add(arg);
            // Set the first script variable's key (name) as the chosen script
            psExec.SelectedScriptFile = psExec.ScriptVariables[0].InputKey;

            // Script is selected 
            Assert.AreEqual(true, psExec.IsScriptSelected);
            // but cannot execute due to empty fields
            Assert.AreEqual(false, psExec.CanExecute(this));
            }

        /// <summary>
        /// When a script file has been chosen and input fields
        /// has been filledo ut by the user. The script shall 
        /// become executable.
        /// </summary>
        [TestMethod]
        public void InputFieldsFilledScriptIsExecutableTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);
            psExec.ScriptCategoryBrowser.Add("Active Directory");
            psExec.ScriptCategoryBrowser.Add("Exchange Server");
            psExec.ScriptCategoryBrowser.Add("Skype");

            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0];

            // dummy script input variables
            string key = "TestKey";
            string description = "TestDescription";
            string type = "TestType";

            // Create argument aka input field
            PsGui.Models.PowershellExecuter.ScriptArgument arg =
                new PsGui.Models.PowershellExecuter.ScriptArgument(key, description, type);
            // Add argument to ScriptVariables collection
            psExec.ScriptVariables.Add(arg);
            // Set the first script variable's key (name) as the chosen script
            psExec.SelectedScriptFile = psExec.ScriptVariables[0].InputKey;

            // Script is selected 
            Assert.AreEqual(true, psExec.IsScriptSelected);

            // fill all input fields
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument field in psExec.ScriptVariables)
                {
                field.InputValue = "input value from user";
                }

            // execute is ok
            Assert.AreEqual(true, psExec.CanExecute(this));
            }

        /// <summary>
        /// A module path leading to a non-existing folder structure
        /// will produce a PsExecException
        /// </summary>
        [TestMethod]
        public void BadModulepathTest()
            {
            // test senere
           // PsExecViewModel psExec = new PsExecViewModel();
           // Assert.ThrowsException<PsExecException>();
            }
        }
    }
