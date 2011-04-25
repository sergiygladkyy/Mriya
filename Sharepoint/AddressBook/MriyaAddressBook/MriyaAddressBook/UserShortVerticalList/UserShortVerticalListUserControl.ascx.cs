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

namespace MriyaAddressBook.UserShortVerticalList
{
    /// <summary>
    /// User list web part. Reads user profiles and shows them in the table
    /// </summary>
    public partial class UserShortVerticalListUserControl : UserControl
    {
        protected TableProfiles _users = new TableProfiles();

        public UserShortVerticalList _webPart = null;
        private SPSite _site = null;

        bool _bShowRefreshShowAll = true;

        int _recordNumber = 2;
        UserShortVerticalList.RecordSelectionType _recordSelectionType = UserShortVerticalList.RecordSelectionType.Randomly;

        protected ABShortListRows[] _dataListRow = new ABShortListRows[] {
            new ABShortListRows("Фотографія"),  
            new ABShortListRows("Прізвище"),  
            new ABShortListRows("Ім'я"),  
            new ABShortListRows("По батькові"),  
            new ABShortListRows("Організація", false),  
            new ABShortListRows("Установа", false),  
            new ABShortListRows("Відділ", false),  
            new ABShortListRows("Посада"),  
            new ABShortListRows("Робочий телефон"),  
            new ABShortListRows("Домашній телефон", false),  
            new ABShortListRows("Електронна пошта", false),
            new ABShortListRows("День народження", false),
            new ABShortListRows("День народження", false),
            new ABShortListRows("Заслуги", false) 
        };

        private enum dataListRowNames
        {
            rowPhoto = 0,
            rowLastName = 1,
            rowFirstName = 2,
            rowMiddleName = 3,
            rowOrganization = 4,
            rowSeparateDivision = 5,
            rowSubDivision = 6,
            rowPosition = 7,
            rowPhoneWork = 8,
            rowPhoneHome = 9,
            rowEmailWork = 10,
            rowBirthday = 11,
            rowBirthdayShort = 12,
            rowMerit = 13
        };

        private string _sNoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        private string _strFilter = "";
        private string _strOrder = "LastName";

