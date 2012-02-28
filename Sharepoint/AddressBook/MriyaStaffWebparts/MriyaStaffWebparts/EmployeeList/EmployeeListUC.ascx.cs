using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using MriyaStaffDAL;
using System.Data;
using System.Data.SqlClient;

namespace MriyaStaffWebparts.EmployeeList
{
    public partial class EmployeeListUC : UserControl
    {
        public EmployeeList _webPart = null;
        
        private string _siteUrl = "";

        // Database
        System.Data.SqlClient.SqlConnection _connectionDb = null;
        private EmployeeTable _tableEmployees = new EmployeeTable();

        // Private attributes
        private bool _bNeedToUpdateLabels = true;
        private bool _bShowErrorMessage = false;
        private bool _bShowDetailsDiv = false;

        // Web part properties
        private string _sConnectionString = "Data Source=NESO;Initial Catalog=OmniTracker;Integrated Security=True";
        private string _sConnectionStringPhoto = "Data Source=NESO;Initial Catalog=MriyaUserData;Integrated Security=True";
        private const string _sEncodeParams = "InntSo873lect23ution";

        private string _sNoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        bool _bShowRefreshShowAll = true;
        bool _bShowNumberOfRecords = true;

        int _recordNumber = 2;
        int _columnNumber = 1;
        EmployeeList.RecordSelectionType _recordSelectionType = EmployeeList.RecordSelectionType.Randomly;

        protected ListItem[] _dataListRow = new ListItem[] {
            new ListItem(Properties.Resources.textHeaderPhoto),  
            new ListItem(Properties.Resources.textHeaderName),  
            new ListItem(Properties.Resources.textHeaderJobTitle, false),  
            new ListItem(Properties.Resources.textHeaderDepartment, false),  
            new ListItem(Properties.Resources.textHeaderDob, false),
            new ListItem(Properties.Resources.textHeaderPhoneWork, false),  
            new ListItem(Properties.Resources.textHeaderPhoneMobile, false),  
            new ListItem(Properties.Resources.textHeaderEmail, false),
            new ListItem(Properties.Resources.textHeaderCity, false)
        };

        private enum dataListRowNames
        {
            rowPhoto = 0,
            rowName = 1,
            rowJobTitle = 2,
            rowDepartment = 3,
            rowBirthday = 4,
            rowPhoneWork = 5,
            rowPhoneMobile = 6,
            rowEmail = 7,
            rowCity = 8
        };

        private bool _bEnableDetails = true;

        private bool _bShowDetailsName = true;
        private bool _bShowDetailsJobTitle = true;
        private bool _bShowDetailsDepartment = true;
        private bool _bShowDetailsDob = true;
        private bool _bShowDetailsWPhone = true;
        private bool _bShowDetailsMhone = true;
        private bool _bShowDetailsEmail = true;
        private bool _bShowDetailsCity = true;

