using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PsGui.ViewModels
    {
    public class MainViewModel : ObservableObject
        {
        private const string relConfigFilePath = @".\config.ini";

        private string powershellScriptFolderPath;
        private string powershellScriptModuleFolderName;
        private string adServerUri;
        private string adLdapPath;
        private string adPriviledgedUserName;
        private string adPriviledgedPassword;

        public ObservableCollection<object> Tabs { get; private set; }

        public MainViewModel()
            {
            GetConfig();
            Tabs = new ObservableCollection<object>();
            Tabs.Add(new PsExecViewModel(powershellScriptFolderPath, powershellScriptModuleFolderName));
            //    Tabs.Add(new ADInfoViewModel(adServerUri, adLdapPath, adPriviledgedUserName, adPriviledgedPassword));
            }

        /// <summary>
        /// Gets the config.ini contents and calls the function
        /// responsible for parsing. If no config.ini file is found
        /// a new one is generated with default values.
        /// </summary>
        private void GetConfig()
            {
            if(!(System.IO.File.Exists(relConfigFilePath)))
                {
                GenerateNewConfigFile();
                }

            string[] lines = System.IO.File.ReadAllLines(relConfigFilePath);
            foreach (string line in lines)
                {
                // if not blank line or comment
                if ((line != null && line != "") && !(line.Trim().StartsWith("#")))
                    {
                    ParseConfig(line);
                    }
                }

            }

        /// <summary>
        /// Parses the configuration file contents provided
        /// as an argument.
        /// </summary>
        /// <param name="line"></param>
        private void ParseConfig(string line)
            {
            string key = line.Split('=')[0].Trim();
            string value = ParseQuotationContent(line);

            switch (key)
                {
                case "psgui_rel_scriptfolder_path": powershellScriptFolderPath = value; break;
                case "psgui_scriptfolder_name": powershellScriptModuleFolderName = value; break;
                case "ad_server_uri": adServerUri = value; break;
                case "ad_ldap_path": adLdapPath = value; break;
                case "ad_priviledged_username": adPriviledgedUserName = value; break;
                case "ad_priviledged_password": adPriviledgedPassword = value; break;
                default: break;
                }
            }

        /// <summary>
        /// Gets the content inside quotation marks in a string.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string ParseQuotationContent(string line)
            {
            Regex regex = new Regex("\".*?\"");
            var match = regex.Match(line);
            string content = match.ToString();
            int firstChar = 1;
            int lastChar = content.Length;
            content = content.Substring(firstChar);
            return content.Substring(0, lastChar - 2);
            }
        
        /// <summary>
        /// Generates a config.ini file with default values.
        /// </summary>
        private void GenerateNewConfigFile()
            {
            string relativeFilePath = ".\\config.ini";
            string[] contents =
                {
                "# This file MUST be placed at the same directory level",
                "# as the executable. If not, the executable will",
                "# generate a new, empty config.ini file.",
                "",
                "# PSGUI",
                "psgui_rel_scriptfolder_path = \".\"",
                "psgui_scriptfolder_name = \"Scripts\"",
                "",
                "# Active Directory",
                "ad_eikanett_fqdn = \"eikanett.eika.no\"",
                "ad_terra_fqdn = \"terra.local\"",
                "ad_principal_name_suffix = \"@EIKANETT.eika.no\"",
                "ad_homedrive_letter = \"H:\"",
                "",
                "## AD: Aktiv Eiendom",
                "ad_terra_aktiv_path = \"OU=Users,OU=Tm,OU=Eiendom,OU=SKALA,DC=terra,DC=local\"",
                "ad_aktiv_dep_prefix = \"AE\"",
                "ad_aktiv_account_prefix = \"H90\"",
                "ad_aktiv_account_start_number = \"5001\"",
                "ad_aktiv_account_end_number = \"9999\"",
                "",
                "# Exchange",
                "exch_server_uri = \"http://eikex103/powershell/\"",
                "exch_smtp_server = \"eikex103\"",
                "exch_mail_encoding = \"utf8\"",
                "",
                "# Skype",
                "skype_server_uri = \"https://eiklyncpool001.terra.local/OcsPowershell\"",
                "skype_id_prefix = \"Terra\"",
                "skype_registrar_pool = \"eiklyncpool001.terra.local\"",
                "skype_external_access_policy = \"Allow Federation+Public+Outside Access\"",
                "skype_conferencing_policy = \"Configure full conferencing\""
                };
            try
                {
                System.IO.File.WriteAllLines(relativeFilePath, contents);
                }
            catch (System.Exception e)
                {
                throw new PsGui.Models.PsGuiException("Could not create new config file. Check write permissions.", e.ToString(), true);
                }
            
            }
        }
    }