        protected void Page_Init(object sender, EventArgs e)
        {
            if (_webPart != null)
            {
                _bShowRefreshShowAll = _webPart.EnableRefreshShowAll;
                _sNoProfileImageFile = _webPart.NoProfileImageFile;

                _dataListRow[(int)dataListRowNames.rowPhoto].Visible = _webPart.ShowColumnPhoto;
                _dataListRow[(int)dataListRowNames.rowLastName].Visible = _webPart.ShowColumnLastName;
                _dataListRow[(int)dataListRowNames.rowFirstName].Visible = _webPart.ShowColumnFirstName;
                _dataListRow[(int)dataListRowNames.rowMiddleName].Visible = _webPart.ShowColumnMiddleName;
                _dataListRow[(int)dataListRowNames.rowOrganization].Visible = _webPart.ShowColumnOrganization;
                _dataListRow[(int)dataListRowNames.rowSeparateDivision].Visible = _webPart.ShowColumnSeparateDivision;
                _dataListRow[(int)dataListRowNames.rowSubDivision].Visible = _webPart.ShowColumnSubDivision;
                _dataListRow[(int)dataListRowNames.rowPosition].Visible = _webPart.ShowColumnPosition;
                _dataListRow[(int)dataListRowNames.rowPhoneWork].Visible = _webPart.ShowColumnWorkPhone;
                _dataListRow[(int)dataListRowNames.rowPhoneHome].Visible = _webPart.ShowColumnHomePhone;
                _dataListRow[(int)dataListRowNames.rowEmailWork].Visible = _webPart.ShowColumnEmail;
                _dataListRow[(int)dataListRowNames.rowBirthday].Visible = _webPart.ShowColumnBirthday;
                _dataListRow[(int)dataListRowNames.rowBirthdayShort].Visible = _webPart.ShowColumnBirthdayShort;
                _dataListRow[(int)dataListRowNames.rowMerit].Visible = _webPart.ShowColumnMerit;

                _recordNumber = _webPart.NumberOfRecords;
                _recordSelectionType = _webPart.SelectionType;
                _users.GetBestEmployeesOnly = _webPart.ShowBestWorkersOnly;
                _users.GetNewEmployeesOnly = _webPart.ShowNewEmployeesOnly;
                _users.NewEmployeeDays = _webPart.NewEmployeesDays;
                _users.GetEmployeesWithBirthdayOnly = _webPart.ShowWhosBirthdayOnly;
                if (_users.GetEmployeesWithBirthdayOnly)
                {
                    DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    DayOfWeek day = now.DayOfWeek;
                    int days = day - DayOfWeek.Monday;

                    if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.Today)
                    {
                        _users.BirthdayStart = now;
                        _users.BirthdayEnd = now;
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.Yesterday)
                    {
                        _users.BirthdayStart = now.AddDays(-1);
                        _users.BirthdayEnd = _users.BirthdayStart;
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.Tomorrow)
                    {
                        _users.BirthdayStart = now.AddDays(1);
                        _users.BirthdayEnd = _users.BirthdayStart;
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.ThisWeek)
                    {
                        _users.BirthdayStart = DateTime.Now.AddDays(-days);
                        _users.BirthdayEnd = _users.BirthdayStart.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.LastWeek)
                    {
                        _users.BirthdayStart = DateTime.Now.AddDays(-days - 7);
                        _users.BirthdayEnd = _users.BirthdayStart.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.NextWeek)
                    {
                        _users.BirthdayStart = DateTime.Now.AddDays(-days + 7);
                        _users.BirthdayEnd = _users.BirthdayStart.AddDays(6);
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.ThisMonth)
                    {
                        _users.BirthdayStart = new DateTime(now.Year, now.Month, 1);
                        _users.BirthdayEnd = _users.BirthdayStart.AddMonths(1).AddDays(-1);
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.LastMonth)
                    {
                        _users.BirthdayStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                        _users.BirthdayEnd = _users.BirthdayStart.AddMonths(1).AddDays(-1);
                    }
                    else if (_webPart.BirthdayTimeframe == UserShortVerticalList.Timeframe.NextMonth)
                    {
                        _users.BirthdayStart = new DateTime(now.Year, now.Month, 1).AddMonths(1);
                        _users.BirthdayEnd = _users.BirthdayStart.AddMonths(1).AddDays(-1);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _site = new SPSite(SPContext.Current.Site.ID);

            panelRefresh.Visible = _bShowRefreshShowAll;
            if (!IsPostBack)
            {
                ReadProfiles();
                ShowRecords();
            }
        }

        public void ReadProfiles()
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    Guid currentSiteId = SPContext.Current.Site.ID;
                    Guid currentWebId = SPContext.Current.Web.ID;

                    using (SPSite site2 = new SPSite(currentSiteId))
                    {
                        using (SPWeb web = site2.OpenWeb(currentWebId))
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

                                _users.Add(profile);
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                labelError.Text = ex.ToString();
                labelError.Visible = true;
            }
        }

        protected void ShowRecords()
        {
            StringBuilder sbCards = new StringBuilder();
            List<RecordUserProfile> profilesSrc = _users.GetProfiles(_strOrder, _strFilter);
            List<RecordUserProfile> profilesDest = new List<RecordUserProfile>();

            if (_recordSelectionType == UserShortVerticalList.RecordSelectionType.Randomly)
            {
                var rnd = new Random();
                int count = (_recordNumber < profilesSrc.Count) ? (_recordNumber) : (profilesSrc.Count);
                while (count > 0)
                {
                    var index = rnd.Next(profilesSrc.Count);
                    profilesDest.Add(profilesSrc[index]);
                    profilesSrc.RemoveAt(index);
                    count--;
                }

            }
            else if (_recordSelectionType == UserShortVerticalList.RecordSelectionType.FromTheEnd)
            {
                int max = (_recordNumber < profilesSrc.Count) ? (_recordNumber) : (profilesSrc.Count);
                for (int i = profilesSrc.Count - 1, cnt = 0; i >= 0 && cnt < max; i--, cnt++)
                    profilesDest.Add(profilesSrc[i]);
            }
            else
            {
                for (int i = 0; i < profilesSrc.Count && i < _recordNumber; i++)
                    profilesDest.Add(profilesSrc[i]);
            }

            foreach (RecordUserProfile profile in profilesDest)
            {
                string sProfileURL = "";
                string sPhotoURL = "";
                string sEmail = "";

                if (profile.ProfileURL.Trim().Length > 0)
                    sProfileURL = "javascript:ShowABShortListProfileDialog('" +
                    profile.LastName.Replace("\'", "\\\'").Replace("\"", "\\\"") + ", " +
                    profile.FirstName.Replace("\'", "\\\'").Replace("\"", "\\\"") + "', '" +
                    System.Web.HttpUtility.UrlEncode(profile.ProfileURL) + "')";

                if (profile.PhotoURL.Trim().Length > 0)
                {
                    sPhotoURL = string.Format("<img  src=\"{0}\">",
                        profile.PhotoURL);
                }
                else
                {
                    sPhotoURL = string.Format("<img  src=\"{0}\">",
                        _site.Url + _sNoProfileImageFile);
                }

                if (profile.EmailWork.Trim().Length > 0)
                    sEmail = string.Format("<a href=\"mailto:{0}\">{0}</a>", profile.EmailWork);


                sbCards.AppendLine("<div class=\"employee_card\">");
                sbCards.AppendLine("<div class=\"card_shadow_rght\">");
                sbCards.AppendLine("<div class=\"card_body\">");
                sbCards.AppendLine("<div class=\"card_padd\">");

                if (_dataListRow[(int)dataListRowNames.rowPhoto].Visible)
                {
                    sbCards.AppendLine("<div class=\"photo_img\">");
                    sbCards.AppendLine(string.Format("<a href=\"{0}\">{1}</a>",
                        sProfileURL, sPhotoURL));
                    sbCards.AppendLine("</div>");
                }

                sbCards.AppendLine("<div class=\"card_txt\">");

                if (_dataListRow[(int)dataListRowNames.rowLastName].Visible || 
                    _dataListRow[(int)dataListRowNames.rowFirstName].Visible ||
                    _dataListRow[(int)dataListRowNames.rowMiddleName].Visible)
                {
                    string sName = "";

                    if (_dataListRow[(int)dataListRowNames.rowMiddleName].Visible &&
                        profile.MiddleName.Trim().Length > 0)
                    {
                        sName = string.Format("<span id=\"card_text_caption_name\"><a href=\"{0}\">{1}{2}{3}</a></span><br/>",
                            sProfileURL,
                            (_dataListRow[(int)dataListRowNames.rowLastName].Visible && profile.LastName.Trim().Length > 0) ? (profile.LastName.Trim() + " ") : (""),
                            (_dataListRow[(int)dataListRowNames.rowFirstName].Visible && profile.FirstName.Trim().Length > 0) ? (profile.FirstName.Trim() + " ") : (""),
                            (_dataListRow[(int)dataListRowNames.rowMiddleName].Visible && profile.MiddleName.Trim().Length > 0) ? (profile.MiddleName.Trim() + " ") : ("")
                            );
                    }
                    else
                    {
                        sName = string.Format("<span id=\"card_text_caption_name\"><a href=\"{0}\">{2}{1}</a></span><br/>",
                            sProfileURL,
                            (_dataListRow[(int)dataListRowNames.rowLastName].Visible && profile.LastName.Trim().Length > 0) ? (profile.LastName.Trim() + " ") : (""),
                            (_dataListRow[(int)dataListRowNames.rowFirstName].Visible && profile.FirstName.Trim().Length > 0) ? (profile.FirstName.Trim() + " ") : ("")
                            );
                    }
                    sbCards.AppendLine(sName);
                }

                if (_dataListRow[(int)dataListRowNames.rowOrganization].Visible)
                {
                    sbCards.AppendLine("<span id=\"card_text_caption_organization\">"
                        + profile.Organization + "</span><br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowSeparateDivision].Visible)
                {
                    sbCards.AppendLine("<span id=\"card_text_caption_sivision\">"
                        + profile.SeparateDivision + "</span><br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowSubDivision].Visible)
                {
                    sbCards.AppendLine("<span id=\"card_text_caption_subdivision\">"
                        + profile.SubDivision + "</span><br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowPosition].Visible)
                {
                    sbCards.AppendLine("<span id=\"card_text_caption_position\">"
                        + profile.Position + "</span><br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowPhoneWork].Visible)
                {
                    sbCards.AppendLine("<span class=\"card_text_caption\" id=\"card_text_caption_wphone\">Телефон (д): </span>" 
                        + profile.PhoneWork + "<br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowPhoneHome].Visible)
                {
                    sbCards.AppendLine("<span class=\"card_text_caption\" id=\"card_text_caption_hphone\">Телефон (р): </span>" 
                        + profile.PhoneHome + "<br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowEmailWork].Visible)
                {
                    sbCards.AppendLine("<span class=\"card_text_caption\" id=\"card_text_caption_email\">E-mail: </span>" 
                        + sEmail + "<br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowBirthday].Visible)
                {
                    string sBirthday = "";

                    if (profile.Birthday != null)
                        sBirthday = profile.BirthdayDT.ToShortDateString();

                    sbCards.AppendLine("<span class=\"card_text_caption\" id=\"card_text_caption_birthday\">Дата народження: </span>" 
                        + sBirthday + "<br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowBirthdayShort].Visible)
                {
                    string sBirthday = "";

                    if (profile.Birthday != null)
                        sBirthday = profile.BirthdayDT.ToString("dd MMMM");


                    sbCards.AppendLine("<span class=\"card_text_caption\" id=\"card_text_caption_birthday\">Дата народження: </span>" 
                        + sBirthday + "<br/>");
                }

                if (_dataListRow[(int)dataListRowNames.rowMerit].Visible)
                {
                    sbCards.AppendLine("<span class=\"card_text_caption\" id=\"card_text_caption_merit\">Заслуги: </span>" 
                        + profile.BestWorkerMerit + "<br/>");
                }
                sbCards.AppendLine("</div>");   // card_txt

                sbCards.AppendLine("<div style=\"clear:both\"></div>");
                sbCards.AppendLine("</div>"); // card_padd
                sbCards.AppendLine("</div>"); // card_body
                sbCards.AppendLine("</div>"); // card_shadow_rght

                sbCards.AppendLine("<div class=\"shad_bttm_lft\"></div>");
                sbCards.AppendLine("<div class=\"shad_bttm_rgt\"></div>");

                sbCards.AppendLine("</div>"); //employee_card
            }
            literalCards.Text = sbCards.ToString();
        }

        protected void linkButtonRefresh_Click(object sender, EventArgs e)
        {
            ReadProfiles();
            ShowRecords();
        }

        protected void linkButtonShowAll_Click(object sender, EventArgs e)
        {
            if (linkButtonRefresh.Visible)
            {
                linkButtonRefresh.Visible = false;
                labelRefreshDelim.Visible = false;
                linkButtonShowAll.Text = "Сховати";
                _recordNumber = int.MaxValue;
            }
            else
            {
                linkButtonRefresh.Visible = true;
                labelRefreshDelim.Visible = true;
                linkButtonShowAll.Text = "Показати всі";
            }
            ReadProfiles();
            ShowRecords();
        }
    }
}
