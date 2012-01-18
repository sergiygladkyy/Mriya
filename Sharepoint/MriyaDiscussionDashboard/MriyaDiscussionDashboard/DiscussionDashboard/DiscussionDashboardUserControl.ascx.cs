using System;
using System.Text;
using System.Linq;
using System.Web;
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

        private const int _pageId = 4; //used for making a topic view URL in ShowRecords()
       


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



            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"
function ShowReplyInDialog(url, title)
// more info: http://www.chaholl.com/archive/2010/11/17/using-the-dialog-framework-in-sharepoint-2010.aspx
	{
        var options = SP.UI.$create_DialogOptions();
        options.title = title;
        options.url = url;
	    var dialog = SP.UI.ModalDialog.showModalDialog(options);
	}
");
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowReplyInDialog", sb.ToString(),true); 

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {           
            // Show error message (if there are any)
            labelErrorMessage.Visible = _bShowErrorMessage;          
            // Show table
            ShowRecords();
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

                        // _sbRecords.AppendLine("<div id=\"discussion_zone\">");

                         try
                         {
                             for (int i = 0; i < numberOfDiscussionsToDisplay; i++)
                             {
                                 Discussion discussion = topics.ElementAt<Discussion>(i);
                                 SPListItem discussionTopic = _discussionBoard.GetItemById((int)discussion.Id); // workaround for having a url

                                 var allReplies = dataContext.GetList<Message>(_discussionBoard.Title)
                                     .ScopeToFolder( SPHttpUtility.UrlPathEncode( discussionTopic.Folder.Url,true), false);

                                 if (allReplies.Count<Message>() == 0)
                                     throw new Exception(Properties.Resources.textExceptionQueryingDiscussionBoardNoReplies);

                                 // this is a really shitty code, but how else can I get the missing Item attributes from an spmetal-wrapped class. Any ideas?
                                 foreach (Message reply in allReplies)
                                 {
                                     SPListItem discussionReply = _discussionBoard.GetItemById((int)reply.Id);

                                     reply.LastModified = (DateTime)discussionReply["Modified"];
                                     try
                                     {
                                         reply.CorrectBodyToShow = (string)discussionReply["CorrectBodyToShow"].ToString();

                                     }
                                     catch (Exception ex)
                                     {

                                         reply.CorrectBodyToShow = ex.Message;
                                     }
                                     
                                     reply.Xml = (string)discussionReply.Xml;
                                 }

                                 var latestReply = (from reply in allReplies
                                                    orderby reply.LastModified descending
                                                    select reply).FirstOrDefault();

                                 latestReply.CorrectBodyToShow = Microsoft.SharePoint.Utilities.SPHttpUtility.ConvertSimpleHtmlToText(latestReply.CorrectBodyToShow, webpart.DigestLength);

                                 //{$SubWebUrl}{$DisplayForm}?PageType={$PageType}&ListId={$ListID}&ID={&ItemID}                                                                  
                                 latestReply.Url = string.Format("http://{0}{1}?PageType={2}&ListId={{{3}}}&ID={4}", SPContext.Current.Site.HostName, _discussionBoard.DefaultDisplayFormUrl, _pageId, _discussionBoard.ID, latestReply.Id);
                                 string onClickHref = string.Format(@"<a  onclick=""ShowReplyInDialog('{0}','{1}');return false;"" href='{0}' target=_self><NOBR>{2}... </NOBR>",latestReply.Url, Server.HtmlEncode(discussion.Title), latestReply.CorrectBodyToShow);

                                 string arrowHref;
                                 if (webpart.UrlForArrow.IndexOf("http://") == -1)
                                     arrowHref = "http://" + SPContext.Current.Site.HostName + webpart.UrlForArrow;
                                 else
                                     arrowHref = webpart.UrlForArrow;
 
                                 if ((i == 0) & (!webpart.EnableTopLine))
                                     _sbRecords.Append("<div class=\"discussion_post first\">");
                                 else
                                     _sbRecords.Append("<div class=\"discussion_post\">");
                                 _sbRecords.Append("<h3>");
                                 _sbRecords.Append("<a href=\"" + boardSubWeb.Url + "//" + discussionTopic.Url + "\">" + discussion.Title + "</a>");
                                 _sbRecords.Append("</h3>");
                                 _sbRecords.Append("<div class=\"discussion_timestamp\">");
                                 _sbRecords.Append(latestReply.LastModified.ToString("dd.MM hh:mm  "));
                                 _sbRecords.Append("</div>");//Discussion timestamp                           
                                 _sbRecords.Append(onClickHref);                                                                                                    
                                 _sbRecords.Append("</a>");
                                  _sbRecords.Append("<img width=\"9\" height=\"7\" src=\"" + arrowHref + "\">");
                                 _sbRecords.Append("</div>");//Discussion post                           
                                
                             }
                         }
                         catch (Exception ex)
                         {
                             _sbRecords.AppendLine(Properties.Resources.textExceptionQueryingDiscussionBoard + ex.Message);
                         }


                        //_sbRecords.Append("</div>");//Discussion Zone

                    }//using datacontext                                                          
              } // using board subweb
                                
            else
                _sbRecords.Append(Properties.Resources.messagePleaseSelectDiscussionBoard);
            
            literalDiscussionRecords.Text = _sbRecords.ToString();
            return true;
        }

        /// <summary>
        /// not used
        /// </summary>
        /// <returns></returns>
        public string CurrentViewId()
        {
            string viewId = string.Empty;

            try
            {
                  SPList currentList = SPContext.Current.Web.GetList(HttpContext.Current.Request.Url.OriginalString);

                foreach (SPView view in currentList.Views)
                {
                    if (HttpContext.Current.Request.Url.AbsolutePath.IndexOf(view.Url, StringComparison.OrdinalIgnoreCase) > 0)
                    {
                        viewId = view.ID.ToString();
                        break;
                    }
                }
            }
            catch (Exception)
            {

                viewId = new Guid().ToString();
            }

            return viewId;
        }

        #region dealing with Sharepoint site collections
        /// <summary>
        /// Gets a SharePoint web site object from its Guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static SPWeb getSPWeb(Guid guid)
        {
            SPWeb web = GetSPWebCollectionFromCurrentSite()[guid]; // doesn't want to return this spweb since it has admin privileges
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
