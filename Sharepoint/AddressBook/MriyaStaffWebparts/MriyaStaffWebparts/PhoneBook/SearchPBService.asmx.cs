using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Data;

namespace MriyaStaffWebparts.PhoneBook
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    // [System.Web.Script.Services.ScriptService]
    public class SearchPBService : System.Web.Services.WebService
    {

        [WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public System.Collections.Generic.List<string> showHints(string prefixText)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            list.Add("a");
            list.Add("aaa");
            list.Add("aaaa");
            return list;
        }
    }
}
