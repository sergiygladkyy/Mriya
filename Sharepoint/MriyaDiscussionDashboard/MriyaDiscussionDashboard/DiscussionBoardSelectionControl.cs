using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts; 

namespace MriyaDiscussionDashboard.DiscussionDashboard
{
    /// <summary>
    /// W.P. property for selecting one Discussion Board to be displayed
    /// </summary>
    /// /////////////////////////////////////////////////////////////////////////////////
    public class DiscussionBoardSelectionControl : EditorPart
    {
        private SPSite thisSite; //TODO: Dispose this
        private SPUser currentUser;
        private List<DiscussionBoardWithMetadata> boardsWithMetadata = new List<DiscussionBoardWithMetadata> { };
        private Label discussionSelectionLabel;
        private DropDownList discussionSelectionDropDownList;

        public DiscussionBoardSelectionControl()
        {
            thisSite = new SPSite(SPContext.Current.Site.ID);
            currentUser = thisSite.OpenWeb().CurrentUser;
        }
        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            discussionSelectionLabel = new Label();
            discussionSelectionDropDownList = new DropDownList();
            FillBoards(thisSite.OpenWeb());

            for (int i = 0; i < boardsWithMetadata.Count; i++)
            {
                //Selection combo and boards collection will have the same index
                discussionSelectionDropDownList.Items.Add(boardsWithMetadata[i].title);
            }
            Controls.Add(discussionSelectionLabel);
            Controls.Add(new HtmlGenericControl("br"));

            if (boardsWithMetadata.Count == 0)
            {
                discussionSelectionLabel.Text = Properties.Resources.textDiscussionSelectionLabelNoDiscussionsFound;
            }
            else
            {
                discussionSelectionLabel.Text = Properties.Resources.textDiscussionSelectionLabel;
                Controls.Add(discussionSelectionDropDownList);
            }
        }
        /// <summary>
        /// Recursively retrieves all discussion boards from website and the subwebs of website
        /// </summary>
        /// <param name="webSite"></param>
        private void FillBoards(SPWeb webSite)
        {          
            if (webSite.ID == thisSite.RootWeb.ID)
                RetrieveDiscussionBoards(webSite);
            foreach (SPWeb web in webSite.Webs)
                if (web.DoesUserHavePermissions(currentUser.LoginName, Microsoft.SharePoint.SPBasePermissions.ViewPages))
                {
                    RetrieveDiscussionBoards(web);
                    FillBoards(web);
                }
        }
        /// <summary>
        /// Retrieves the list of available discussion boards for the given subweb
        /// </summary>
        /// <param name="webSite"></param>
        private void RetrieveDiscussionBoards(SPWeb webSite)
        {
            foreach (SPList list in webSite.Lists)
            {
                if (list.BaseTemplate == SPListTemplateType.DiscussionBoard)
                    boardsWithMetadata.Add(new DiscussionBoardWithMetadata(webSite, list));
            }

        }
        /// <summary>
        /// Updates Web Part data with what the User has selected in the wp editor
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            // sync with the new property changes here
            EnsureChildControls();
            DiscussionDashboard dashBoardToEdit = this.WebPartToEdit as DiscussionDashboard;

            if (discussionSelectionDropDownList.Items.Count > 0)
                dashBoardToEdit.currentBoardWithMetadata = boardsWithMetadata[discussionSelectionDropDownList.SelectedIndex];

            dashBoardToEdit.SaveChanges();
            return true;
        }
        /// <summary>
        /// Reads Web Part's data and updates wp editor
        /// </summary>
        public override void SyncChanges()
        {
            // sync with the new property changes here
            EnsureChildControls();
            DiscussionDashboard dashBoardToEdit = this.WebPartToEdit as DiscussionDashboard;

            if (dashBoardToEdit.currentBoardWithMetadata != null)
                for (int i = 0; i < boardsWithMetadata.Count(); i++ )
                    if (boardsWithMetadata[i].boardID == dashBoardToEdit.currentBoardWithMetadata.boardID)
                        discussionSelectionDropDownList.SelectedIndex = i;


                //discussionSelectionDropDownList.SelectedIndex = boardsWithMetadata.IndexOf(dashBoardToEdit.currentBoardWithMetadata);
        }
        /// <summary>
        /// On Cancel rollback all the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Cancel_Click(object sender, EventArgs e)
        {
            // Do something if necessary
        }
    }
}