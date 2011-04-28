using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using UserProfilesDAL;

namespace MriyaAddressBook.UserList
{
    /// <summary>
    /// User list web part. Reads user profiles and shows them in the table
    /// </summary>
    public partial class UserListUserControl : UserControl
    {
        protected TableProfiles _users = new TableProfiles();

        public UserList _webPart = null;
        private SPSite _site = null;

        protected ABTableColumn[] _tableColumns = new ABTableColumn[] {
            new ABTableColumn("Фотографія", ""),  
            new ABTableColumn("Прізвище", "LastName"),  
            new ABTableColumn("Ім'я", "FirstName"),  
            new ABTableColumn("По батькові", "MiddleName", false),  
            new ABTableColumn("Організація", "Organization", false),  
            new ABTableColumn("Установа", "SeparateDivision"),  
            new ABTableColumn("Відділ", "SubDivision", false),  
            new ABTableColumn("Посада", "Position", false),  
            new ABTableColumn("Робочий телефон", "WorkPhone"),  
            new ABTableColumn("Домашній телефон", "HomePhone", false),  
            new ABTableColumn("Електронна пошта", "WorkEmail"),
            new ABTableColumn("День народження", "Birthday", false),
            new ABTableColumn("День народження", "Birthday", false),
            new ABTableColumn("Заслуги", "Merit", false) 
        };

        private enum columnNames
        {
            columnPhoto = 0,
            columnLastName = 1,
            columnFirstName = 2,
            columnMiddleName = 3,
            columnOrganization = 4,
            columnSeparateDivision = 5,
            columnSubDivision = 6,
            columnPosition = 7,
            columnPhoneWork = 8,
            columnPhoneHome = 9,
            columnEmailWork = 10,
            columnBirthday = 11,
            columnBirthdayShort = 12,
            columnMerit = 13
        };

        private string _sCaptionText = "Адресна книга";
        private string _sPhotoImageFile = "/SiteCollectionImages/photo_icn.gif";
        private string _sNoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        private string _strFilter = "";
        private string _strOrder = "LastName";

