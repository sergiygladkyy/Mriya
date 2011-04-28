using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using UserProfilesDAL.v81c_AddressBook;

namespace UserProfilesDAL
{
    public class RecordUserProfile
    {
        private ulong ulUID = 0;
        private Guid guidID = new Guid();
        private string sLastName = "";
        private string sFirstName = "";
        private string sMiddleName = "";
        private DateTime dtBirthday = DateTime.MinValue;
        private string sPhoneWork = "";
        private string sEmailWork = "";
        private string sPhoneHome = "";
        private string sEmailHome = "";
        private string sINN = "";
        private string sSSN = "";
        private string sOrganization = "";
        private string sSubDivision = "";
        private string sSeparateDivision = "";
        private string sPosition = "";
        private DateTime dtEmployed = DateTime.MinValue;
        private bool bBestWorker = false;
        private bool bBestWorkerWeekly = false;
        private string sBestWorkerMerit = "";
        private byte[] bytesPhoto = null;
        private string sPhotoURL = "";
        private string sAccountName = "";
        private string sProfileURL = "";
        private UserProfile userProfile = null;
        AddressBookItem importUserProfile = null;

        #region Properties

        public ulong _ID
        {
            get { return ulUID; }
            set { ulUID = value; }
        }

        public Guid ID
        {
            get { return guidID; }
            set { guidID = value; }
        }

        public string LastName
        {
            get { return sLastName; }
            set { sLastName = value; }
        }

        public string FirstName
        {
            get { return sFirstName; }
            set { sFirstName = value; }
        }

        public string MiddleName
        {
            get { return sMiddleName; }
            set { sMiddleName = value; }
        }

        public object Birthday
        {
            get {
                if (dtBirthday != DateTime.MinValue)
                    return dtBirthday;
                else
                    return null;
            }
            set {
                try
                {
                    dtBirthday = Convert.ToDateTime(value);
                }
                catch
                {
                    dtBirthday = DateTime.MinValue;
                }
            }
        }

        public DateTime BirthdayDT
        {
            get { return dtBirthday; }
            set { dtBirthday = value; }
        }

        public string PhoneWork
        {
            get { return sPhoneWork; }
            set { sPhoneWork = value; }
        }

        public string EmailWork
        {
            get { return sEmailWork; }
            set { sEmailWork = value; }
        }

        public string PhoneHome
        {
            get { return sPhoneHome; }
            set { sPhoneHome = value; }
        }

        public string EmailHome
        {
            get { return sEmailHome; }
            set { sEmailHome = value; }
        }
        
        public string INN
        {
            get { return sINN; }
            set { sINN = value; }
        }

        public string SSN
        {
            get { return sSSN; }
            set { sSSN = value; }
        }

        public string Organization
        {
            get { return sOrganization; }
            set { sOrganization = value; }
        }

        public string SubDivision
        {
            get { return sSubDivision; }
            set { sSubDivision = value; }
        }

        public string SeparateDivision
        {
            get { return sSeparateDivision; }
            set { sSeparateDivision = value; }
        }

        public string Position
        {
            get { return sPosition; }
            set { sPosition = value; }
        }

        public object DateOfEmployment
        {
            get
            {
                if (dtEmployed != DateTime.MinValue)
                    return dtEmployed;
                else
                    return null;
            }
            set
            {
                try
                {
                    dtEmployed = Convert.ToDateTime(value);
                }
                catch
                {
                    dtEmployed = DateTime.MinValue;
                }
            }
        }

        public DateTime EmploymentDate
        {
            get { return dtEmployed; }
            set { dtEmployed = value; }
        }
        
        public bool BestWorker 
        {
            get { return bBestWorker; }
            set { bBestWorker = value; }
        }

        public bool BestWorkerWeekly
        {
            get { return bBestWorkerWeekly; }
            set { bBestWorkerWeekly = value; }
        }

        public string BestWorkerMerit
        {
            get { return sBestWorkerMerit; }
            set { sBestWorkerMerit = value; }
        }

        public byte[] Photo
        {
            get { return bytesPhoto; }
            set { bytesPhoto = value; }
        }

        public string PhotoURL
        {
            get { return sPhotoURL; }
            set { sPhotoURL = value; }
        }

        public string AccountName
        {
            get { return sAccountName; }
            set { sAccountName = value; }
        }

        public string ProfileURL
        {
            get { return sProfileURL; }
            set { sProfileURL = value; }
        }

        public UserProfile UserProfile 
        {
            get
            {
                return this.userProfile;
            }
            set
            {
                this.userProfile = value;
                this.importUserProfile = null;
                if (this.userProfile != null)
                    UpdatePropertiesFromUserProfile();
            }
        }

        #endregion Properties

        public RecordUserProfile()
        {
        }

