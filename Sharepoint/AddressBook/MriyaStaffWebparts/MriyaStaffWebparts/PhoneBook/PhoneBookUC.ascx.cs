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

namespace MriyaStaffWebparts.PhoneBook
{
    public partial class PhoneBookUC : UserControl
    {
        // Public attributes
        public PhoneBook _webPart = null;

        private string _siteUrl = "";

        // Database
        System.Data.SqlClient.SqlConnection _connectionDb = null;
        private EmployeeTable _tableEmployees = new EmployeeTable();

        // Private attributes
        private bool _bNeedToUpdateLabels = true;
        private bool _bShowErrorMessage = false;
        private bool _bShowDetailsDiv = false;

        // Data table
        private TableHeaderRow _rowTableHeader = new TableHeaderRow();
        private TableHeaderRow _rowTableFooter = new TableHeaderRow();
        private int _nRecordsOnPage = 20;
        private int _nPagesTotal = 0;
        private int _nScreenPage = 0;
        private int _nScreenPageOffset = 0;

        // Column definition
        private enum columnNames
        {
            columnPhoto = 0,
            columnName = 1,
            columnEmail = 2,
            columnPhoneMobile = 3,
            columnPhoneWork = 4,
            columnPhones = 5,
            columnDepartment = 6,
            columnJobTitle = 7,
            columnJob = 8,
            columnCity = 9
        };
        private TableColumn[] _tableColumns = new TableColumn[] {
            new TableColumn(Properties.Resources.textHeaderPhoto, ""),  
            new TableColumn(Properties.Resources.textHeaderName, "Name"),  
            new TableColumn(Properties.Resources.textHeaderEmail, "Email", false),  
            new TableColumn(Properties.Resources.textHeaderPhoneMobile, "PhoneMobile", false),  
            new TableColumn(Properties.Resources.textHeaderPhoneWork, "Phone", false),  
            new TableColumn(Properties.Resources.textHeaderPhones, "Phones"),  
            new TableColumn(Properties.Resources.textHeaderDepartment, "Department", false),  
            new TableColumn(Properties.Resources.textHeaderJobTitle, "JobTitle", false),  
            new TableColumn(Properties.Resources.textHeaderJob, "Job"),  
            new TableColumn(Properties.Resources.textHeaderCity, "City", false)
        };

        // Web part properties
        private string _sConnectionString = "Data Source=NESO;Initial Catalog=OmniTracker;Integrated Security=True";
        private string _sConnectionStringPhoto = "Data Source=NESO;Initial Catalog=MriyaUserData;Integrated Security=True";
        private const string _sEncodeParams = "InntSo873lect23ution";

        private const int MAXPAGEFOOTERBUTTONS = 9;
        private const int MAXFOOTERBUTTONS = MAXPAGEFOOTERBUTTONS + 2; // Forth and back
        private LinkButton[] _footerButtons = new LinkButton[MAXFOOTERBUTTONS];

        private string _sCaptionText = Properties.Resources.textWPCaption;
        private string _sPhotoImageFile = "/SiteCollectionImages/photo_icn.gif";
        private string _sNoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        private bool _bShowCaptionPanel = true;
        private bool _bShowSearchPanel = true;
        private bool _bShowSearchStatusPanel = true;

