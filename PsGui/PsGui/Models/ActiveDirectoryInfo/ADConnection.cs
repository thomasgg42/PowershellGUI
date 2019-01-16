using System;
using System.DirectoryServices;

namespace PsGui.Models.ActiveDirectoryInfo
{
    public class ADConnection
        {

        public ADConnection(string serverURI, string ldapPath, string priviledgedUserName, string priviledgedUserPw)
            {
            // Føler at denne ikke passer til ADUser - kanskje bedre med egen klasse?
            // Her bør det benyttes en config fil
            // For tungvindt å skrive inn manuelt 
            // hardcode er nogo
            //     DirectoryEntry ldapConnection = new DirectoryEntry("eikdc201.eikanett.eika.no", "<brukernavn>", "<pw>");
            // ldapConnection.Path = "LDAP://OU=Customers,OU=SKALA,DC=EIKANETT,DC=eika,DC=no";


            DirectoryEntry ldapConnection = new DirectoryEntry(serverURI, priviledgedUserName, priviledgedUserPw);
            ldapConnection.Path = ldapPath;
            ldapConnection.AuthenticationType = AuthenticationTypes.Secure;

            DirectorySearcher search = new DirectorySearcher(ldapConnection);
            search.Filter = "(samaccountname=<hbruker>)";
            SearchResult result = search.FindOne();
            if (result != null)
                {
                ResultPropertyCollection fields = result.Properties;
                foreach (String ldapField in fields.PropertyNames)
                    {
                    foreach (Object myColl in fields[ldapField])
                        {
                        //  tmp += ldapField + ": " + myColl.ToString() + "\n";
                        }
                    }
                }
            else
                {
                //  tmp = "fail";
                }
            }  
        }
}