        public RecordUserProfile(UserProfile userProfile)
        {
            this.UserProfile = userProfile;
        }

        public RecordUserProfile(AddressBookItem item)
        {
            ImportUserProfile(item);
        }

        public void Clear()
        {
            ClearProperties();
            userProfile = null;
            importUserProfile = null;
        }

        public void ImportUserProfile(AddressBookItem item)
        {
            this.importUserProfile = item;
            this.userProfile = null;
            if (this.importUserProfile != null)
                UpdatePropertiesFromImportUserProfile();
        }

        public void SetProperty(string strPropName, string strValue)
        {
            if (strPropName == "UserProfile_GUID")
            {
                try
                {
                    guidID = new Guid(strValue);
                }
                catch { }
            }
            else if (strPropName == "LastName")
            {
                sLastName = strValue;
            }
            else if (strPropName == "FirstName")
            {
                sFirstName = strValue;
            }
            else if (strPropName == "Mr-MiddleName")
            {
                sMiddleName = strValue;
            }
            else if (strPropName == "SPS-Birthday")
            {
                try
                {
                    dtBirthday = Convert.ToDateTime(strValue);
                }
                catch { dtBirthday = DateTime.MinValue; }
            }
            else if (strPropName == "WorkPhone")
            {
                sPhoneWork = strValue;
            }
            else if (strPropName == "WorkEmail")
            {
                sEmailWork = strValue;
            }
            else if (strPropName == "Office")
            {
                sSeparateDivision = strValue;
            }
            else if (strPropName == "Department")
            {
                sSubDivision = strValue;
            }
            //else if (strPropName == "SPS-JobTitle")
            //{
            //    sPosition = strValue;
            //}
            else if (strPropName == "Title")
            {
                sPosition = strValue;
            }
            else if (strPropName == "SPS-HireDate")
            {
                try
                {
                    dtEmployed = Convert.ToDateTime(strValue);
                }
                catch { dtEmployed = DateTime.MinValue; }
            }
            else if (strPropName == "PictureURL")
            {
                sPhotoURL = strValue;
            }
            else if (strPropName == "AccountName")
            {
                sAccountName = strValue;
            }
            // Custom values
            else if (strPropName == "Mr-Inn")
            {
                sINN = strValue;
            }
            else if (strPropName == "Mr-Ssn")
            {
                sSSN = strValue;
            }
            else if (strPropName == "Mr-Organization")
            {
                sOrganization = strValue;
            }
            else if (strPropName == "Mr-BestWorker")
            {
                try
                {
                    bBestWorker = Convert.ToBoolean(strValue);
                }
                catch { bBestWorker = false; }
            }
            else if (strPropName == "Mr-BestWorkerWeekly")
            {
                try
                {
                    bBestWorkerWeekly = Convert.ToBoolean(strValue);
                }
                catch { bBestWorkerWeekly = false; }
            }
            else if (strPropName == "Mr-BestWorkerMerit")
            {
                sBestWorkerMerit = strValue;
            }
        }

        private void ClearProperties()
        {
            guidID = new Guid();
            sLastName = "";
            sFirstName = "";
            sMiddleName = "";
            dtBirthday = DateTime.MinValue;
            sPhoneWork = "";
            sEmailWork = "";
            sPhoneHome = "";
            sEmailHome = "";
            sINN = "";
            sSSN = "";
            sOrganization = "";
            sSubDivision = "";
            sSeparateDivision = "";
            sPosition = "";
            dtEmployed = DateTime.MinValue;
            bBestWorker = false;
            bBestWorkerWeekly = false;
            sBestWorkerMerit = "";
            bytesPhoto = null;
            sPhotoURL = "";
            sAccountName = "";
            sProfileURL = "";
        }

