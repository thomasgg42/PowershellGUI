using PsGui.Models.ActiveDirectoryInfo;
using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace PsGui.ViewModels
    {
    class ADInfoViewModel
        {
        private const int MAXUSERS = 3;

        private ADUser[] users;
        private ADConnection connection;
        private string _samAccountName;
        private int currentUserNumber;
        private int lastUserNumber;

        public string TabName { get; } = "AD Info";

        /// <summary>
        /// Gets the user properties from the user search result.
        /// </summary>
        /// <param name="user"></param>
        private void GetUserProperties(SearchResult user)
            {
            // TODO: handle empty result
            ResultPropertyCollection fields = user.Properties;
            foreach (string ldapField in fields.PropertyNames)
                {
                foreach (Object myColl in fields[ldapField])
                    {
                    SetUserProperty(myColl.ToString());
                    }
                }
            }

        /// <summary>
        /// Filters out each property and stores the values
        /// in the user object.
        /// </summary>
        /// <param name="property"></param>
        private void SetUserProperty(string property)
            {
            string key = "";
            string value = "";
            string[] keyValuePair = property.Split(':');
            key = keyValuePair[0];
            value = keyValuePair[1];


            switch (key)
                {
                case "givenname": GivenName = value; break;
                case "surname": SurName = value; break;
                case "title": Title = value;  break;
                case "mail": Mail = value;  break;
                case "homedirectory": HomeDirectory = value; break;
                case "department": Department = value;  break;
                case "userprincipalname": PrincipalName = value; break;
                case "mobile": Phone = value; break;
                case "samaccountname": SamAccountName = value; break;
                case "extensionAttribute8": break;
                case "extensionattribute10": break;
                default: break;
                }
            }

        /// <summary>
        /// Constructor initiates a new AD connection and the user
        /// array.
        /// </summary>
        /// <param name="serverURI"></param>
        /// <param name="ldapPath"></param>
        /// <param name="priviledgedUserName"></param>
        /// <param name="priviledgedUserPw"></param>
        public ADInfoViewModel(string serverURI, string ldapPath, string priviledgedUserName, string priviledgedUserPw)
            {

            /*
            try
                {
                connection = new ADConnection(serverURI, ldapPath, priviledgedUserName, priviledgedUserPw);
                }
            catch (Exception e)
                {
                throw new ADInfoException("Tilkobling til AD-server feilet!", e.ToString());
                }
            
            users = new ADUser[MAXUSERS];
            // Create empty initial user to prevent GUI fields
            // from reading uninitialized object properties
            currentUserNumber = -1;
            lastUserNumber = currentUserNumber;
            NewUser();
            SamAccountName = "H804602";

            */
              Test(serverURI, ldapPath,  priviledgedUserName,  priviledgedUserPw);
            }


        public void Test(string serverURI, string ldapPath, string priviledgedUserName, string priviledgedUserPw)
            {
            DirectoryEntry LDAPConnection = new DirectoryEntry(serverURI, priviledgedUserName, priviledgedUserPw);
            LDAPConnection.Path = ldapPath;
            LDAPConnection.AuthenticationType = AuthenticationTypes.Secure;

            string samAccountName = "H804602";
            DirectorySearcher searchResult = new DirectorySearcher(LDAPConnection);
            searchResult.Filter = "(samaccountname=" + samAccountName + ")";
            SearchResult res = searchResult.FindOne();

            ResultPropertyCollection fields = res.Properties;
            foreach (string ldapField in fields.PropertyNames)
                {
                foreach (Object myColl in fields[ldapField])
                    {
                    SetUserProperty(myColl.ToString());
                    }
                }
            }

        /// <summary>
        /// Increments the last user number, creates a new, empty user object
        /// and adds it to the user list. Sets the current user number to the 
        /// newly created user.
        /// </summary>
        public void NewUser()
            {
            if (lastUserNumber < MAXUSERS)
                {
                users[++lastUserNumber] = new ADUser();
                currentUserNumber = lastUserNumber;
                }
            else
                {
                // bruke throw?
                new ADInfoException("Max number of users reached (" + MAXUSERS + ")");
                }
            }

        /// <summary>
        /// Clears the user user data. Setting the user private members
        ///  to empty strings.
        /// </summary>
        public void ClearUserData()
            {
            users[currentUserNumber].ClearData();
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
        /// Stores the SamAccountName and calls the functions
        /// responsible for finding the AD-object matching
        /// the given SamAccountName and parsing the object's
        /// properties.
        /// </summary>
        public string SamAccountName
            {
            get
                {
                return users[currentUserNumber].SamAccountName;
                }
            set
                {
                // if value.length = "h90xxxx".length 
                // og utfør on propertyChanged?
                // kun ved if GETUserProperties == true? 
                // dårlig ide å lagre om du skrev feil?
                // bruke lagre-knapp som lagrer info for sammenlikning
                // Man skal kun lagre et objekt om man ønsker å sammenlikne det
                // kan dermed hente inn en ny user onpropertychanged om dette ikke er en 
                // tidskrevende prossess
                
                if (value != null)
                    {
                    SearchResult adUser = null;
                    
                    try
                        {
                        adUser = connection.FindUserObject(value);
                        }
                    catch (Exception e)
                        {
                        // throw?
                        throw new ADInfoException("There's a problem with the connection to the domain.", e.ToString(), true);
                        }
                
                    // If user is found, allocate memory for new user
                    // and save the user properties.
                    if(adUser != null)
                        {
                        NewUser();
                        GetUserProperties(adUser);
                        _samAccountName = value;
                        }
                    else
                        {
                        System.Windows.MessageBox.Show("AD user null");
                        }
                    }
                }
            }

        /// <summary>
        /// Gets or set sthe user title.
        /// </summary>
        public string Title
            {
            get
                {
                return users[currentUserNumber].Title;
                }

            set
                {
                users[currentUserNumber].Title = value;
                }
            }
        
        /// <summary>
        /// Sets or gets the user mail.
        /// </summary>
        public string Mail
            {
            get
                {
                return users[currentUserNumber].Mail;
                }
            set
                {
                users[currentUserNumber].Mail = value;
                }
            }

        /// <summary>
        /// Sets or gets the user surname.
        /// </summary>
        public string SurName
            {
            get
                {
                return users[currentUserNumber].SurName;
                }
            set
                {
                users[currentUserNumber].SurName = value;
                }
            }

        /// <summary>
        /// Sets or gets the user given name.
        /// </summary>
        public string GivenName
            {
            get
                {
                return users[currentUserNumber].GivenName;
                }
            set
                {
                users[currentUserNumber].GivenName = value;
                }
            }

        /// <summary>
        /// Sets or gets the user home directory.
        /// </summary>
        public string HomeDirectory
            {
            get
                {
                return users[currentUserNumber].HomeDirectory;
                }
            set
                {
                users[currentUserNumber].HomeDirectory = value;
                }
            }

        /// <summary>
        /// Sets or gets the user department.
        /// </summary>
        public string Department
            {
            get
                {
                return users[currentUserNumber].Department;
                }
            set
                {
                users[currentUserNumber].Department = value;
                }
            }

        /// <summary>
        /// Sets or gets extension attribute 10.
        /// </summary>
        public string ExtensionAttribute8
            {
            get
                {
                return users[currentUserNumber].ExtensionAttribute8;
                }
            set
                {
                users[currentUserNumber].ExtensionAttribute8 = value;
                }
            }

        /// <summary>
        /// Sets or gets extension attribute 10.
        /// </summary>
        public string ExtensionAttribute10
            {
            get
                {
                return users[currentUserNumber].ExtensionAttribute10;
                }
            set
                {
                users[currentUserNumber].ExtensionAttribute10 = value;
                }
            }

        /// <summary>
        /// Sets or gets the user principal name.
        /// </summary>
        public string PrincipalName
            {
            get
                {
                return users[currentUserNumber].PrincipalName;
                }
            set
                {
                users[currentUserNumber].PrincipalName = value;
                }
            }

        /// <summary>
        /// Sets or gets the user phone number.
        /// </summary>
        public string Phone
            {
            get
                {
                return users[currentUserNumber].Phone;
                }
            set
                {
                users[currentUserNumber].Phone = value;
                }
            }

        }
    }