        private bool _bShowDetailsName = true;
        private bool _bShowDetailsJobTitle = true;
        private bool _bShowDetailsDepartment = true;
        private bool _bShowDetailsDob = true;
        private bool _bShowDetailsWPhone = true;
        private bool _bShowDetailsMhone = true;
        private bool _bShowDetailsEmail = true;
        private bool _bShowDetailsCity = true;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_webPart != null)
            {
                _bShowCaptionPanel = _webPart.EnableCaption;
                _sCaptionText = _webPart.CaptionText;
                _bShowSearchPanel = _webPart.EnableSearch;
                _bShowSearchStatusPanel = _webPart.EnableSearchStatus;
                _sPhotoImageFile = _webPart.PhotoImageFile;
                _sNoProfileImageFile = _webPart.NoProfileImageFile;
                if (_webPart.TablePaging)
                    _nRecordsOnPage = _webPart.TablePageSize;
                else
                    _nRecordsOnPage = 0;

                _sConnectionString = _webPart.OTConnectionString;
                _sConnectionStringPhoto = _webPart.PhotoConnectionString;

                _tableColumns[(int)columnNames.columnPhoto].Visible = _webPart.ShowColumnPhoto;
                _tableColumns[(int)columnNames.columnName].Visible = _webPart.ShowColumnName;
                _tableColumns[(int)columnNames.columnEmail].Visible = _webPart.ShowColumnEmail;
                _tableColumns[(int)columnNames.columnPhones].Visible = _webPart.ShowColumnPhones;
                _tableColumns[(int)columnNames.columnJob].Visible = _webPart.ShowColumnJob;
                _tableColumns[(int)columnNames.columnCity].Visible = _webPart.ShowColumnCity;

                _bShowDetailsName = _webPart.ShowDetailsName;
                _bShowDetailsJobTitle = _webPart.ShowDetailsJobTitle;
                _bShowDetailsDepartment = _webPart.ShowDetailsDepartment;
                _bShowDetailsDob = _webPart.ShowDetailsDob;
                _bShowDetailsWPhone = _webPart.ShowDetailsPhoneWork;
                _bShowDetailsMhone = _webPart.ShowDetailsPhoneMobile;
                _bShowDetailsEmail = _webPart.ShowDetailsEmail;
                _bShowDetailsCity = _webPart.ShowColumnCity;
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
            SetTextBoxHint(textBoxSearch, Properties.Resources.textSearchLabel);
            SetLocalizedText();
            CreateTableHeader();
            CreateTableFooter();

            if (bPost)
            {
                if (ViewState["bNeedToUpdateLabels"] != null)
                    _bNeedToUpdateLabels = (bool)ViewState["bNeedToUpdateLabels"];

                if (ViewState["_nScreenPage"] != null)
                    _nScreenPage = Convert.ToInt32(ViewState["_nScreenPage"]);
                if (ViewState["_nScreenPageOffset"] != null)
                    _nScreenPageOffset = Convert.ToInt32(ViewState["_nScreenPageOffset"]);
                if (ViewState["_nPagesTotal"] != null)
                    _nPagesTotal = Convert.ToInt32(ViewState["_nPagesTotal"]);

                // Restore custom filters and sort order
                if (ViewState["_tableEmployees.FilterCity"] != null)
                    _tableEmployees.FilterCity = ViewState["_tableEmployees.FilterCity"].ToString();
                if (ViewState["_tableEmployees.FilterDepartment"] != null)
                    _tableEmployees.FilterDepartment = ViewState["_tableEmployees.FilterDepartment"].ToString();
                if (ViewState["_tableEmployees.FilterCustom"] != null)
                    _tableEmployees.FilterCustom = ViewState["_tableEmployees.FilterCustom"].ToString();
                if (ViewState["_tableEmployees.SortOrder"] != null)
                    _tableEmployees.SortOrder = ViewState["_tableEmployees.SortOrder"].ToString();

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
                FillFilterCombos();
            }

            panelMrPBookCaption.Visible = _bShowCaptionPanel;
            panelSearch.Visible = _bShowSearchPanel;
            panelMrPBookStatus.Visible = _bShowSearchStatusPanel;
        }

        protected override void OnPreRender(EventArgs e)
        {
            // Show table
            ShowTable();

            // Show error message (if there are any)
            labelError.Visible = _bShowErrorMessage;

            base.OnPreRender(e);
        }

