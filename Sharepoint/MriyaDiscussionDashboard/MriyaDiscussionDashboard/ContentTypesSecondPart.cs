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
        private string _xml;

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
                    this.OnPropertyChanging("Url", this._url);
                    this._url = value;
                    this.OnPropertyChanged("Url");
                }
            }
        }

        /// <summary>
        /// User's reply without the rest of the discussion attached to it
        /// </summary>
        public string CorrectBodyToShow
        {
            get
            {
                return this._correctBodyToShow;
            }
            set
            {
                if (value != this._correctBodyToShow)
                {
                    this.OnPropertyChanging("CorrectBodyToShow", this._correctBodyToShow);
                    this._correctBodyToShow = value;
                    this.OnPropertyChanged("CorrectBodyToShow");
                }
            }
        }

        /// <summary>
        /// Full list fieldset in xml
        /// </summary>
        public string Xml
        {
            get
            {
                return this._xml;
            }
            set
            {
                if (value != this._xml)
                {
                    this.OnPropertyChanging("Xml", this._xml);
                    this._xml = value;
                    this.OnPropertyChanged("Xml");
                }
            }
        }

    }
}
