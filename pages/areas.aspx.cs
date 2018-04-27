using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_areas : System.Web.UI.Page
{
    private Guid guid = new Guid("081A90D6-DC98-4116-B6C5-6348CDA831D3");
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        if (Core._getPageAccess(Convert.ToInt32(Session["role"]), guid) == false)
        {
            grid.InnerHtml = "У Вас нет прав для просмотра данной страницы";
        }
        else
        {
            DataSet ds = Core._getAllMap();

            if (!IsPostBack)
            {
                Dictionary<int, string> topics = Core._getTopics();
                topic.Items.Clear();
                foreach (KeyValuePair<int, string> topic_el in topics)
                {
                    topic.Items.Add(
                        new ListItem()
                        {
                            Text = topic_el.Value,
                            Value = topic_el.Key.ToString()
                        });
                }
                
                int val = -1;

                if (ds.Tables.Count != 0)
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        ListItem ls = new ListItem();
                        ls.Text = item["title"].ToString();
                        ls.Value = item["id"].ToString();

                        if (val == -1)
                            val = Convert.ToInt32(item["id"].ToString());

                        mapslist.Items.Add(ls);
                    }

                    _getAreas(val);
                }
            }

            topic.AutoPostBack = false;

            if (Request.Params["edit"] != null)
            {
                create.Visible = false;
                save.Visible = true;
                TitleType.InnerText = "Изменение области";
                topic.AutoPostBack = true;

                ds = Core._getAreaNameFromId(Convert.ToInt32(Request.Params["edit"]));

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    foreach (ListItem item_l in mapslist.Items)
                    {
                        if (item_l.Value == item["map_id"].ToString())
                            item_l.Selected = true;
                        else
                            item_l.Selected = false;
                    }
                    _getAreas(Convert.ToInt32(item["map_id"]));

                    address.Value = item["url"].ToString();
                    panelinfo.Style["display"] = "block";
                    P_logo.Attributes["src"] = item["image"].ToString();
                    title_finder.InnerText = item["title"].ToString();
                    description_finder.InnerText = item["subtitle"].ToString().Length > 50 ? item["subtitle"].ToString().Substring(0, 48) + "..." : item["subtitle"].ToString();

                    int topic_value = Convert.ToInt32(item["topic_id"]);

                    foreach (ListItem item_topic in topic.Items)
                    {
                        if (!IsPostBack)
                        {
                            if (item_topic.Value == topic_value.ToString())
                            {
                                item_topic.Selected = true;
                            }
                            else
                            {
                                item_topic.Selected = false;
                            }
                        }
                    }

                    //Основные поля
                    S_title.Value = item["title"].ToString();
                    S_description.Value = item["subtitle"].ToString();
                    S_image.Value = item["image"].ToString();
                }
            }
            else
            {
                create.Visible = true;
                save.Visible = false;
                TitleType.InnerText = "Новая область";
            }
        }
    }
    protected void maps_SelectedIndexChanged(object sender, EventArgs e)
    {
        int val =  Convert.ToInt32(mapslist.SelectedValue);
        _getAreas(val);
    }
    private void _getAreas(int val)
    {
        DataSet areas = Core._getAllAreas(val);
        P_arealist.InnerHtml = string.Empty;
        if (areas.Tables.Count != 0)
        {
            foreach (DataRow item in areas.Tables[0].Rows)
            {
                Controls_listitem l_item = (Controls_listitem)Page.LoadControl("../Controls/listitem.ascx");
                l_item.Link = "/pages/maps.aspx#";
                l_item.Head = item["title"].ToString();
                l_item.Decription = item["subtitle"].ToString() == string.Empty ? "Без описания" : item["subtitle"].ToString();
                l_item.IDent = Convert.ToInt32(item["id"].ToString());
                l_item.Topic = item["topic"].ToString();

                int countDocs = Core._getCountDocsForAreas(Convert.ToInt32(item["id"].ToString()));
                l_item.CountDocs = countDocs;

                P_arealist.Controls.Add(l_item);
            }

            if (areas.Tables[0].Rows.Count == 0)
            {
                P_arealist.InnerHtml = "<small style = 'color: grey'>Список пуст</small>";
            }
        }
    }
    protected void create_Click(object sender, EventArgs e)
    {
        Core.Map area = new Core.Map();
        area.Url = address.Value;
        area.Name = S_title.Value;
        area.description = S_description.Value;
        area.img = S_image.Value;
        area.topic = Convert.ToInt32(topic.SelectedValue);

        Core.CreateSubCategory(Convert.ToInt32(mapslist.SelectedValue), area, Core.WriteType.create);

        address.Value = string.Empty;
        S_title.Value = string.Empty;
        S_description.Value = string.Empty;
        S_image.Value = string.Empty;

        _getAreas(Convert.ToInt32(mapslist.SelectedValue));
    }
    protected void save_Click(object sender, EventArgs e)
    {
        Core.Map area = new Core.Map();
        area.id = Convert.ToInt32(Request.Params["edit"]);
        area.Url = address.Value;
        area.Name = S_title.Value;
        area.description = S_description.Value;
        area.img = S_image.Value;
        area.topic = Convert.ToInt32(topic.SelectedValue);

        Core.CreateSubCategory(Convert.ToInt32(mapslist.SelectedValue), area, Core.WriteType.edit);
    }
}