using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

using UserProfilesDAL;

namespace MriyaAddressBook.UserProfileProvider
{
    [ToolboxItemAttribute(false)]
    public class UserProfileProvider : WebPart, ITableProfiles
    {
        private TableProfiles _tableProfiles = null;

        public TableProfiles ProgilesTable
        {
            get
            {
                if (_tableProfiles == null)
                {
                    _tableProfiles = new TableProfiles();
                }
                return _tableProfiles;

            }
        }

        [ConnectionProvider("ITableProfiles")]
        public ITableProfiles GetUserProfileProvider()
        {
            return this;
        }

        protected override void CreateChildControls()
        {
            Label lblHello = new Label();
            lblHello.Text = "Data Provider for consuming webparts";
            Controls.Add(lblHello);
        }
    }
}
