﻿using System;
using System.DirectoryServices;

namespace PsGui.Models.ActiveDirectoryInfo
{
    public class ADConnection
        {
        private DirectoryEntry LDAPConnection;

        /// <summary>
        /// Initiates the LDAP connection against the AD server. Stores
        /// the connection.
        /// </summary>
        /// <param name="serverURI"></param>
        /// <param name="priviledgedUserName"></param>
        /// <param name="priviledgedUserPw"></param>
        /// <param name="LDAPPath"></param>
        private void InitiateConnection(string serverURI, string priviledgedUserName, string priviledgedUserPw, string LDAPPath)
            {
            LDAPConnection = new DirectoryEntry(serverURI, priviledgedUserName, priviledgedUserPw);
            LDAPConnection.Path = LDAPPath;
            LDAPConnection.AuthenticationType = AuthenticationTypes.Secure;
            }

        public ADConnection(string serverURI, string LDAPPath, string priviledgedUserName, string priviledgedUserPw)
            {
            // Føler at denne ikke passer til ADUser - kanskje bedre med egen klasse?
            // Her bør det benyttes en config fil
            // For tungvindt å skrive inn manuelt 
            // hardcode er nogo
            //     DirectoryEntry ldapConnection = new DirectoryEntry("eikdc201.eikanett.eika.no", "<brukernavn>", "<pw>");
            // ldapConnection.Path = "LDAP://OU=Customers,OU=SKALA,DC=EIKANETT,DC=eika,DC=no";
            InitiateConnection(serverURI, priviledgedUserName, priviledgedUserPw, LDAPPath);


          //  DirectorySearcher search = new DirectorySearcher(ldapConnection);
          //  search.Filter = "(samaccountname=<hbruker>)";
           // SearchResult result = search.FindOne();
           /*
            if (result != null)
                {
                ResultPropertyCollection fields = result.Properties;
                foreach (string ldapField in fields.PropertyNames)
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
                */
            }

        public SearchResult FindUserObject()
            {
            DirectorySearcher searchResult = new DirectorySearcher(LDAPConnection);
            searchResult.Filter = "";
            return searchResult.FindOne();
            }

        }
}
