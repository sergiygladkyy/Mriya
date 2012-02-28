using System;
using System.Text;
using System.Security.Cryptography;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using MriyaStaffDAL;
using System.Data;
using System.Data.SqlClient;

namespace MriyaStaffWebparts.PhoneBookMini
{
    public partial class PhoneBookMiniUserControl : UserControl
    {
        public PhoneBookMini _webPart { get; set; }

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

        private string _sCaptionText = "Пошук працівників";
        private string _sPhotoImageFile = "/SiteCollectionImages/photo_icn.gif";
        private string _sNoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        private bool _bShowDetailsName = true;
        private bool _bShowDetailsJobTitle = true;
        private bool _bShowDetailsDepartment = true;
        private bool _bShowDetailsDob = true;
        private bool _bShowDetailsWPhone = true;
        private bool _bShowDetailsMhone = true;
        private bool _bShowDetailsEmail = true;
        private bool _bShowDetailsCity = true;

        private int _nMaxResults = 9;

        private bool _bExecuting = false;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_webPart != null)
            {
                _sCaptionText = _webPart.CaptionText;
                _nMaxResults = _webPart.MaxResults;

                _sPhotoImageFile = _webPart.PhotoImageFile;
                _sNoProfileImageFile = _webPart.NoProfileImageFile;

                _sConnectionString = _webPart.OTConnectionString;
                _sConnectionStringPhoto = _webPart.PhotoConnectionString;

                _bShowDetailsName = _webPart.ShowDetailsName;
                _bShowDetailsJobTitle = _webPart.ShowDetailsJobTitle;
                _bShowDetailsDepartment = _webPart.ShowDetailsDepartment;
                _bShowDetailsDob = _webPart.ShowDetailsDob;
                _bShowDetailsWPhone = _webPart.ShowDetailsPhoneWork;
                _bShowDetailsMhone = _webPart.ShowDetailsPhoneMobile;
                _bShowDetailsEmail = _webPart.ShowDetailsEmail;
                _bShowDetailsCity = _webPart.ShowDetailsCity;
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
            SetTextBoxHint(textBoxSearch, _sCaptionText);
            SetLocalizedText();

            if (bPost)
            {
                if (ViewState["_tableEmployees.FilterCustom"] != null)
                    _tableEmployees.FilterCustom = ViewState["_tableEmployees.FilterCustom"].ToString();

                if (ViewState["_bShowDetailsDiv"] != null)
                    _bShowDetailsDiv = Convert.ToBoolean(ViewState["_bShowDetailsDiv"]);

                if (Request.Form["__EVENTTARGET"] == "ShowRecordDetails")
                {
                    onShowDetails(Request.Form["__EVENTARGUMENT"]);
                }
            }
            else
            {
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Show table
            ShowResults();

            // Show error message (if there are any)
            labelError.Visible = _bShowErrorMessage;

            base.OnPreRender(e);
        }

        protected void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (_bExecuting)
                return;
            _bExecuting = true;

            StartSearch();
        }

        protected void imageButtonClear_Click(object sender, ImageClickEventArgs e)
        {
            if (_bExecuting)
                return;
            _bExecuting = true;

            StopSearch();
        }

        protected void imageButtonSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (_bExecuting)
                return;
            _bExecuting = true;

