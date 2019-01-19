

namespace PsGui.Models.ActiveDirectoryInfo
    {
    class ADUser
        {
        public string LockedOut { get; set; }
        public string Phone { get; set; }
        public string PrincipalName { get; set; }
        public string Department { get; set; }
        public string HomeDirectory { get; set; }
        public string GivenName { get; set; }
        public string SurName { get; set; }
        public string Mail { get; set; }
        public string Title { get; set; }
        public string ExtensionAttribute10 { get; set; }
        public string ExtensionAttribute8 { get; set; }
        public string SamAccountName { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ADUser()
            {
            ClearData();
            }

        /// <summary>
        /// Clears all user data.
        /// </summary>
        public void ClearData()
            {
            // Temp values while testing 
            LockedOut = "True";
            Title = "IT-ansvarlig (Vikar)";
            Mail = "loffe.lofferud@oink.doink.com";
            SurName = "Lofferud";
            GivenName = "Loffe";
            HomeDirectory = @"\\oink.doink\buker\home\h905050";
            Department = "13348";
            ExtensionAttribute8 = "1905";
            ExtensionAttribute10 = "adfaejih9h_dkjf";
            PrincipalName = "h905050@oink.doink.com";
            Phone = "+47 492582592";
            SamAccountName = "h905050";
            /*
            _lockedOut = "false";
            _title = "";
            _mail = "";
            _surName = "";
            _givenName = "";
            _homeDirectory = "";
            _department = "";
            _extensionAttribute8 = "";
            _extensionAttribute10 = "";
            _principalName = "";
            _phone = "";
            _samAccountName = "";
            */
            }

        /*

        /// <summary>
        /// Sets or gets the samAccountName
        /// </summary>
        public string SamAccountName
            {
            get
                {
                return _samAccountName;
                }
            set
                {
                _samAccountName = value;
                }
            }

        /// <summary>
        /// Sets or gets the title.
        /// </summary>
        public string Title
            {
            get
                {
                return _title;
                }

            set
                {
                _title = value;
                }
            }

        /// <summary>
        /// Sets or gets the boolean lockedout-value as a string.
        /// </summary>
        public string LockedOut
            {
            get
                {
                return _lockedOut;
                }
            set
                {
                _lockedOut = value;
                }
            }

        /// <summary>
        /// Sets or gets the mail.
        /// </summary>
        public string Mail
            {
            get
                {
                return _mail;
                }
            set
                {
                _mail = value;
                }
            }

        /// <summary>
        /// Sets or gets the surname.
        /// </summary>
        public string SurName
            {
            get
                {
                return _surName;
                }
            set
                {
                _surName = value;
                }
            }

        /// <summary>
        /// Sets or gets the given name.
        /// </summary>
        public string GivenName
            {
            get
                {
                return _givenName;
                }
            set
                {
                _givenName = value;
                }
            }

        /// <summary>
        /// Sets or gets the home directory.
        /// </summary>
        public string HomeDirectory
            {
            get
                {
                return _homeDirectory;
                }
            set
                {
                _homeDirectory = value;
                }
            }

        /// <summary>
        /// Sets or gets the department.
        /// </summary>
        public string Department
            {
            get
                {
                return _department;
                }
            set
                {
                _department = value;
                }
            }

        /// <summary>
        /// Sets or gets extension attribute 10.
        /// </summary>
        public string ExtensionAttribute8
            {
            get
                {
                return _extensionAttribute8;
                }
            set
                {
                _extensionAttribute8 = value;
                }
            }

        /// <summary>
        /// Sets or gets extension attribute 10.
        /// </summary>
        public string ExtensionAttribute10
            {
            get
                {
                return _extensionAttribute10;
                }
            set
                {
                _extensionAttribute10 = value;
                }
            }

        /// <summary>
        /// Sets or gets the principal name.
        /// </summary>
        public string PrincipalName
            {
            get
                {
                return _principalName;
                }
            set
                {
                _principalName = value;
                }
            }

        /// <summary>
        /// Sets or gets the phone number.
        /// </summary>
        public string Phone
            {
            get
                {
                return _phone;
                }
            set
                {
                _phone = value;
                }
            }

        */
        }
    }
