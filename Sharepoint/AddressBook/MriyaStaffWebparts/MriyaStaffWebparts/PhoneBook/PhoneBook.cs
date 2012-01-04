using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MriyaStaffWebparts.PhoneBook
{
    public class PhoneBook : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MriyaStaffWebparts/PhoneBook/PhoneBookUC.ascx";

        #region Attributes

        // Properties defaults
        const bool      c_EnableCaption = true;
        const string    c_CaptionText = "Телефонна книга";
        const bool      c_EnableSearch = true;
        const bool      c_EnableSearchStatus = true;

        const string    c_PhotoImageFile = "/SiteCollectionImages/photo_icn.gif";
        const string    c_NoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        const string    c_OTConnectionString = "Data Source=NESO;Initial Catalog=OmniTracker;Integrated Security=True";
        const string    c_PhotoConnectionString = "Data Source=NESO;Initial Catalog=MriyaUserData;Integrated Security=True";

        // Data table
        const bool      c_TablePaging = true;
        const int       c_TablePageSize = 20;
        const bool      c_ShowColumnPhoto = true;
        const bool      c_ShowColumnName = true;
        const bool      c_ShowColumnEmail = false;
        const bool      c_ShowColumnPhoneMobile = false;
        const bool      c_ShowColumnPhoneWork = false;
        const bool      c_ShowColumnPhones = true;
        const bool      c_ShowColumnDepartmemt = false;
        const bool      c_ShowColumnJobTitle = false;
        const bool      c_ShowColumnJob = true;
        const bool      c_ShowColumnCity = false;

        // Details window
        const bool      c_ShowDetailsName = true;
        const bool      c_ShowDetailsJobTitle = true;
        const bool      c_ShowDetailsDepartment = true;
        const bool      c_ShowDetailsDob = true;
        const bool      c_ShowDetailsPhoneWork = true;
        const bool      c_ShowDetailsPhoneMobile = true;
        const bool      c_ShowDetailsEmail = true;
        const bool      c_ShowDetailsCity = true;

        // Properties
        private bool    enableCaption = c_EnableCaption;
        private string  captionText = Properties.Resources.textWPCaption;
        private bool    enableSearch = c_EnableSearch;
        private bool    enableSearchStatus = c_EnableSearchStatus;
        private string  photoImageFile = c_PhotoImageFile;
        private string  noProfileImageFile = c_NoProfileImageFile;

        private string  otConnectionString = c_OTConnectionString;
        private string  photoConnectionString = c_PhotoConnectionString;

        private bool    tablePaging = c_TablePaging;
        private int     tablePageSize = c_TablePageSize;
        private bool    showColumnPhoto = c_ShowColumnPhoto;
        private bool    showColumnName = c_ShowColumnName;
        private bool    showColumnEmail = c_ShowColumnEmail;
        //private bool    showColumnPhoneMobile = c_ShowColumnPhoneMobile;
        //private bool    showColumnPhoneWork = c_ShowColumnPhoneWork;
        private bool    showColumnPhones = c_ShowColumnPhones;
        //private bool    showColumnDepartmemt = c_ShowColumnDepartmemt;
        //private bool    showColumnJobTitle = c_ShowColumnJobTitle;
        private bool    showColumnJob = c_ShowColumnJob;
        private bool    showColumnCity = c_ShowColumnCity;

        private bool    showDetailsName = c_ShowDetailsName;
        private bool    showDetailsJobTitle = c_ShowDetailsJobTitle;
        private bool    showDetailsDepartment = c_ShowDetailsDepartment;
        private bool    showDetailsDob = c_ShowDetailsDob;
        private bool    showDetailsPhoneWork = c_ShowDetailsPhoneWork;
        private bool    showDetailsPhoneMobile = c_ShowDetailsPhoneMobile;
        private bool    showDetailsEmail = c_ShowDetailsEmail;
        private bool    showDetailsCity = c_ShowDetailsCity;

        #endregion Attributes

        /// <summary>
        /// WebPart custom properties
        /// </summary>
        #region WebPart properties

        #region Additional

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати заголовок"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб показати заголовок."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_EnableCaption)
        ]
        public bool EnableCaption
        {
            get
            {
                return enableCaption;
            }
            set
            {
                enableCaption = value;
            }
        }

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
        System.Web.UI.WebControls.WebParts.WebDisplayName("Дозволити пошук"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб показати панель пошуку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_EnableSearch)
        ]
        public bool EnableSearch
        {
            get
            {
                return enableSearch;
            }
            set
            {
                enableSearch = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Статус пошуку"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, щоб показати інформацію про кількість знайдених записів."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_EnableSearchStatus)
        ]
        public bool EnableSearchStatus
        {
            get
            {
                return enableSearchStatus;
            }
            set
            {
                enableSearchStatus = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Відображати посторінково"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, щоб відображати записи телефонної книги посторінково."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_TablePaging)
        ]
        public bool TablePaging
        {
            get
            {
                return tablePaging;
            }
            set
            {
                tablePaging = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Розмір сторінки"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть кількість записів відображаються на сторінці."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_TablePageSize)
        ]
        public int TablePageSize
        {
            get
            {
                return tablePageSize;
            }
            set
            {
                tablePageSize = value;
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

        #region Table columns

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Фотографія"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnPhoto)
         ]
        public bool ShowColumnPhoto
        {
            get
            {
                return showColumnPhoto;
            }
            set
            {
                showColumnPhoto = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("ПІБ"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnName)
         ]
        public bool ShowColumnName
        {
            get
            {
                return showColumnName;
            }
            set
            {
                showColumnName = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Email"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnEmail)
         ]
        public bool ShowColumnEmail
        {
            get
            {
                return showColumnEmail;
            }
            set
            {
                showColumnEmail = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Телефони"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnPhones)
         ]
        public bool ShowColumnPhones
        {
            get
            {
                return showColumnPhones;
            }
            set
            {
                showColumnPhones = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Підрозділ та посада"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnJob)
         ]
        public bool ShowColumnJob
        {
            get
            {
                return showColumnJob;
            }
            set
            {
                showColumnJob = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Місто"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnCity)
         ]
        public bool ShowColumnCity
        {
            get
            {
                return showColumnCity;
            }
            set
            {
                showColumnCity = value;
            }
        }

        #endregion Table columns

        #region Details fields

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("ПІБ"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowColumnName)
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
         System.ComponentModel.DefaultValue(c_ShowColumnJobTitle)
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
         System.ComponentModel.DefaultValue(c_ShowDetailsDob)
         ]
        public bool ShowDetailsDob
        {
            get
            {
                return showDetailsDob;
            }
            set
            {
                showDetailsDob = value;
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

        #endregion WebPart properties

        protected override void CreateChildControls()
        {
            if (ScriptManager.GetCurrent(this.Page) == null)
            {
                ScriptManager manager = new ScriptManager();
                manager.EnablePartialRendering = true;
                Controls.Add(manager);
            }

            PhoneBookUC control = (PhoneBookUC)Page.LoadControl(_ascxPath);
            if (control != null)
            {
                control._webPart = this;
                Controls.Add(control);
            } 
        }
    }
}
