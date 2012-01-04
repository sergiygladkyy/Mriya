using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MriyaStaffDAL
{
    public class EmployeeRecord
    {
        // Attributes
        private int _ID = 0;
        private string _sLastName = "";
        private string _sFirstName = "";
        private string _sMiddleName = "";
        private DateTime _dtDobDT = DateTime.MinValue;
        private DateTime _dtEmployedDT = DateTime.MinValue;
        private string _sPhoneWork = "";
        private string _sPhoneMobile = "";
        private string _sEmail = "";
        private string _sDepartment = "";
        private string _sJobTitle = "";
        private string _sCity = "";
        private string _sOrgCity = "";
        private string _sOrgStreet = "";
        private string _sOrgHouseNo = "";
        private string _sOrgPostBox = "";
        private string _sLogin = "";
        private byte[] _bytePhoto = null;
        private object _Reserved = null;

        #region Properties

        // Properties
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string LastName
        {
            get { return _sLastName; }
            set { _sLastName = value; }
        }

        public string FirstName
        {
            get { return _sFirstName; }
            set { _sFirstName = value; }
        }

        public string MiddleName
        {
            get { return _sMiddleName; }
            set { _sMiddleName = value; }
        }

        public DateTime dtDOB
        {
            get { return _dtDobDT; }
            set { _dtDobDT = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0); }
        }

        public object DOB
        {
            get 
            {
                if (_dtDobDT == DateTime.MinValue)
                    return null;
                return _dtDobDT; 
            }
            set 
            {
                if (value != null)
                {
                    try
                    {
                        DateTime dttemp = Convert.ToDateTime(value);
                        _dtDobDT = new DateTime(dttemp.Year, dttemp.Month, dttemp.Day, 0, 0, 0);
                    }
                    catch
                    {
                        _dtDobDT = DateTime.MinValue;
                    }
                }
                else
                {
                    _dtDobDT = DateTime.MinValue;
                }
            }
        }

        public DateTime dtEmployed
        {
            get { return _dtEmployedDT; }
            set { _dtEmployedDT = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0); }
        }

        public object Employed
        {
            get
            {
                if (_dtEmployedDT == DateTime.MinValue)
                    return null;
                return _dtEmployedDT;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        DateTime dttemp = Convert.ToDateTime(value);
                        _dtEmployedDT = new DateTime(dttemp.Year, dttemp.Month, dttemp.Day, 0, 0, 0);
                    }
                    catch
                    {
                        _dtEmployedDT = DateTime.MinValue;
                    }
                }
                else
                {
                    _dtEmployedDT = DateTime.MinValue;
                }
            }
        }

        public string PhoneWork
        {
            get { return _sPhoneWork; }
            set { _sPhoneWork = value; }
        }

        public string PhoneMobile
        {
            get { return _sPhoneMobile; }
            set { _sPhoneMobile = value; }
        }

        public string Email
        {
            get { return _sEmail; }
            set { _sEmail = value; }
        }

        public string Department
        {
            get { return _sDepartment; }
            set { _sDepartment = value; }
        }

        public string JobTitle
        {
            get { return _sJobTitle; }
            set { _sJobTitle = value; }
        }

        public string City
        {
            get { return _sCity; }
            set { _sCity = value; }
        }

        public string OrgCity
        {
            get { return _sOrgCity; }
            set { _sOrgCity = value; }
        }

        public string OrgStreet
        {
            get { return _sOrgStreet; }
            set { _sOrgStreet = value; }
        }

        public string OrgHouseNo
        {
            get { return _sOrgHouseNo; }
            set { _sOrgHouseNo = value; }
        }

        public string Login
        {
            get { return _sLogin; }
            set { _sLogin = value; }
        }

        public object Reserved
        {
            get { return _Reserved; }
            set { _Reserved = value; }
        }

        public string OrgPostBox
        {
            get { return _sOrgPostBox; }
            set { _sOrgPostBox = value; }
        }

        public byte[] Photo
        {
            get { return _bytePhoto; }
            set { _bytePhoto = value; }
        }

        #endregion Properties

        public EmployeeRecord()
        {
        }

        public void Clear()
        {
            _ID = 0;
            _sLastName = "";
            _sFirstName = "";
            _sMiddleName = "";
            _dtDobDT = DateTime.MinValue;
            _dtEmployedDT = DateTime.MinValue;
            _sPhoneWork = "";
            _sPhoneMobile = "";
            _sEmail = "";
            _sDepartment = "";
            _sJobTitle = "";
            _sCity = "";
            _sOrgCity = "";
            _sOrgStreet = "";
            _sOrgHouseNo = "";
            _sOrgPostBox = "";
            _sLogin = "";
            _bytePhoto = null;
            _Reserved = null;
       }
    }
}
