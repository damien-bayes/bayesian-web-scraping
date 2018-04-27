using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_user : System.Web.UI.Page
{
    public string username = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        username = Context.User.Identity.Name;
        DataSet ds = Core._getProfile(username);

        if (ds.Tables[0].Rows.Count != 0)
        {
            MP_P_logo.Src = ds.Tables[0].Rows[0]["avatar"].ToString().Replace(",/Handler.ashx", "");
        }
    } 
}