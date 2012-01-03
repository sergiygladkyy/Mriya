using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;

namespace MriyaGallery
{
    public class PropertyImages : EditorPart
    {
        Literal m_Content = new Literal();
        int m_ItemsCount = 0;
        bool m_bAddVideo = false;

        string m_FormName = "gallery_properties_form_";

        public PropertyImages()
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }

        /// <summary>
        /// Creates part custom controls
        /// </summary>
        protected override void CreateChildControls()
        {
            m_FormName = "gallery_properties_form_" + this.ID;

            // Run JS script on page load
            string scriptID = "gallery_properties_call_" + ID;
            if (!Page.ClientScript.IsStartupScriptRegistered(scriptID))
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), scriptID, CallJS(), true);

            }

            // Create HTML content here
            StringBuilder sbContent = new StringBuilder();
            
            sbContent.AppendFormat("<span class=\"styleMGPCaption\">{0}</span>",  Properties.Resources.PropertiesCaption).AppendLine();
            sbContent.AppendFormat("<div id=\"{0}\"></div>",  m_FormName).AppendLine();
            sbContent.AppendLine("<div id=\"log\"></div>");

            m_Content.Text = sbContent.ToString();
            Controls.Add(m_Content);

            base.CreateChildControls();
            this.ChildControlsCreated = true;
        }

        /// <summary>
        /// Updates Web Part data with what the User has selected in the wp editor
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            // sync with the new property changes here
            EnsureChildControls();

            System.Web.UI.Control panel = FindControlRecursive(m_Content, m_FormName);

                for (int i = 0; i < 1000; i++)
                {

                    System.Web.UI.Control textSImage = FindControlRecursive(Page, "s_img_" + i.ToString());
                    System.Web.UI.Control textBImage = m_Content.FindControl("b_img_" + i.ToString());
                    System.Web.UI.Control textVideo = m_Content.FindControl("video_" + i.ToString());


                    if ((textSImage == null || textSImage.Visible == false) && (textBImage == null || textBImage.Visible == false))
                        continue;

                }

            //
            GalleryWebPart galleryWebPart = this.WebPartToEdit as GalleryWebPart;

            m_bAddVideo = (galleryWebPart.Type == GalleryWebPart.GalleryType.Video);

            galleryWebPart.SaveChanges();

            return true;
        }

        /// <summary>
        /// Reads Web Part's data and updates wp editor
        /// </summary>
        public override void SyncChanges()
        {
            GalleryWebPart galleryWebPart = this.WebPartToEdit as GalleryWebPart;

            m_bAddVideo = (galleryWebPart.Type == GalleryWebPart.GalleryType.Video);
            m_ItemsCount = galleryWebPart.MaxItemsCount;

            // sync with the new property changes here
            EnsureChildControls();
        }

        /// <summary>
        /// Prepare JS parameters which are based on GalleryItems
        /// </summary>
        /// <returns></returns>
        private string CallJS()
        {
            StringBuilder sbJS = new StringBuilder(@"
	            function #function_name#() {
		            var form = new GalleryPropertiesForm('#form_name#', {
			            max_rows: #max_rows#,
                        show_video: #show_video#,
			            labels: {
				            small_image: '#caption_small_image#:',
				            big_image:   '#caption_big_image#:',
				            video:       '#caption_video#:',
				            add_row:     '#caption_add#',
				            delete_row:  '#caption_delete#'
			            }
                        #data#
		            });
		
		            form.show();
	            }
                #function_name#();
            ");

            sbJS.Replace("#function_name#", "CreateForm_" + this.ID);
            sbJS.Replace("#form_name#", m_FormName);
            sbJS.Replace("#max_rows#", m_ItemsCount.ToString());
            sbJS.Replace("#show_video#", ((m_bAddVideo) ? ("true") : ("false")));
            sbJS.Replace("#caption_small_image#", Properties.Resources.PropertiesCaptionSmallImage);
            sbJS.Replace("#caption_big_image#", Properties.Resources.PropertiesCaptionBigImage);
            sbJS.Replace("#caption_video#", Properties.Resources.PropertiesCaptionVideo);
            sbJS.Replace("#caption_add#", Properties.Resources.PropertiesCaptionAddItem);
            sbJS.Replace("#caption_delete#", Properties.Resources.PropertiesCaptionDeleteItem);
            sbJS.Replace("#data#", GetJSParams());

            return sbJS.ToString();
        }
        
        private string GetJSParams()
        {
            GalleryWebPart galleryWebPart = this.WebPartToEdit as GalleryWebPart;
            StringBuilder sbParams = new StringBuilder();

            int count = galleryWebPart.GalleryItems.Count;
            if (count > 0)
            {
                sbParams.AppendLine(",");
                sbParams.AppendLine("data: {");
                for (int i = 0; i < count; i++)
                {
                    sbParams.AppendFormat("{0}: {{", i);
                    sbParams.AppendFormat("   id:    {0},", (i + 1));
                    sbParams.AppendFormat("   s_img: '{0}',", galleryWebPart.GalleryItems[i].Thumbnail);
                    sbParams.AppendFormat("   b_img: '{0}',", galleryWebPart.GalleryItems[i].Image);
                    sbParams.AppendFormat("   video: '{0}'", galleryWebPart.GalleryItems[i].Video);
                    sbParams.Append("}");
                    if (i < count - 1)
                        sbParams.AppendLine(",");
                    else
                        sbParams.AppendLine();
                }
                sbParams.AppendLine("}");
            }
            return sbParams.ToString();
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="container">The container to search for the control passed. Remember
        /// all controls (Panel, GroupBox, Form, etc are all containsers for controls
        /// </param>
        /// <param name="name">Name of the control to look for</param>
        /// <returns></returns>
        /// 
        public Control FindControlRecursive(Control container, string name)
        {
            if (container.ID == name)
                return container;

            foreach (Control ctrl in container.Controls)
            {
                Control foundCtrl = FindControlRecursive(ctrl, name);
                if (foundCtrl != null)
                    return foundCtrl;
            }
            return null;
        }
    }

}
