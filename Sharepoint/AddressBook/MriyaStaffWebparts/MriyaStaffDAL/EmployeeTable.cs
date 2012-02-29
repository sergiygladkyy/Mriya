using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MriyaStaffDAL
{
    [System.ComponentModel.DataObject(true)]
    public class EmployeeTable
    {
        private bool _bEmpty = true;
        private int _nCountTotal = -1;
        private List<EmployeeRecord> _Records = new List<EmployeeRecord>();
        private string _sFilterCity = "";
        private string _sFilterDepartment = "";
        private string _sFilterCustom = "";
        private string _sFilterCustom1 = "";
        private DateTime _dtFilterDobMin = DateTime.MinValue;
        private DateTime _dtFilterDobMax = DateTime.MinValue;
        private DateTime _dtFilterEmployedMin = DateTime.MinValue;
        private DateTime _dtFilterEmployedMax = DateTime.MinValue;
        private string _sSortOrder = "";
        private int _nPage = 0;
        private int _nRecordsOnPage = 0;

        #region Properties

        public bool IsEmpty
        {
            get { return _bEmpty; }
        }

        public int Count
        {
            get { return GetCount(); }
        }

        public int CountTotal
        {
            get { return GetCountTotal(); }
        }

        public string FilterCustom 
        {
            get { return _sFilterCustom; }
            set { _sFilterCustom = value.Trim(); } 
        }

        public string FilterCustom1
        {
            get { return _sFilterCustom1; }
            set { _sFilterCustom1 = value.Trim(); }
        }

        public string FilterCity
        {
            get { return _sFilterCity; }
            set { _sFilterCity = value.Trim(); }
        }

        public string FilterDepartment
        {
            get { return _sFilterDepartment; }
            set { _sFilterDepartment = value.Trim(); }
        }

        public DateTime FilterDOBMinDT
        {
            get { return _dtFilterDobMin; }
            set { _dtFilterDobMin = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0); }
        }

        public object FilterDOBMin
        {
            get
            {
                if (_dtFilterDobMin == DateTime.MinValue)
                    return null;
                return _dtFilterDobMin;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        DateTime dttemp = Convert.ToDateTime(value);
                        _dtFilterDobMin = new DateTime(dttemp.Year, dttemp.Month, dttemp.Day, 0, 0, 0);
                    }
                    catch
                    {
                        _dtFilterDobMin = DateTime.MinValue;
                    }
                }
                else
                {
                    _dtFilterDobMin = DateTime.MinValue;
                }
            }
        }
        
        public DateTime FilterDOBMaxDT
        {
            get { return _dtFilterDobMax; }
            set { _dtFilterDobMax = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0); }
        }

        public object FilterDOBMax
        {
            get
            {
                if (_dtFilterDobMax == DateTime.MinValue)
                    return null;
                return _dtFilterDobMax;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        DateTime dttemp = Convert.ToDateTime(value);
                        _dtFilterDobMax = new DateTime(dttemp.Year, dttemp.Month, dttemp.Day, 0, 0, 0);
                    }
                    catch
                    {
                        _dtFilterDobMax = DateTime.MinValue;
                    }
                }
                else
                {
                    _dtFilterDobMax = DateTime.MinValue;
                }
            }
        }

        public DateTime FilterEmployedMinDT
        {
            get { return _dtFilterEmployedMin; }
            set { _dtFilterEmployedMin = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0); }
        }

        public object FilterEmployedMin
        {
            get
            {
                if (_dtFilterEmployedMin == DateTime.MinValue)
                    return null;
                return _dtFilterEmployedMin;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        DateTime dttemp = Convert.ToDateTime(value);
                        _dtFilterEmployedMin = new DateTime(dttemp.Year, dttemp.Month, dttemp.Day, 0, 0, 0);
                    }
                    catch
                    {
                        _dtFilterEmployedMin = DateTime.MinValue;
                    }
                }
                else
                {
                    _dtFilterEmployedMin = DateTime.MinValue;
                }
            }
        }

        public DateTime FilterEmployedMaxDT
        {
            get { return _dtFilterEmployedMax; }
            set { _dtFilterEmployedMax = new DateTime(value.Year, value.Month, value.Day, 0, 0, 0); }
        }

        public object FilterEmployedMax
        {
            get
            {
                if (_dtFilterEmployedMax == DateTime.MinValue)
                    return null;
                return _dtFilterEmployedMax;
            }
            set
            {
                if (value != null)
                {
                    try
                    {
                        DateTime dttemp = Convert.ToDateTime(value);
                        _dtFilterEmployedMax = new DateTime(dttemp.Year, dttemp.Month, dttemp.Day, 0, 0, 0);
                    }
                    catch
                    {
                        _dtFilterEmployedMax = DateTime.MinValue;
                    }
                }
                else
                {
                    _dtFilterEmployedMax = DateTime.MinValue;
                }
            }
        }

        public string SortOrder
        {
            get { return _sSortOrder; }
            set { _sSortOrder = value.Trim().ToLower(); }
        }

        public int Page
        {
            get { return _nPage; }
            set { _nPage = value; }
        }

        public int RecordsOnPage
        {
            get { return _nRecordsOnPage; }
            set { _nRecordsOnPage = value; }
        }

        #endregion Properties

        public EmployeeTable()
        {
        }

        public EmployeeTable(SqlConnection connection)
        {
            ReadFromDB(connection);
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<EmployeeRecord> GetRecords()
        {
            return _Records;
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<EmployeeRecord> GetRecordsPage(int startRows, int maxRows)
        {
            return _Records;
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select, false)]
        public List<EmployeeRecord> GetRecordsPageSorted(int startRows, int maxRows, string sortBy, ref int totalEmployees)
        {
            return _Records;
        }

        public int GetCount()
        {
            return _Records.Count;
        }

        public int GetCountTotal()
        {
            if (_nCountTotal == -1)
                _nCountTotal = GetCount();

            return _nCountTotal;
        }

        public void Clear()
        {
            _Records.Clear();
            _bEmpty = true;
            _nCountTotal = -1;
            _sFilterCity = "";
            _sFilterDepartment = "";
            _sFilterCustom = "";
            _sFilterCustom1 = "";
            _nPage = 0;
            _nRecordsOnPage = 0;
       }

        public void ReadFromDB(SqlConnection connection)
        {
            SqlCommand sqlSelect = BuildSelectQuery(connection, false);
            SqlCommand sqlSelectCnt = BuildSelectQuery(connection, true);
            bool connectionCloseOnExit = false;

            _Records.Clear();
            _bEmpty = true;
            _nCountTotal = -1;

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connectionCloseOnExit = true;
                connection.Open();
            }

            SqlDataReader sqlReaderCnt = sqlSelectCnt.ExecuteReader();
            if (sqlReaderCnt.Read())
            {
                try { _nCountTotal = Convert.ToInt32(sqlReaderCnt["cnt"]); }
                catch { _nCountTotal = -1; }
            }
            sqlReaderCnt.Close();

            SqlDataReader sqlReader = sqlSelect.ExecuteReader();

            _bEmpty = false;

            while (sqlReader.Read())
            {
                if (sqlReader["id"] == null || sqlReader["id"] == DBNull.Value)
                    continue;

                EmployeeRecord record = new EmployeeRecord();


                try { record.ID = Convert.ToInt32(sqlReader["id"]); }
                catch { continue; }

                if (sqlReader["last_name"] != null && sqlReader["last_name"] != DBNull.Value)
                    record.LastName = sqlReader["last_name"].ToString().Trim();
                if (sqlReader["first_name"] != null && sqlReader["first_name"] != DBNull.Value)
                    record.FirstName = sqlReader["first_name"].ToString().Trim();
                if (sqlReader["middle_name"] != null && sqlReader["middle_name"] != DBNull.Value)
                    record.MiddleName = sqlReader["middle_name"].ToString().Trim();

                if (sqlReader["dob"] != null && sqlReader["dob"] != DBNull.Value)
                {
                    try { record.dtDOB = Convert.ToDateTime(sqlReader["dob"]); }
                    catch { record.DOB = null; }
                }
                if (sqlReader["employed"] != null && sqlReader["employed"] != DBNull.Value)
                {
                    try { record.dtEmployed = Convert.ToDateTime(sqlReader["employed"]); }
                    catch { record.Employed = null; }
                }

                if (sqlReader["phone_work"] != null && sqlReader["phone_work"] != DBNull.Value)
                    record.PhoneWork = sqlReader["phone_work"].ToString().Trim();
                if (sqlReader["phone_mobile"] != null && sqlReader["phone_mobile"] != DBNull.Value)
                    record.PhoneMobile = sqlReader["phone_mobile"].ToString().Trim();
                if (sqlReader["email"] != null && sqlReader["email"] != DBNull.Value)
                    record.Email = sqlReader["email"].ToString().Trim();
                if (sqlReader["department"] != null && sqlReader["department"] != DBNull.Value)
                    record.Department = sqlReader["department"].ToString().Trim();
                if (sqlReader["job_title"] != null && sqlReader["job_title"] != DBNull.Value)
                    record.JobTitle = sqlReader["job_title"].ToString().Trim();
                if (sqlReader["city"] != null && sqlReader["city"] != DBNull.Value)
                    record.City = sqlReader["city"].ToString().Trim();

                
                _Records.Add(record);
            }
            sqlReader.Close();

            if (CountTotal < 0) _nCountTotal = _Records.Count;

            if (connectionCloseOnExit == true)
                connection.Close();
        }

        public EmployeeRecord GetRecordByID(SqlConnection connection, int id)
        {
            EmployeeRecord record = new EmployeeRecord();
            SqlCommand sqlSelect = BuildSelectByIdQuery(connection, id);
            bool connectionCloseOnExit = false;

            if (connection.State != System.Data.ConnectionState.Open)
            {
                connectionCloseOnExit = true;
                connection.Open();
            }

            SqlDataReader sqlReader = sqlSelect.ExecuteReader();
            if (sqlReader.Read())
            {
                try { record.ID = Convert.ToInt32(sqlReader["id"]); }
                catch { }

                if (sqlReader["last_name"] != null && sqlReader["last_name"] != DBNull.Value)
                    record.LastName = sqlReader["last_name"].ToString().Trim();
                if (sqlReader["first_name"] != null && sqlReader["first_name"] != DBNull.Value)
                    record.FirstName = sqlReader["first_name"].ToString().Trim();
                if (sqlReader["middle_name"] != null && sqlReader["middle_name"] != DBNull.Value)
                    record.MiddleName = sqlReader["middle_name"].ToString().Trim();

                if (sqlReader["dob"] != null && sqlReader["dob"] != DBNull.Value)
                {
                    try { record.dtDOB = Convert.ToDateTime(sqlReader["dob"]); }
                    catch { record.DOB = null; }
                }
                if (sqlReader["employed"] != null && sqlReader["employed"] != DBNull.Value)
                {
                    try { record.dtEmployed = Convert.ToDateTime(sqlReader["employed"]); }
                    catch { record.Employed = null; }
                }

                if (sqlReader["phone_work"] != null && sqlReader["phone_work"] != DBNull.Value)
                    record.PhoneWork = sqlReader["phone_work"].ToString().Trim();
                if (sqlReader["phone_mobile"] != null && sqlReader["phone_mobile"] != DBNull.Value)
                    record.PhoneMobile = sqlReader["phone_mobile"].ToString().Trim();
                if (sqlReader["email"] != null && sqlReader["email"] != DBNull.Value)
                    record.Email = sqlReader["email"].ToString().Trim();
                if (sqlReader["department"] != null && sqlReader["department"] != DBNull.Value)
                    record.Department = sqlReader["department"].ToString().Trim();
                if (sqlReader["job_title"] != null && sqlReader["job_title"] != DBNull.Value)
                    record.JobTitle = sqlReader["job_title"].ToString().Trim();
                if (sqlReader["city"] != null && sqlReader["city"] != DBNull.Value)
                    record.City = sqlReader["city"].ToString().Trim();
                if (sqlReader["login_name"] != null && sqlReader["login_name"] != DBNull.Value)
                    record.Login = sqlReader["login_name"].ToString().Trim();
                if (sqlReader["org_city"] != null && sqlReader["org_city"] != DBNull.Value)
                    record.OrgCity = sqlReader["org_city"].ToString().Trim();
                if (sqlReader["org_street"] != null && sqlReader["org_street"] != DBNull.Value)
                    record.OrgStreet = sqlReader["org_street"].ToString().Trim();
                if (sqlReader["org_house_no"] != null && sqlReader["org_house_no"] != DBNull.Value)
                    record.OrgHouseNo = sqlReader["org_house_no"].ToString().Trim();
                if (sqlReader["post_box"] != null && sqlReader["post_box"] != DBNull.Value)
                    record.OrgPostBox = sqlReader["post_box"].ToString().Trim();


                _Records.Add(record);
            }
            sqlReader.Close();

            if (connectionCloseOnExit == true)
                connection.Close();

            return record;
        }

        private SqlCommand BuildSelectQuery(SqlConnection connection, bool countQuery)
        {
            bool param_custom = (FilterCustom.Length > 0);
            bool param_custom1 = (FilterCustom1.Length > 0);
            bool param_department = (FilterDepartment.Length > 0);
            bool param_city = (FilterCity.Length > 0);
            bool paging = (RecordsOnPage > 0);
            int offset = (paging) ? (_nPage * _nRecordsOnPage + 1) : (1);
            int page_end = (paging) ? (offset + _nRecordsOnPage) : (Int32.MaxValue);
            StringBuilder sbQuery = new StringBuilder();
            SqlCommand query = new SqlCommand();

            if (countQuery)
                sbQuery.Append("SELECT COUNT(id) AS cnt ");
            else
                sbQuery.Append("SELECT * ");
            sbQuery.Append("FROM ");
            sbQuery.Append("( ");
            sbQuery.AppendFormat("SELECT *, ROW_NUMBER() OVER(ORDER BY {0}) AS RowNum ",
                GetOrderBy());
            sbQuery.Append("FROM ");
            sbQuery.Append("( ");
            sbQuery.Append("SELECT ");
            sbQuery.Append("u407.request AS id, ");
            sbQuery.Append("u407.f8_nachname AS last_name, ");
            sbQuery.Append("u407.f7_vorname AS first_name, ");
            sbQuery.Append("u407.f34_ AS middle_name, ");
            sbQuery.Append("u407.f16_geburtst AS dob, ");
            sbQuery.Append("u278.f10_internen AS phone_work, ");
            sbQuery.Append("u407.f9_mobil AS phone_mobile, ");
            sbQuery.Append("u407.f26_email AS email, ");
            sbQuery.Append("a278.f18_titel AS department, ");
            sbQuery.Append("u407.f61_ AS job_title, ");
            sbQuery.Append("u407.f32_ad AS city, ");
            sbQuery.Append("u407.f45_ AS employed ");
            if (param_custom)
            {
                sbQuery.Append(", LOWER( ");
                sbQuery.Append("ISNULL(u407.f8_nachname + ' ', '') ");
                sbQuery.Append("+ ISNULL(u407.f7_vorname + ' ', '') "); 
                sbQuery.Append("+ ISNULL(u407.f34_ + ' ', '') ");
                sbQuery.Append("+ REPLACE(REPLACE(REPLACE(REPLACE(ISNULL(u278.f10_internen + ' ', ''),')',''),'(',''),'+',''), ' ', '') "); 
                sbQuery.Append("+ REPLACE(REPLACE(REPLACE(REPLACE(ISNULL(u407.f9_mobil + ' ', ''),')',''),'(',''),'+',''), ' ', '') ");
                sbQuery.Append("+ ISNULL(u407.f26_email + ' ', '') ");
                sbQuery.Append("+ ISNULL(a278.f18_titel + ' ', '') ");
                sbQuery.Append("+ ISNULL(u407.f61_ + ' ', '') ");
                sbQuery.Append("+ ISNULL(u407.f32_ad + ' ', '') ");
                sbQuery.Append(") AS search_field ");
            }
            sbQuery.Append("FROM [OmniTracker].[dbo].[UserFields407] AS u407 ");
            sbQuery.Append("LEFT OUTER JOIN [OmniTracker].[dbo].[UserFields278] AS u278 ON u407.request = u278.request ");
            sbQuery.Append("LEFT OUTER JOIN [OmniTracker].[dbo].[UserFields278] AS a278 ON u407.f44_ = a278.request ");
            sbQuery.Append("WHERE u407.f60_ <> 1 ");
            sbQuery.Append("AND u278.f9_status = 161 ");
            sbQuery.Append(") AS SearchTable ");
            sbQuery.Append("WHERE id > 0 ");
            if (param_custom)
            {
                List<string> listSearchParams = GetSearchParams(_sFilterCustom);
                int count = listSearchParams.Count;

                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        sbQuery.AppendFormat("AND search_field like @search_field{0} ", i + 1);
                        query.Parameters.Add(String.Format("@search_field{0}", i + 1), SqlDbType.NVarChar).Value = string.Format("%{0}%", listSearchParams[i]);
                    }
                }
            }
            if (param_custom1)
            {
                List<string> listSearchParams = GetSearchParams(_sFilterCustom1);
                int count = listSearchParams.Count;

                if (count > 0)
                {
                    sbQuery.Append("AND (");
                    for (int i = 0; i < count; i++)
                    {
                        if (i > 0)
                            sbQuery.Append("OR ");
                        sbQuery.AppendFormat("last_name like @last_name{0} ", i + 1);
                        query.Parameters.Add(String.Format("@last_name{0}", i + 1), SqlDbType.NVarChar).Value = string.Format("{0}%", listSearchParams[i]);
                        sbQuery.AppendFormat("OR first_name like @first_name{0} ", i + 1);
                        query.Parameters.Add(String.Format("@first_name{0}", i + 1), SqlDbType.NVarChar).Value = string.Format("{0}%", listSearchParams[i]);
                        sbQuery.AppendFormat("OR middle_name like @middle_name{0} ", i + 1);
                        query.Parameters.Add(String.Format("@middle_name{0}", i + 1), SqlDbType.NVarChar).Value = string.Format("{0}%", listSearchParams[i]);
                    }
                    sbQuery.Append(") ");
                }
            }
            if (param_department)
            {
                sbQuery.Append("AND department = @department ");
                query.Parameters.Add("@department", SqlDbType.NVarChar).Value = _sFilterDepartment;
            }
            if (param_city)
            {
                sbQuery.Append("AND city = @city ");
                query.Parameters.Add("@city", SqlDbType.NVarChar).Value = _sFilterCity;
            }
            if ((_dtFilterDobMin != DateTime.MinValue && _dtFilterDobMax == DateTime.MinValue) ||
                (_dtFilterDobMax != DateTime.MinValue && _dtFilterDobMin == DateTime.MinValue)
                )
            {
                DateTime date = (_dtFilterDobMin != DateTime.MinValue) ? (_dtFilterDobMin) : (_dtFilterDobMax);

                if (date.Day == 29 && date.Month == 2)
                {
                    sbQuery.AppendFormat("AND '{0}-{1}-{2}' = CAST( ('{0}-' + STR(MONTH(dob)) + '-' + STR(DAY(dob))) AS DATETIME) ",
                        date.Year, date.Month, date.Day);
                }
                else
                {
                    // We have troubles with a leaf day (29 Feb) and this query
                    sbQuery.AppendFormat("AND dob = CAST( (STR(YEAR(dob)) + '-{0}-{1}') AS DATETIME) ",
                        date.Month, date.Day);
                }
            }
            else if (_dtFilterDobMin != DateTime.MinValue && _dtFilterDobMax != DateTime.MinValue)
            {
                DateTime date_min = (_dtFilterDobMin < _dtFilterDobMax) ? (_dtFilterDobMin) : (_dtFilterDobMax);
                DateTime date_max = (_dtFilterDobMin > _dtFilterDobMax) ? (_dtFilterDobMin) : (_dtFilterDobMax);
                bool bOr = date_min.Year != date_max.Year;

                if ((date_min.Day == 29 && date_min.Month == 2) ||
                    (date_max.Day == 29 && date_max.Month == 2))
                {
                    sbQuery.AppendFormat("AND ('{0}-{1}-{2}' <= CAST( ('{0}-' + STR(MONTH(dob)) + '-' + STR(DAY(dob))) AS DATETIME) ",
                        date_min.Year, date_min.Month, date_min.Day);
                    sbQuery.AppendFormat("{3} '{0}-{1}-{2}' >= CAST( ('{0}-' + STR(MONTH(dob)) + '-' + STR(DAY(dob))) AS DATETIME) ) ",
                        date_max.Year, date_max.Month, date_max.Day, (bOr) ? ("OR") : ("AND"));
                }
                else
                {
                    // We have troubles if min or max date is a leaf day (29 Feb), query will be failed
                    sbQuery.AppendFormat("AND (dob >= CAST( (STR(YEAR(dob)) + '-{0}-{1}') AS DATETIME) ",
                        date_min.Month, date_min.Day);
                    sbQuery.AppendFormat("{2} dob <= CAST( (STR(YEAR(dob)) + '-{0}-{1}') AS DATETIME) ) ",
                        date_max.Month, date_max.Day, (bOr) ? ("OR") : ("AND"));
                }
            }
            if ((_dtFilterEmployedMin != DateTime.MinValue && _dtFilterEmployedMax == DateTime.MinValue) ||
                (_dtFilterEmployedMax != DateTime.MinValue && _dtFilterEmployedMin == DateTime.MinValue)
                )
            {
                DateTime date = (_dtFilterEmployedMin != DateTime.MinValue) ? (_dtFilterEmployedMin) : (_dtFilterEmployedMax);
                sbQuery.AppendFormat("AND employed = '{0}-{1}-{2}') AS DATETIME) ",
                    date.Year, date.Month, date.Day);
            }
            else if (_dtFilterEmployedMin != DateTime.MinValue && _dtFilterEmployedMax != DateTime.MinValue)
            {
                DateTime date_min = (_dtFilterEmployedMin < _dtFilterEmployedMax) ? (_dtFilterEmployedMin) : (_dtFilterEmployedMax);
                DateTime date_max = (_dtFilterEmployedMin > _dtFilterEmployedMax) ? (_dtFilterEmployedMin) : (_dtFilterEmployedMax);
                bool bOr = date_min.Year != date_max.Year;

                sbQuery.AppendFormat("AND (employed >= '{0}-{1}-{2}' ",
                    date_min.Year, date_min.Month, date_min.Day);
                sbQuery.AppendFormat("{3} employed <= '{0}-{1}-{2}') ",
                    date_max.Year, date_max.Month, date_max.Day, (bOr) ? ("OR") : ("AND"));
            }
            sbQuery.Append(") AS RowConstrainedResult ");
            if (!countQuery)
            {
                sbQuery.Append("WHERE ");
                sbQuery.Append("RowNum >= @r1 AND RowNum < @r2 ");
                sbQuery.Append("ORDER BY RowNum ");

                query.Parameters.Add("@r1", SqlDbType.Int).Value = offset;
                query.Parameters.Add("@r2", SqlDbType.Int).Value = page_end;
            }
            query.Connection = connection;
            query.CommandText = sbQuery.ToString();

            return query;
        }

        private SqlCommand BuildSelectByIdQuery(SqlConnection connection, int id)
        {
            StringBuilder sbQuery = new StringBuilder();
            SqlCommand query = new SqlCommand();

            sbQuery.AppendLine("SELECT u407.request AS id,");
            sbQuery.AppendLine("u407.f8_nachname AS last_name,");
            sbQuery.AppendLine("u407.f7_vorname AS first_name,");
            sbQuery.AppendLine("u407.f34_ AS middle_name,");
            sbQuery.AppendLine("u407.f16_geburtst AS dob,");
            sbQuery.AppendLine("u278.f10_internen AS phone_work,");
            sbQuery.AppendLine("u407.f9_mobil AS phone_mobile,");
            sbQuery.AppendLine("u407.f26_email AS email,");
            sbQuery.AppendLine("a278.f18_titel AS department,");
            sbQuery.AppendLine("u407.f61_ AS job_title,");
            sbQuery.AppendLine("u407.f32_ad AS city,");
            sbQuery.AppendLine("u407.f30_ad AS login_name,");
            sbQuery.AppendLine("u407.f45_ AS employed,");
            sbQuery.AppendLine("u407.f23_otbenutz AS user_reserved,");
            sbQuery.AppendLine("u408.f6_ort AS org_city,");
            sbQuery.AppendLine("u408.f7_strae AS org_street,");
            sbQuery.AppendLine("u408.f8_hausnumme AS org_house_no,");
            sbQuery.AppendLine("u408.f14_postfach AS post_box");
            sbQuery.AppendLine("FROM");
            sbQuery.AppendLine("[OmniTracker].[dbo].[UserFields407] AS u407");
            sbQuery.AppendLine("LEFT OUTER JOIN [OmniTracker].[dbo].ReferenceList2444 AS link_table ON u407.request = link_table.request");
            sbQuery.AppendLine("LEFT OUTER JOIN [OmniTracker].[dbo].[UserFields408] AS u408 ON link_table.reference = u408.request");
            sbQuery.AppendLine("LEFT OUTER JOIN [OmniTracker].[dbo].[UserFields278] AS u278 ON u407.request = u278.request");
            sbQuery.AppendLine("LEFT OUTER JOIN [OmniTracker].[dbo].[UserFields278] AS a278 ON u407.f44_ = a278.request");
            sbQuery.AppendLine("WHERE u407.request = @request");

            query.Connection = connection;
            query.CommandText = sbQuery.ToString();
            query.Parameters.Add("@request", SqlDbType.Int).Value = id;

            return query;
        }

        private string GetOrderBy()
        {
            string orderby = "";

            switch (SortOrder)
            {
                case "name":
                    orderby = "last_name, first_name, middle_name";
                    break;
                case "email":
                    orderby = "email";
                    break;
                case "phonemobile":
                    orderby = "phone_mobile";
                    break;
                case "phonework":
                    orderby = "phone_work";
                    break;
                case "phones":
                    orderby = "phone_mobile, phone_work";
                    break;
                case "department":
                    orderby = "department";
                    break;
                case "jobtitle":
                    orderby = "job_title";
                    break;
                case "job":
                    orderby = "department, job_title";
                    break;
                case "city":
                    orderby = "city";
                    break;
                default:
                    orderby = "last_name, first_name, middle_name";
                    break;
            }

            return orderby;
        }

        List<string> GetSearchParams(string search)
        {
            List<string> list = new List<string>();

            string sslist = search.Trim().ToLower();

            if (sslist.Length > 0)
            {
                string slist_item = "";
                foreach (char c in sslist)
                {
                    if (char.IsSeparator(c) || c == '-')
                    {
                        if (slist_item.Length > 0)
                        {
                            list.Add(slist_item);
                            slist_item = "";
                        }
                        continue;
                    }

                    if (c == '(' || c == ')' || c == '+') continue;

                    slist_item += c;
                }
                if (slist_item.Length > 0)
                {
                    list.Add(slist_item);
                    slist_item = "";
                }
            }

            return list;
        }
    }
}
