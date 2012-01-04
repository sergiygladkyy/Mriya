using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MriyaGallery
{
    /// <summary>
    /// Base class for Mriya image and video galleries
    /// </summary>
    public class GalleryWebPart : WebPart
    {
        // Definitions
        protected const string c_PathLayouts = "/_layouts/MriyaGallery/";
        protected const string c_PathCSS = c_PathLayouts + "CSS/";
        protected const string c_PathJS = c_PathLayouts + "JS/";
        protected const string c_PathPlayers = c_PathJS + "players/";
        protected const string c_PathImages = "/_layouts/images/MriyaGallery/";

        protected const int c_MaxImages = 11;
        protected const int c_MaxVideos = 5;
        protected const int c_MaxItems = c_MaxImages;

        protected const int c_SliderImageWidth = 45;
        protected const int c_SliderImageHeight = 30;
        protected const int c_SliderVisibleImages = 4;
        protected const int c_SliderWidth = 220;
        protected const int c_SliderDuration = 550;

        protected const int c_PreviewWidth = 220;
        protected const int c_PreviewHeight = 147;

        // Attributes
        protected int m_MaxItems = c_MaxImages;

        protected int m_SliderImageWidth = c_SliderImageWidth;
        protected int m_SliderImageHeight = c_SliderImageHeight;
        protected int m_SliderVisibleImages = c_SliderVisibleImages;
        protected int m_SliderWidth = c_SliderWidth;
        protected int m_SliderDuration = c_SliderDuration;

        protected int m_PreviewWidth = c_PreviewWidth;
        protected int m_PreviewHeight = c_PreviewHeight;

        // Type and property to specify what exactly gallery is

        public enum GalleryType
        {
            Images = 1,
            Video = 2
        }

        public GalleryType Type { get; private set; }

        public int MaxItemsCount { get { return m_MaxItems; } }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="type">Type of the gallery</param>
        public GalleryWebPart(GalleryType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// Create children custom controls, register CSS and JS
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            // Add style sheets
            HtmlLink linkImage = new HtmlLink();
            linkImage.Href = c_PathCSS + "StylesheetImageGallery.css";
            linkImage.Attributes.Add("type", "text/css");
            linkImage.Attributes.Add("rel", "stylesheet");
            this.Page.Header.Controls.Add(linkImage);

            HtmlLink linkVideo = new HtmlLink();
            linkVideo.Href = c_PathCSS + "StylesheetVideoGallery.css";
            linkVideo.Attributes.Add("type", "text/css");
            linkVideo.Attributes.Add("rel", "stylesheet");
            this.Page.Header.Controls.Add(linkVideo);

            HtmlLink linkProperties = new HtmlLink();
            linkProperties.Href = c_PathCSS + "StylesheetProperties.css";
            linkProperties.Attributes.Add("type", "text/css");
            linkProperties.Attributes.Add("rel", "stylesheet");
            this.Page.Header.Controls.Add(linkProperties);

            // Add Java scripts
            ClientScriptManager csm = Page.ClientScript;
            if (!csm.IsClientScriptIncludeRegistered("javascript_jquery_min"))
                csm.RegisterClientScriptInclude(this.GetType(), "javascript_jquery_min", c_PathJS + "jquery-1.3.2.min.js");
            if (!csm.IsClientScriptIncludeRegistered("javascript_jquery_easing"))
                csm.RegisterClientScriptInclude(this.GetType(), "javascript_jquery_easing", c_PathJS + "jquery.easing.1.3.js");
            if (!csm.IsClientScriptIncludeRegistered("javascript_gallery"))
                csm.RegisterClientScriptInclude(this.GetType(), "javascript_gallery", c_PathJS + "gallery.js");
            if (!csm.IsClientScriptIncludeRegistered("javascript_player_silverlight"))
                csm.RegisterClientScriptInclude(this.GetType(), "javascript_player_silverlight", c_PathPlayers + "silverlight.js");
            if (!csm.IsClientScriptIncludeRegistered("javascript_player_wmvplayer"))
                csm.RegisterClientScriptInclude(this.GetType(), "javascript_player_wmvplayer", c_PathPlayers + "wmvplayer.js");
            if (!csm.IsClientScriptIncludeRegistered("javascript_player_qtime"))
                csm.RegisterClientScriptInclude(this.GetType(), "javascript_player_qtime", c_PathPlayers + "AC_QuickTime.js");
        }

        /// <summary>
        /// This method sets a flag indicating that the personalization data has changed.
        /// This will allow the changes to the Web Part properties from outside the Web Part class.
        /// </summary>
        public virtual void SaveChanges()
        {
            this.SetPersonalizationDirty();
        }

        /// <summary>
        /// Called when a control enters edit display mode. It creates EditPart controls
        /// </summary>
        /// <returns>The collection of EditorPart controls</returns>
        public override EditorPartCollection CreateEditorParts()
        {
            ArrayList editorArray = new ArrayList();
            PropertyImages edPart = new PropertyImages();
            edPart.ID = this.ID + "_PropertyImages";
            editorArray.Add(edPart);
            EditorPartCollection editorParts =
              new EditorPartCollection(editorArray);
            return editorParts;
        }

        /// <summary>
        /// Web part custom properties
        /// </summary>
        #region WebPart Properties


        List<GalleryItem> m_GalleryItems = new List<GalleryItem>();

        [Personalizable(true)]
        public List<GalleryItem> GalleryItems
        {
            get
            {
                return m_GalleryItems;
            }
            set
            {
                m_GalleryItems = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Загальна ширина слайдера в пікселях"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть значення"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_SliderWidth)
        ]
        public int SliderWidth
        {
            get
            {
                return m_SliderWidth;
            }
            set
            {
                m_SliderWidth = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Кількість елементів, що відображаються в слайдері"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть значення"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_SliderVisibleImages)
        ]
        public int SliderVisibleImages
        {
            get
            {
                return m_SliderVisibleImages;
            }
            set
            {
                m_SliderVisibleImages = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Ширина картинки в слайдері (пікселі)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть значення"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_SliderImageWidth)
        ]
        public int SliderImageWidth
        {
            get
            {
                return m_SliderImageWidth;
            }
            set
            {
                m_SliderImageWidth = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Висота картинки в слайдері (пікселі)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть значення"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_SliderImageHeight)
        ]
        public int SliderImageHeight
        {
            get
            {
                return m_SliderImageHeight;
            }
            set
            {
                m_SliderImageHeight = value;
            }
        }
        
        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Ширина області відображення елемента галереї(пікселі)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть значення"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_PreviewWidth)
        ]
        public int PreviewWidth
        {
            get
            {
                return m_PreviewWidth;
            }
            set
            {
                m_PreviewWidth = value;
            }
        }

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Висота області відображення елемента галереї(пікселі)"),
        System.Web.UI.WebControls.WebParts.WebDescription("Введіть значення"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_PreviewHeight)
        ]
        public int PreviewHeight
        {
            get
            {
                return m_PreviewHeight;
            }
            set
            {
                m_PreviewHeight = value;
            }
        }

        #endregion WebPart Properties
    }
}
