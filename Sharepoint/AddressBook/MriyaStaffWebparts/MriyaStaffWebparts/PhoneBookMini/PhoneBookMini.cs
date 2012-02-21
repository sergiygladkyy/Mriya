using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MriyaStaffWebparts.PhoneBookMini
{
    [ToolboxItemAttribute(false)]
    public class PhoneBookMini : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MriyaStaffWebparts/PhoneBookMini/PhoneBookMiniUserControl.ascx";

        #region Attributes

        const string c_CaptionText = "Пошук працівників";

        const int c_MaxResults = 9;

        const string c_PhotoImageFile = "/SiteCollectionImages/photo_icn.gif";
        const string c_NoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        const string c_OTConnectionString = "Data Source=NESO;Initial Catalog=OmniTracker;Integrated Security=True";
        const string c_PhotoConnectionString = "Data Source=NESO;Initial Catalog=MriyaUserData;Integrated Security=True";

        // Details window
        const bool c_ShowDetailsPopup = true;
        const bool c_ShowDetailsName = true;
        const bool c_ShowDetailsJobTitle = true;
        const bool c_ShowDetailsDepartment = true;
        const bool c_ShowDetailsDOB = true;
        const bool c_ShowDetailsPhoneWork = true;
        const bool c_ShowDetailsPhoneMobile = true;
        const bool c_ShowDetailsEmail = true;
        const bool c_ShowDetailsCity = true;

        // Properties
        private string captionText = c_CaptionText;

        private int maxResults = c_MaxResults;

        private string photoImageFile = c_PhotoImageFile;
        private string noProfileImageFile = c_NoProfileImageFile;

        private string otConnectionString = c_OTConnectionString;
        private string photoConnectionString = c_PhotoConnectionString;

        private bool showDetailsName = c_ShowDetailsName;
        private bool showDetailsJobTitle = c_ShowDetailsJobTitle;
        private bool showDetailsDepartment = c_ShowDetailsDepartment;
        private bool showDetailsDOB = c_ShowDetailsDOB;
        private bool showDetailsPhoneWork = c_ShowDetailsPhoneWork;
        private bool showDetailsPhoneMobile = c_ShowDetailsPhoneMobile;
        private bool showDetailsEmail = c_ShowDetailsEmail;
        private bool showDetailsCity = c_ShowDetailsCity;

        #endregion

        #region Properties

        #region Additional

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Заголовок"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть заголовок для телефонної книги"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_CaptionText)
        ]
        public string CaptionText
        {
            get
            {
                return captionText;
            }
            set
            {
                captionText = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Максимальна кількість записів в результаті пошуку"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть кількість записів відображаються в віджіті."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_MaxResults)
        ]
        public int MaxResults
        {
            get
            {
                return maxResults;
            }
            set
            {
                maxResults = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Відносний шлях до файлу з іконкою у колонці Фото"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть відносний шлях до файлу."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_PhotoImageFile)
        ]
        public string PhotoImageFile
        {
            get
            {
                return photoImageFile;
            }
            set
            {
                photoImageFile = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Відносний шлях до файлу з іконою \"Фотографія користувача відсутня\""),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть відносний шлях до файлу."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_NoProfileImageFile)
        ]
        public string NoProfileImageFile
        {
            get
            {
                return noProfileImageFile;
            }
            set
            {
                noProfileImageFile = value;
            }
        }

        #endregion Additional

        #region Data source

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Строка підключення до MS SQL бази даних OmniTrack"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть строку підключення до MS SQL бази даних OmniTrack."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Отримання даних"),
        System.ComponentModel.DefaultValue(c_OTConnectionString)
        ]
        public string OTConnectionString
        {
            get
            {
                return otConnectionString;
            }
            set
            {
                otConnectionString = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Строка підключення до MS SQL бази даних фотографій"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть строку підключення до MS SQL бази даних фотографій."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Отримання даних"),
        System.ComponentModel.DefaultValue(c_PhotoConnectionString)
        ]
        public string PhotoConnectionString
        {
            get
            {
                return photoConnectionString;
            }
            set
            {
                photoConnectionString = value;
            }
        }

        #endregion Data source

        #region Details fields

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("ПІБ"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsName)
         ]
        public bool ShowDetailsName
        {
            get
            {
                return showDetailsName;
            }
            set
            {
                showDetailsName = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Посада"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsJobTitle)
         ]
        public bool ShowDetailsJobTitle
        {
            get
            {
                return showDetailsJobTitle;
            }
            set
            {
                showDetailsJobTitle = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Підрозділ"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsDepartment)
         ]
        public bool ShowDetailsDepartment
        {
            get
            {
                return showDetailsDepartment;
            }
            set
            {
                showDetailsDepartment = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Дата народження"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsDOB)
         ]
        public bool ShowDetailsDob
        {
            get
            {
                return showDetailsDOB;
            }
            set
            {
                showDetailsDOB = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Робочий (внутрішній) телефон"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsPhoneWork)
         ]
        public bool ShowDetailsPhoneWork
        {
            get
            {
                return showDetailsPhoneWork;
            }
            set
            {
                showDetailsPhoneWork = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Мобільний телефон"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsPhoneMobile)
         ]
        public bool ShowDetailsPhoneMobile
        {
            get
            {
                return showDetailsPhoneMobile;
            }
            set
            {
                showDetailsPhoneMobile = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Email"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsEmail)
         ]
        public bool ShowDetailsEmail
        {
            get
            {
                return showDetailsEmail;
            }
            set
            {
                showDetailsEmail = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Місто"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsCity)
         ]
        public bool ShowDetailsCity
        {
            get
            {
                return showDetailsCity;
            }
            set
            {
                showDetailsCity = value;
            }
        }

        #endregion Details fields

        #endregion Properties


        protected override void CreateChildControls()
        {
            if (ScriptManager.GetCurrent(this.Page) == null)
            {
                ScriptManager manager = new ScriptManager();
                manager.EnablePartialRendering = true;
                Controls.Add(manager);
            }

            PhoneBookMiniUserControl control = (PhoneBookMiniUserControl)Page.LoadControl(_ascxPath);
            if (control != null)
            {
                control._webPart = this;
                Controls.Add(control);
            }
        }
    }
}
