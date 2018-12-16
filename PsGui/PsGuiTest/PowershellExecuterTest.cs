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
        /// When the program launches. One shall not be able to 
        /// execute a script. The selected script shall be set 
        /// to an empty string. A modulepath shall be set to a existing
        /// folder structure at the same directory level as the program.
        /// </summary>
        [TestMethod]
        public void InitialTest()
            {
            string modulePath = ".";
            PsExecViewModel psExec = new PsExecViewModel(modulePath);
            Assert.AreEqual(false, psExec.CanExecute(this));
            Assert.AreEqual("", psExec.SelectedScript);
            Assert.IsNotNull(psExec.ModulePath);
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
