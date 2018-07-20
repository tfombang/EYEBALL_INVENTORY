using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EYEBALL_INVENTORY
{
    class Accounts
    {
        private string _username;
        private string _password;
        private string _accountType;

        public string username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string password
        {
            get { return _password; }
            set { _password = value; }
        }

        public string accountType
        {
            get { return _accountType; }
            set { _accountType = value; }
        }
    }
}
