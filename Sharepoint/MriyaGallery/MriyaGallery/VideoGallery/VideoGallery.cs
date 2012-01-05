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

namespace MriyaGallery.VideoGallery
{
    /// <summary>
    /// Mriya video gallery web part implementation (on main page)
    /// </summary>
    [ToolboxItemAttribute(false)]
    public class VideoGallery : GalleryWebPart
    {
        public VideoGallery() : base(GalleryWebPart.GalleryType.Video)
        {
            m_MaxItems = c_MaxVideos;
            m_UniqueCSSIdentifier = "video_gallery";
        }

        /// <summary>
        /// Provides a way for EditorPart controls to get a reference to the server controls they are associated with
        /// </summary>
        public override object WebBrowsableObject
        {
            // Return a reference to the Web Part instance.
            get { return this; }
        }
    }
}
