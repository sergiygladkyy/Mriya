using System;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

using UserProfilesDAL;

namespace MriyaAddressBook.UserList
{
    public class UserList : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MriyaAddressBook/UserList/UserListUserControl.ascx";

        private ITableProfiles _provider = null;

        public enum SPDataSourcer
        {
            API,
            SQL
        }

        public enum Timeframe
        {
            Today,
            Yesterday,
            Tomorrow,
            LastMonth,
            ThisMonth,
            NextMonth,
            LastWeek,
            ThisWeek,
            NextWeek
        }

        // Properties defaults
        const bool c_EnableCaption = true;
        const string c_CaptionText = "Адресна книга";
        const bool c_EnableSearch = true;
        const string c_PhotoImageFile = "/SiteCollectionImages/photo_icn.gif";
        const string c_NoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        const SPDataSourcer c_SPDataSource = SPDataSourcer.API;
        const string c_SPConnectionString = "Data Source=(local);Initial Catalog=\"User Profile Service Application_ProfileDB_987e27f4752344ee939de2826d85a9ad\";Integrated Security=True";
        const string c_SPSiteProfiles = "http://intranet.contoso.com/my";

        const bool c_ShowColumnPhoto = true;
        const bool c_ShowColumnPhotoIcon = true;
        const bool c_ShowColumnLastName = true;
        const bool c_ShowColumnFirstName = true;
        const bool c_ShowColumnMiddleName = false;
        const bool c_ShowColumnOrganization = false;
        const bool c_ShowColumnSeparateDivision = true;
        const bool c_ShowColumnSubDivision = false;
        const bool c_ShowColumnPosition = false;
        const bool c_ShowColumnWorkPhone = true;
        const bool c_ShowColumnHomePhone = false;
        const bool c_ShowColumnEmail = true;
        const bool c_ShowColumnBirthday = false;
        const bool c_ShowColumnBirthdayShort = false;
        const bool c_ShowColumnMerit = false;

        const bool c_ShowBestWorkersWeeklyOnly = false;
        const bool c_ShowBestWorkersOnly = false;
        const bool c_ShowNewEmployeesOnly = false;
        const uint c_NewEmployeesDays = 30;
        const bool c_ShowWhosBirthdayOnly = false;
        const Timeframe c_BirthdayTimeframe = Timeframe.ThisWeek;

        // Properties
        private bool enableCaption;
        private string captionText;
        private bool enableSearch;
        private string photoImageFile;
        private string noProfileImageFile;

        private SPDataSourcer spDataSource;
        private string spConnectionString;
        private string spSiteProfiles;
        
        private bool showColumnPhoto;
        private bool showColumnPhotoIcon;
        private bool showColumnLastName;
        private bool showColumnFirstName;
        private bool showColumnMiddleName;
        private bool showColumnOrganization;
        private bool showColumnSeparateDivision;
        private bool showColumnSubDivision;
        private bool showColumnPosition;
        private bool showColumnWorkPhone;
        private bool showColumnHomePhone;
        private bool showColumnEmail;
        private bool showColumnBirthday;
        private bool showColumnBirthdayShort;
        private bool showColumnMerit;

        private bool showBestWorkersWeeklyOnly;
        private bool showBestWorkersOnly;
        private bool showNewEmployeesOnly;
        private uint newEmployeesDays;
        private bool showWhosBirthdayOnly;
        private Timeframe birthdayTimeframe;

        private TableProfiles _tableProfiles = null;

        // Constructor
        public UserList()
        {
            // Initialize private variables.
            enableCaption = c_EnableCaption;
            captionText = c_CaptionText;
            enableSearch = c_EnableSearch;
            photoImageFile = c_PhotoImageFile;
            noProfileImageFile = c_NoProfileImageFile;

            spDataSource = c_SPDataSource;
            spConnectionString = c_SPConnectionString;
            spSiteProfiles = c_SPSiteProfiles;

            showColumnPhoto = c_ShowColumnPhoto;
            showColumnPhotoIcon = c_ShowColumnPhotoIcon;
            showColumnLastName = c_ShowColumnLastName;
            showColumnFirstName = c_ShowColumnFirstName;
            showColumnMiddleName = c_ShowColumnMiddleName;
            showColumnOrganization = c_ShowColumnOrganization;
            showColumnSeparateDivision = c_ShowColumnSeparateDivision;
            showColumnSubDivision = c_ShowColumnSubDivision;
            showColumnPosition = c_ShowColumnPosition;
            showColumnWorkPhone = c_ShowColumnWorkPhone;
            showColumnHomePhone = c_ShowColumnHomePhone;
            showColumnEmail = c_ShowColumnEmail;
            showColumnBirthday = c_ShowColumnBirthday;
            showColumnBirthdayShort = c_ShowColumnBirthdayShort;
            showColumnMerit = c_ShowColumnMerit;

            showNewEmployeesOnly = c_ShowNewEmployeesOnly;
            showBestWorkersWeeklyOnly = c_ShowBestWorkersWeeklyOnly;
            showBestWorkersOnly = c_ShowBestWorkersOnly;
            newEmployeesDays = c_NewEmployeesDays;
            showWhosBirthdayOnly = c_ShowWhosBirthdayOnly;
            birthdayTimeframe = c_BirthdayTimeframe;
        }

        //
        // Web part custom properties
        // Category: Table columns
        //

        #region WebPart custom properties

