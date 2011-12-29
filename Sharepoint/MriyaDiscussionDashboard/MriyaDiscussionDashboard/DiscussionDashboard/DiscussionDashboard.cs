using System;
using System.ComponentModel;
using System.Collections.Generic;
//using System.Web;
//using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace MriyaDiscussionDashboard.DiscussionDashboard
{
    /// <summary>
    /// This class represents the web part itself
    /// </summary>
    /// /////////////////////////////////////////////////////////////////////////////////
    [ToolboxItemAttribute(false)]
    public class DiscussionDashboard : WebPart
    {
        [Personalizable(true)]
        public DiscussionBoardWithMetadata currentBoardWithMetadata { get; set; }

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/MriyaDiscussionDashboard/DiscussionDashboard/DiscussionDashboardUserControl.ascx";

        const int c_NumberOfDiscussionsToDisplay = 2;
        int numberOfDiscussionsToDisplay = c_NumberOfDiscussionsToDisplay;

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
            System.Web.UI.WebControls.WebParts.WebDisplayName("Кількість дискусій"),
            System.Web.UI.WebControls.WebParts.WebDescription("Відображаються найактивніші."),
            System.Web.UI.WebControls.WebParts.Personalizable(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
            System.ComponentModel.Category("Додаткова категорія"),
            System.ComponentModel.DefaultValue(c_NumberOfDiscussionsToDisplay)]

        public int NumberOfDiscussionsToDisplay
        {
            get
            {
                return numberOfDiscussionsToDisplay;
            }
            set
            {
                numberOfDiscussionsToDisplay = value;
            }
        }
        
      //const bool c_EnableCaption = true;
      //bool enableCaption = c_EnableCaption;
      //[System.Web.UI.WebControls.WebParts.WebBrowsable(true),
      // System.Web.UI.WebControls.WebParts.WebDisplayName("Показувати заголовок"),
      // System.Web.UI.WebControls.WebParts.WebDescription("Виберіть, для того, щоб показати заголовок."),
      // System.Web.UI.WebControls.WebParts.Personalizable(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
      // System.ComponentModel.Category("Додатково категорія"),
      // System.ComponentModel.DefaultValue(c_EnableCaption)]
        
      //  public bool EnableCaption
      //  {
      //      get
      //      {
      //          return enableCaption;
      //      }
      //      set
      //      {
      //          enableCaption = value;
      //      }
      //  }

   

        
        /// <summary>
        /// Cusotm configuration sections (aka Editor Parts)
        /// </summary>
        /// <returns></returns>
        public override EditorPartCollection CreateEditorParts()
        {
            DiscussionBoardSelectionControl boardSelectionControl = new DiscussionBoardSelectionControl();
            boardSelectionControl.ID = this.ID + "_boardSelectionControl";
            List<EditorPart> editorParts = new List<EditorPart> { boardSelectionControl};             
            return new EditorPartCollection(editorParts);
        }

        public override object WebBrowsableObject
        {
            // Return a reference to the Web Part instance.
            get { return this; }
        }
        /// <summary>
        /// Create html control
        /// </summary>
        protected override void CreateChildControls()
        {
            DiscussionDashboardUserControl control = (DiscussionDashboardUserControl)Page.LoadControl(_ascxPath);
            control.webpart = this;
            Controls.Add(control);
        }
        /// <summary>
        /// This method sets a flag indicating that the personalization data has changed.
        /// This will allow the changes to the Web Part properties from outside the Web Part class.
        /// </summary>
        public void SaveChanges()
        {
            this.SetPersonalizationDirty();
        }
    }
}
