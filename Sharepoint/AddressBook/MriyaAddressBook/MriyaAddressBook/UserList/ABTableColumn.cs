using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MriyaAddressBook.UserList
{
    /// <summary>
    /// Address book table columns header
    /// </summary>
    public class ABTableColumn
    {
        private string _strCaption = "";
        private string _strSortCommand = "";
        private LinkButton _linkButton = null;
        private bool _bVisible = false;

        #region Properties

        public string Caption
        {
            get { return _strCaption; }
            set { _strCaption = value; }
        }

        public string SortCommand
        {
            get { return _strSortCommand; }
            set { _strSortCommand = value; }
        }

        public LinkButton LinkButton
        {
            get { return _linkButton; }
            set { _linkButton = value; }
        }

        public bool Visible
        {
            get { return _bVisible; }
            set { _bVisible = value; }
        }

        #endregion Properties

        /// <summary>
        /// Default constructor
        /// </summary>
        public ABTableColumn()
        {
        }

        /// <summary>
        /// Constructor, creates column which is visible by default
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        /// <param name="strSortCommand">Column sort command</param>
        public ABTableColumn(string strCaption, string strSortCommand)
        {
            SetProperties(strCaption, strSortCommand, true, null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        /// <param name="strSortCommand">Column sort command</param>
        /// <param name="bVisible">True if column is visible</param>
        public ABTableColumn(string strCaption, string strSortCommand, bool bVisible)
        {
            SetProperties(strCaption, strSortCommand, bVisible, null);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        /// <param name="strSortCommand">Column sort command</param>
        /// <param name="bVisible">True if column is visible</param>
        /// <param name="linkButtonHeader">Column header button</param>
        public ABTableColumn(string strCaption, string strSortCommand, bool bVisible, LinkButton linkButtonHeader)
        {
            SetProperties(strCaption, strSortCommand, bVisible, linkButtonHeader);
        }

        /// <summary>
        /// Sets object properties
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        /// <param name="strSortCommand">Column sort command</param>
        /// <param name="bVisible">True if column is visible</param>
        /// <param name="linkButtonHeader">Column header button</param>
        private void SetProperties(string strCaption, string strSortCommand, bool bVisible, LinkButton linkButtonHeader)
        {
            this._strCaption = strCaption;
            this._strSortCommand = strSortCommand;
            this._bVisible = bVisible;
            this._linkButton = linkButtonHeader;
        }
    }
}
