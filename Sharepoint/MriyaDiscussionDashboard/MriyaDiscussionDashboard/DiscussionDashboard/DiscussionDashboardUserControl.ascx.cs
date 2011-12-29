using System;
using System.Text;
using System.Linq;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
//using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Linq;

namespace MriyaDiscussionDashboard.DiscussionDashboard
{
    public partial class DiscussionDashboardUserControl : UserControl
    {
        public DiscussionDashboard webpart = null;
        
        //private SPSite _site = null;

        private bool _bShowErrorMessage = false;
       


        public SPUser _currentUser;
        private SPList _discussionBoard;

        StringBuilder _sbRecords = new StringBuilder();


        #region Page generation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            //_site = new SPSite(SPContext.Current.Site.ID);
            //_currentUser = _site.OpenWeb().CurrentUser;


 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            // Show table
            ShowRecords();

            // Show error message (if there are any)
            labelErrorMessage.Visible = _bShowErrorMessage;

            base.OnPreRender(e);
        }       
        #endregion Page generation

        protected bool ShowRecords()
        {
                        

            if (webpart.currentBoardWithMetadata != null)

                using (SPWeb boardSubWeb = getSPWeb(webpart.currentBoardWithMetadata.subWebID))
                {
                    if (boardSubWeb != null)
                    {
                        _discussionBoard = boardSubWeb.Lists.GetList(webpart.currentBoardWithMetadata.boardID, false);
                    }


                    /// LINQ Way
                    using (DataContext dataContext = new DataContext(boardSubWeb.Url))
                    {

                        EntityList<Discussion> allDiscussions = dataContext.GetList<Discussion>(_discussionBoard.Title);

                        var topics = from  discussion in allDiscussions
                                     orderby discussion.LastUpdated descending
                                     select discussion;


                        int numberOfDiscussionsToDisplay = topics.Count<Discussion>() < webpart.NumberOfDiscussionsToDisplay ? topics.Count<Discussion>() : webpart.NumberOfDiscussionsToDisplay;

                         _sbRecords.AppendLine("<div id=\"discussion_zone\">");

                         try
                         {
                             for (int i = 0; i < numberOfDiscussionsToDisplay; i++)
                             {
                                 Discussion discussion = topics.ElementAt<Discussion>(i);
                                 SPListItem discussionItem = _discussionBoard.GetItemById((int)discussion.Id); // workaround for having a url

                                 var allReplies = dataContext.GetList<Message>(_discussionBoard.Title)
                                     .ScopeToFolder( SPHttpUtility.UrlPathEncode( discussionItem.Folder.Url,true), false); 

                                 // this is a really shitty code, but I can't get a system LastModified attribute from the spmetal-wrapped class. Any ideas?
                                 foreach (Message reply in allReplies)
                                 {
                                     reply.LastModified = (DateTime)_discussionBoard.GetItemById((int)reply.Id)["Modified"];
                                     reply.Url = (string)_discussionBoard.GetItemById((int)reply.Id).Url; //this doesn't work, needs some conversion
                                 }

                                 var latestReply = (from reply in allReplies
                                                    orderby reply.LastModified descending
                                                    select reply).FirstOrDefault();

                                 //string trimmedBody = Microsoft.SharePoint.Utilities.SPHttpUtility.ConvertSimpleHtmlToText(latestReply.Body, 50); 
                                 

                                 if (i == 0)
                                     _sbRecords.Append("<div class=\"discussion_post first\">");
                                 else
                                     _sbRecords.Append("<div class=\"discussion_post\">");
                                 _sbRecords.Append("<h3>");
                                 _sbRecords.Append("<a href=\"" + discussionItem.Url + "\">" + discussion.Title + "</a>");
                                 _sbRecords.Append("</h3>");
                                 _sbRecords.Append(latestReply.LastModified.ToString("dd.MM hh:mm "));
                                 _sbRecords.Append("<a href=\"" + latestReply.Url + "\">" + "trimmedBody" + "</a>");
                                 _sbRecords.Append("</div>");//Discussion post                           
                             }
                         }
                         catch (Exception ex)
                         {
                             _sbRecords.AppendLine(Properties.Resources.textExceptionQueryingDiscussionBoard + ex.Message);
                         }


                        _sbRecords.Append("</div>");//Discussion Zone

                    }//using datacontext                                                          
              } // using board subweb
                                
            else
                _sbRecords.Append(Properties.Resources.messagePleaseSelectDiscussionBoard);
            
            literalDiscussionRecords.Text = _sbRecords.ToString();
            return true;
        }

        #region dealing with Sharepoint site collections

        /// <summary>
        /// Gets a SharePoint web site object from its Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static SPWeb getSPWeb(Guid guid)
        {
            SPWeb web = GetSPWebCollectionFromCurrentSite()[guid]; // don't want to return this spweb since it has admin privileges
            return getRootSPSite().OpenWeb(web.ID);
        }
        public static SPSite getRootSPSite()
        {
            return SPContext.Current.Site;
        }
        /// <summary>
        /// Gets the collection of all the sites in the site collection.
        /// </summary>
        /// <param name="site">A SharePoint site collection</param>
        /// <returns>The collection of all the sites in the site collection</returns>
        public static SPWebCollection GetSPWebCollectionFromSite(SPSite site)
        {
            SPUserToken token = site.SystemAccount.UserToken;
            SPSite elevatedSiteCollection = new SPSite(site.ID, token);
            return elevatedSiteCollection.AllWebs;
        }

        /// <summary>
        /// Gets the collection of all the sites in the current site collection (so this is context specific).
        /// </summary>
        /// <returns> Collection of all the sites in the current site collection</returns>
        public static SPWebCollection GetSPWebCollectionFromCurrentSite()
        {
            return GetSPWebCollectionFromSite(SPContext.Current.Site);
        }

        #endregion
    }
}


//     <div id="discussion_zone">

//      <div class="discussion_post first">
//       <h3>
//            <a href="#">Mriya welcomes the guests at the party...</a>
//       </h3>                           	
//      08.08 12:55 
//        <a href="#">
//          "Something about it..." 
//          <img width="9" height="7" src="arrow_link.gif">
//        </a>
//      </div>

//      <div class="discussion_post">
//         <h3>
//              <a href="#">Mriya welcomes the guests at the party...</a>
//         </h3>                           	
//      08.08 12:55 
//        <a href="#">
//          "Something about it..." 
//          <img width="9" height="7" src="arrow_link.gif">
//        </a>
//      </div>

//     </div>
