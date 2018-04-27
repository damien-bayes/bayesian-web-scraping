using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_lemmas : System.Web.UI.Page
{
    private Guid guid = new Guid("c10c6929-5c26-4cbf-86f5-59fc38e48eba");
    public string url = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        _buildMenu();

        if (Request.Params["id"] != null)
        {
            DataSet areanameo = Core._getAreaNameFromId(Convert.ToInt32(Request.Params["id"]));
            foreach (DataRow item in areanameo.Tables[0].Rows)
            {
                selected.InnerText = item["title"].ToString();
                url = item["url"].ToString();
            }
        }
        else
        {
            selected.InnerText = "Выберите область";
            build.Disabled = true;
        }
    }

    private void _buildMenu()
    {
        string
            _content = string.Empty;

        DataSet maps = Core._getAllMap();

        foreach (DataRow item in maps.Tables[0].Rows)
        {

            DataSet areas = Core._getAllAreas(Convert.ToInt32(item["id"]));

            _content += "<li><a style = 'padding-right: 50px !important;' " + (areas.Tables[0].Rows.Count != 0 ? "class=\"dropdown-toggle\"" : "") + " href=\"#\">" + item["title"].ToString() + "</a>";

            if (areas.Tables[0].Rows.Count != 0)
                _content += "<ul class=\"dropdown-menu\" data-role=\"dropdown\" style=\"display: block;\">";

            foreach (DataRow item_area in areas.Tables[0].Rows)
            {
                _content += "<li><a href=\"/pages/lemmas.aspx?id=" + item_area["id"].ToString() + "\">" + item_area["title"].ToString() + "</a></li>";
            }

            if (areas.Tables[0].Rows.Count != 0)
                _content += "</ul>";
            _content += "</li>";
        }
        menus.InnerHtml = _content;
    }
}