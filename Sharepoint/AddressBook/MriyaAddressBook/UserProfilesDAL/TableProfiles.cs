using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SqlServer;

namespace UserProfilesDAL
{
    public class TableProfiles
    {
        private List<RecordUserProfile> userProfiles = new List<RecordUserProfile>();
        private ulong _ulID = 0;

        private bool _bBestEmployees = false;
        private bool _bBestEmployeesWeekly = false;
        private bool _bNewEmployees = false;
        private uint _uiNewEmployees = 30;
        private bool _bBirthday= false;
        private DateTime _dtBirthdayStart = DateTime.MinValue;
        private DateTime _dtBirthdayEnd = DateTime.MinValue;

        private bool _bEmpty = true;

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

        public bool IsEmpty 
        {
            get { return _bEmpty; }
            set { _bEmpty = value; }
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
                    _bBestEmployeesWeekly = false;
                    _bNewEmployees = false;
                    _bBirthday = false;
                }
            } 
        }

        /// <summary>
        /// If true then GetProfiles() will return only records with BestWorkerWeekly property set to true 
        /// </summary>
        public bool GetBestEmployeesWeeklyOnly
        {
            get { return _bBestEmployeesWeekly; }
            set
            {
                _bBestEmployeesWeekly = value;
                if (_bBestEmployeesWeekly)
                {
                    _bBestEmployees = false;
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
                    _bBestEmployeesWeekly = false;
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
                    _bBestEmployeesWeekly = false;
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

        public void Add(TableProfiles tableUserProfiles)
        {
            this.Clear();
            foreach (RecordUserProfile record in tableUserProfiles.userProfiles)
            {
                Add(record);
            }
            this._bEmpty = false;
        }

        public void ReadSPProfiles(Guid currentSiteId)
        {
            this.Clear();
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(currentSiteId))
                {
                    SPServiceContext sc = SPServiceContext.GetContext(site);
                    UserProfileManager upm = new UserProfileManager(sc);
                    foreach (UserProfile profile in upm)
                    {
                        // TODO: Figure out how to filter the list and skill all service accounts
                        if (profile["AccountName"].Value != null &&
                            (profile["AccountName"].Value.ToString().ToUpper().IndexOf("\\SM_") >= 0 ||
                            profile["AccountName"].Value.ToString().ToUpper().IndexOf("\\SP_") >= 0))
                            continue;
                        if (profile["AccountName"].Value != null &&
                            profile["AccountName"].Value.ToString() == profile.DisplayName)
                            continue;

                        Add(profile);
                    }
                }
            });
            this._bEmpty = false;
        }

        public void ReadSPProfiles(string strSiteUrl)
        {
            this.Clear();
            using (SPSite site2 = new SPSite(strSiteUrl))
            {
                using (SPWeb web = site2.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    SPServiceContext sc = SPServiceContext.GetContext(site2);
                    UserProfileManager upm = new UserProfileManager(sc);
                    foreach (UserProfile profile in upm)
                    {
                        // TODO: Figure out how to filter the list and skill all service accounts
                        if (profile["AccountName"].Value != null &&
                            (profile["AccountName"].Value.ToString().ToUpper().IndexOf("\\SM_") >= 0 ||
                            profile["AccountName"].Value.ToString().ToUpper().IndexOf("\\SP_") >= 0))
                            continue;
                        if (profile["AccountName"].Value != null &&
                            profile["AccountName"].Value.ToString() == profile.DisplayName)
                            continue;

                        Add(profile);
                    }
                }
            }
            this._bEmpty = false;
        }

        public void ReadSqlSPProfiles(string strConnectionString)
        {
            ReadSqlSPProfiles(strConnectionString, "");
        }

        public void ReadSqlSPProfiles(string strConnectionString, string strSiteProfiles)
        {
            this.Clear();

            SqlConnection sqlConnection = new SqlConnection(strConnectionString);
            SqlDataAdapter daProfiles = new SqlDataAdapter("SELECT [RecordID], [UserID], [NTName], [PreferredName], [Email], [Manager], [PictureUrl] " +
                "FROM dbo.UserProfile_Full " +
                "WHERE [bDeleted] = 0", sqlConnection);
            SqlDataAdapter daPropertyValues = new SqlDataAdapter("SELECT [RecordID], [PropertyName], [PropertyVal] " +
                "FROM dbo.PropertyList AS pl, dbo.UserProfileValue AS pv " +
                "WHERE 	pv.PropertyID = pl.PropertyID", sqlConnection);
            DataTable tableProfiles = new DataTable();
            DataTable tablePropertyValues = new DataTable();

            sqlConnection.Open();

            daProfiles.Fill(tableProfiles);
            daPropertyValues.Fill(tablePropertyValues);

            sqlConnection.Close();

            // Read data
            foreach (DataRow rowProfile in tableProfiles.Rows)
            {
                long id = 0;

                try { id = Convert.ToInt64(rowProfile["RecordID"]); }
                catch { continue; }

                DataRow[] rowPropValues = tablePropertyValues.Select("RecordID = " + id.ToString());

                RecordUserProfile rp = new RecordUserProfile();

                string strDisplayName = rowProfile["PreferredName"].ToString();
                rp.AccountName = rowProfile["NTName"].ToString();

                foreach (DataRow rowValue in rowPropValues)
                {
                    if (rowValue["PropertyName"] == null || rowValue["PropertyName"] == DBNull.Value)
                        continue;
                    if (rowValue["PropertyVal"] == null || rowValue["PropertyVal"] == DBNull.Value)
                        continue;
                    string strName = rowValue["PropertyName"].ToString().Trim();
                    string strVal = rowValue["PropertyVal"].ToString();

                    rp.SetProperty(strName, strVal);
                }

                if (rp.AccountName == null ||
                    rp.AccountName.ToUpper().IndexOf("\\SM_") >= 0 ||
                    rp.AccountName.ToUpper().IndexOf("\\SP_") >= 0 ||
                    rp.AccountName == strDisplayName ||
                    rp.LastName.Trim().Length < 1)
                    continue;

                rp.ProfileURL = strSiteProfiles + "/" + "Person.aspx?accountname=" +
                    System.Web.HttpUtility.UrlEncode(rp.AccountName);

                Add(rp);
            }

            this._bEmpty = false;
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
                if (_bBestEmployeesWeekly && p.BestWorkerWeekly == false)
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