            StartSearch();
        }

        protected void linkButtonCloseDetails_Command(object sender, CommandEventArgs e)
        {
            ShowRecordDetails(false, 0);
        }

        protected void onShowDetails(string sid)
        {
            int id = 0;
            if (!int.TryParse(sid, out id))
                id = 0;
            ShowRecordDetails(true, id);
        }

        #region Implementation

        protected void StartSearch()
        {
            if (textBoxSearch.Text.Trim().Length < 1)
            {
                StopSearch();
                return;
            }

            if (_tableEmployees.FilterCustom != textBoxSearch.Text)
            {
                ViewState["_tableEmployees.FilterCustom"] = _tableEmployees.FilterCustom = SanitizeSearchText(textBoxSearch.Text);
                panelResults.Visible = true;
            }
        }

        protected void StopSearch()
        {
            textBoxSearch.Text = "";
            SetTextBoxHint(textBoxSearch, _sCaptionText);
            panelResults.Visible = false;
            ViewState["_tableEmployees.FilterCustom"] = _tableEmployees.FilterCustom = "";
        }

        protected void ShowResults()
        {
            if (panelResults.Visible == false)
                return;

            try
            {
                _tableEmployees.ReadFromDB(_connectionDb);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("The error was occured while reading data table: <br/>" + ex.Message);
                labelError.Visible = true;
                return;
            }

            if (_tableEmployees.CountTotal < 1)
            {
                labelRecords.Text = Properties.Resources.textNoRecordsFound;
                labelRecords.Visible = true;
                return;
            }
            else
            {
                labelRecords.Text = "";
                labelRecords.Visible = false;
            }

            string encodedDefPhotoUrl = Server.UrlEncode(_siteUrl + _sNoProfileImageFile);
            string encodedConnectionString = Server.UrlEncode(EncryptString(_sConnectionStringPhoto, _sEncodeParams));

            int nRec = 0;
            foreach (EmployeeRecord rec in _tableEmployees.GetRecords())
            {
                TableRow row = new TableRow();

                string showDetailsCmd = String.Format("javascript:__doPostBack('ShowRecordDetails', '{0}');", rec.ID);

                TableCell cell = new TableCell();
                HyperLink hlCell = new HyperLink();
                LiteralControl lcCell = new LiteralControl();

                string sName = rec.LastName;

                //if (sName.Length > 0)
                //    sName += "<br/>";

                if (rec.FirstName.Length > 0)
                {
                    if (sName.Length > 0)
                        sName += " ";
                    sName += rec.FirstName;
                }
                if (rec.MiddleName.Length > 0)
                {
                    if (sName.Length > 0)
                        sName += " ";
                    sName += rec.MiddleName;
                }

                //lbCell.Command += new CommandEventHandler(buttonTableCell_Command);
                cell.ID = this.ClientID + "_lastname_cell_" + nRec.ToString();
                hlCell.CssClass = "styleMrPBookMiniDetailsLink";
                hlCell.Text = sName;
                hlCell.NavigateUrl = showDetailsCmd;
                cell.Controls.Add(hlCell);
                row.Cells.Add(cell);

                if (nRec++ % 2 != 0)
                    row.CssClass = "styleMrPBookMiniRowAlt";
                else
                    row.CssClass = "styleMrPBookMiniRow";

                tableResults.Rows.Add(row);

                if (nRec > _nMaxResults)
                {
                    labelRecords.Text = "Уточніть пошук будь ласка...";
                    labelRecords.Visible = true;
                    break;
                }
            }

        }

        protected void SetTextBoxHint(TextBox textBox, string defaultText)
        {
            textBox.Attributes.Add("onfocus", "clearMrPBSearchText(this,'" + defaultText + "')");
            textBox.Attributes.Add("onblur", "resetMrPBSearchText(this,'" + defaultText + "')");

            if (textBox.Text == "")
            {
                textBox.Text = defaultText;
                textBox.ForeColor = System.Drawing.Color.Silver;
            }
            else if (textBox.Text == defaultText)
            {
                textBox.ForeColor = System.Drawing.Color.Silver;
            }
            else
            {
                textBox.ForeColor = System.Drawing.Color.Black;
            }
        }

        private void SetLocalizedText()
        {
            // Show localized text
            if (ViewState["bNeedToUpdateLabels"] != null)
                _bNeedToUpdateLabels = (bool)ViewState["bNeedToUpdateLabels"];

            if (_bNeedToUpdateLabels == true)
            {
                textBoxSearch.ToolTip = _sCaptionText;
                imageButtonClear.ToolTip = Properties.Resources.textButtonClearSearch;
                imageButtonSearch.ToolTip = Properties.Resources.textSearchButtonLabel;
                ViewState["bNeedToUpdateLabels"] = _bNeedToUpdateLabels = false;
            }
        }

        private void ShowErrorMessage(string message)
        {
            labelError.Text = message;
            _bShowErrorMessage = true;
        }


        private string SanitizeSearchText(string search)
        {
            string sanitized = search.Trim();
            if (sanitized == Properties.Resources.textSearchLabel)
                sanitized = "";
            return sanitized;
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

        #endregion

    }
}