        private void UpdatePropertiesFromUserProfile()
        {
            ClearProperties();

            if (userProfile == null)
                return;
            

            if (userProfile["UserProfile_GUID"].Value != null)
            {
                try
                {
                    guidID = (Guid)userProfile["UserProfile_GUID"].Value;
                }
                catch { }
            }
            if (userProfile["LastName"].Value != null)
                sLastName = userProfile["LastName"].Value.ToString();
            if (userProfile["FirstName"].Value != null)
                sFirstName = userProfile["FirstName"].Value.ToString();
            if (userProfile["SPS-Birthday"].Value != null)
            {
                try
                {
                    dtBirthday = Convert.ToDateTime(userProfile["SPS-Birthday"].Value);
                }
                catch { }
            }
            if (userProfile["WorkPhone"].Value != null)
                sPhoneWork = userProfile["WorkPhone"].Value.ToString();
            if (userProfile["HomePhone"].Value != null)
                sPhoneHome = userProfile["HomePhone"].Value.ToString();
            if (userProfile["WorkEmail"].Value != null)
                sEmailWork = userProfile["WorkEmail"].Value.ToString();
            if (userProfile["Office"].Value != null)
                sSeparateDivision = userProfile["Office"].Value.ToString();
            if (userProfile["Department"].Value != null)
                sSubDivision = userProfile["Department"].Value.ToString();
            if (userProfile["SPS-JobTitle"].Value != null)
                sPosition = userProfile["SPS-JobTitle"].Value.ToString();
            // Override job title
            if (userProfile["Title"].Value != null)
                sPosition = userProfile["Title"].Value.ToString();
            if (userProfile["SPS-HireDate"].Value != null)
            {
                try
                {
                    dtEmployed = Convert.ToDateTime(userProfile["SPS-HireDate"].Value);
                }
                catch { }
            }
            if (userProfile["PictureURL"].Value != null)
                sPhotoURL = userProfile["PictureURL"].Value.ToString();
            if (userProfile["AccountName"].Value != null)
                sAccountName = userProfile["AccountName"].Value.ToString();
            if (userProfile.PersonalUrl.AbsoluteUri != null)
                sProfileURL = userProfile.PersonalUrl.AbsoluteUri;
            
            // Custom values
            try
            {
                if (userProfile["Mr-MiddleName"].Value != null)
                    sMiddleName = userProfile["Mr-MiddleName"].Value.ToString();
            }
            catch
            {
            }
            try
            {
                if (userProfile["Mr-Inn"].Value != null)
                    sINN = userProfile["Mr-Inn"].Value.ToString();
            }
            catch
            {
            }
            try
            {
                if (userProfile["Mr-Ssn"].Value != null)
                    sSSN = userProfile["Mr-Ssn"].Value.ToString();
            }
            catch
            {
            }
            try
            {
                if (userProfile["Mr-Organization"].Value != null)
                    sOrganization = userProfile["Mr-Organization"].Value.ToString();
            }
            catch
            {
            }
            
            try
            {
                if (userProfile["Mr-BestWorker"].Value != null)
                    bBestWorker = Convert.ToBoolean(userProfile["Mr-BestWorker"].Value);
            }
            catch
            {
                bBestWorker = false;
            }
            try
            {
                if (userProfile["Mr-BestWorkerWeekly"].Value != null)
                    bBestWorkerWeekly = Convert.ToBoolean(userProfile["Mr-BestWorkerWeekly"].Value);
            }
            catch
            {
                bBestWorkerWeekly = false;
            }
            try
            {
                if (userProfile["Mr-BestWorkerMerit"].Value != null)
                    sBestWorkerMerit = userProfile["Mr-BestWorkerMerit"].Value.ToString();
            }
            catch
            {
                sBestWorkerMerit = "";
            }

        }

        private void UpdatePropertiesFromImportUserProfile()
        {
            ClearProperties();
            
            if (importUserProfile == null)
                return;

            sLastName = (importUserProfile.Surname != null) ? (importUserProfile.Surname) : ("");
            sFirstName = (importUserProfile.Name != null) ? (importUserProfile.Name) : ("");
            sMiddleName = (importUserProfile.MiddleName != null) ? (importUserProfile.MiddleName) : (""); 
            dtBirthday = (importUserProfile.Birthday == null)?(DateTime.MinValue):(Convert.ToDateTime(importUserProfile.Birthday));
            sINN = (importUserProfile.INN != null) ? (importUserProfile.INN) : ("");
            sSSN = (importUserProfile.SSN != null) ? (importUserProfile.SSN) : ("");
            sOrganization = (importUserProfile.Organization != null) ? (importUserProfile.Organization) : ("");
            sSubDivision = (importUserProfile.Subdivision != null) ? (importUserProfile.Subdivision) : ("");
            sSeparateDivision = (importUserProfile.SeparateDivision != null) ? (importUserProfile.SeparateDivision) : (""); 
            sPosition = (importUserProfile.OrganizationalPosition != null) ? (importUserProfile.OrganizationalPosition) : (""); 
            sPhoneWork = (importUserProfile.PhoneAtWork != null) ? (importUserProfile.PhoneAtWork) : ("");
            sEmailWork = (importUserProfile.Email != null) ? (importUserProfile.Email) : ("");
            sAccountName = (importUserProfile.LDAP != null) ? (importUserProfile.LDAP) : ("");
            dtEmployed = (importUserProfile.HiringDate == null) ? (DateTime.MinValue) : (Convert.ToDateTime(importUserProfile.HiringDate));
            bytesPhoto = importUserProfile.Photo;

            if (sAccountName.StartsWith("\\\\"))
                sAccountName = sAccountName.Remove(0, 2);
        }

    }
}
