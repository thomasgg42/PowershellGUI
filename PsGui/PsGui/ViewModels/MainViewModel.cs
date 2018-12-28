using System.Collections.ObjectModel;

namespace PsGui.ViewModels
    {
    public class MainViewModel : ObservableObject
        {
        private string powershellScriptModulePath = ".";
        private string powershellScriptModuleFolderName = "Modules";

        public ObservableCollection<object> Tabs { get; private set; }

        public MainViewModel()
            {
            Tabs = new ObservableCollection<object>();
            Tabs.Add(new PsExecViewModel(powershellScriptModulePath, powershellScriptModuleFolderName));
            }

        }
    }
