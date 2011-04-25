using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MriyaAddressBook.UserShortVerticalList
{
    /// <summary>
    /// Address book table columns header
    /// </summary>
    public class ABShortListRows
    {
        private string _strCaption = "";
        private bool _bVisible = false;

        #region Properties

        public string Caption
        {
            get { return _strCaption; }
            set { _strCaption = value; }
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
        public ABShortListRows()
        {
        }

        /// <summary>
        /// Constructor, creates column which is visible by default
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        public ABShortListRows(string strCaption)
        {
            SetProperties(strCaption, true);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        /// <param name="bVisible">True if column is visible</param>
        public ABShortListRows(string strCaption,bool bVisible)
        {
            SetProperties(strCaption, bVisible);
        }

        /// <summary>
        /// Sets object properties
        /// </summary>
        /// <param name="strCaption">Column caption</param>
        /// <param name="bVisible">True if column is visible</param>
        private void SetProperties(string strCaption, bool bVisible)
        {
            this._strCaption = strCaption;
            this._bVisible = bVisible;
        }
    }
}
