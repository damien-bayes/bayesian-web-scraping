using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_elements : System.Web.UI.Page
{
    private Guid guid = new Guid("f463587e-b248-4c06-92bb-5af9875111e1");
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

            areas_list.Attributes.Add("onchange", "setpage(this);");
            map = Request.Params["map"];
            area = Request.Params["area"];

            

            if (Request.Params["map"] != null)
            {
                int index_ = 1;
                DataSet ds = Core._getIntentionsArea(Convert.ToInt32(area));
                arhive.InnerHtml = string.Empty;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Controls_elements_item pr = (Controls_elements_item)Page.LoadControl("~/Controls/elements_item.ascx");
                    pr.ID_ = row["id"].ToString();
                    if (Convert.ToInt32(row["tp"]) != 2)
                    {
                        pr.nameElement = "Блок";
                        pr.Description = "Поиск ссылок в данном блоке";
                        pr.Src_ = "/screenshots/" + row["imageurl"];
                    }
                    else
                    {
                        pr.nameElement = "Ссылка";
                        pr.Description = "Выбираються все ссылки аналогичные этой";
                        pr.Src_ = "/screenshots/" + row["imageurl"];
                    }
                    index_++;
                    arhive.Controls.Add(pr);
                }

                DataSet areanameo = Core._getAreaNameFromId(Convert.ToInt32(area));
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

            }
            else
            {
                contentfrm.Style.Add("background-color", "white");

                default_text += "<div style = \"text-align:center; margin-top: 200px;\"><h2>Область просмотра веб страниц</h2><div>";
                default_text += "<p style = \"color: #666\">Добавляйте в Chrome веб-приложения, темы и расширения.</p>";
            }

            _buildMenu();
        }
    }

    private void _buildMenu()
    {
        string
            _content = string.Empty;

        ListItem emp = new ListItem();
        emp.Text = string.Empty;
        emp.Enabled = true;
        areas_list.Items.Add(emp);

        DataSet maps = Core._getAllMap();

        foreach (DataRow item in maps.Tables[0].Rows)
        {

            DataSet areas = Core._getAllAreas(Convert.ToInt32(item["id"]));

            _content += "<li><a style = 'padding-right: 50px !important;' " + (areas.Tables[0].Rows.Count != 0 ? "class=\"dropdown-toggle\"" : "") + " href=\"#\">" + item["title"].ToString() + "</a>";

            if (areas.Tables[0].Rows.Count != 0)
                _content += "<ul class=\"dropdown-menu\" data-role=\"dropdown\" style=\"display: block;\">";

            foreach (DataRow item_area in areas.Tables[0].Rows)
            {
                _content += "<li><a href=\"/pages/elements.aspx?map=" + item["id"].ToString() + "&area=" + item_area["id"].ToString() + "\">" + item_area["title"].ToString() + "</a></li>";

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


}