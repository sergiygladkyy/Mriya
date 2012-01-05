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
    /// <summary>
    /// Gallery items custom webpart property implementation
    /// </summary>
    public class PropertyImages : EditorPart
    {
        private bool m_bAddVideo = false;
        private bool m_bAddVideoRestored = false;

        private int m_nMaxItemsCount = 0;
        private bool m_bMaxItemsCountRestored = false;

        UpdatePanel m_PanelAJAX = new UpdatePanel();
        private Panel m_PanelContent = new Panel();
        private Label m_Caption = new Label();
        private Table m_TableItems = new Table();
        private LinkButton m_LinkAdd = new LinkButton();

        private List<TextBox> m_TextSmallImage = new List<TextBox>();
        private List<TextBox> m_TextBigImage = new List<TextBox>();
        private List<TextBox> m_TextVideo = new List<TextBox>();
        private List<TextBox> m_TextDescription = new List<TextBox>();

        private List<int> m_ItemsID = null;

        #region Properties

        protected List<int> ItemIDCollection
        {
            get
            {
                if (m_ItemsID == null)
                {
                    if (this.ViewState["m_ItemsID"] != null)
                    {
                        m_ItemsID = (List<int>)this.ViewState["m_ItemsID"];
                    }
                    else
                    {
                        m_ItemsID = new List<int>();
                        this.ViewState["m_ItemsID"] = m_ItemsID;
                    }
                }
                return m_ItemsID;
            }

            set
            {
                m_ItemsID = value;
                this.ViewState["m_ItemsID"] = m_ItemsID;
            }
        }

        protected int NextItemID
        {
            get
            {
                List<int> ids = ItemIDCollection;
                int max = 0;
                foreach (int id in ids)
                    if (id > max) max = id;
                max++;
                return max;
            }
        }

        protected bool AddVideo 
        {
            get
            {
                if (!m_bAddVideoRestored && this.ViewState["m_bAddVideo"] != null)
                {
                    m_bAddVideo = (bool)this.ViewState["m_bAddVideo"];
                    m_bAddVideoRestored = true;
                }
                return m_bAddVideo;

            }
            set
            {
                this.ViewState["m_bAddVideo"] = m_bAddVideo = value;
                m_bAddVideoRestored = true;
            }
        }

        protected int MaxItems
        {
            get
            {
                if (!m_bMaxItemsCountRestored && this.ViewState["m_nMaxItemsCount"] != null)
                {
                    m_nMaxItemsCount = (int)this.ViewState["m_nMaxItemsCount"];
                    m_bMaxItemsCountRestored = true;
                }
                return m_nMaxItemsCount;

            }
            set
            {
                this.ViewState["m_nMaxItemsCount"] = m_nMaxItemsCount = value;
                m_bMaxItemsCountRestored = true;
            }
        }

        #endregion

        public PropertyImages()
        {
        }

        /// <summary>
        /// Creates part custom controls
        /// </summary>
        protected override void CreateChildControls()
        {
            // Enable AJAX 
            if (ScriptManager.GetCurrent(this.Page) == null)
            {
                ScriptManager manager = new ScriptManager();
                manager.EnablePartialRendering = true;
                Controls.Add(manager);
            }


            //LiteralControl 
            m_PanelContent.CssClass = "gallery_properties";
            m_PanelContent.Controls.Add(new LiteralControl("<span class=\"gallery_properties_caption\"><h3>" +
                Properties.Resources.PropertiesCaption + "</h3></span>"));


            m_PanelContent.Controls.Add(m_TableItems);
            
            List<int> ids = ItemIDCollection;
            foreach (int id in ids)
            {
                AddItem(id);
            }

            m_PanelContent.Controls.Add(new LiteralControl("<br/>"));

            m_LinkAdd.ID = "buttonAddItem";
            m_LinkAdd.CssClass = "add_control";
            m_LinkAdd.Text = Properties.Resources.PropertiesCaptionAddItem;
            m_LinkAdd.Command += new CommandEventHandler(OnAddCommand);
            m_PanelContent.Controls.Add(m_LinkAdd);
            m_PanelContent.Controls.Add(new LiteralControl("<br/><br/>"));

            m_PanelAJAX.ID = "updatePanel_" + ID;
            m_PanelAJAX.ContentTemplateContainer.Controls.Add(m_PanelContent);
            Controls.Add(m_PanelAJAX);

            base.CreateChildControls();
            this.ChildControlsCreated = true;
        }

        /// <summary>
        /// Updates Web Part data with what the User has selected in the wp editor
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            GalleryWebPart galleryWebPart = this.WebPartToEdit as GalleryWebPart;

            // sync with the new property changes here
            EnsureChildControls();

            AddVideo = (galleryWebPart.Type == GalleryWebPart.GalleryType.Video);
            MaxItems = galleryWebPart.MaxItemsCount;

            // Read out form fields
            List<GalleryItem> gitems = new List<GalleryItem>();
            List<int> ids = ItemIDCollection;
            foreach (int id in ids)
            {
                string sId = id.ToString();
                string imageSmallID = "textBoxSmallImage" + sId;
                string imageBigID = "textBoxBigImage" + sId;
                string videoID = "textBoxVideo" + sId;
                string descriptionID = "textBoxDescription" + sId;
                GalleryItem item = new GalleryItem();

                // Small image
                foreach (TextBox textBox in m_TextSmallImage)
                {
                    if (textBox.ID == imageSmallID)
                    {
                        item.Thumbnail = textBox.Text.Trim();
                        break;
                    }
                }
                // Big image
                foreach (TextBox textBox in m_TextBigImage)
                {
                    if (textBox.ID == imageBigID)
                    {
                        item.Image = textBox.Text.Trim();
                        break;
                    }
                }
                // Video
                if (AddVideo)
                {
                    foreach (TextBox textBox in m_TextVideo)
                    {
                        if (textBox.ID == videoID)
                        {
                            item.Video = textBox.Text.Trim();
                            break;
                        }
                    }

                    if ((item.Image.Length > 0 || item.Thumbnail.Length > 0) && item.Video.Length > 0)
                        gitems.Add(item);
                }
                else
                {
                    foreach (TextBox textBox in m_TextDescription)
                    {
                        if (textBox.ID == descriptionID)
                        {
                            item.Description = textBox.Text.Trim();
                            break;
                        }
                    }

                    if (item.Image.Length > 0 || item.Thumbnail.Length > 0)
                        gitems.Add(item);
                }
            }
            galleryWebPart.GalleryItems = gitems;
            galleryWebPart.SaveChanges();

            return true;
        }

        /// <summary>
        /// Reads Web Part's data and updates wp editor
        /// </summary>
        public override void SyncChanges()
        {
            GalleryWebPart galleryWebPart = this.WebPartToEdit as GalleryWebPart;

            AddVideo = (galleryWebPart.Type == GalleryWebPart.GalleryType.Video);
            MaxItems = galleryWebPart.MaxItemsCount;
            List<GalleryItem> gitems = galleryWebPart.GalleryItems;

            // sync with the new property changes here
            EnsureChildControls();

            // Fill form fields
            for (int i = 0; i < gitems.Count; i++)
            {
                AddItem(-1);

                if (m_TextSmallImage.Count > 0)
                    m_TextSmallImage[m_TextSmallImage.Count - 1].Text = gitems[i].Thumbnail;
                if (m_TextBigImage.Count > 0)
                    m_TextBigImage[m_TextBigImage.Count - 1].Text = gitems[i].Image;
                if (AddVideo)
                {
                    if (m_TextVideo.Count > 0)
                        m_TextVideo[m_TextVideo.Count - 1].Text = gitems[i].Video;
                }
                else
                {
                    if (m_TextDescription.Count > 0)
                        m_TextDescription[m_TextDescription.Count - 1].Text = gitems[i].Description;
                }
            }
        }


        /// <summary>
        /// Creates new gallery properties item
        /// </summary>
        /// <param name="id">-1 if item is new, >= 0 recreate item after page is reloaded</param>
        private void AddItem(int id)
        {
            // Create new table row, add controls
            TableRow rowItem = new TableRow();
            TableCell cellItem = new TableCell();

            if (id < 1)
            {
                id = NextItemID;
                List<int> ids = ItemIDCollection;
                ids.Add(id);
                ItemIDCollection = ids;
            }
            string sId = id.ToString();

            cellItem.CssClass = "rows_container";
            rowItem.ID = "row_item_" + sId;
            rowItem.Cells.Add(cellItem);
            m_TableItems.Rows.Add(rowItem);

            Table tableChildItem = new Table();
            tableChildItem.CssClass = "row row_" + sId;

            cellItem.Controls.Add(tableChildItem);

            tableChildItem.Rows.Add(
                CreateTextControl("textBoxSmallImage" + sId, 
                "label small_image_label", "small_image", 
                Properties.Resources.PropertiesCaptionSmallImage,
                ref m_TextSmallImage)
                );
            tableChildItem.Rows.Add(
                CreateTextControl("textBoxBigImage" + sId, 
                "label big_image_label", "big_image",
                Properties.Resources.PropertiesCaptionBigImage,
                ref m_TextBigImage)
                );
            if (AddVideo)
            {
                tableChildItem.Rows.Add(
                    CreateTextControl("textBoxVideo" + sId,
                    "label video_label", "label_video",
                    Properties.Resources.PropertiesCaptionVideo,
                    ref m_TextVideo)
                    );
            }
            else
            {
                tableChildItem.Rows.Add(
                    CreateTextControl("textBoxDescription" + sId,
                    "label description_label", "description",
                    Properties.Resources.PropertiesCaptionDescription,
                    ref m_TextDescription)
                    );
            }
            tableChildItem.Rows.Add(
                CreateButtonControl("buttonDeleteItem" + sId, 
                "delete_row", "delete_control", 
                Properties.Resources.PropertiesCaptionDeleteItem, 
                "deleteItem" + id.ToString(), id.ToString())
                );

            m_LinkAdd.Visible = (m_TableItems.Rows.Count < MaxItems);
        }

        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="id">Item ID</param>
        protected void DeleteItem(int id)
        {
            string rowID = "row_item_" + id.ToString();
            int deleteIndex = -1;
            for (int i = 0; i < m_TableItems.Rows.Count; i++)
            {
                if (m_TableItems.Rows[i].ID == rowID)
                {
                    deleteIndex = i;
                    break;
                }
            }

            if (deleteIndex >= 0)
            {
                m_TableItems.Rows.RemoveAt(deleteIndex);
            }

            List<int> ids = ItemIDCollection;
            for (int i = 0; i < ids.Count; i++)
            {
                if (ids[i] == id)
                {
                    ids.RemoveAt(i);
                    break;
                }
            }
            ItemIDCollection = ids;

            m_LinkAdd.Visible = (m_TableItems.Rows.Count < MaxItems);
        }

        /// <summary>
        /// Creates gallery item properties text control
        /// </summary>
        /// <param name="id">TextBox ID</param>
        /// <param name="cssLabel">CSS style of item label</param>
        /// <param name="cssControl">CSS style of the TextBox</param>
        /// <param name="label">Field label</param>
        /// <returns>TableRow with created crontrol</returns>
        TableRow CreateTextControl(string id, string cssLabel, string cssControl, string label, ref List<TextBox> listControls)
        {
            TableRow row = new TableRow();
            
            TableCell cell = new TableCell();

            TextBox control = new TextBox();
            control.ID = id;
            control.CssClass = cssControl;
            control.CausesValidation = false;

            if (label.Trim().Length > 0)
                cell.Controls.Add(new LiteralControl("<div class=\"" + cssLabel + "\"><nobr>" + label + "</nobr></div>"));
            cell.Controls.Add(control);
            listControls.Add(control);

            row.Cells.Add(cell);

            return row;
        }

        /// <summary>
        /// Creates gallery item properties text control
        /// </summary>
        /// <param name="id">TextBox ID</param>
        /// <param name="cssCell">CSS style of the table cell</param>
        /// <param name="cssControl">CSS style of the TextBox</param>
        /// <param name="label">Field label</param>
        /// <param name="command">Command name</param>
        /// <param name="commandArgs">Command arguments - id of the controls</param>
        /// <returns>TableRow with created crontrol</returns>
        TableRow CreateButtonControl(string id, string cssCell, string cssControl, string label, string command, string commandArgs)
        {
            TableRow row = new TableRow();

            TableCell cell = new TableCell();
            cell.CssClass = cssCell;

            LinkButton control = new LinkButton();
            control.ID = id;
            control.CssClass = cssControl;
            control.Text = label;
            control.Command += new CommandEventHandler(OnDeleteItemCommand);
            control.CommandName = command;
            control.CommandArgument = commandArgs;

            cell.Controls.Add(control);
            row.Cells.Add(cell);

            return row;
        }

        #region Events handler

        void OnAddCommand(object sender, CommandEventArgs e)
        {
            AddItem(-1);
        }

        void OnDeleteItemCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument == null)
                return;

            string se = e.CommandArgument.ToString();
            int id = 0;

            if (!int.TryParse(se, out id))
                return;

            DeleteItem(id);
        }

        #endregion
    }

}
