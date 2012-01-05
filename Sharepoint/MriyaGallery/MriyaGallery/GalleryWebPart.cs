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

        protected const bool c_UseDefaultCSS = true;

        // Attributes
        protected string m_UniqueCSSIdentifier = "gallery";

        protected int m_MaxItems = c_MaxImages;

        protected int m_SliderImageWidth = c_SliderImageWidth;
        protected int m_SliderImageHeight = c_SliderImageHeight;
        protected int m_SliderVisibleImages = c_SliderVisibleImages;
        protected int m_SliderWidth = c_SliderWidth;
        protected int m_SliderDuration = c_SliderDuration;

        protected int m_PreviewWidth = c_PreviewWidth;
        protected int m_PreviewHeight = c_PreviewHeight;

        protected bool m_UseDefaultCSS = c_UseDefaultCSS;

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

            if (UseDefaultCSS)
            {

                // Add style sheets
                HtmlLink linkGallery = new HtmlLink();
                linkGallery.Href = c_PathCSS + "StylesheetGallery.css";
                linkGallery.Attributes.Add("type", "text/css");
                linkGallery.Attributes.Add("rel", "stylesheet");
                this.Page.Header.Controls.Add(linkGallery);

                HtmlLink linkProperties = new HtmlLink();
                linkProperties.Href = c_PathCSS + "StylesheetProperties.css";
                linkProperties.Attributes.Add("type", "text/css");
                linkProperties.Attributes.Add("rel", "stylesheet");
                this.Page.Header.Controls.Add(linkProperties);
            }

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

            // Register gallery script
            if (!csm.IsClientScriptBlockRegistered(m_UniqueCSSIdentifier + "_InitializeUniqueKeySlider"))
                csm.RegisterClientScriptBlock(this.GetType(), m_UniqueCSSIdentifier + "_InitializeUniqueKeySlider",
                    CreateJSUniqueKeySlider(m_UniqueCSSIdentifier), true);
        }

        /// <summary>
        /// Renders the control to the specified HTML writer 
        /// </summary>
        /// <param name="output">The HtmlTextWriter object that receives the server control content</param>
        protected override void Render(HtmlTextWriter output)
        {
            RenderChildren(output);

            // Securely write out HTML
            output.Write(CreateGalleryConent(m_UniqueCSSIdentifier));
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
        /// Creates gallery content
        /// </summary>
        /// <param name="idClass">Unique class name, e.g: video_gallery or image_gallery</param>
        /// <returns>HTML code to be rendered in Render()</returns>
        virtual protected string CreateGalleryConent(string idClass)
        {
            StringBuilder sbPart = new StringBuilder();

            sbPart.AppendLine("<div class=\"gallery " + idClass + "\">");
            sbPart.AppendLine("    <div class=\"gallery_preview\">");
            if (GalleryItems.Count > 0)
                sbPart.AppendLine("        <img src=\"" + GalleryItems[0].Image + "\" class=\"preview\"/>");
            sbPart.AppendLine("    </div>");

            sbPart.AppendLine("    <div class=\"gallery_slider\">");
            sbPart.AppendLine("        <div class=\"prev_btn_container\">");
            sbPart.AppendLine("            <img src=\"" + c_PathImages + "prev_ar_lft.png\" class=\"prev_btn\" />");
            sbPart.AppendLine("        </div>");
            sbPart.AppendLine("        <div class=\"container\">");
            sbPart.AppendLine("            <div class=\"slides\">");
            sbPart.AppendLine("            <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            sbPart.AppendLine("            <tr>");

            for (int i = 0; i < GalleryItems.Count && i < m_MaxItems; i++)
            {
                sbPart.AppendLine("                <td class=\"zoom" + ((i == 0) ? (" first") : ("")) + "\">");
                sbPart.AppendLine("                    <div class=\"item" + ((i == 0) ? (" current") : ("")) + "\" node=\"" + i.ToString() +
                    "\"><a href=\"#\" onclick=\"return false;\"><img src=\"" +
                    GalleryItems[i].Image + "\" alt=\"item_" + (i + 1).ToString() + "\" /></a></div>");
                sbPart.AppendLine("                </td>");
            }
            sbPart.AppendLine("            </tr>");
            sbPart.AppendLine("            </table>");
            sbPart.AppendLine("            </div>");
            sbPart.AppendLine("        </div>");
            sbPart.AppendLine("        <div class=\"next_btn_container\">");
            sbPart.AppendLine("            <img src=\"" + c_PathImages + "prev_ar_rgt.png\" class=\"next_btn\" />");
            sbPart.AppendLine("        </div>");
            sbPart.AppendLine("    </div>");
            sbPart.AppendLine("</div>");

            return sbPart.ToString();
        }

        /// <summary>
        /// Generates JavaScript to run image gallery
        /// </summary>
        /// <param name="idClass">Unique class name, e.g: video_gallery or image_gallery</param>
        /// <returns>String which containing JavaScript</returns>

        virtual protected string CreateJSUniqueKeySlider(string idClass)
        {
            StringBuilder sbJS = new StringBuilder();


            string functionName = "initializeUniqueKeySlider_" + this.UniqueID;
            sbJS.AppendLine("function " + functionName + "()");
            sbJS.AppendLine("{");
            sbJS.AppendLine("   var gallery = new mriyaGallery('." + idClass + " .gallery_slider', '." + idClass + " .gallery_preview', {");
            sbJS.AppendLine("   use_share_point_silver_light: false,");
            sbJS.AppendLine("   players_dir: '" + c_PathPlayers + "',");
            sbJS.AppendLine("   slider: {");
            sbJS.AppendLine("       img_width:  " + m_SliderImageWidth.ToString() + ",");
            sbJS.AppendLine("       img_height: " + m_SliderImageHeight.ToString() + ",");
            sbJS.AppendLine("       max_images: " + m_MaxItems.ToString() + ",");
            sbJS.AppendLine("       visible_images: " + m_SliderVisibleImages.ToString() + ",");
            sbJS.AppendLine("       item_border:  {top: 0, right: 0, bottom: 0, left: 0},");
            sbJS.AppendLine("       item_padding: {top: 8, right: 2, bottom: 0, left: 2},");
            sbJS.AppendLine("       slider_width: " + m_SliderWidth.ToString() + ",");
            sbJS.AppendLine("       duration: " + m_SliderDuration.ToString() + ",");
            sbJS.AppendLine("       easing: 'jswing',");
            sbJS.AppendLine("   },");
            sbJS.AppendLine("   preview: {");
            sbJS.AppendLine("       width:  " + m_PreviewWidth.ToString() + ",");
            sbJS.AppendLine("       height: " + m_PreviewHeight.ToString() + ",");
            sbJS.AppendLine("       border:  {top: 0, right: 0, bottom: 0, left: 0},");
            sbJS.AppendLine("       padding: {top: 0, right: 0, bottom: 0, left: 0},");
            sbJS.AppendLine("       auto_play: false");
            sbJS.AppendLine("   },");
            sbJS.AppendLine("   data: {");
            for (int i = 0; i < GalleryItems.Count && i < m_MaxItems; i++)
            {
                sbJS.AppendLine("       " + i.ToString() + ": {");
                sbJS.AppendLine("           s_img: '" + GalleryItems[i].Thumbnail + "'");
                sbJS.AppendLine("         , b_img: '" + GalleryItems[i].Image + "'");
                if (Type == GalleryType.Video)
                    sbJS.AppendLine("         , video: '" + GalleryItems[i].Video + "'");
                if (Type == GalleryType.Images && GalleryItems[i].Description != null && GalleryItems[i].Description.Trim().Length > 0)
                    sbJS.AppendLine("   , description: '" + GalleryItems[i].Description + "'");

                sbJS.Append("       }");
                if (i < m_MaxItems - 1)
                    sbJS.AppendLine(",");
                else
                    sbJS.AppendLine("");
            }
            sbJS.AppendLine("       }");
            sbJS.AppendLine("   });");

            sbJS.AppendLine("   gallery.initialize();");
            sbJS.AppendLine("}");

            sbJS.AppendLine("_spBodyOnLoadFunctionNames.push('" + functionName + "');");

            return sbJS.ToString();
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

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Використовувати вбудовані стилі CSS"),
        System.Web.UI.WebControls.WebParts.WebDescription("Використовувати вбудовані стилі CSS"),
        System.Web.UI.WebControls.WebParts.Personalizable(
        System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Параметри галереї"),
        System.ComponentModel.DefaultValue(c_UseDefaultCSS)
        ]
        public bool UseDefaultCSS
        {
            get
            {
                return m_UseDefaultCSS;
            }
            set
            {
                m_UseDefaultCSS = value;
            }
        }

        #endregion WebPart Properties
    }
}
