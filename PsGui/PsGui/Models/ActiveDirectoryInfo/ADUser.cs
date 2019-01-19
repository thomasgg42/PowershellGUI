

namespace PsGui.Models.ActiveDirectoryInfo
    {
    class ADUser
        {
        private string _lockedOut;
        private string _phone;
        private string _principalName;
        private string _department;
        private string _homeDirectory;
        private string _givenName;
        private string _surName;
        private string _mail;
        private string _title;
        private string _extensionAttribute10;
        private string _extensionAttribute8;
        private string _samAccountName;

        /// <summary>
        /// Constructor.
        /// </summary>
        public ADUser()
            {


            }

        /// <summary>
        /// Clears all user data.
        /// </summary>
        public void ClearData()
            {
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
            }

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
        }
    }
