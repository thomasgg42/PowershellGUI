using System;
using PsGui.ViewModels;
using PsGui.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0];
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
            }

        /// <summary>
        /// When a new script is chosen after previously selecting a script
        /// in a previous category, all the previous script variables shall
        /// be deleted and new script variables shall be created from the newly
        /// chosen script.
        /// </summary>
        [TestMethod]
        public void NewScriptChosenInNewCategoryTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);
            psExec.ScriptCategoryBrowser.Add("Active Directory");
            psExec.ScriptCategoryBrowser.Add("Exchange Server");
            psExec.ScriptCategoryBrowser.Add("Skype");
            psExec.SelectedScriptCategory = psExec.ScriptCategoryBrowser[0];
            //todo
            }

        /// <summary>
        /// A script must be chosen before script input fields can be shown.
        /// There shall be a script input field matching each of the defined
        /// script command line arguments in the script header file.
        /// </summary>
        [TestMethod]
        public void ScriptChosenEmptyInputFieldsTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);

            // Script exists, empty input fields
            psExec.SelectedScriptFile = "SomeTestScript.ps1";
            Assert.AreEqual(true, psExec.IsScriptSelected);
            Assert.AreEqual(true, psExec.ScriptCategoryBrowser.Count == 0);


            // Script not exists
            psExec.SelectedScriptFile = "";
            Assert.AreEqual(false, psExec.IsScriptSelected);
            }


        /// <summary>
        /// Before a powershell script is launched, all argument input
        /// fields must contain legal values.
        /// </summary>
        [TestMethod]
        public void ScriptArgumentTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);

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