        // Filter
        private bool _bGetNewEmployeesOnly = false;
        private uint _nNewEmployeeDays = 30;
        private bool _bGetEmployeesWithBirthdayOnly = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_webPart != null)
            {

                _sNoProfileImageFile = _webPart.NoProfileImageFile;
                _bShowRefreshShowAll = _webPart.EnableRefreshShowAll;


                _sConnectionString = _webPart.OTConnectionString;
                _sConnectionStringPhoto = _webPart.PhotoConnectionString;

                _dataListRow[(int)dataListRowNames.rowPhoto].Visible = _webPart.ShowColumnPhoto;
                _dataListRow[(int)dataListRowNames.rowName].Visible = _webPart.ShowColumnName;
                _dataListRow[(int)dataListRowNames.rowJobTitle].Visible = _webPart.ShowColumnJob;
                _dataListRow[(int)dataListRowNames.rowDepartment].Visible = _webPart.ShowColumnDepartment;
                _dataListRow[(int)dataListRowNames.rowBirthday].Visible = _webPart.ShowColumnDOB;
                _dataListRow[(int)dataListRowNames.rowPhoneWork].Visible = _webPart.ShowColumnPhoneWork;
                _dataListRow[(int)dataListRowNames.rowPhoneMobile].Visible = _webPart.ShowColumnPhoneMobile;
                _dataListRow[(int)dataListRowNames.rowEmail].Visible = _webPart.ShowColumnEmail;
                _dataListRow[(int)dataListRowNames.rowCity].Visible = _webPart.ShowColumnCity;

                _bEnableDetails = _webPart.ShowDetailsPopup;

                _bShowDetailsName = _webPart.ShowDetailsName;
                _bShowDetailsJobTitle = _webPart.ShowDetailsJobTitle;
                _bShowDetailsDepartment = _webPart.ShowDetailsDepartment;
                _bShowDetailsDob = _webPart.ShowDetailsDOB;
                _bShowDetailsWPhone = _webPart.ShowDetailsPhoneWork;
                _bShowDetailsMhone = _webPart.ShowDetailsPhoneMobile;
                _bShowDetailsEmail = _webPart.ShowDetailsEmail;
                _bShowDetailsCity = _webPart.ShowColumnCity;

                _recordNumber = _webPart.NumberOfRecords;
                _columnNumber = _webPart.NumberOfColumns;
                _recordSelectionType = _webPart.SelectionType;
                _bGetNewEmployeesOnly = _webPart.ShowNewEmployeesOnly;
                _nNewEmployeeDays = _webPart.NewEmployeesDays;
                _bGetEmployeesWithBirthdayOnly = _webPart.ShowWhosBirthdayOnly;
                if (_bGetEmployeesWithBirthdayOnly)
                {
                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    DayOfWeek day = now.DayOfWeek;
                    int days = day - DayOfWeek.Monday;

                    if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.Today)
                    {
                        _tableEmployees.FilterDOBMinDT = now;
                        _tableEmployees.FilterDOBMax = null;
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.Yesterday)
                    {
                        _tableEmployees.FilterDOBMinDT = now.AddDays(-1);
                        _tableEmployees.FilterDOBMax = null;
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.Tomorrow)
                    {
                        _tableEmployees.FilterDOBMinDT = now.AddDays(1);
                        _tableEmployees.FilterDOBMax = null;
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.ThisWeek)
                    {
                        _tableEmployees.FilterDOBMinDT = DateTime.Now.AddDays(-days);
                        _tableEmployees.FilterDOBMaxDT = _tableEmployees.FilterDOBMinDT.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.LastWeek)
                    {
                        _tableEmployees.FilterDOBMinDT = DateTime.Now.AddDays(-days - 7);
                        _tableEmployees.FilterDOBMaxDT = _tableEmployees.FilterDOBMinDT.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.NextWeek)
                    {
                        _tableEmployees.FilterDOBMinDT = DateTime.Now.AddDays(-days + 7);
                        _tableEmployees.FilterDOBMaxDT = _tableEmployees.FilterDOBMinDT.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.ThisMonth)
                    {
                        _tableEmployees.FilterDOBMinDT = new DateTime(now.Year, now.Month, 1);
                        _tableEmployees.FilterDOBMaxDT = _tableEmployees.FilterDOBMinDT.AddMonths(1).AddDays(-1);
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.LastMonth)
                    {
                        _tableEmployees.FilterDOBMinDT = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                        _tableEmployees.FilterDOBMaxDT = _tableEmployees.FilterDOBMinDT.AddMonths(1).AddDays(-1);
                    }
                    else if (_webPart.BirthdayTimeframe == EmployeeList.Timeframe.NextMonth)
                    {
                        _tableEmployees.FilterDOBMinDT = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                        _tableEmployees.FilterDOBMaxDT = _tableEmployees.FilterDOBMinDT.AddMonths(1).AddDays(-1);
                    }
                }
                else if (_bGetNewEmployeesOnly)
                {
                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                    _tableEmployees.FilterDOBMin = null;
                    _tableEmployees.FilterDOBMax = null;

                    _tableEmployees.FilterEmployedMaxDT = now;
                    _tableEmployees.FilterEmployedMinDT = now.AddDays(-_nNewEmployeeDays);
                }
            }

            // Create connection to the DB
            try
            {
                _connectionDb = new System.Data.SqlClient.SqlConnection(_sConnectionString);
                _connectionDb.Open();
            }
            catch (Exception ex)
            {
                ShowErrorMessage("The error was occured while open connection to the database:<br/>" +
                    ex.Message);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            bool bPost = this.IsPostBack;

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                _siteUrl = site.Url;
            }

            // Set search hint
            SetLocalizedText();

            panelRefresh.Visible = _bShowRefreshShowAll;

            if (bPost)
            {
                if (ViewState["bNeedToUpdateLabels"] != null)
                    _bNeedToUpdateLabels = (bool)ViewState["bNeedToUpdateLabels"];

                if (ViewState["_tableEmployees.FilterDOBMinDT"] != null)
                    _tableEmployees.FilterDOBMinDT = Convert.ToDateTime(ViewState["_tableEmployees.FilterDOBMinDT"]);
                if (ViewState["_tableEmployees.FilterDOBMaxDT"] != null)
                    _tableEmployees.FilterDOBMaxDT = Convert.ToDateTime(ViewState["_tableEmployees.FilterDOBMaxDT"]);
                if (ViewState["_tableEmployees.FilterEmployedMinDT"] != null)
                    _tableEmployees.FilterEmployedMinDT = Convert.ToDateTime(ViewState["_tableEmployees.FilterEmployedMinDT"]);
                if (ViewState["_tableEmployees.FilterEmployedMaxDT"] != null)
                    _tableEmployees.FilterEmployedMaxDT = Convert.ToDateTime(ViewState["_tableEmployees.FilterEmployedMaxDT"]);

                if (ViewState["_bShowDetailsDiv"] != null)
                    _bShowDetailsDiv = Convert.ToBoolean(ViewState["_bShowDetailsDiv"]);

                if (Request.Form["__EVENTTARGET"] == "ShowRecordDetails")
                {
                    onShowDetails(Request.Form["__EVENTARGUMENT"]);
                }
            }
            else
            {
                // Fill auxilary (filter) combos
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Show table
            ShowRecords();

            // Show error message (if there are any)
            labelError.Visible = _bShowErrorMessage;

            base.OnPreRender(e);
        }

        protected void linkButtonRefresh_Click(object sender, EventArgs e)
        {

        }

        protected void linkButtonShowAll_Click(object sender, EventArgs e)
        {
            if (linkButtonRefresh.Visible)
            {
                linkButtonRefresh.Visible = false;
                labelRefreshDelim.Visible = false;
                linkButtonShowAll.Text = Properties.Resources.textButtonHide;
                _bShowNumberOfRecords = false;
                _recordNumber = int.MaxValue;
            }
            else
            {
                linkButtonRefresh.Visible = true;
                labelRefreshDelim.Visible = true;
                linkButtonShowAll.Text = Properties.Resources.textButtonShowAll;
                _bShowNumberOfRecords = true;
            }
        }

        protected void onShowDetails(string sid)
        {
            if (!_bEnableDetails)
                return;

            int id = 0;
            if (!int.TryParse(sid, out id))
                id = 0;
            ShowRecordDetails(true, id);
        }

        protected void linkButtonCloseDetails_Command(object sender, CommandEventArgs e)
        {
            ShowRecordDetails(false, 0);
        }

        #region Implementation

        private void SetLocalizedText()
        {
            // Show localized text
            if (_bNeedToUpdateLabels == true)
            {
                linkButtonRefresh.Text = Properties.Resources.textButtonRefresh;
                linkButtonShowAll.Text = Properties.Resources.textButtonShowAll;
                linkButtonCloseDetailsHdr.Text = Properties.Resources.textButtonCloseDetails;
                linkButtonCloseDetailsFtr.Text = Properties.Resources.textButtonCloseDetails;
                ViewState["bNeedToUpdateLabels"] = _bNeedToUpdateLabels = false;
            }
        }

        private void ShowErrorMessage(string message)
        {
            labelError.Text = message;
            _bShowErrorMessage = true;
        }

        private static string EncryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the encoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToEncrypt = UTF8.GetBytes(Message);

            // Step 5. Attempt to encrypt the string
            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the encrypted string as a base64 encoded string
            return Convert.ToBase64String(Results);
        }

        private static string DecryptString(string Message, string Passphrase)
        {
            byte[] Results;
            System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();

            // Step 1. We hash the passphrase using MD5
            // We use the MD5 hash generator as the result is a 128 bit byte array
            // which is a valid length for the TripleDES encoder we use below

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(Passphrase));

            // Step 2. Create a new TripleDESCryptoServiceProvider object
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            // Step 3. Setup the decoder
            TDESAlgorithm.Key = TDESKey;
            TDESAlgorithm.Mode = CipherMode.ECB;
            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            // Step 4. Convert the input string to a byte[]
            byte[] DataToDecrypt = Convert.FromBase64String(Message);

            // Step 5. Attempt to decrypt the string
            try
            {
                ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
            }
            finally
            {
                // Clear the TripleDes and Hashprovider services of any sensitive information
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            // Step 6. Return the decrypted string in UTF8 format
            return UTF8.GetString(Results);
        }

        private void ShowRecordDetails(bool show, int rec_id)
        {
            EmployeeRecord rec = null;
            string javaScript = "";
            
            try
            {
                rec = (show) ? (_tableEmployees.GetRecordByID(_connectionDb, rec_id)) : (null);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("The error was occured while open connection to the database:<br/>" +
                    ex.Message);
            }

            if (show && rec == null)
                show = false;

            panelMrPBookRecordDetails.Visible = show;
            ViewState["_bShowDetailsDiv"] = _bShowDetailsDiv = show;
            string encodedDefPhotoUrl = Server.UrlEncode(_siteUrl + _sNoProfileImageFile);
            string encodedConnectionString = Server.UrlEncode(EncryptString(_sConnectionStringPhoto, _sEncodeParams));
            string sPictureHandler = String.Format("{0}/_layouts/MriyaStaffWebparts/ShowPhoto.ashx?id={1}&npi={2}&cs={3}",
                _siteUrl, rec_id, encodedDefPhotoUrl, encodedConnectionString);

            if (show)
            {
                StringBuilder sb = new StringBuilder();
                string sName = "";

                if (rec.MiddleName.Length > 0)
                {
                    sName += rec.LastName;
                    if (sName.Length > 0) sName += " ";
                    sName += rec.FirstName;
                    if (sName.Length > 0) sName += " ";
                    sName += rec.MiddleName;
                }
                else
                {
                    sName += rec.FirstName;
                    if (sName.Length > 0) sName += " ";
                    sName += rec.LastName;
                }

                int line = 0;
                string cssRowStyle = "styleMrPBookDetailsRow";
                string cssRowStyleAlt = "styleMrPBookDetailsRowAlt";
                sb.AppendLine("<table class=\"styleMrPBookDetailsTable\" padding=\"0\" cellspacing=\"0\" border=\"0\" style=\"width:100%;\">");
                if (_bShowDetailsName && sName.Trim().Length > 0)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td colspan=\"2\"><div class=\"stylrMrPBookName\">{1}</div></td></tr>\n",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), sName);
                }
                if (_bShowDetailsJobTitle && rec.JobTitle.Trim().Length > 0)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td style=\"width:100px;\"><div class=\"stylrMrPBookPosition\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderJobTitle);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookPosition\">{0}</div></td></tr>\n",
                        rec.JobTitle);
                }
                if (_bShowDetailsDepartment && rec.Department.Trim().Length > 0)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td valign=\"top\"><div class=\"stylrMrPBookDeparment\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderDepartment);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookDeparment\">{0}</div></td></tr>\n",
                        rec.Department);
                }
                if (_bShowDetailsDob && rec.DOB != null)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td valign=\"top\"><div class=\"stylrMrPBookDob\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderDob);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookDob\">{0}</div></td></tr>\n",
                        rec.dtDOB.ToString("m"));
                }
                if (_bShowDetailsWPhone && rec.PhoneWork.Trim().Length > 0)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td valign=\"top\"><div class=\"stylrMrPBookWPhone\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderPhoneWork1);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookWPhone\">{0}</div></td></tr>\n",
                        rec.PhoneWork);
                }
                if (_bShowDetailsMhone && rec.PhoneMobile.Trim().Length > 0)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td valign=\"top\"><div class=\"stylrMrPBookMPhone\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderPhoneMobile);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookMPhone\">{0}</div></td></tr>\n",
                        rec.PhoneMobile);
                }
                if (_bShowDetailsEmail && rec.Email.Trim().Length > 0)
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td valign=\"top\"><div class=\"stylrMrPBookEmail\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderEmail);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookEmail\"><a href=\"mailto:{0}\">{0}</a></div></td></tr>\n",
                        rec.Email);
                }
                if (_bShowDetailsCity && (rec.City.Trim().Length > 0 || rec.OrgCity.Trim().Length > 0))
                {
                    sb.AppendFormat("<tr class=\"{0}\"><td valign=\"top\"><div class=\"stylrMrPBookCity\"><span class=\"styleMrPBookDetailsLabel\">{1}: </span></div></td>",
                        ((line++ % 2) != 0) ? (cssRowStyleAlt) : (cssRowStyle), Properties.Resources.textHeaderCity);
                    sb.AppendFormat("<td valign=\"top\"><div class=\"stylrMrPBookCity\">{0}</div></td></tr>\n",
                        (rec.City.Length > 0) ? (rec.City) : (rec.OrgCity));
                }
                sb.AppendLine("</table>");
                literalDetails.Text = sb.ToString();
                imageDetails.ImageUrl = sPictureHandler;
            }
            else
            {
                javaScript = "hideDetailsDiv('divMrPBookRecordDetailsOuter');";
                imageDetails.ImageUrl = _siteUrl + _sNoProfileImageFile;
                literalDetails.Text = "";
            }
            javaScript = String.Format("showDetailsDiv({0});",
                (show) ? ('1') : ('0'));
            this.Page.ClientScript.RegisterStartupScript(this.GetType(),
                "RecDetailsWindowInCenterScreen", javaScript, true);
        }

        protected bool ShowRecords()
        {
            StringBuilder sbCards = new StringBuilder();

            try
            {
                _tableEmployees.ReadFromDB(_connectionDb);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("The error was occured while reading data table: <br/>" + ex.Message);
                labelError.Visible = true;
                return false;
            }

            List<EmployeeRecord> recordsCollectionSrc = _tableEmployees.GetRecords();
            List<EmployeeRecord> recordsCollection = new List<EmployeeRecord>();
            int iAllCount = recordsCollectionSrc.Count;
            int iMaxRecordNumber = (_recordNumber == int.MaxValue) ? (int.MaxValue) : (_recordNumber * _columnNumber);

            // Select records
            if (_recordSelectionType == EmployeeList.RecordSelectionType.Randomly)
            {
                var rnd = new Random();
                int count = (iMaxRecordNumber < recordsCollectionSrc.Count) ? (iMaxRecordNumber) : (recordsCollectionSrc.Count);
                while (count > 0)
                {
                    var index = rnd.Next(recordsCollectionSrc.Count);
                    recordsCollection.Add(recordsCollectionSrc[index]);
                    recordsCollectionSrc.RemoveAt(index);
                    count--;
                }

            }
            else if (_recordSelectionType == EmployeeList.RecordSelectionType.FromTheEnd)
            {
                int max = (iMaxRecordNumber < recordsCollectionSrc.Count) ? (iMaxRecordNumber) : (recordsCollectionSrc.Count);
                for (int i = recordsCollectionSrc.Count - 1, cnt = 0; i >= 0 && cnt < max; i--, cnt++)
                    recordsCollection.Add(recordsCollectionSrc[i]);
            }
            else
            {
                for (int i = 0; i < recordsCollectionSrc.Count && i < _recordNumber; i++)
                    recordsCollection.Add(recordsCollectionSrc[i]);
            }

            if (_columnNumber > 1)
            {
                int iRecsCount = recordsCollection.Count;
                int iRec = 0;
                sbCards.AppendLine("<table class=\"styleMrEListTable\">");
                for (int rc = 0; rc < _recordNumber; rc++)
                {
                    sbCards.AppendLine("<tr class=\"styleMrEListTableRow\">");
                    for (int cc = 0; cc < _columnNumber; cc++)
                    {
                        sbCards.AppendLine("<td>");
                        if (iRec < iRecsCount)
                        {
                            sbCards.Append(ShowRecord(recordsCollection[iRec++]));
                        }
                        sbCards.AppendLine("</td>");
                    }
                    sbCards.AppendLine("</tr>");
                    if (iRec >= iRecsCount)
                        break;
                }
                sbCards.AppendLine("</table>");
            }
            else
            {
                foreach (EmployeeRecord rec in recordsCollection)
                {
                    sbCards.Append(ShowRecord(rec));
                }

            }

            literalCards.Text = sbCards.ToString();

            if (_bShowNumberOfRecords)
            {
                linkButtonShowAll.Text = string.Format("{0} ({1})",
                    Properties.Resources.textButtonShowAll, iAllCount);
            }
            return true;
        }

        string ShowRecord(EmployeeRecord record)
        {
            StringBuilder sbRecord = new StringBuilder();
            string sEmail = "";
            bool bTodayIsBirthday = false;
            bool bEOLNeeded = false;
            string showDetailsCmd = (_bEnableDetails) ? 
                (String.Format("javascript:__doPostBack('ShowRecordDetails', '{0}');", record.ID)) : 
                ("");

            if (record.DOB != null && record.dtDOB.Day == DateTime.Today.Day &&
                    record.dtDOB.Month == DateTime.Today.Month)
            {
                bTodayIsBirthday = true;
            }

            if (record.Email.Trim().Length > 0)
                sEmail = string.Format("<a href=\"mailto:{0}\">{0}</a>", record.Email);

            sbRecord.AppendLine("<div class=\"styleMrEListCard\">");
            sbRecord.AppendLine("<div class=\"styleMrEListCardShadowRght\">");
            sbRecord.AppendLine("<div class=\"styleMrEListCardBody\">");
            sbRecord.AppendLine("<div class=\"styleMrEListCardPAdd\">");

            if (_dataListRow[(int)dataListRowNames.rowPhoto].Visible)
            {
                string encodedDefPhotoUrl = Server.UrlEncode(_siteUrl + _sNoProfileImageFile);
                string encodedConnectionString = Server.UrlEncode(EncryptString(_sConnectionStringPhoto, _sEncodeParams));
                string sPictureHandler = String.Format("{0}/_layouts/MriyaStaffWebparts/ShowPhoto.ashx?id={1}&npi={2}&cs={3}",
                    _siteUrl, record.ID, encodedDefPhotoUrl, encodedConnectionString);

                sbRecord.AppendLine("<div class=\"styleMrEListCardPhotoImg\">");
                sbRecord.AppendLine(string.Format("<a href=\"{0}\"><img  src=\"{1}\"></a>",
                    showDetailsCmd, sPictureHandler));
                sbRecord.AppendLine("</div>");
            }

            sbRecord.AppendLine("<div class=\"styleMrEListCardText\">");

            if (_dataListRow[(int)dataListRowNames.rowName].Visible)
            {
                if (bEOLNeeded)
                    sbRecord.AppendLine("<br/>");

                string sEName = record.LastName;

                if (sEName.Length > 0)
                    sEName += "<br/>";

                if (record.FirstName.Length > 0)
                {
                    if (sEName.Length > 0)
                        sEName += " ";
                    sEName += record.FirstName;
                }
                if (record.MiddleName.Length > 0)
                {
                    if (sEName.Length > 0)
                        sEName += " ";
                    sEName += record.MiddleName;
                }

                sbRecord.AppendFormat("<span class=\"styleMrEListCardName{0}\"><a href=\"{1}\">{2}</a></span>\n",
                        ((bTodayIsBirthday) ? (" styleMrEListCardTodayIsBirthday") : ("")),
                        showDetailsCmd, sEName);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowJobTitle].Visible)
            {
                sbRecord.AppendFormat("{0}<span class=\"styleMrEListCardJobTitle\">{1}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    record.JobTitle);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowDepartment].Visible)
            {
                sbRecord.AppendFormat("{0}<span class=\"styleMrEListCardDepartment\">{1}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    record.Department);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowBirthday].Visible)
            {
                string sBirthday = "";

                if (record.DOB != null)
                    sBirthday = record.dtDOB.ToString("dd MMMM");

                sbRecord.AppendFormat("{0}<span class=\"{1}\"><span class=\"styleMrEListCardItemCaption\">Дата народження:</span> {2}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    ((bTodayIsBirthday == true) ? ("styleMrEListCardBirthday styleMrEListCardTodayIsBirthday") : ("styleMrEListCardBirthday")),
                    sBirthday);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowPhoneWork].Visible)
            {
                sbRecord.AppendFormat("{0}<span class=\"styleMrEListCardPhoneWork\"><span class=\"styleMrEListCardItemCaption\">{1}:</span> {2}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    Properties.Resources.textHeaderPhoneWork1,
                    record.PhoneWork);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowPhoneMobile].Visible)
            {
                sbRecord.AppendFormat("{0}<span class=\"styleMrEListCardPhoneMobile\"><span class=\"styleMrEListCardItemCaption\">{1}:</span> {2}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    Properties.Resources.textHeaderPhoneMobile,
                    record.PhoneMobile);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowEmail].Visible)
            {
                sbRecord.AppendFormat("{0}<span class=\"styleMrEListCardEmail\"><span class=\"styleMrEListCardItemCaption\">E-mail:</span> {1}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    sEmail);
                bEOLNeeded = true;
            }

            if (_dataListRow[(int)dataListRowNames.rowCity].Visible)
            {
                sbRecord.AppendFormat("{0}<span class=\"styleMrEListCardCity\">{1}</span>\n",
                    (bEOLNeeded) ? ("<br/>") : (""),
                    record.City);
                bEOLNeeded = true;
            }
            sbRecord.AppendLine("</div>");   // styleMrEListCardText

            sbRecord.AppendLine("<div style=\"clear:both\"></div>");
            sbRecord.AppendLine("</div>"); // styleMrEListCardPAdd
            sbRecord.AppendLine("</div>"); // styleMrEListCardBody
            sbRecord.AppendLine("</div>"); // styleMrEListCardShadowRght

            sbRecord.AppendLine("<div class=\"styleMrEListCardShadBttmLft\"></div>");
            sbRecord.AppendLine("<div class=\"styleMrEListCardShadBttmRgt\"></div>");

            sbRecord.AppendLine("</div>"); //styleMrEListCard

            return sbRecord.ToString();
        }

        #endregion Implementation
    }
}
