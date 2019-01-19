using System.Collections.ObjectModel;

namespace PsGui.ViewModels
    {
    public class MainViewModel : ObservableObject
        {
        private string powershellScriptModulePath = ".";
        private string powershellScriptModuleFolderName = "Modules";

        private string ADServerURI = "eikdc201.eikanett.eika.no";
        private string ADldapPath = "LDAP://OU=Customers,OU=SKALA,DC=EIKANETT,DC=eika,DC=no";
        private string ADpriviledgedUserName = "";
        private string ADpriviledgedPassword = "";

        public ObservableCollection<object> Tabs { get; private set; }

        public MainViewModel()
            {
            Tabs = new ObservableCollection<object>();
            Tabs.Add(new PsExecViewModel(powershellScriptModulePath, powershellScriptModuleFolderName));
            Tabs.Add(new ADInfoViewModel(ADServerURI, ADldapPath, ADpriviledgedUserName, ADpriviledgedPassword));
            }

        }
    }
