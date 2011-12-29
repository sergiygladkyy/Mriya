using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint;

namespace MriyaDiscussionDashboard.DiscussionDashboard
{

    public partial class Item
    {
        private DateTime _lastModified;
        private string _url;
        private string _correctBodyToShow;

        partial void OnLoaded()
        {
            
           // do something here
                        
        }

        /// <summary>
        /// Manually added for having date time for Linq
        /// </summary>
        public DateTime LastModified
        {
            get
            {
                return this._lastModified;
            }
            set
            {
                if (value != this._lastModified)
                {
                    this.OnPropertyChanging("LastModified", this._lastModified);
                    this._lastModified = value;
                    this.OnPropertyChanged("LastModified");
                }
            }
        }

        /// <summary>
        /// List item URL
        /// </summary>
        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                if (value != this._url)
                {
                    this.OnPropertyChanging("Url", this._lastModified);
                    this._url = value;
                    this.OnPropertyChanged("Url");
                }
            }
        }

    }

            /// <summary>
        /// List item URL 
        /// </summary>
        public string Url
        {
            get
            {
                return this._url;
            }
            set
            {
                if (value != this._url)
                {
                    this.OnPropertyChanging("Url", this._lastModified);
                    this._url = value;
                    this.OnPropertyChanged("Url");
                }
            }
        }

    }

}
