using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_reference : System.Web.UI.Page
{
    private Guid guid = new Guid("f3dbe885-c67a-42c7-9637-00326fdfcc33");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        string[] ds = Core._getStopWords();
        int
            i = 1;

        content_log.InnerHtml = string.Empty;

        foreach (string item in ds)
        {
            content_log.InnerHtml += "<tr><td>" + i.ToString() + "</td><td>" + item.ToString() + "</td></tr>";
            i++;
        }

        DataSet states = Core.QueryInBase("taskstate");
        P_states.InnerHtml = string.Empty;
        foreach (DataRow item in states.Tables[0].Rows)
        {
            P_states.InnerHtml += "<tr><td>" + item["id"].ToString() + "</td><td>" + item["state"].ToString() + "</td></tr>";
        }

        Dictionary<int, string> totics_v = Core._getTopics();
        foreach (KeyValuePair<int,string> item in totics_v)
        {
            topics.InnerHtml += "<tr><td>" + item.Key.ToString() + "</td><td>" + item.Value + "</td></tr>";
        }
    }
}