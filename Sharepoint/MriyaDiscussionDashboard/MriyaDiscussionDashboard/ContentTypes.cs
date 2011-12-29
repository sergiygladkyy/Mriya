using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;


/// Unfinished definition
namespace MriyaDiscussionDashboard.DiscussionDashboard
{


    /// <summary>
    /// Create a new folder.
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "Folder", Id = "0x0120")]
    //[Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(SummaryTask))]
    [Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Discussion))]
    public partial class Folder : Item
    {

        private string _name;

        private System.Nullable<int> _itemChildCountId;

        private string _itemChildCountItemChildCount;

        private System.Nullable<int> _folderChildCountId;

        private string _folderChildCountFolderChildCount;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        public Folder()
        {
            this.OnCreated();
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "FileLeafRef", Storage = "_name", Required = true, FieldType = "File")]
        public virtual string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if ((value != this._name))
                {
                    this.OnPropertyChanging("Name", this._name);
                    this._name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [Microsoft.SharePoint.Linq.RemovedColumnAttribute()]
        public override string Title
        {
            get
            {
                throw new System.InvalidOperationException("Field Title was removed from content type Folder.");
            }
            set
            {
                throw new System.InvalidOperationException("Field Title was removed from content type Folder.");
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "ItemChildCount", Storage = "_itemChildCountId", ReadOnly = true, FieldType = "Lookup", IsLookupId = true)]
        public System.Nullable<int> ItemChildCountId
        {
            get
            {
                return this._itemChildCountId;
            }
            set
            {
                if ((value != this._itemChildCountId))
                {
                    this.OnPropertyChanging("ItemChildCountId", this._itemChildCountId);
                    this._itemChildCountId = value;
                    this.OnPropertyChanged("ItemChildCountId");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "ItemChildCount", Storage = "_itemChildCountItemChildCount", ReadOnly = true, FieldType = "Lookup", IsLookupValue = true)]
        public string ItemChildCountItemChildCount
        {
            get
            {
                return this._itemChildCountItemChildCount;
            }
            set
            {
                if ((value != this._itemChildCountItemChildCount))
                {
                    this.OnPropertyChanging("ItemChildCountItemChildCount", this._itemChildCountItemChildCount);
                    this._itemChildCountItemChildCount = value;
                    this.OnPropertyChanged("ItemChildCountItemChildCount");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "FolderChildCount", Storage = "_folderChildCountId", ReadOnly = true, FieldType = "Lookup", IsLookupId = true)]
        public System.Nullable<int> FolderChildCountId
        {
            get
            {
                return this._folderChildCountId;
            }
            set
            {
                if ((value != this._folderChildCountId))
                {
                    this.OnPropertyChanging("FolderChildCountId", this._folderChildCountId);
                    this._folderChildCountId = value;
                    this.OnPropertyChanged("FolderChildCountId");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "FolderChildCount", Storage = "_folderChildCountFolderChildCount", ReadOnly = true, FieldType = "Lookup", IsLookupValue = true)]
        public string FolderChildCountFolderChildCount
        {
            get
            {
                return this._folderChildCountFolderChildCount;
            }
            set
            {
                if ((value != this._folderChildCountFolderChildCount))
                {
                    this.OnPropertyChanging("FolderChildCountFolderChildCount", this._folderChildCountFolderChildCount);
                    this._folderChildCountFolderChildCount = value;
                    this.OnPropertyChanged("FolderChildCountFolderChildCount");
                }
            }
        }
    }


    /// <summary>
    /// Create a new discussion topic.
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "Discussion", Id = "0x012002")]
    public partial class Discussion : Folder
    {

        private string _discussionSubject;

        private string _version0;

        private string _body;

        private string _reply;

        private string _post;

        private string _threading;

        private string _postedBy;

        private System.Nullable<System.DateTime> _lastUpdated;

        private string _eMailSender;

        private System.Nullable<int> _modifiedById;

        private string _modifiedByNameWithPicture;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        public Discussion()
        {
            this.OnCreated();
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Title", Storage = "_title", Required = true, FieldType = "Text")]
        public override string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if ((value != this._title))
                {
                    this.OnPropertyChanging("Title", this._title);
                    this._title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "DiscussionTitle", Storage = "_discussionSubject", ReadOnly = true, FieldType = "Computed")]
        public string DiscussionSubject
        {
            get
            {
                return this._discussionSubject;
            }
            set
            {
                if ((value != this._discussionSubject))
                {
                    this.OnPropertyChanging("DiscussionSubject", this._discussionSubject);
                    this._discussionSubject = value;
                    this.OnPropertyChanged("DiscussionSubject");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "_UIVersionString", Storage = "_version0", ReadOnly = true, FieldType = "Text")]
        public string Version0
        {
            get
            {
                return this._version0;
            }
            set
            {
                if ((value != this._version0))
                {
                    this.OnPropertyChanging("Version0", this._version0);
                    this._version0 = value;
                    this.OnPropertyChanged("Version0");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Body", Storage = "_body", FieldType = "Note")]
        public string Body
        {
            get
            {
                return this._body;
            }
            set
            {
                if ((value != this._body))
                {
                    this.OnPropertyChanging("Body", this._body);
                    this._body = value;
                    this.OnPropertyChanged("Body");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "ReplyNoGif", Storage = "_reply", ReadOnly = true, FieldType = "Computed")]
        public string Reply
        {
            get
            {
                return this._reply;
            }
            set
            {
                if ((value != this._reply))
                {
                    this.OnPropertyChanging("Reply", this._reply);
                    this._reply = value;
                    this.OnPropertyChanged("Reply");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "BodyAndMore", Storage = "_post", ReadOnly = true, FieldType = "Computed")]
        public string Post
        {
            get
            {
                return this._post;
            }
            set
            {
                if ((value != this._post))
                {
                    this.OnPropertyChanging("Post", this._post);
                    this._post = value;
                    this.OnPropertyChanged("Post");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Threading", Storage = "_threading", ReadOnly = true, FieldType = "Computed")]
        public string Threading
        {
            get
            {
                return this._threading;
            }
            set
            {
                if ((value != this._threading))
                {
                    this.OnPropertyChanging("Threading", this._threading);
                    this._threading = value;
                    this.OnPropertyChanged("Threading");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "PersonViewMinimal", Storage = "_postedBy", ReadOnly = true, FieldType = "Computed")]
        public string PostedBy
        {
            get
            {
                return this._postedBy;
            }
            set
            {
                if ((value != this._postedBy))
                {
                    this.OnPropertyChanging("PostedBy", this._postedBy);
                    this._postedBy = value;
                    this.OnPropertyChanged("PostedBy");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "DiscussionLastUpdated", Storage = "_lastUpdated", ReadOnly = true, FieldType = "DateTime")]
        public System.Nullable<System.DateTime> LastUpdated
        {
            get
            {
                return this._lastUpdated;
            }
            set
            {
                if ((value != this._lastUpdated))
                {
                    this.OnPropertyChanging("LastUpdated", this._lastUpdated);
                    this._lastUpdated = value;
                    this.OnPropertyChanged("LastUpdated");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "EmailSender", Storage = "_eMailSender", FieldType = "Note")]
        public string EMailSender
        {
            get
            {
                return this._eMailSender;
            }
            set
            {
                if ((value != this._eMailSender))
                {
                    this.OnPropertyChanging("EMailSender", this._eMailSender);
                    this._eMailSender = value;
                    this.OnPropertyChanged("EMailSender");
                }
            }
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [Microsoft.SharePoint.Linq.RemovedColumnAttribute()]
        public override string Name
        {
            get
            {
                throw new System.InvalidOperationException("Field FileLeafRef was removed from content type Discussion.");
            }
            set
            {
                throw new System.InvalidOperationException("Field FileLeafRef was removed from content type Discussion.");
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "MyEditor", Storage = "_modifiedById", ReadOnly = true, FieldType = "User", IsLookupId = true)]
        public System.Nullable<int> ModifiedById
        {
            get
            {
                return this._modifiedById;
            }
            set
            {
                if ((value != this._modifiedById))
                {
                    this.OnPropertyChanging("ModifiedById", this._modifiedById);
                    this._modifiedById = value;
                    this.OnPropertyChanged("ModifiedById");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "MyEditor", Storage = "_modifiedByNameWithPicture", ReadOnly = true, FieldType = "User", IsLookupValue = true)]
        public string ModifiedByNameWithPicture
        {
            get
            {
                return this._modifiedByNameWithPicture;
            }
            set
            {
                if ((value != this._modifiedByNameWithPicture))
                {
                    this.OnPropertyChanging("ModifiedByNameWithPicture", this._modifiedByNameWithPicture);
                    this._modifiedByNameWithPicture = value;
                    this.OnPropertyChanged("ModifiedByNameWithPicture");
                }
            }
        }
    }

    /// <summary>
    /// Create a new message.
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "Message", Id = "0x0107")]
    public partial class Message : Item
    {

        private string _discussionSubject;

        private string _version0;

        private string _body;

        private string _reply;

        private string _post;

        private string _threading;

        private string _postedBy;

        private string _eMailSender;

        private System.Nullable<int> _modifiedById;

        private string _modifiedByNameWithPicture;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        public Message()
        {
            this.OnCreated();
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "DiscussionTitle", Storage = "_discussionSubject", ReadOnly = true, FieldType = "Computed")]
        public string DiscussionSubject
        {
            get
            {
                return this._discussionSubject;
            }
            set
            {
                if ((value != this._discussionSubject))
                {
                    this.OnPropertyChanging("DiscussionSubject", this._discussionSubject);
                    this._discussionSubject = value;
                    this.OnPropertyChanged("DiscussionSubject");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "_UIVersionString", Storage = "_version0", ReadOnly = true, FieldType = "Text")]
        public string Version0
        {
            get
            {
                return this._version0;
            }
            set
            {
                if ((value != this._version0))
                {
                    this.OnPropertyChanging("Version0", this._version0);
                    this._version0 = value;
                    this.OnPropertyChanged("Version0");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Body", Storage = "_body", FieldType = "Note")]
        public string Body
        {
            get
            {
                return this._body;
            }
            set
            {
                if ((value != this._body))
                {
                    this.OnPropertyChanging("Body", this._body);
                    this._body = value;
                    this.OnPropertyChanged("Body");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "ReplyNoGif", Storage = "_reply", ReadOnly = true, FieldType = "Computed")]
        public string Reply
        {
            get
            {
                return this._reply;
            }
            set
            {
                if ((value != this._reply))
                {
                    this.OnPropertyChanging("Reply", this._reply);
                    this._reply = value;
                    this.OnPropertyChanged("Reply");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "BodyAndMore", Storage = "_post", ReadOnly = true, FieldType = "Computed")]
        public string Post
        {
            get
            {
                return this._post;
            }
            set
            {
                if ((value != this._post))
                {
                    this.OnPropertyChanging("Post", this._post);
                    this._post = value;
                    this.OnPropertyChanged("Post");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Threading", Storage = "_threading", ReadOnly = true, FieldType = "Computed")]
        public string Threading
        {
            get
            {
                return this._threading;
            }
            set
            {
                if ((value != this._threading))
                {
                    this.OnPropertyChanging("Threading", this._threading);
                    this._threading = value;
                    this.OnPropertyChanged("Threading");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "PersonViewMinimal", Storage = "_postedBy", ReadOnly = true, FieldType = "Computed")]
        public string PostedBy
        {
            get
            {
                return this._postedBy;
            }
            set
            {
                if ((value != this._postedBy))
                {
                    this.OnPropertyChanging("PostedBy", this._postedBy);
                    this._postedBy = value;
                    this.OnPropertyChanged("PostedBy");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "EmailSender", Storage = "_eMailSender", FieldType = "Note")]
        public string EMailSender
        {
            get
            {
                return this._eMailSender;
            }
            set
            {
                if ((value != this._eMailSender))
                {
                    this.OnPropertyChanging("EMailSender", this._eMailSender);
                    this._eMailSender = value;
                    this.OnPropertyChanged("EMailSender");
                }
            }
        }

        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [Microsoft.SharePoint.Linq.RemovedColumnAttribute()]
        public override string Title
        {
            get
            {
                throw new System.InvalidOperationException("Field Title was removed from content type Message.");
            }
            set
            {
                throw new System.InvalidOperationException("Field Title was removed from content type Message.");
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "MyEditor", Storage = "_modifiedById", ReadOnly = true, FieldType = "User", IsLookupId = true)]
        public System.Nullable<int> ModifiedById
        {
            get
            {
                return this._modifiedById;
            }
            set
            {
                if ((value != this._modifiedById))
                {
                    this.OnPropertyChanging("ModifiedById", this._modifiedById);
                    this._modifiedById = value;
                    this.OnPropertyChanged("ModifiedById");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "MyEditor", Storage = "_modifiedByNameWithPicture", ReadOnly = true, FieldType = "User", IsLookupValue = true)]
        public string ModifiedByNameWithPicture
        {
            get
            {
                return this._modifiedByNameWithPicture;
            }
            set
            {
                if ((value != this._modifiedByNameWithPicture))
                {
                    this.OnPropertyChanging("ModifiedByNameWithPicture", this._modifiedByNameWithPicture);
                    this._modifiedByNameWithPicture = value;
                    this.OnPropertyChanged("ModifiedByNameWithPicture");
                }
            }
        }
    }


    /// <summary>
    /// Create a new list item.
    /// </summary>
    [Microsoft.SharePoint.Linq.ContentTypeAttribute(Name = "Item", Id = "0x01")]
    //[Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Announcement))]
    [Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Folder))]
    //[Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Event))]
    //[Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Document))]
    //[Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Link))]
    //[Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Task))]
    [Microsoft.SharePoint.Linq.DerivedEntityClassAttribute(Type = typeof(Message))]
    public partial class Item : Microsoft.SharePoint.Linq.ITrackEntityState, Microsoft.SharePoint.Linq.ITrackOriginalValues, System.ComponentModel.INotifyPropertyChanged, System.ComponentModel.INotifyPropertyChanging
    {

        private System.Nullable<int> _id;

        private System.Nullable<int> _version;

        private string _path;

        private Microsoft.SharePoint.Linq.EntityState _entityState;

        private System.Collections.Generic.IDictionary<string, object> _originalValues;

        protected string _title;

        


        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate();
        partial void OnCreated();
        #endregion

        Microsoft.SharePoint.Linq.EntityState Microsoft.SharePoint.Linq.ITrackEntityState.EntityState
        {
            get
            {
                return this._entityState;
            }
            set
            {
                if ((value != this._entityState))
                {
                    this._entityState = value;
                }
            }
        }

        System.Collections.Generic.IDictionary<string, object> Microsoft.SharePoint.Linq.ITrackOriginalValues.OriginalValues
        {
            get
            {
                if ((null == this._originalValues))
                {
                    this._originalValues = new System.Collections.Generic.Dictionary<string, object>();
                }
                return this._originalValues;
            }
        }

        public Item()
        {
            this.OnCreated();           
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "ID", Storage = "_id", ReadOnly = true, FieldType = "Counter")]
        public System.Nullable<int> Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((value != this._id))
                {
                    this.OnPropertyChanging("Id", this._id);
                    this._id = value;
                    this.OnPropertyChanged("Id");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "owshiddenversion", Storage = "_version", ReadOnly = true, FieldType = "Integer")]
        public System.Nullable<int> Version
        {
            get
            {
                return this._version;
            }
            set
            {
                if ((value != this._version))
                {
                    this.OnPropertyChanging("Version", this._version);
                    this._version = value;
                    this.OnPropertyChanged("Version");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "FileDirRef", Storage = "_path", ReadOnly = true, FieldType = "Lookup", IsLookupValue = true)]
        public string Path
        {
            get
            {
                return this._path;
            }
            set
            {
                if ((value != this._path))
                {
                    this.OnPropertyChanging("Path", this._path);
                    this._path = value;
                    this.OnPropertyChanged("Path");
                }
            }
        }

        [Microsoft.SharePoint.Linq.ColumnAttribute(Name = "Title", Storage = "_title", Required = true, FieldType = "Text")]
        public virtual string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                if ((value != this._title))
                {
                    this.OnPropertyChanging("Title", this._title);
                    this._title = value;
                    this.OnPropertyChanged("Title");
                }
            }
        }

     

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if ((null != this.PropertyChanged))
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void OnPropertyChanging(string propertyName, object value)
        {
            if ((null == this._originalValues))
            {
                this._originalValues = new System.Collections.Generic.Dictionary<string, object>();
            }
            if ((false == this._originalValues.ContainsKey(propertyName)))
            {
                this._originalValues.Add(propertyName, value);
            }
            if ((null != this.PropertyChanging))
            {
                this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
            }
        }


    }

}
