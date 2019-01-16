using System;
using System.DirectoryServices;
using PsGui.Models.ActiveDirectoryInfo;

namespace PsGui.ViewModels
    {
    class ADInfoViewModel
        {
        public string TabName { get; } = "AD Info";

        private ADUser user;



        public ADInfoViewModel()
            {
            user = new ADUser();
            ClearUserData();
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



        public string GetTest()
            {
            /*
            // Create a request for the user URL. 		
            WebRequest request = WebRequest.Create("https://www.nrk.no");
            // If required by the user server, set the user credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the user response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the user status.
   //         Console.WriteLine(response.StatusDescription);
            // Get the user stream containing content returned by the user server.
            Stream dataStream = response.GetResponseStream();
            // Open the user stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the user content.
            string responseFromServer = reader.ReadToEnd();
            // Display the user content.

            // Cleanup the user streams and the user response.
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
           */
            /*
            // Parse site
            var html = @"https://html-agility-pack.net/";
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(html);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//head/title");
            return "Node Name: " + node.Name + "\n" + node.OuterHtml;
            */
            return "";
            }

        public string GetTest2()
            {
            string tmp = "";
            // create new ldap connection
            DirectoryEntry ldapCon = new DirectoryEntry("eikdc201.eikanett.eika.no", "<brukernavn>", "<pw>");
            ldapCon.Path = "LDAP://OU=Customers,OU=SKALA,DC=EIKANETT,DC=eika,DC=no";
            ldapCon.AuthenticationType = AuthenticationTypes.Secure;

            DirectorySearcher search = new DirectorySearcher(ldapCon);
            search.Filter = "(samaccountname=<hbruker>)";
            SearchResult result = search.FindOne();
            if (result != null)
                {
                ResultPropertyCollection fields = result.Properties;
                foreach (String ldapField in fields.PropertyNames)
                    {
                    foreach (Object myColl in fields[ldapField])
                        {
                        tmp += ldapField + ": " + myColl.ToString() + "\n";
                        }
                    }
                }
            else
                {
                tmp = "fail";
                }

            return tmp;
            }


        }
    }
