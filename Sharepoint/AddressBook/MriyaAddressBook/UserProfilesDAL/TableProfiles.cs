using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;

namespace UserProfilesDAL
{
    public class TableProfiles
    {
        private List<RecordUserProfile> userProfiles = new List<RecordUserProfile>();
        private ulong _ulID = 0;

        private bool _bBestEmployees = false;
        private bool _bNewEmployees = false;
        private uint _uiNewEmployees = 30;
        private bool _bBirthday= false;
        private DateTime _dtBirthdayStart = DateTime.MinValue;
        private DateTime _dtBirthdayEnd = DateTime.MinValue;

        #region Properties

        /// <summary>
        /// Return number of the records
        /// </summary>
        public int Count 
        {
            get 
            {
                return GetCount();
            }
        }

        /// <summary>
        /// If true then GetProfiles() will return only records with BestWorker property set to true 
        /// </summary>
        public bool GetBestEmployeesOnly 
        {
            get { return _bBestEmployees; }
            set 
            { 
                _bBestEmployees = value;
                if (_bBestEmployees)
                {
                    _bNewEmployees = false;
                    _bBirthday = false;
                }
            } 
        }

        /// <summary>
        /// If true then GetProfiles() will return only records which EmploymentDate is not earlier
        /// than specified NewEmployeeDays days ago
        /// </summary>
        public bool GetNewEmployeesOnly 
        {
            get { return _bNewEmployees; }
            set 
            { 
                _bNewEmployees = value;
                if (_bNewEmployees)
                {
                    _bBestEmployees = false;
                    _bBirthday = false;
                }
            } 
        }

        /// <summary>
        /// Specifies number of dates while record is valid to be a NewEmployee record
        /// </summary>
        public uint NewEmployeeDays 
        {
            get { return _uiNewEmployees; }
            set { _uiNewEmployees = value; } 
        }

        /// <summary>
        /// If true then GetProfiles() will return only records where emplyees have birthday
        /// within specified timeframe (BirthdayStart, BirthdayEnd)
        /// </summary>
        public bool GetEmployeesWithBirthdayOnly 
        {
            get { return _bBirthday; }
            set
            {
                _bBirthday = value;
                if (_bBirthday)
                {
                    _bNewEmployees = false;
                    _bBestEmployees = false;
                }
            }
        }

        /// <summary>
        /// The earliest date of the timeframe
        /// </summary>
        public DateTime BirthdayStart 
        {
            get { return _dtBirthdayStart; }
            set { _dtBirthdayStart = value; }
        }

        /// <summary>
        /// The latest date of the timeframe
        /// </summary>
        public DateTime BirthdayEnd
        {
            get { return _dtBirthdayEnd; }
            set { _dtBirthdayEnd = value; }
        }

        #endregion Properties

        public TableProfiles()
        {
        }

        public void Clear()
        {
            _ulID = 0;
            userProfiles.Clear();
        }

        public int GetCount()
        {
            return GetCount(int.MaxValue, 0, string.Empty, string.Empty);
        }

        public int GetCount(int maximumRows, int startRowIndex)
        {
            return GetCount(maximumRows, startRowIndex, string.Empty, string.Empty);
        }

        public int GetCount(string sortExpression, string filterExpression)
        {
            return GetCount(int.MaxValue, 0, sortExpression, filterExpression);
        }

        public int GetCount(string sortExpression)
        {
            return GetCount(int.MaxValue, 0, sortExpression, string.Empty);
        }

        public int GetCount(int maximumRows, int startRowIndex, string sortExpression, string filterExpression)
        {
            return GetProfiles(maximumRows, startRowIndex, sortExpression, filterExpression).Count;
        }

        public void Add(RecordUserProfile recordUserProfile)
        {
            recordUserProfile._ID = ++_ulID;
            userProfiles.Add(recordUserProfile);
        }

        public void Add(UserProfile userProfile)
        {
            RecordUserProfile record = new RecordUserProfile(userProfile);

            Add(record);
        }

        public List<RecordUserProfile> GetProfiles()
        {
            return GetProfiles(int.MaxValue, 0, string.Empty, string.Empty);
        }

        public List<RecordUserProfile> GetProfiles(int maximumRows, int startRowIndex)
        {
            return GetProfiles(maximumRows, startRowIndex, string.Empty, string.Empty);
        }

