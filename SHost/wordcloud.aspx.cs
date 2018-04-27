using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SHost_wordcloud : System.Web.UI.Page
{
    public Dictionary<string, int> cloud = new Dictionary<string, int>();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["id"] != null)
            cloud = Core.GetWordsForCloud(Convert.ToInt32(Request.Params["id"]));
    }
}