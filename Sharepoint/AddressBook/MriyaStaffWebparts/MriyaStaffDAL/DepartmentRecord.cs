using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MriyaStaffDAL
{
    public class DepartmentRecord
    {
        private string _sName = "";

        public string Name
        {
            get { return _sName; }
            set { _sName = value; }
        }
    }
}
