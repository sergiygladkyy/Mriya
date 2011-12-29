using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;


namespace MriyaDiscussionDashboard.DiscussionDashboard
{
    /// <summary>
    /// Single item in the list of the available dasboards, displayed as a web part property
    /// The selected one is used as by the user control
    /// </summary>
    /// /////////////////////////////////////////////////////////////////////////////////
    [Serializable]
    public class DiscussionBoardWithMetadata
    {
        public Guid subWebID = Guid.Empty;
        public Guid boardID = Guid.Empty;
        private string _title = Properties.Resources.textDiscussionBoardUnknown;

        public string title
        {
            get
            {
                return _title;                
            }
        }

        public DiscussionBoardWithMetadata(SPWeb subWeb, SPList board)
        {
            if ((subWeb != null) & (board != null))
            {
                this.subWebID = subWeb.ID;
                this.boardID = board.ID;
                _title = subWeb.Title + " - " + board.Title;                     
            }
            
        }
    }
}
