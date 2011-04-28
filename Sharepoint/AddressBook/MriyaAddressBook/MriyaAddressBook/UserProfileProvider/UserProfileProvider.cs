using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;

using UserProfilesDAL;

namespace MriyaAddressBook.UserProfileProvider
{
    public class UserProfileProvider : WebPart, ITableProfiles
    {
        private Label _labelError = null;
        private TableProfiles _tableProfiles = null;

        public enum SPDataSourcer
        {
            API,
            SQL
        }

        // Default properties
        const SPDataSourcer c_SPDataSource = SPDataSourcer.API;
        const string c_SPConnectionString = "Data Source=(local);Initial Catalog=\"User Profile Service Application_ProfileDB_987e27f4752344ee939de2826d85a9ad\";Integrated Security=True";
        const string c_SPSiteProfiles = "http://intranet.contoso.com/my";

        // Properties
        private SPDataSourcer spDataSource;
        private string spConnectionString;
        private string spSiteProfiles;

        #region Properties 

        public TableProfiles ProfilesTable
        {
            get
            {
                if (_tableProfiles == null)
                    ReadProfiles();
                return _tableProfiles;

            }
        }

        #region Web Part custom properties

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Джерело отримання даних"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, джередо, щоб отримати дані."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Отримання даних"),
        System.ComponentModel.DefaultValue(c_SPDataSource)
        ]
        public SPDataSourcer SPDataSource
        {
            get
            {
                return spDataSource;
            }
            set
            {
                spDataSource = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Строка підключення до MS SQL"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть строку підключення до MS SQL."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Отримання даних"),
        System.ComponentModel.DefaultValue(c_SPConnectionString)
        ]
        public string SPConnectionString
        {
            get
            {
                return spConnectionString;
            }
            set
            {
                spConnectionString = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Адреса колекції сайтів профілів користувачів"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть адресу колекції сайтів профілів користувачів."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Отримання даних"),
        System.ComponentModel.DefaultValue(c_SPSiteProfiles)
        ]
        public string SPSiteProfiles
        {
            get
            {
                return spSiteProfiles;
            }
            set
            {
                spSiteProfiles = value;
            }
        }

        #endregion Web Part custom properties

        #endregion Properties

        [ConnectionProvider("UserProfileProvider")]
        public ITableProfiles GetUserProfileProvider()
        {
            return this;
        }

        public UserProfileProvider()
        {
            spDataSource = c_SPDataSource;
            spConnectionString = c_SPConnectionString;
            spSiteProfiles = c_SPSiteProfiles;
        }

        protected override void CreateChildControls()
        {
            Label lblHello = new Label();
            if (_labelError == null)
            {
                _labelError = new Label();
                _labelError.ForeColor = System.Drawing.Color.Red;
                _labelError.Visible = false;
            }
            lblHello.Text = "Data Provider for consuming webparts";

            Controls.Add(_labelError);
            Controls.Add(lblHello);
        }

        public void ReadProfiles()
        {
            try
            {
                _tableProfiles = new TableProfiles();
                if (SPDataSource == SPDataSourcer.API)
                    _tableProfiles.ReadSPProfiles(SPContext.Current.Site.ID);
                else
                    _tableProfiles.ReadSqlSPProfiles(SPConnectionString, SPSiteProfiles);
            }
            catch (Exception ex)
            {
                if (_labelError == null)
                    _labelError = new Label();
                _labelError.ForeColor = System.Drawing.Color.Red;
                _labelError.Text = "Помилка при отриманні профілів користувача!<br/><br/>\n\n" + ex.Message;
                _labelError.Visible = true;
            }
        }

    }
}
