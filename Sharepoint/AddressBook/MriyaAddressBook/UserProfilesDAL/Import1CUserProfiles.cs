using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UserProfilesDAL.v81c_AddressBook;

namespace UserProfilesDAL
{
    public class Import1CUserProfiles
    {
        public static TableProfiles ImportRecords()
        {
            return ImportRecords(string.Empty, string.Empty, string.Empty);
        }

        public static TableProfiles ImportRecords(string strUrl)
        {
            return ImportRecords(strUrl, string.Empty, string.Empty);
        }

        public static TableProfiles ImportRecords(string strUserName, string strPassword)
        {
            return ImportRecords(string.Empty, strUserName, strPassword);
        }

        public static TableProfiles ImportRecords(string strUrl, string strUserName, string strPassword)
        {
            AddressBook proxy = new AddressBook();
            AddressBookItem[] ab = null;
            TableProfiles table = new TableProfiles();

            if (strUrl.Trim().Length > 0)
            {
                proxy.Url = strUrl;
            }
            if (strUserName.Trim().Length > 0 || strPassword.Trim().Length > 0)
            {
                proxy.Credentials = new System.Net.NetworkCredential(strUserName, strPassword);
            }
            proxy.Timeout = Properties.Settings.Default.soapTimeoutSec * 1000;

            ab = proxy.GetList();

            foreach (AddressBookItem item in ab)
                table.Add(new RecordUserProfile(item));

            return table;
        }
    }
}
