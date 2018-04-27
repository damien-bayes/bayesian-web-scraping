using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_paths : System.Web.UI.Page
{
    private Guid guid = new Guid("445e4431-797d-4603-8f9b-f13bf70d35a1");
    public string map = string.Empty;
    public string area = string.Empty;

    public string areaid = string.Empty;
    public string default_text = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");
        if (Core._getPageAccess(Convert.ToInt32(Session["role"]), guid) == false)
        {
            grid.InnerHtml = "<div style = 'padding-left:100px;'>У Вас нет прав для просмотра данной страницы</div>";
        }
        else
        {
            bool debug = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["debug"]);
            debuging.Visible = debug;

            areas_list.Attributes.Add("onchange","setpage(this);");
            map = Request.Params["map"];
            areaid = Request.Params["area"];

            if (IsPostBack == false)
            {

                if (Request.Params["area"] != null)
                {
                    default_text = string.Empty;
                    DataSet areanameo = Core._getAreaNameFromId(Convert.ToInt32(areaid));
                    foreach (DataRow item in areanameo.Tables[0].Rows)
                    {
                        //selected.InnerText = item["title"].ToString();
                        area = item["url"].ToString();
                        areaid = item["id"].ToString();

                        address_browser.Value = item["url"].ToString();
                        string title_ = item["title"].ToString();
                        if (title_.Length > 20)
                        {
                            title_ = title_.Substring(0, 19) + "..";
                        }
                        browser_title.InnerText = title_;
                    }

                    DataSet paths = Core._getPathForArea(Convert.ToInt32(areaid));
                    foreach (DataRow item in paths.Tables[0].Rows)
                    {
                        if (item["routestype"].ToString() == "1")
                        {
                            //Это паджинатор
                            pgn.Style["display"] = "block";
                            ajax.Style["display"] = "none";

                            P_url.Value = item["url"].ToString();
                            Pattern_url.Value = item["patternUrl"].ToString();
                            

                            P_step.Value = item["step"].ToString();
                            P_start.Value = item["start"].ToString();
                            P_end.Value = item["end"].ToString();

                            P_url.Attributes.Add("required", "required");
                            P_step.Attributes.Add("required", "required");
                            P_start.Attributes.Add("required", "required");
                            P_end.Attributes.Add("required", "required");
                        }
                        else
                        {
                            //Это Ajax
                            pgn.Style["display"] = "none";
                            ajax.Style["display"] = "block";

                            P_text.Value = item["element"].ToString();
                            P_count.Value = item["step"].ToString();

                            P_text.Attributes.Add("required", "required");
                            P_count.Attributes.Add("required", "required");
                        }
                        break;
                    }
                }
                else
                {
                    contentfrm.Style.Add("background-color", "white");

                    default_text += "<div style = \"text-align:center; margin-top: 200px;\"><h2>Область просмотра веб страниц</h2><div>";
                    default_text += "<p style = \"color: #666\">Добавляйте в Chrome веб-приложения, темы и расширения.</p>";
                }
            }
            _buildMenu();
        }
    }

    private void _buildMenu()
    {
        string
            _content = string.Empty;

        DataSet maps = Core._getAllMap();

        areas_list.Items.Clear();
        ListItem emp = new ListItem();
        emp.Text = string.Empty;
        emp.Enabled = true;
        areas_list.Items.Add(emp);

        foreach (DataRow item in maps.Tables[0].Rows)
        {

            DataSet areas = Core._getAllAreas(Convert.ToInt32(item["id"]));

            _content += "<li><a style = 'padding-right: 50px !important;' " + (areas.Tables[0].Rows.Count != 0 ? "class=\"dropdown-toggle\"" : "") + " href=\"#\">" + item["title"].ToString() + "</a>";

            if (areas.Tables[0].Rows.Count != 0)
                _content += "<ul class=\"dropdown-menu\" data-role=\"dropdown\" style=\"display: block;\">";

            foreach (DataRow item_area in areas.Tables[0].Rows)
            {
                _content += "<li><a href=\"/pages/paths.aspx?map=" + item["id"].ToString() + "&area=" + item_area["id"].ToString() + "\">" + item_area["title"].ToString() + "</a></li>";

                ListItem ls = new ListItem();
                ls.Text = item_area["title"].ToString();
                ls.Value = "map=" + item["id"].ToString() + "&area=" + item_area["id"].ToString();
                if (Request.Params["map"] != null)
                {
                    if (Request.Params["area"] != null)
                    {
                        string prm = "map=" + Request.Params["map"] + "&area=" + Request.Params["area"];
                        if (prm == ls.Value)
                            ls.Selected = true;
                    }
                }
                areas_list.Items.Add(ls);
            }

            if (areas.Tables[0].Rows.Count != 0)
                _content += "</ul>";
            _content += "</li>";
        }
        //menus.InnerHtml = _content;
    }
    protected void savepg_Click(object sender, EventArgs e)
    {
        int areaid = Convert.ToInt32(Request.Params["area"]);
        int start = Convert.ToInt32(P_start.Value);
        int end = Convert.ToInt32(P_end.Value);
        int step = Convert.ToInt32(P_step.Value);
        
        Core.saveNewPath(
            areaid, 1, 
            start, end, 
            P_url.Value, Pattern_url.Value, 
            step, 
            string.Empty, 
            false);
    }

    protected void saveajax_Click(object sender, EventArgs e)
    {
        int areaid = Convert.ToInt32(Request.Params["area"]);
        int count = Convert.ToInt32(P_count.Value);

        Core.saveNewPath(
            areaid, 
            2, 1, 1, 
            string.Empty, string.Empty, 
            count, 
            P_text.Value, 
            false);
    }
}