        public TableProfiles TableProfiles
        {
            get { return _tableProfiles; }
        }

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
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть назву для списку користувачів."),
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

        #region Data receive

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

        #endregion Data Receive

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
         System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати тільки іконку фотографії профілю в колонці Фото"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати тільки іконку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnPhotoIcon)
         ]
        public bool ShowColumnPhotoIcon
        {
            get
            {
                return showColumnPhotoIcon;
            }
            set
            {
                showColumnPhotoIcon = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Прізвище"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnLastName)
         ]
        public bool ShowColumnLastName
        {
            get
            {
                return showColumnLastName;
            }
            set
            {
                showColumnLastName = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Ім'я"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Колонки таблиці"),
         System.ComponentModel.DefaultValue(c_ShowColumnFirstName)
         ]
        public bool ShowColumnFirstName
        {
            get
            {
                return showColumnFirstName;
            }
            set
            {
                showColumnFirstName = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("По батькові"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnMiddleName)
        ]
        public bool ShowColumnMiddleName
        {
            get
            {
                return showColumnMiddleName;
            }
            set
            {
                showColumnMiddleName = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Організація"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnOrganization)
        ]
        public bool ShowColumnOrganization
        {
            get
            {
                return showColumnOrganization;
            }
            set
            {
                showColumnOrganization = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Установа"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnSeparateDivision)
        ]
        public bool ShowColumnSeparateDivision
        {
            get
            {
                return showColumnSeparateDivision;
            }
            set
            {
                showColumnSeparateDivision = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Відділ"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnSubDivision)
        ]
        public bool ShowColumnSubDivision
        {
            get
            {
                return showColumnSubDivision;
            }
            set
            {
                showColumnSubDivision = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Посада"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnPosition)
        ]
        public bool ShowColumnPosition
        {
            get
            {
                return showColumnPosition;
            }
            set
            {
                showColumnPosition = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Робочий телефон"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnWorkPhone)
        ]
        public bool ShowColumnWorkPhone
        {
            get
            {
                return showColumnWorkPhone;
            }
            set
            {
                showColumnWorkPhone = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Домашній телефон"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnHomePhone)
        ]
        public bool ShowColumnHomePhone
        {
            get
            {
                return showColumnHomePhone;
            }
            set
            {
                showColumnHomePhone = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Електронна пошта"),
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
        System.Web.UI.WebControls.WebParts.WebDisplayName("День народження (повна дата)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnBirthday)
        ]
        public bool ShowColumnBirthday
        {
            get
            {
                return showColumnBirthday;
            }
            set
            {
                showColumnBirthday = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("День народження (день, місяць)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnBirthdayShort)
        ]
        public bool ShowColumnBirthdayShort
        {
            get
            {
                return showColumnBirthdayShort;
            }
            set
            {
                showColumnBirthdayShort = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Заслуги"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Колонки таблиці"),
        System.ComponentModel.DefaultValue(c_ShowColumnMerit)
        ]
        public bool ShowColumnMerit
        {
            get
            {
                return showColumnMerit;
            }
            set
            {
                showColumnMerit = value;
            }
        }

        #endregion Table columns

        #region Filter data

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати тільки Працівників тижня"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб активувати."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_ShowBestWorkersWeeklyOnly)
        ]
        public bool ShowBestWorkersWeeklyOnly
        {
            get
            {
                return showBestWorkersWeeklyOnly;
            }
            set
            {
                showBestWorkersWeeklyOnly = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати тільки кращих працівників"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб активувати."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_ShowBestWorkersOnly)
        ]
        public bool ShowBestWorkersOnly
        {
            get
            {
                return showBestWorkersOnly;
            }
            set
            {
                showBestWorkersOnly = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати тільки новачків"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб активувати."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_ShowNewEmployeesOnly)
        ]
        public bool ShowNewEmployeesOnly
        {
            get
            {
                return showNewEmployeesOnly;
            }
            set
            {
                showNewEmployeesOnly = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Новачок - актуально протягом зазначенной кількості днів після прийому на роботу. Вкажіть кількість днів:"),
        System.Web.UI.WebControls.WebParts.WebDescription("Вкажіть кількість днів."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_NewEmployeesDays)
        ]
        public uint NewEmployeesDays
        {
            get
            {
                return newEmployeesDays;
            }
            set
            {
                newEmployeesDays = value;
            }
        }


        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати лише тих, у кого день народження"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб активувати."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_ShowWhosBirthdayOnly)
        ]
        public bool ShowWhosBirthdayOnly
        {
            get
            {
                return showWhosBirthdayOnly;
            }
            set
            {
                showWhosBirthdayOnly = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("День народження був чи буде, выберіть периіод:"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть період."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Фільтр"),
         System.ComponentModel.DefaultValue(c_BirthdayTimeframe)
         ]
        public Timeframe BirthdayTimeframe
        {
            get
            {
                return birthdayTimeframe;
            }
            set
            {
                birthdayTimeframe = value;
            }
        }

        #endregion Filter data

        #endregion WebPart custom properties

        [ConnectionConsumer("UserProfileProvider")]
        public void RegisterUserProfileProvider(ITableProfiles provider)
        {
            this._provider = provider;
            _tableProfiles = this._provider.ProfilesTable;
        }

        protected override void CreateChildControls()
        {
            UserListUserControl control = (UserListUserControl)Page.LoadControl(_ascxPath);
            if (control != null)
            {
                control._webPart = this;
                Controls.Add(control);
            }
        }
    }
}
