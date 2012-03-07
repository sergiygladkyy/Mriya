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

        /// <summary>
        /// Determines the max number recent discussions that should be shown
        /// </summary>
        const int c_NumberOfDiscussionsToDisplay = 2;
        int numberOfDiscussionsToDisplay = c_NumberOfDiscussionsToDisplay;

        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Кількість дискусій"),
        System.Web.UI.WebControls.WebParts.WebDescription("Відображаються найактивніші."),
        System.Web.UI.WebControls.WebParts.Personalizable(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Ще параметри"),
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

        /// <summary>
        /// Number of symbols to display in the digest
        /// </summary>
        const int c_DigestLength = 50;
        int digestLength = c_DigestLength;
        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Кількість букв у дайждесті"),
        System.Web.UI.WebControls.WebParts.WebDescription("Якщо повідомлення занадто довге, буде відображено лише зазначену кількість букв."),
        System.Web.UI.WebControls.WebParts.Personalizable(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Ще параметри"),
        System.ComponentModel.DefaultValue(c_DigestLength)]
        public int DigestLength
        {
            get
            {
                return digestLength;
            }
            set
            {
                digestLength = value;
            }
        }

        const string c_UrlForArrow = "/_layouts/1033/IMAGES/NextEvent.gif";
        string urlForArrow = c_UrlForArrow;
        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
        System.Web.UI.WebControls.WebParts.WebDisplayName("Aдреса декоративної стрілки"),
        System.Web.UI.WebControls.WebParts.WebDescription("Посилання на стрілку яку виводиться наприкінці кожного дайджесту"),
        System.Web.UI.WebControls.WebParts.Personalizable(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
        System.ComponentModel.Category("Ще параметри"),
        System.ComponentModel.DefaultValue(c_UrlForArrow)]
        public string UrlForArrow
        {
            get
            {
                return urlForArrow;
            }
            set
            {
                if (!(value == string.Empty))
                {
                    urlForArrow = value;
                }
                else
                    urlForArrow = c_UrlForArrow;

            }
        }

        /// <summary>
        /// Enables a top line in the content div. Useful when several web parts are stacked
        /// </summary>
        const bool c_EnableTopLine = false;
        bool enableTopLine = c_EnableTopLine;
        [System.Web.UI.WebControls.WebParts.WebBrowsable(true),
         System.Web.UI.WebControls.WebParts.WebDisplayName("Верхня розділова лінія"),
         System.Web.UI.WebControls.WebParts.WebDescription("Виберіть опцію, якщо понад цією веб частиною розташовується ще одна така ж"),
         System.Web.UI.WebControls.WebParts.Personalizable(System.Web.UI.WebControls.WebParts.PersonalizationScope.Shared),
         System.ComponentModel.Category("Ще параметри"),
         System.ComponentModel.DefaultValue(c_EnableTopLine)]

        public bool EnableTopLine
        {
            get
            {
                return enableTopLine;
            }
            set
            {
                enableTopLine = value;
            }
        }

   

        
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
