using PsGui.Models.ActiveDirectoryInfo;
using System;
using System.DirectoryServices;

namespace PsGui.ViewModels
    {
    class ADInfoViewModel
        {
        public string TabName { get; } = "AD Info";

        private ADUser user;
        private ADConnection connection;
        private string _samAccountName;

        private void GetUserProperties(SearchResult user)
            {
            if (user != null)
                {
                ResultPropertyCollection fields = user.Properties;
                foreach (string ldapField in fields.PropertyNames)
                    {
                    foreach (Object myColl in fields[ldapField])
                        {
                        SetUserProperty(myColl.ToString());
                        }
                    }
                }
            }

        private void SetUserProperty(string property)
            {
            // logikk for å skille ut hvert felt
            // key:value 
            // split på : og ta [0] for key

            string key = "";


            switch (key)
                {
                case "mobile": break;
                case "company": break;
                case "samaccountname": break;
                case "extensionAttribute8": break;
                case "extensionattribute10": break;
                }
            }

        public ADInfoViewModel(string serverURI, string ldapPath, string priviledgedUserName, string priviledgedUserPw)
            {
            connection = new ADConnection(serverURI, ldapPath, priviledgedUserName, priviledgedUserPw);
            user       = new ADUser();
            ClearUserData();
            }

        /// <summary>
        /// Stores the SamAccountName and calls the function
        /// responsible for finding the AD-object matching
        /// the given SamAccountName and parsing the object's
        /// properties.
        /// </summary>
        public string SamAccountName
            {
            get
                {
                return _samAccountName;
                }
            set
                {
                GetUserProperties(connection.FindUserObject(value));
                _samAccountName = value;
                }
            }

        /// <summary>
        /// Clears the user user data. Setting the user private members
        ///  to empty strings.
        /// </summary>
        public void ClearUserData()
            {
            user.ClearData();
            }

        /// <summary>
        /// Gets the user AD user object from the user domain. Stores the user user
        /// attributes in the user ADUSer object.
        /// </summary>
        /// <param name="serverURI"></param>
        /// <param name="priviledgedUserName"></param>
        /// <param name="priviledgedUserPw"></param>
        public void GetADUser()
            {

            }

        /// <summary>
        /// Gets or set sthe user title.
        /// </summary>
        public string Title
            {
            get
                {
                return user.Title;
                }

            set
                {
                user.Title = value;
                }
            }
        
        /// <summary>
        /// Sets or gets the user mail.
        /// </summary>
        public string Mail
            {
            get
                {
                return user.Mail;
                }
            set
                {
                user.Mail = value;
                }
            }

        /// <summary>
        /// Sets or gets the user surname.
        /// </summary>
        public string SurName
            {
            get
                {
                return user.SurName;
                }
            set
                {
                user.SurName = value;
                }
            }

        /// <summary>
        /// Sets or gets the user given name.
        /// </summary>
        public string GivenName
            {
            get
                {
                return user.GivenName;
                }
            set
                {
                user.GivenName = value;
                }
            }

        /// <summary>
        /// Sets or gets the user home directory.
        /// </summary>
        public string HomeDirectory
            {
            get
                {
                return user.HomeDirectory;
                }
            set
                {
                user.HomeDirectory = value;
                }
            }

        /// <summary>
        /// Sets or gets the user department.
        /// </summary>
        public string Department
            {
            get
                {
                return user.Department;
                }
            set
                {
                user.Department = value;
                }
            }

        /// <summary>
        /// Sets or gets extension attribute 10.
        /// </summary>
        public string ExtensionAttribute8
            {
            get
                {
                return user.ExtensionAttribute8;
                }
            set
                {
                user.ExtensionAttribute8 = value;
                }
            }

        /// <summary>
        /// Sets or gets extension attribute 10.
        /// </summary>
        public string ExtensionAttribute10
            {
            get
                {
                return user.ExtensionAttribute10;
                }
            set
                {
                user.ExtensionAttribute10 = value;
                }
            }

        /// <summary>
        /// Sets or gets the user principal name.
        /// </summary>
        public string PrincipalName
            {
            get
                {
                return user.PrincipalName;
                }
            set
                {
                user.PrincipalName = value;
                }
            }

        /// <summary>
        /// Sets or gets the user phone number.
        /// </summary>
        public string Phone
            {
            get
                {
                return user.Phone;
                }
            set
                {
                user.Phone = value;
                }
            }

        }
    }
