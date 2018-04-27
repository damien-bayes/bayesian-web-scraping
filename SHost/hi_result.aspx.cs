using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SHost_hi_result : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["hi"] != null)
        {
            int area_id = Convert.ToInt32(Request.Params["hi"]);
            DataSet ds = Core.GetHIresult(area_id);
            string tsr
                = string.Empty;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                tsr += "<tr class=\"odd\"><td class=\"sorting_1\">" + item["term"].ToString() + "</td><td class=\" \">" + item["A"].ToString() + "</td><td class=\" \">" + item["B"].ToString() + "</td><td class=\" \">" + item["C"].ToString() + "</td><td class=\" \">" + item["D"].ToString() + "</td><td class=\" \">" + item["HI"].ToString() + "</td><td class=\" \">" + item["MI"].ToString() + "</td><td class=\" \">" + item["IG"].ToString() + "</td></tr>";
            }
            his.InnerHtml = tsr;
        }
    }
}