        void buttonHeader_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName.Trim().Length > 0)
            {
                ViewState["_tableEmployees.SortOrder"] = _tableEmployees.SortOrder = e.CommandName;
            }
        }

        void buttonFooter_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Next")
            {
                _nScreenPageOffset++;
                if (_nScreenPageOffset == MAXPAGEFOOTERBUTTONS)
                {
                    if (_nScreenPage + MAXPAGEFOOTERBUTTONS < _nPagesTotal)
                        ScrollPagesForward(true);
                    else
                        _nScreenPageOffset = MAXPAGEFOOTERBUTTONS - 1;
                }
            }
            else if (e.CommandName == "Previous")
            {
                _nScreenPageOffset--;
                if (_nScreenPageOffset < 0)
                {
                    if (_nScreenPage > 0)
                        ScrollPagesBackward(true);
                    else
                        _nScreenPageOffset = 0;
                }
            }
            else if (e.CommandName == "Page")
            {
                int newOffset = 0;
                try { newOffset = Convert.ToInt32(e.CommandArgument); }
                catch { newOffset = 0; }

                // Scroll forward (half of rack) if last page button is clicked
                if (newOffset == MAXPAGEFOOTERBUTTONS - 1 && _nScreenPage + MAXPAGEFOOTERBUTTONS < _nPagesTotal)
                    ScrollPagesForward(false);
                // Scroll backward (half of rack) if first page button is clicked
                else if (newOffset == 0 && _nScreenPage > 0)
                    ScrollPagesBackward(false);
                // Go to the selected page
                else
                    _nScreenPageOffset = newOffset;
            }
            ViewState["_nScreenPage"] = _nScreenPage;
            ViewState["_nScreenPageOffset"] = _nScreenPageOffset;
        }

        protected void buttonSearchFilter_Click(object sender, EventArgs e)
        {
            panelFilterBlock.Visible = !panelFilterBlock.Visible;
            if (panelFilterBlock.Visible)
            {
                buttonSearchFilter.Text = Properties.Resources.textSearchButtonFilterSearchAlt;
                buttonSearchFilter.CssClass = "styleMrPBookSearchCancelFilterButton";
            }
            else
            {
                listCity.SelectedIndex = 0;
                listDepartment.SelectedIndex = 0;
                textBoxSearch.Text = "";

                ViewState["_nScreenPage"] = _nScreenPage = 0;
                ViewState["_nScreenPageOffset"] = _nScreenPageOffset = 0;

                ViewState["_tableEmployees.FilterCustom"] = _tableEmployees.FilterCustom = "";
                ViewState["_tableEmployees.FilterCity"] = _tableEmployees.FilterCity = "";
                ViewState["_tableEmployees.FilterDepartment"] = _tableEmployees.FilterDepartment = "";
                buttonSearchFilter.Text = Properties.Resources.textSearchButtonFilterSearch;
                buttonSearchFilter.CssClass = "styleMrPBookSearchFilterButton";

                SetTextBoxHint(textBoxSearch, Properties.Resources.textSearchLabel);
            }
        }

        protected void listCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["_nScreenPage"] = _nScreenPage = 0;
            ViewState["_nScreenPageOffset"] = _nScreenPageOffset = 0;
            if (listCity.SelectedIndex > 0)
                ViewState["_tableEmployees.FilterCity"] = _tableEmployees.FilterCity = listCity.Items[listCity.SelectedIndex].Text;
            else
                ViewState["_tableEmployees.FilterCity"] = _tableEmployees.FilterCity = "";
        }

        protected void listDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["_nScreenPage"] = _nScreenPage = 0;
            ViewState["_nScreenPageOffset"] = _nScreenPageOffset = 0;
            if (listDepartment.SelectedIndex > 0)
                ViewState["_tableEmployees.FilterDepartment"] = _tableEmployees.FilterDepartment = listDepartment.Items[listDepartment.SelectedIndex].Text;
            else
                ViewState["_tableEmployees.FilterDepartment"] = _tableEmployees.FilterDepartment = "";
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {
            if (_tableEmployees.FilterCustom != textBoxSearch.Text)
            {
                ViewState["_nScreenPage"] = _nScreenPage = 0;
                ViewState["_nScreenPageOffset"] = _nScreenPageOffset = 0;
                ViewState["_tableEmployees.FilterCustom"] = _tableEmployees.FilterCustom = SanitizeSearchText(textBoxSearch.Text);
            }
        }

        protected void buttonClearSearch_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";

            ViewState["_nScreenPage"] = _nScreenPage = 0;
            ViewState["_nScreenPageOffset"] = _nScreenPageOffset = 0;
            ViewState["_tableEmployees.FilterCustom"] = _tableEmployees.FilterCustom = "";

            SetTextBoxHint(textBoxSearch, Properties.Resources.textSearchLabel);
        }

        protected void onShowDetails(string sid)
        {
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
            if (_bNeedToUpdateLabels == true)
            {
                labelMrPBookCaption.Text = Properties.Resources.textWPCaption;
                buttonClearSearch.Text = Properties.Resources.textButtonClearSearch;
                buttonSearch.Text = Properties.Resources.textSearchButtonLabel;
                buttonSearch.ToolTip = Properties.Resources.textSearchHint;
                buttonSearchFilter.Text = Properties.Resources.textSearchButtonFilterSearch;
                buttonSearchFilter.ToolTip = Properties.Resources.textSearchFilterHint;
                labelCity.Text = Properties.Resources.textCity;
                listCity.ToolTip = Properties.Resources.textCityHint;
                labelDepartment.Text = Properties.Resources.textDepartment;
                listDepartment.ToolTip = Properties.Resources.textDepartmentHint;
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

        private bool FillFilterCombos()
        {
            // Reset
            listCity.Items.Clear();
            listDepartment.Items.Clear();

            // First item is empty
            listCity.Items.Add("");
            listDepartment.Items.Add("");

            // Read items from DB
            try
            {
                CityTable tableCities = new CityTable(_connectionDb);

                foreach (CityRecord cr in tableCities.GetRecords())
                {
                    listCity.Items.Add(cr.Name);
                }

                DepartmentTable tableDepartments = new DepartmentTable(_connectionDb);
                foreach (DepartmentRecord dr in tableDepartments.GetRecords())
                {
                    listDepartment.Items.Add(dr.Name);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("The error was occured while filling filter combo boxes: <br/>" + ex.Message);
                return false;
            }
            return true;
        }

        private void CreateTableHeader()
        {
            tableMrPBook.Rows.Clear();

            int col = 0;
            foreach (TableColumn column in _tableColumns)
            {
                col++;

                if (!column.Visible) continue;

                column.LinkButton = new LinkButton();
                column.LinkButton.CausesValidation = false;
                column.LinkButton.ID = string.Format("lbCellHeader{0}{1}", this.ClientID, col);
                column.LinkButton.Text = column.Caption;
                column.LinkButton.CommandName = column.SortCommand;
                column.LinkButton.Command += new CommandEventHandler(buttonHeader_Command);

                TableHeaderCell cell = new TableHeaderCell();
                cell.Controls.Add(column.LinkButton);
                _rowTableHeader.Cells.Add(cell);
            }
            _rowTableHeader.CssClass = "styleMrPBookTableHeader";
            _rowTableHeader.Visible = false;
            tableMrPBook.Rows.Add(_rowTableHeader);
        }

        private void CreateTableFooter()
        {
            tableMrPBookFooter.Rows.Clear();

            if (_nRecordsOnPage < 1) return;

            double percent_width_firstlast = 20;
            double percent_n = (100 - 2 * percent_width_firstlast) / MAXPAGEFOOTERBUTTONS;
            for (int i = 0; i < MAXFOOTERBUTTONS; i++)
            {
                TableHeaderCell cell = new TableHeaderCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                _footerButtons[i] = new LinkButton();
                _footerButtons[i].CausesValidation = false;
                _footerButtons[i].ID = string.Format("buttonMrPBookFooter{0}{1}", this.ClientID, i);
                if (i == 0)
                {
                    _footerButtons[i].Text = Properties.Resources.textFooterPrevious;
                    _footerButtons[i].CommandName = "Previous"; 
                    cell.Width = Unit.Percentage(percent_width_firstlast);
                }
                else if (i == MAXFOOTERBUTTONS - 1)
                {
                    _footerButtons[i].Text = Properties.Resources.textFooterNext;
                    _footerButtons[i].CommandName = "Next";
                    cell.Width = Unit.Percentage(percent_width_firstlast);
                }
                else
                {
                    _footerButtons[i].Text = String.Format("{0}", _nScreenPage + 1 + i);
                    _footerButtons[i].CommandName = "Page";
                    _footerButtons[i].CommandArgument = String.Format("{0}", i - 1);
                    cell.Width = Unit.Percentage(percent_n);
                }
                _footerButtons[i].Command += new CommandEventHandler(buttonFooter_Command);

                cell.Controls.Add(_footerButtons[i]);
                _rowTableFooter.Cells.Add(cell);
            }
            _rowTableFooter.CssClass = "styleMrPBookTableFooter";
            tableMrPBookFooter.Rows.Add(_rowTableFooter);
            _rowTableFooter.Visible = (_nPagesTotal > 0);
        }

        private bool ShowTable()
        {
            try
            {
                _tableEmployees.Page = _nScreenPage + _nScreenPageOffset;
                _tableEmployees.RecordsOnPage = _nRecordsOnPage;
                _tableEmployees.ReadFromDB(_connectionDb);
            }
            catch (Exception ex)
            {
                ShowErrorMessage("The error was occured while reading data table: <br/>" + ex.Message);
                labelError.Visible = true;
                return false;
            }

            if (_nRecordsOnPage > 0)
                _nPagesTotal = (int)Math.Ceiling((double)_tableEmployees.CountTotal / (double)_nRecordsOnPage);
            else
                _nPagesTotal = 0;
            ViewState["_nPagesTotal"] = _nPagesTotal; 

            if (_bShowSearchStatusPanel)
            {
                if (_tableEmployees.CountTotal < 1)
                    labelRecords.Text = Properties.Resources.textNoRecordsFound;
                else
                    labelRecords.Text = String.Format(Properties.Resources.textRecorsFound, _tableEmployees.CountTotal);
            }

            string encodedDefPhotoUrl = Server.UrlEncode(_siteUrl + _sNoProfileImageFile);
            string encodedConnectionString = Server.UrlEncode(EncryptString(_sConnectionStringPhoto, _sEncodeParams));

            // Show records
            int nRec = 0;
            foreach (EmployeeRecord rec in _tableEmployees.GetRecords())
            {
                TableRow row = new TableRow();

                string showDetailsCmd = String.Format("javascript:__doPostBack('ShowRecordDetails', '{0}');", rec.ID);
    
                if (_tableColumns[(int)columnNames.columnPhoto].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    string sPictureHandler = String.Format("{0}/_layouts/MriyaStaffWebparts/ShowPhoto.ashx?id={1}&npi={2}&cs={3}",
                        _siteUrl, rec.ID, encodedDefPhotoUrl, encodedConnectionString);

                    string sPhotoURL = string.Format("<a onmouseover=\"showMrPBPhotoDialog('{0}', '{1}'); return true;\" " +
                        "onmouseout=\"hideMrPBPhotoDialog('{1}'); return true;\" " +
                        "href=\"{3}\"> " +
                        "<img id=\"{0}\" width=\"23\" height=\"16\" src=\"{2}\"></a>",
                        this.ClientID + "_photo_img_ctl_" + nRec.ToString(),
                        sPictureHandler, _siteUrl + _sPhotoImageFile, showDetailsCmd);
                    lcCell.Text = sPhotoURL;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnName].Visible)
                {
                    TableCell cell = new TableCell();
                    HyperLink hlCell = new HyperLink();
                    LiteralControl lcCell = new LiteralControl();

                    string sName = rec.LastName;

                    if (sName.Length > 0)
                        sName += "<br/>";

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
                    hlCell.CssClass = "styleMrPBookShowDetailsLink";
                    hlCell.Text = sName;
                    hlCell.NavigateUrl = showDetailsCmd;
                    cell.Controls.Add(hlCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnEmail].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    lcCell.Text = rec.Email;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnPhoneWork].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    cell.Width = Unit.Pixel(120);

                    lcCell.Text = rec.PhoneWork;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnPhoneMobile].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    lcCell.Text = rec.PhoneMobile;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnPhones].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    cell.Width = Unit.Pixel(120);

                    lcCell.Text = rec.PhoneWork;
                    if (lcCell.Text.Length > 0)
                        lcCell.Text += "<br/>";
                    lcCell.Text += rec.PhoneMobile;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnDepartment].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    lcCell.Text = rec.Department;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnJobTitle].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    lcCell.Text = rec.JobTitle;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnJob].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    lcCell.Text = rec.Department;
                    if (lcCell.Text.Length > 0)
                        lcCell.Text += "<br/>";
                    lcCell.Text += rec.JobTitle;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }
                if (_tableColumns[(int)columnNames.columnCity].Visible)
                {
                    TableCell cell = new TableCell();
                    LiteralControl lcCell = new LiteralControl();

                    lcCell.Text = rec.City;
                    cell.Controls.Add(lcCell);
                    row.Cells.Add(cell);
                }

                if (nRec++ % 2 != 0)
                    row.CssClass = "styleMrPBookTableRowAlt";
                else
                    row.CssClass = "styleMrPBookTableRow";
                
                tableMrPBook.Rows.Add(row);
            }

            _rowTableHeader.Visible = (nRec > 0);

            if (_bShowErrorMessage)
                labelError.Visible = true;

            UpdateTableFooter();
         
            return true;
        }

        private void UpdateTableFooter()
        {
            if (_nRecordsOnPage < 1)
                return;

            // Show/hid Previos and Next buttons in the table footer
            _footerButtons[0].Visible = (_nScreenPage != 0 || _nScreenPageOffset != 0);
            _footerButtons[MAXFOOTERBUTTONS - 1].Visible = (_nScreenPage + _nScreenPageOffset < _nPagesTotal - 1);

            // Update all page buttons, set text and command arguments
            for (int i = MAXPAGEFOOTERBUTTONS - 1; i >= 0; i--)
            {
                // Remove all cells which are not visible to keep hor formating by center
                if (i + _nScreenPage >= _nPagesTotal)
                {
                    _rowTableFooter.Cells.RemoveAt(i + 1);
                    continue;
                }
                _footerButtons[i + 1].Text = String.Format("{0}", _nScreenPage + i + 1);
                _footerButtons[i + 1].CommandArgument = String.Format("{0}", i);
                _footerButtons[i + 1].Enabled = !(i == _nScreenPageOffset);
                _footerButtons[i + 1].Visible = true;
            }

            _rowTableFooter.Visible = (_nPagesTotal > 1);
        }

        private void ScrollPagesForward(bool selectNextPage)
        {
            if (_nScreenPage + MAXPAGEFOOTERBUTTONS < _nPagesTotal)
            {
                _nScreenPageOffset = (int)(MAXPAGEFOOTERBUTTONS / 2);
                _nScreenPage += (MAXPAGEFOOTERBUTTONS - _nScreenPageOffset - 1);
                if (selectNextPage)
                    _nScreenPage++;

                if (_nScreenPageOffset + _nScreenPage >= _nPagesTotal)
                {
                    _nScreenPageOffset = 0;
                    _nScreenPage = _nPagesTotal - MAXPAGEFOOTERBUTTONS;
                }
            }
        }

        private void ScrollPagesBackward(bool selectPrevPage)
        {
            if (_nScreenPage > 0)
            {
                _nScreenPageOffset = (int)(MAXPAGEFOOTERBUTTONS / 2);
                _nScreenPage -= (_nScreenPageOffset);
                if (selectPrevPage)
                    _nScreenPage--;

                if (_nScreenPage < 0)
                {
                    int adjust = -_nScreenPage;
                    _nScreenPageOffset = (int)(MAXPAGEFOOTERBUTTONS / 2) - adjust;
                    _nScreenPage = 0;
                }
            }
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

        #endregion Implementation
    }
}
