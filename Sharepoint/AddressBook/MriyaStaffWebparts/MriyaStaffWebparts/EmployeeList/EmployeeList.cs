using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MriyaStaffWebparts.EmployeeList
{

    public class EmployeeList : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MriyaStaffWebparts/EmployeeList/EmployeeListUC.ascx";

        // Custom properties data definition
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

        #region Attributes

        const bool c_EnableRefreshShowAll = true;
        const string c_NoProfileImageFile = "/SiteCollectionImages/mrab_no_profile_image.png";

        const string c_OTConnectionString = "Data Source=NESO;Initial Catalog=OmniTracker;Integrated Security=True";
        const string c_PhotoConnectionString = "Data Source=NESO;Initial Catalog=MriyaUserData;Integrated Security=True";

        // Data table
        const bool c_ShowColumnPhoto = true;
        const bool c_ShowColumnName = true;
        const bool c_ShowColumnJobTitle = false;
        const bool c_ShowColumnDepartment = false;
        const bool c_ShowColumnDOB = false;
        const bool c_ShowColumnPhoneWork = false;
        const bool c_ShowColumnPhoneMobile = false;
        const bool c_ShowColumnEmail = false;
        const bool c_ShowColumnCity = false;

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

        const int c_NumberOfRecords = 2;
        const int c_NumberOfColumns = 1;
        const RecordSelectionType c_SelectionType = RecordSelectionType.Randomly;
        //const bool c_ShowBestWorkersWeeklyOnly = false;
        //const bool c_ShowBestWorkersOnly = false;
        const bool c_ShowNewEmployeesOnly = false;
        const uint c_NewEmployeesDays = 30;
        const bool c_ShowWhosBirthdayOnly = false;
        const Timeframe c_BirthdayTimeframe = Timeframe.ThisWeek;

        // Properties
        private bool   enableRefreshShowAll = c_EnableRefreshShowAll;
        private string noProfileImageFile = c_NoProfileImageFile;

        private string otConnectionString = c_OTConnectionString;
        private string photoConnectionString = c_PhotoConnectionString;

        private bool showColumnPhoto = c_ShowColumnPhoto;
        private bool showColumnName = c_ShowColumnName;
        private bool showColumnDOB = c_ShowColumnDOB;
        private bool showColumnEmail = c_ShowColumnEmail;
        private bool showColumnPhoneMobile = c_ShowColumnPhoneMobile;
        private bool showColumnPhoneWork = c_ShowColumnPhoneWork;
        private bool showColumnDepartment = c_ShowColumnDepartment;
        private bool showColumnJobTitle = c_ShowColumnJobTitle;
        private bool showColumnCity = c_ShowColumnCity;

        private bool showDetailsPopup = c_ShowDetailsPopup;
        private bool showDetailsName = c_ShowDetailsName;
        private bool showDetailsJobTitle = c_ShowDetailsJobTitle;
        private bool showDetailsDepartment = c_ShowDetailsDepartment;
        private bool showDetailsDOB = c_ShowDetailsDOB;
        private bool showDetailsPhoneWork = c_ShowDetailsPhoneWork;
        private bool showDetailsPhoneMobile = c_ShowDetailsPhoneMobile;
        private bool showDetailsEmail = c_ShowDetailsEmail;
        private bool showDetailsCity = c_ShowDetailsCity;

        private int                     numberOfRecords = c_NumberOfRecords;
        private int                     numberOfColumns = c_NumberOfColumns;
        private RecordSelectionType     selectionType = c_SelectionType;
        //private bool                    showBestWorkersWeeklyOnly = c_ShowBestWorkersWeeklyOnly;
        //private bool                    showBestWorkersOnly = c_ShowBestWorkersOnly;
        private bool                    showNewEmployeesOnly = c_ShowNewEmployeesOnly;
        private uint                    newEmployeesDays = c_NewEmployeesDays;
        private bool                    showWhosBirthdayOnly = c_ShowWhosBirthdayOnly;
        private Timeframe               birthdayTimeframe = c_BirthdayTimeframe;

        #endregion Attributes

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

        #region Show data

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Фотографія"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
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
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
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
         System.Web.UI.WebControls.WebParts.WebDisplayName("Посада"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
         System.ComponentModel.DefaultValue(c_ShowColumnJobTitle)
         ]
        public bool ShowColumnJob
        {
            get
            {
                return showColumnJobTitle;
            }
            set
            {
                showColumnJobTitle = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Підрозділ"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
         System.ComponentModel.DefaultValue(c_ShowColumnDepartment)
         ]
        public bool ShowColumnDepartment
        {
            get
            {
                return showColumnDepartment;
            }
            set
            {
                showColumnDepartment = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Дата народження"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
         System.ComponentModel.DefaultValue(c_ShowColumnDOB)
         ]
        public bool ShowColumnDOB
        {
            get
            {
                return showColumnDOB;
            }
            set
            {
                showColumnDOB = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Робочий телефон (внутрішній)"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
         System.ComponentModel.DefaultValue(c_ShowColumnPhoneWork)
         ]
        public bool ShowColumnPhoneWork
        {
            get
            {
                return showColumnPhoneWork;
            }
            set
            {
                showColumnPhoneWork = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Мобільний телефон"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
         System.ComponentModel.DefaultValue(c_ShowColumnPhoneMobile)
         ]
        public bool ShowColumnPhoneMobile
        {
            get
            {
                return showColumnPhoneMobile;
            }
            set
            {
                showColumnPhoneMobile = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Email"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
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
         System.Web.UI.WebControls.WebParts.WebDisplayName("Місто"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб показати дані."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані карток"),
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

        #endregion Show data

        #region Details fields

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Дозволити спливаюче вікно з деталями про працівника"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть для того, щоб дозволити."),
         System.Web.UI.WebControls.WebParts.Personalizable(
         System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Дані у вікні перегляду запису"),
         System.ComponentModel.DefaultValue(c_ShowDetailsPopup)
         ]
        public bool ShowDetailsPopup
        {
            get
            {
                return showDetailsPopup;
            }
            set
            {
                showDetailsPopup = value;
            }
        }

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
         System.ComponentModel.DefaultValue(c_ShowDetailsDOB)
         ]
        public bool ShowDetailsDOB
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
        System.Web.UI.WebControls.WebParts.WebDisplayName("Введіть кількість стовбців для відображення списку"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть кількість стовбців для відображення списку."),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Фільтр"),
        System.ComponentModel.DefaultValue(c_NumberOfColumns)
        ]
        public int NumberOfColumns
        {
            get
            {
                return numberOfColumns;
            }
            set
            {
                numberOfColumns = value;
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

        protected override void CreateChildControls()
        {
            if (ScriptManager.GetCurrent(this.Page) == null)
            {
                ScriptManager manager = new ScriptManager();
                manager.EnablePartialRendering = true;
                Controls.Add(manager);
            }

            EmployeeListUC control = (EmployeeListUC)Page.LoadControl(_ascxPath);
            if (control != null)
            {
                control._webPart = this;
                Controls.Add(control);
            } 
        }
    }
}
