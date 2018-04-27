#region directives
using System;
using System.Data;
using System.ServiceProcess;
#endregion

#region master-page script
public partial class MasterPage : System.Web.UI.MasterPage
{
    #region global variables
    string _userName;
    #endregion

    #region properties
    public string UserName
    {
        get { return _userName; }
        set { _userName = value; }
    }

    public string HeaderTitle
    {
        get { return headerTitle.InnerText; }
        set { headerTitle.InnerText = value; }
    }

    public string HeaderDescription
    {
        get { return headerDescription.InnerText; }
        set { headerDescription.InnerText = value; }
    }

    public string ServiceName { get { return "CWServerService"; }}

    public string MachineName { get { return "10.10.9.132"; }}
    #endregion

    #region functions
    protected void Page_Load(object sender, EventArgs e)
    {
        string redirectPage = "Login.aspx";

        if (!Context.User.Identity.IsAuthenticated)
            Response.Redirect(redirectPage);
        else
        {
            if (Context.User.Identity.AuthenticationType.Equals("Forms", StringComparison.OrdinalIgnoreCase))
            {
                UserName = Context.User.Identity.Name;
                greetingsUser.InnerText = string.Format("Здравствуйте, {0}", UserName);

                DataSet dataSet = Core.GetProfile(UserName);

                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    //avatar.Src = ds.Tables[0].Rows[0]["avatar"].ToString().Replace(",/Handler.ashx", "");
                    //avatar.Attributes.Add("title", Username);
                }
                else throw new Exception("Ooops, Core.GetProfile(UserName) exception. Object equals null or count is less than zero.");

                authorRights.InnerText = string.Format("©{0} Cube Crawler, Inc. Все права защищены.", DateTime.Now.Year).ToUpper();
            }
            else Response.Redirect(redirectPage);
        }

        /*
        if (GetServiceStatus())
        {
            //signal.Src = "images/cube-crawler-supervisor-images/svg/cube-logo-3.svg";
            //signal.Attributes.Add("title", "Связь со службой Cube Crawler присутствует");
        }*/
    }

    bool GetServiceStatus()
    {
        bool statusResult = false;
        ServiceController service = null;

        try
        {
            service = !string.IsNullOrEmpty(MachineName) ? new ServiceController(ServiceName, MachineName) : new ServiceController(ServiceName);
            ServiceControllerStatus serviceControllerStatus = service.Status;
            service.Close();

            if (serviceControllerStatus == ServiceControllerStatus.Running)
                statusResult = true;
        }
        catch { }

        return statusResult;
    }

    /*
    protected void LogOut_Click(object sender, EventArgs e)
    {
        FormsAuthentication.SignOut();
        Session["role"] = 0;
        Response.Redirect("/login.aspx", true);
    }*/
    #endregion
}
#endregion