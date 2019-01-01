using PsGui.ViewModels;
using PsGui.Models.PowershellExecuter;
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
    /// <summary>
    /// Tests the PowershellExecuter program. Expects a Modules folder
    /// with 3 categories; ActiveDirectory, Exchange, Skype, where 
    /// ActiveDirectory contains 3 scripts.
    /// </summary>
    [TestClass]
    public class PowershellExecuterTest
        {
        /// <summary>
        /// When the program launches. A list of available categories 
        /// (directories) shall be saved. A list of available scripts from
        /// the default chosen category shall be saved. One shall not be able to 
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
            string moduleFolder = "Modules";
            PsExecViewModel psExec = new PsExecViewModel(modulePath, moduleFolder);

            Assert.IsNotNull(psExec.ModulePath);
            Assert.AreEqual(false, psExec.ScriptCategoryBrowser.Count == 0);
            Assert.AreEqual(false, psExec.ScriptFileBrowser.Count == 0);
            Assert.AreEqual("ActiveDirectory", psExec.SelectedScriptCategory);
            Assert.AreEqual( false, psExec.IsScriptSelected);
            Assert.AreEqual("", psExec.SelectedScriptFile);
            Assert.AreEqual(false, psExec.ScriptCanExecute(this));
            }


        /// <summary>
        /// When a category is chosen. The current script shall be cleared from
        /// the dropdown menu. A new list of scripts shall then be available from
        /// the dropdown menu.
        /// When a script file is chosen in a selected category. All input fields 
        /// defined in the script shall be loaded into the ScriptVariables collection.
        /// Each collection object shall contain a ScriptArgument containing information
        /// gathered from the script file. The script argument's input field shall be empty.
        /// </summary>
        [TestMethod]
        public void NewCategoryChosenTest()
            {
            string modulePath = ".";
            string moduleFolder = "Modules";
            PsExecViewModel psExec = new PsExecViewModel(modulePath, moduleFolder);

            // Select a script category
            psExec.ScriptCategoryBrowser[0].IsSelectedCategory = true;
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0].FriendlyName;
            psExec.SelectedScriptFile = psExec.ScriptFileBrowser[0];

            // Select a script in the category, containing two input fields
            psExec.SelectedScriptFile = psExec.ScriptFileBrowser[1];
            psExec.IsScriptSelected = true;
            psExec.ScriptVariables.Add(new ScriptArgument("name", "First name", "string"));
            psExec.ScriptVariables.Add(new ScriptArgument("weight", "Body weight", "int"));

            // Add value to input fields
            psExec.ScriptVariables[0].InputValue = "Testbert";
            psExec.ScriptVariables[1].InputValue = "80";

            // Select a new script category
            psExec.ScriptCategoryBrowser[0].IsSelectedCategory = false;
            psExec.ScriptCategoryBrowser[1].IsSelectedCategory = true;
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[1].FriendlyName;


            Assert.AreEqual(true, psExec.ScriptCategoryBrowser.Count == 3);
            Assert.AreEqual(true, psExec.SelectedScriptFile == "");
            Assert.AreEqual(false, psExec.IsScriptSelected);
            Assert.AreEqual(false, psExec.ScriptCanExecute(this));
            foreach (PsGui.Models.PowershellExecuter.ScriptArgument arg in psExec.ScriptVariables)
                {
                Assert.AreEqual(true, arg.HasNoInput());
                }
            }



        /// <summary>
        /// When a script file has been chosen and input fields
        /// has been filledo ut by the user. The script shall 
        /// become executable.
        /// </summary
        [TestMethod]
        public void InputFieldsFilledScriptIsExecutableTest()
            {
            string modulePath = ".";
            string moduleFolder = "Modules";
            PsExecViewModel psExec = new PsExecViewModel(modulePath, moduleFolder);

            // Select a script category
            psExec.ScriptCategoryBrowser[0].IsSelectedCategory = true;
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0].FriendlyName;
            psExec.SelectedScriptFile = psExec.ScriptFileBrowser[0];

            // Select a script in the category, containing two input fields
            psExec.SelectedScriptFile = psExec.ScriptFileBrowser[1];
            psExec.IsScriptSelected = true;
            psExec.ScriptVariables.Add(new ScriptArgument("name", "First name", "string"));
            psExec.ScriptVariables.Add(new ScriptArgument("weight", "Body weight", "int"));

            // Add value to input fields
            psExec.ScriptVariables[0].InputValue = "Testbert";
            psExec.ScriptVariables[1].InputValue = "80";

            // execute is ok
            Assert.AreEqual(true, psExec.ScriptCanExecute(this));
            }

        /// <summary>
        /// A module path leading to a non-existing folder structure
        /// will produce a PsExecException
        /// </summary>
        [TestMethod]
        public void BadModulepathTest()
            {
            string modulePath = ".//..//";
            string moduleFolder = "Modules";
            PsExecViewModel psExec = new PsExecViewModel(modulePath, moduleFolder);
            }

        /// <summary>
        /// When a powershell script's header area has mispelled a variable
        /// type, an exception must be thrown.
        /// </summary>
        [TestMethod]
        public void TestBadArgumentTypeException()
            {
            string modulePath = ".";
            string moduleFolder = "Modules";
            PsExecViewModel psExec = new PsExecViewModel(modulePath, moduleFolder);
            }
        }
    }