        private bool _bShowCaptionPanel = true;
        private bool _bShowSearchPanel = true;
        private bool _bShowPhotoIconOnly = true;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_webPart != null)
            {
                if (_webPart.TableProfiles != null)
                    _users.Add(_webPart.TableProfiles);

                _bShowCaptionPanel = _webPart.EnableCaption;
                _sCaptionText = _webPart.CaptionText;
                _bShowSearchPanel = _webPart.EnableSearch;
                _sPhotoImageFile = _webPart.PhotoImageFile;
                _sNoProfileImageFile = _webPart.NoProfileImageFile;

                _bShowPhotoIconOnly = _webPart.ShowColumnPhotoIcon;

                _tableColumns[(int)columnNames.columnPhoto].Visible = _webPart.ShowColumnPhoto;
                _tableColumns[(int)columnNames.columnLastName].Visible = _webPart.ShowColumnLastName;
                _tableColumns[(int)columnNames.columnFirstName].Visible = _webPart.ShowColumnFirstName;
                _tableColumns[(int)columnNames.columnMiddleName].Visible = _webPart.ShowColumnMiddleName;
                _tableColumns[(int)columnNames.columnOrganization].Visible = _webPart.ShowColumnOrganization;
                _tableColumns[(int)columnNames.columnSeparateDivision].Visible = _webPart.ShowColumnSeparateDivision;
                _tableColumns[(int)columnNames.columnSubDivision].Visible = _webPart.ShowColumnSubDivision;
                _tableColumns[(int)columnNames.columnPosition].Visible = _webPart.ShowColumnPosition;
                _tableColumns[(int)columnNames.columnPhoneWork].Visible = _webPart.ShowColumnWorkPhone;
                _tableColumns[(int)columnNames.columnPhoneHome].Visible = _webPart.ShowColumnHomePhone;
                _tableColumns[(int)columnNames.columnEmailWork].Visible = _webPart.ShowColumnEmail;
                _tableColumns[(int)columnNames.columnBirthday].Visible = _webPart.ShowColumnBirthday;
                _tableColumns[(int)columnNames.columnBirthdayShort].Visible = _webPart.ShowColumnBirthdayShort;
                _tableColumns[(int)columnNames.columnMerit].Visible = _webPart.ShowColumnMerit;

                _users.GetBestEmployeesOnly = _webPart.ShowBestWorkersOnly;
                _users.GetBestEmployeesWeeklyOnly = _webPart.ShowBestWorkersWeeklyOnly;
                _users.GetNewEmployeesOnly = _webPart.ShowNewEmployeesOnly;
                _users.NewEmployeeDays = _webPart.NewEmployeesDays;
                _users.GetEmployeesWithBirthdayOnly = _webPart.ShowWhosBirthdayOnly;
                if (_users.GetEmployeesWithBirthdayOnly)
                {
                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); 
                    DayOfWeek day = now.DayOfWeek;
                    int days = day - DayOfWeek.Monday;

                    if (_webPart.BirthdayTimeframe == UserList.Timeframe.Today)
                    {
                        _users.BirthdayStart = now;
                        _users.BirthdayEnd = now;
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.Yesterday)
                    {
                        _users.BirthdayStart = now.AddDays(-1);
                        _users.BirthdayEnd = _users.BirthdayStart;
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.Tomorrow)
                    {
                        _users.BirthdayStart = now.AddDays(1);
                        _users.BirthdayEnd = _users.BirthdayStart;
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.ThisWeek)
                    {
                        _users.BirthdayStart = DateTime.Now.AddDays(-days);
                        _users.BirthdayEnd = _users.BirthdayStart.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.LastWeek)
                    {
                        _users.BirthdayStart = DateTime.Now.AddDays(-days - 7);
                        _users.BirthdayEnd = _users.BirthdayStart.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.NextWeek)
                    {
                        _users.BirthdayStart = DateTime.Now.AddDays(-days + 7);
                        _users.BirthdayEnd = _users.BirthdayStart.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.ThisMonth)
                    {
                        _users.BirthdayStart = new DateTime(now.Year, now.Month, 1);
                        _users.BirthdayEnd = _users.BirthdayStart.AddMonths(1).AddDays(-1);
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.LastMonth)
                    {
                        _users.BirthdayStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                        _users.BirthdayEnd = _users.BirthdayStart.AddMonths(1).AddDays(-1);
                    }
                    else if (_webPart.BirthdayTimeframe == UserList.Timeframe.NextMonth)
                    {
                        _users.BirthdayStart = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                        _users.BirthdayEnd = _users.BirthdayStart.AddMonths(1).AddDays(-1);
                    }
                }
            }

            // Create table buttons
            int col = 0;
            foreach (ABTableColumn column in _tableColumns)
            {
                col++;
                
                if (column.Visible == false) continue;

                column.LinkButton = new LinkButton();
                column.LinkButton.ID = string.Format("lbCellHeader{0}", col);
                column.LinkButton.Text = column.Caption;
                column.LinkButton.CommandName = column.SortCommand;
                column.LinkButton.Command += new CommandEventHandler(lbCell_Command);
            }
        }

        /// <summary>
        /// Called when page is loading
        /// </summary>
        /// <param name="sender">Who called</param>
        /// <param name="e">Event arguments</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _site = new SPSite(SPContext.Current.Site.ID);
             
            if (Page.IsPostBack)
            {
                if (ViewState["strFilter"] != null)
                    _strFilter = ViewState["strFilter"].ToString();
                if (ViewState["strOrder"] != null)
                    _strOrder = ViewState["strOrder"].ToString();
            }

            if (_bShowSearchPanel == false)
                _strFilter = "";

            panelCaption.Visible = _bShowCaptionPanel;
            labelCaption.Text = _sCaptionText;

            panelSearch.Visible = _bShowSearchPanel;
            buttonShowAll.Visible = (_strFilter.Trim().Length > 0);

            if (_users.IsEmpty)
                ReadProfiles();
            ShowGrid();
        }

        public void ReadProfiles()
        {
            try
            {
                if (_webPart != null && _webPart.SPDataSource == UserList.SPDataSourcer.SQL)
                    _users.ReadSqlSPProfiles(_webPart.SPConnectionString, _webPart.SPSiteProfiles);
                else
                    _users.ReadSPProfiles( SPContext.Current.Site.ID);
            }
            catch (Exception ex)
            {
                labelError.Text = "Помилка при отриманні профілів користувача!<br/><br/>\n\n" + ex.Message;
                labelError.Visible = true;
            }
        }

        protected void ShowGrid()
        {
            tableProfiles.Rows.Clear();

            // Draw header here
            TableHeaderRow rowHeader = new TableHeaderRow();
            foreach (ABTableColumn column in _tableColumns)
            {
                if (!column.Visible) continue;

                TableHeaderCell cell = new TableHeaderCell();
                cell.Controls.Add(column.LinkButton);
                rowHeader.Cells.Add(cell);
            }
            tableProfiles.Rows.Add(rowHeader);

            int nRow = 0;
            foreach (RecordUserProfile profile in _users.GetProfiles(_strOrder, _strFilter))
            {
                ++nRow;
                TableRow row = new TableRow();

                string sProfileURL = "";
                string sPhotoURL = "";
                string sEmail = "";

                if (profile.ProfileURL.Trim().Length > 0)
                    sProfileURL = "javascript:ShowABProfileDialog('" +
                    profile.LastName.Replace("\'", "\\\'").Replace("\"", "\\\"") + ", " +
                    profile.FirstName.Replace("\'", "\\\'").Replace("\"", "\\\"") + "', '" +
                    System.Web.HttpUtility.UrlEncode(profile.ProfileURL) + "')";

                if (_bShowPhotoIconOnly)
                {
                    if (profile.PhotoURL.Trim().Length > 0)
                    {
                        sPhotoURL = string.Format("<a onmouseover=\"ShowABPhotoDialog('{0}', '{1}'); return true;\" " +
                            "onmouseout=\"HideABPhotoDialog('{3}'); return true;\" " +
                            "href=\"javascript:ShowABPhotoDialog('{0}', '{1}')\"> " +
                            "<img id=\"{0}\" width=\"23\" height=\"16\" src=\"{2}\"></a>",
                            this.ClientID + "_photo_img_ctl_" + nRow.ToString(), 
                            profile.PhotoURL,
                            _site.Url + _sPhotoImageFile,
                            _site.Url + _sNoProfileImageFile);
                    }
                    else
                    {
                        sPhotoURL = string.Format("<a onmouseover=\"ShowABPhotoDialog('{0}', '{1}'); return true;\" " +
                            "onmouseout=\"HideABPhotoDialog('{1}'); return true;\" " +
                            "href=\"javascript:ShowABPhotoDialog('{0}', '{1}')\"> " +
                            "<img id=\"{0}\" width=\"23\" height=\"16\" src=\"{2}\"></a>",
                            this.ClientID + "_photo_img_ctl_" + nRow.ToString(),
                            _site.Url + _sNoProfileImageFile,
                            _site.Url + _sPhotoImageFile);
                    }
                }
                else
                {
                    if (profile.PhotoURL.Trim().Length > 0)
                    {
                        sPhotoURL = string.Format("<div class=\"photo_img\"><img width=\"80\" height=\"80\" src=\"{0}\"></div>",
                            profile.PhotoURL);
                    }
                    else
                    {
                        sPhotoURL = string.Format("<div class=\"photo_img\"><img width=\"80\" height=\"80\" src=\"{0}\"></div>",
                            _site.Url + _sNoProfileImageFile);
                    }
                }

                if (profile.EmailWork.Trim().Length > 0)
                    sEmail = string.Format("mailto:{0}", profile.EmailWork);

                if (_tableColumns[(int)columnNames.columnPhoto].Visible)
                {
                    TableCell cellPhoto = new TableCell();
                    LiteralControl lcPhoto = new LiteralControl();
                    lcPhoto.Text = sPhotoURL;
                    cellPhoto.Controls.Add(lcPhoto);
                    row.Cells.Add(cellPhoto);
                }

                if (_tableColumns[(int)columnNames.columnLastName].Visible)
                {
                    TableCell cellLastName = new TableCell();
                    HyperLink linkLastName = new HyperLink();
                    linkLastName.Text = profile.LastName;
                    linkLastName.NavigateUrl = sProfileURL;
                    cellLastName.Controls.Add(linkLastName);
                    row.Cells.Add(cellLastName);
                }

                if (_tableColumns[(int)columnNames.columnFirstName].Visible)
                {
                    TableCell cellFirstName = new TableCell();
                    HyperLink linkFirstName = new HyperLink();
                    linkFirstName.Text = profile.FirstName;
                    linkFirstName.NavigateUrl = sProfileURL;
                    cellFirstName.Controls.Add(linkFirstName);
                    row.Cells.Add(cellFirstName);
                }

                if (_tableColumns[(int)columnNames.columnMiddleName].Visible)
                {
                    TableCell cellMiddleName = new TableCell();
                    HyperLink linkMiddleName = new HyperLink();
                    linkMiddleName.Text = profile.MiddleName;
                    linkMiddleName.NavigateUrl = sProfileURL;
                    cellMiddleName.Controls.Add(linkMiddleName);
                    row.Cells.Add(cellMiddleName);
                }

                if (_tableColumns[(int)columnNames.columnOrganization].Visible)
                {
                    TableCell cellOrganization = new TableCell();
                    cellOrganization.Controls.Add(new LiteralControl(profile.Organization));
                    row.Cells.Add(cellOrganization);
                }

                if (_tableColumns[(int)columnNames.columnSeparateDivision].Visible)
                {
                    TableCell cellSeparateDivision = new TableCell();
                    cellSeparateDivision.Controls.Add(new LiteralControl(profile.SeparateDivision));
                    row.Cells.Add(cellSeparateDivision);
                }

                if (_tableColumns[(int)columnNames.columnSubDivision].Visible)
                {
                    TableCell cellSubDivision = new TableCell();
                    cellSubDivision.Controls.Add(new LiteralControl(profile.SubDivision));
                    row.Cells.Add(cellSubDivision);
                }

                if (_tableColumns[(int)columnNames.columnPosition].Visible)
                {
                    TableCell cellPosition = new TableCell();
                    cellPosition.Controls.Add(new LiteralControl(profile.Position));
                    row.Cells.Add(cellPosition);
                }

                if (_tableColumns[(int)columnNames.columnPhoneWork].Visible)
                {
                    TableCell cellPhoneWork = new TableCell();
                    cellPhoneWork.Controls.Add(new LiteralControl(profile.PhoneWork));
                    row.Cells.Add(cellPhoneWork);
                }

                if (_tableColumns[(int)columnNames.columnPhoneHome].Visible)
                {
                    TableCell cellHomeWork = new TableCell();
                    cellHomeWork.Controls.Add(new LiteralControl(profile.PhoneHome));
                    row.Cells.Add(cellHomeWork);
                }

                if (_tableColumns[(int)columnNames.columnEmailWork].Visible)
                {
                    TableCell cellEmail = new TableCell();
                    HyperLink linkEmail = new HyperLink();
                    linkEmail.Text = profile.EmailWork;
                    linkEmail.NavigateUrl = sEmail;
                    cellEmail.Controls.Add(linkEmail);
                    row.Cells.Add(cellEmail);
                }

                if (_tableColumns[(int)columnNames.columnBirthday].Visible)
                {
                    string sBirthday = "";

                    if (profile.Birthday != null)
                        sBirthday = profile.BirthdayDT.ToShortDateString();

                    TableCell cellMerit = new TableCell();
                    cellMerit.Controls.Add(new LiteralControl(sBirthday));
                    row.Cells.Add(cellMerit);
                }

                if (_tableColumns[(int)columnNames.columnBirthdayShort].Visible)
                {
                    string sBirthday = "";

                    if (profile.Birthday != null)
                        sBirthday = profile.BirthdayDT.ToString("dd MMMM");

                    TableCell cellMerit = new TableCell();
                    cellMerit.Controls.Add(new LiteralControl(sBirthday));
                    row.Cells.Add(cellMerit);
                }

                if (_tableColumns[(int)columnNames.columnMerit].Visible)
                {
                    TableCell cellMerit = new TableCell();
                    cellMerit.Controls.Add(new LiteralControl(profile.BestWorkerMerit));
                    row.Cells.Add(cellMerit);
                }

                tableProfiles.Rows.Add(row);
            }
        }

        void lbCell_Command(object sender, CommandEventArgs e)
        {
            _strOrder = e.CommandName;
            ViewState["strOrder"] = _strOrder;
            ShowGrid();
        }

        protected void buttonSearch_Click(object sender, EventArgs e)
        {

            _strFilter = textBoxSearch.Text.Trim();
            ViewState["strFilter"] = _strFilter;
            buttonShowAll.Visible = true;
            ShowGrid();
        }

        protected void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            _strFilter = textBoxSearch.Text.Trim();
            ViewState["strFilter"] = _strFilter;
            buttonShowAll.Visible = true;
            ShowGrid();
        }

        protected void buttonShowAll_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            _strFilter = "";
            ViewState.Remove("strFilter");
            buttonShowAll.Visible = false;
            ShowGrid();
        }
    }
}