        public List<RecordUserProfile> GetProfiles(string sortExpression, string filterExpression)
        {
            return GetProfiles(int.MaxValue, 0, sortExpression, filterExpression);
        }

        public List<RecordUserProfile> GetProfiles(string sortExpression)
        {
            return GetProfiles(int.MaxValue, 0, sortExpression, string.Empty);
        }

        public List<RecordUserProfile> GetProfiles(int maximumRows, int startRowIndex, string sortExpression, string filterExpression)
        {
            int nRange = maximumRows;

            if (userProfiles.Count < 1)
                return userProfiles;

            if (startRowIndex >= userProfiles.Count)
                startRowIndex = userProfiles.Count - 1;
            if  (nRange > userProfiles.Count - startRowIndex)
                nRange = userProfiles.Count - startRowIndex;

            // TODO: ascending, descending
            var list = from up in userProfiles.GetRange(startRowIndex, nRange)
                       where Filter(up, filterExpression) == true
                       orderby OrderBy(sortExpression, up) // ascending
                       select up;

            List<RecordUserProfile> listSelected = new List<RecordUserProfile>();
            DateTime dateNewEmployeeValid = DateTime.Now.AddDays((double)-_uiNewEmployees);

            foreach (RecordUserProfile p in list)
            {
                if (_bBestEmployees && p.BestWorker == false)
                    continue;
                if (_bNewEmployees && p.EmploymentDate < dateNewEmployeeValid)
                    continue;
                if (_bBirthday)
                {
                    if (p.Birthday == null)
                        continue;

                    DateTime start = new DateTime(_dtBirthdayStart.Year, p.BirthdayDT.Month, p.BirthdayDT.Day);
                    DateTime end = new DateTime(_dtBirthdayEnd.Year, p.BirthdayDT.Month, p.BirthdayDT.Day);

                    if (start < _dtBirthdayStart || end > _dtBirthdayEnd)
                        continue;
                }
                listSelected.Add(p);
            }

            return listSelected;
        }

        private bool Filter(RecordUserProfile record, string filter)
        {
            string strFilter = filter.Trim().ToLower();

            if (filter.Length < 1)
                return true;

            if (record.LastName.ToLower().Contains(strFilter))
                return true;
            if (record.FirstName.ToLower().Contains(strFilter))
                return true;
            if (record.EmailWork.ToLower().Contains(strFilter))
                return true;
            if (record.SeparateDivision.ToLower().Contains(strFilter))
                return true;
            if (record.Position.ToLower().Contains(strFilter))
                return true;

            // Check phone

            string strPhoneFilter = filter.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            string strPhone = record.PhoneWork.Trim().ToLower().Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            if (strPhone.Contains(strPhoneFilter))
                return true;

            return false;
        }

        private object OrderBy(string sortKey, RecordUserProfile record)
        {
            if (sortKey == "ID")
                return record.ID;
            else if (sortKey == "AccountName")
                return record.AccountName;
            else if (sortKey == "LastName")
                return record.LastName;
            else if (sortKey == "FirstName")
                return record.FirstName;
            else if (sortKey == "MiddleName")
                return record.MiddleName;
            else if (sortKey == "Birthday")
                return record.BirthdayDT;
            else if (sortKey == "SSN")
                return record.SSN;
            else if (sortKey == "INN")
                return record.INN;
            else if (sortKey == "Organization")
                return record.Organization;
            else if (sortKey == "SubDivision")
                return record.SubDivision;
            else if (sortKey == "SeparateDivision" || sortKey == "Office")
                return record.SeparateDivision;
            else if (sortKey == "Position" || sortKey == "JobPosition" || sortKey == "Title" || sortKey == "JobTitle")
                return record.Position;
            else if (sortKey == "EmailWork" || sortKey == "WorkEmail")
                return record.EmailWork;
            else if (sortKey == "EmailHome" || sortKey == "HomeEmail")
                return record.EmailHome;
            else if (sortKey == "PhoneWork" || sortKey == "WorkPhone")
                return record.PhoneWork;
            else if (sortKey == "PhoneHome" || sortKey == "HomePhone")
                return record.PhoneHome;
            else if (sortKey == "EmploymentDate" || sortKey == "Employed" || sortKey == "DateOfEmployment")
                return record.EmploymentDate;
            else if (sortKey == "Merit" || sortKey == "Merits")
                return record.BestWorkerMerit;
            else 
                return "";
        }
    }
}
