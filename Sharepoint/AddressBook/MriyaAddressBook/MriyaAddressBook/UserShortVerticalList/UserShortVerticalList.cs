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

namespace MriyaAddressBook.UserShortVerticalList
{
    public class UserShortVerticalList : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MriyaAddressBook/UserShortVerticalList/UserShortVerticalListUserControl.ascx";

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

        public enum RecordSelectionType
        {
            Randomly,
            FromTheBeginning,
            FromTheEnd
        }

        // Properties defaults
        const bool c_EnableRefreshShowAll = true;
        const string    c_NoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        const SPDataSourcer c_SPDataSource = SPDataSourcer.API;
        const string c_SPConnectionString = "Data Source=(local);Initial Catalog=\"User Profile Service Application_ProfileDB_987e27f4752344ee939de2826d85a9ad\";Integrated Security=True";
        const string c_SPSiteProfiles = "http://intranet.contoso.com/my";

        const bool c_ShowColumnPhoto = true;
        const bool c_ShowBigPhoto = false;
        const bool c_ShowColumnLastName = true;
        const bool c_ShowColumnFirstName = true;
        const bool c_ShowColumnMiddleName = false;
        const bool c_ShowColumnOrganization = false;
        const bool c_ShowColumnSeparateDivision = false;
        const bool c_ShowColumnSubDivision = false;
        const bool c_ShowColumnPosition = true;
        const bool c_ShowColumnWorkPhone = false;
        const bool c_ShowColumnHomePhone = false;
        const bool c_ShowColumnEmail = false;
        const bool c_ShowColumnBirthday = false;
        const bool c_ShowColumnBirthdayShort = false;
        const bool c_ShowColumnMerit = false;

        const int c_NumberOfRecords = 2;
        const RecordSelectionType c_SelectionType = RecordSelectionType.Randomly;
        const bool c_ShowBestWorkersWeeklyOnly = false;
        const bool c_ShowBestWorkersOnly = false;
        const bool c_ShowNewEmployeesOnly = false;
        const uint c_NewEmployeesDays = 30;
        const bool c_ShowWhosBirthdayOnly = false;
        const Timeframe c_BirthdayTimeframe = Timeframe.ThisWeek;

        // Properties
        bool enableRefreshShowAll;
        private string noProfileImageFile;

        private SPDataSourcer spDataSource;
        private string spConnectionString;
        private string spSiteProfiles;
        
        private bool showColumnPhoto;
        private bool showBigPhoto;
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

        private int numberOfRecords;
        private RecordSelectionType selectionType;
        private bool showBestWorkersWeeklyOnly;
        private bool showBestWorkersOnly;
        private bool showNewEmployeesOnly;
        private uint newEmployeesDays;
        private bool showWhosBirthdayOnly;
        private Timeframe birthdayTimeframe;

        private TableProfiles _tableProfiles = null;

        // Constructor
        public UserShortVerticalList()
        {
            // Initialize private variables.
            enableRefreshShowAll = c_EnableRefreshShowAll;
            noProfileImageFile = c_NoProfileImageFile;

            spDataSource = c_SPDataSource;
            spConnectionString = c_SPConnectionString;
            spSiteProfiles = c_SPSiteProfiles;

            showColumnPhoto = c_ShowColumnPhoto;
            showBigPhoto = c_ShowBigPhoto;
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

            numberOfRecords = c_NumberOfRecords;
            selectionType = c_SelectionType;
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
        System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати кнопки \"Оновити\" і \"Показати всі\""),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб активувати."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Додатково"),
        System.ComponentModel.DefaultValue(c_EnableRefreshShowAll)
        ]
        public bool EnableRefreshShowAll
        {
            get
            {
                return enableRefreshShowAll;
            }
            set
            {
                enableRefreshShowAll = value;
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

        #region Data

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Фотографія"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані"),
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
         System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати фото великого розміру"),
         System.Web.UI.WebControls.WebParts.WebDescription("Показувати фото великого розміру."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані"),
         System.ComponentModel.DefaultValue(c_ShowBigPhoto)
         ]
        public bool ShowBigPhoto
        {
            get
            {
                return showBigPhoto;
            }
            set
            {
                showBigPhoto = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Прізвище"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати колонку."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані"),
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
         System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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
        System.ComponentModel.Category("Дані"),
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

        #endregion Data

        #region Filter data

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Введіть кількість записів, які будуть показані у списку"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть кількість записів, які будуть показані у списку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_NumberOfRecords)
        ]
        public int NumberOfRecords
        {
            get
            {
                return numberOfRecords;
            }
            set
            {
                numberOfRecords = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Виберіть спосіб відбору записів із загального списку (випадково, з початку списку, з кінця списку)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Виберіть спосіб відбору записів із загального списку (випадково, з початку списку, з кінця списку)."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_SelectionType)
        ]
        public RecordSelectionType SelectionType
        {
            get
            {
                return selectionType;
            }
            set
            {
                selectionType = value;
            }
        }

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
            // DEBUG
            //DateTime _t1 = DateTime.Now;
            //DateTime _t2 = DateTime.Now;

            //_t1 = DateTime.Now;

            if (ScriptManager.GetCurrent(this.Page) == null)
            {
                ScriptManager manager = new ScriptManager();
                manager.EnablePartialRendering = true;
                Controls.Add(manager);
            }

            UserShortVerticalListUserControl control = (UserShortVerticalListUserControl)Page.LoadControl(_ascxPath);
            if (control != null)
            {
                control._webPart = this;
                Controls.Add(control);
            }

            // DEBUG
            //LiteralControl ctlDebug = new LiteralControl();
            //_t2 = DateTime.Now;
            //ctlDebug.Text = string.Format("<br/><br/>Total time is {0} msec<br/>Data provider is {1}",
            //    (_t2 - _t1).Milliseconds,
            //    (this._provider == null) ? ("not connected") : ("connected"));
            //Controls.Add(ctlDebug);
        }

    }
}
