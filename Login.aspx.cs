#region directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region User Authentication Script
public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.Identity.IsAuthenticated)
            Response.Redirect("Home.aspx");
    }

    protected void logIn_Click(object sender, EventArgs e)
    {
        if (Core.ValidateUser(username.Value, password.Value))
        {
            FormsAuthenticationTicket tkt;
            string cookiestr;
            HttpCookie ck;

            tkt = new FormsAuthenticationTicket(
                1,
                username.Value,
                DateTime.Now,   
                DateTime.Now.AddMinutes(30),
                chkPersistCookie.Checked,
                "your custom data");

            cookiestr = FormsAuthentication.Encrypt(tkt);
            ck = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr);

            if (chkPersistCookie.Checked)
                ck.Expires = tkt.Expiration;
            
            ck.Path = FormsAuthentication.FormsCookiePath;
            Response.Cookies.Add(ck);

            string strRedirect = Request["ReturnUrl"];

            if (strRedirect == null)
                strRedirect = "Home.aspx";
            
            Session["role"] = Core._getRole(username.Value);
            Response.Redirect(strRedirect, true);
        }
        else
        {
            Response.Redirect("login.aspx", true);
        }
    }
}
#endregion