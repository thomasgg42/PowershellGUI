using System;
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
        
        /// <summary>
        /// Constructor iniates AD-connection.
        /// </summary>
        /// <param name="serverURI"></param>
        /// <param name="LDAPPath"></param>
        /// <param name="priviledgedUserName"></param>
        /// <param name="priviledgedUserPw"></param>
        public ADConnection(string serverURI, string LDAPPath, string priviledgedUserName, string priviledgedUserPw)
            {
            InitiateConnection(serverURI, priviledgedUserName, priviledgedUserPw, LDAPPath);
            }

        /// <summary>
        /// Searches Active Directory for a user object with
        /// the provided samAccountName. Returns a SearchResult
        /// if found.
        /// </summary>
        /// <param name="samAccountName"></param>
        /// <returns></returns>
        public SearchResult FindUserObject(string samAccountName)
            {
            DirectorySearcher searchResult = new DirectorySearcher(LDAPConnection);
            searchResult.Filter = "(samaccountname=" + samAccountName + ")";
            return searchResult.FindOne();
            }



        }
}
