using System;
using System.ComponentModel;
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
    [ToolboxItemAttribute(false)]
    public class VideoGallery : GalleryWebPart
    {
        public VideoGallery()
        {
            m_MaxItems = c_MaxVideos; 
        }

        protected override void CreateChildControls()
        {
            // Call parent to include css/js
            base.CreateChildControls();

            // Initialize items
            m_Items.Clear();
            m_Items.Add(new GalleryItem("/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_1.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_1.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_1.wmv"
                ));
            m_Items.Add(new GalleryItem("/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_2.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_2.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_2.wmv"
                ));
            m_Items.Add(new GalleryItem("/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_3.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_3.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_3.wmv"
                ));
            m_Items.Add(new GalleryItem("/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_4.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_4.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_4.wmv"
                ));
            m_Items.Add(new GalleryItem("/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_5.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_5.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_5.mp4"
                ));
            m_Items.Add(new GalleryItem("/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_6.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_6.jpg",
                "/sites/mriya/SiteCollectionImages/Mriya Theme/galleries/video/video_6.flv"
                ));


            ClientScriptManager csm = Page.ClientScript;
            if (!csm.IsClientScriptBlockRegistered("videoInitializeUniqueKeySlider"))
                csm.RegisterClientScriptBlock(this.GetType(), "videoInitializeUniqueKeySlider", CreateJSUniqueKeySlider(), true);
        }

        protected override void Render(HtmlTextWriter output)
        {
            RenderChildren(output);

            // Securely write out HTML
            StringBuilder sbPart = new StringBuilder();

            sbPart.AppendLine("<div id=\"video_gallery_UniqueKey1\" class=\"gallery\">");
            sbPart.AppendLine("    <div id=\"gallery_preview_UniqueKey1\" class=\"gallery_preview\">");
            sbPart.AppendLine("        <a class=\"photo_icon\" href=\"#\"><img width=\"45\" height=\"39\" src=\"" +
                c_PathImages + "video_icon.png\"></a>");
            if (m_Items.Count > 0)
                sbPart.AppendLine("        <img src=\"" + m_Items[0].Image + "\" class=\"preview\"/>");
            sbPart.AppendLine("    </div>");

            sbPart.AppendLine("    <div id=\"gallery_slider_UniqueKey1\" class=\"gallery_slider\">");
            sbPart.AppendLine("        <div class=\"prev_btn_container\">");
            sbPart.AppendLine("            <img width=\"5\" height=\"11\" src=\"" + c_PathImages + "prev_ar_lft.png\" class=\"prev_btn\" />");
            sbPart.AppendLine("        </div>");
            sbPart.AppendLine("        <div class=\"container\">");
            sbPart.AppendLine("            <div class=\"slides\">");
            sbPart.AppendLine("            <table border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");
            sbPart.AppendLine("            <tr>");

            for (int i = 0; i < m_Items.Count && i < m_MaxItems; i++)
            {
                sbPart.AppendLine("                <td class=\"zoom" + ((i == 0) ? (" first") : ("")) + "\">");
                sbPart.AppendLine("                    <div class=\"item\" node=\"" + i.ToString() +
                    "\"><a href=\"#\" onclick=\"return false;\"><img src=\"" +
                    m_Items[i].Image + "\" alt=\"video_" + (i + 1).ToString() + "\" /></a></div>");
                sbPart.AppendLine("                </td>");
            }
            sbPart.AppendLine("            </tr>");
            sbPart.AppendLine("            </table>");
            sbPart.AppendLine("            </div>");
            sbPart.AppendLine("        </div>");
            sbPart.AppendLine("        <div class=\"next_btn_container\">");
            sbPart.AppendLine("            <img width=\"5\" height=\"11\" src=\"" + c_PathImages + "prev_ar_rgt.png\" class=\"next_btn\" />");
            sbPart.AppendLine("        </div>");
            sbPart.AppendLine("    </div>");
            sbPart.AppendLine("</div>");

            output.Write(sbPart.ToString());
        }

        protected string CreateJSUniqueKeySlider()
        {
            StringBuilder sbJS = new StringBuilder();

            string functionName = "initializeUniqueKeySlider_" + this.UniqueID;
            sbJS.AppendLine("function " + functionName + "()");
            sbJS.AppendLine("{");
            sbJS.AppendLine("   var gallery = new oiltecGallery('gallery_slider_UniqueKey1', 'gallery_preview_UniqueKey1', {");
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
            for (int i = 0; i < m_Items.Count && i < m_MaxItems; i++)
            {
                sbJS.AppendLine("       " + i.ToString() + ": {");
                sbJS.AppendLine("           s_img: '" + m_Items[i].Thumbnail + "',");
                sbJS.AppendLine("           b_img: '" + m_Items[i].Image + "',");
                sbJS.AppendLine("           video: '" + m_Items[i].Video + "'");
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
    }
}
