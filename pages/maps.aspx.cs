#region directives
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region pages_maps script
public partial class pages_maps : System.Web.UI.Page
{
    #region global variables
    Guid guid = new Guid("bad79350-a814-4b16-a3e5-f6b3b63b0fdd");
    #endregion

    /// <summary>
    /// Page Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        ((MasterPage)this.Master).HeaderTitle = "Карты";
        ((MasterPage)this.Master).HeaderDescription = "Создание и редактирование карт поиска.";

        if (User.Identity.IsAuthenticated)
        {
            if (Core._getPageAccess(Convert.ToInt32(Session["role"]), guid) != false)
            {
                _pageRefresh();

                if (Request.Params["edit"] != null)
                {
                    //create.Visible = false;
                    //save.Visible = true;
                    //TitleType.InnerText = "Изменение карты";

                    DataSet ds = Core._getMapFromId(Convert.ToInt32(Request.Params["edit"]), true);

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        //address.Value = item["url"].ToString();
                        //panelinfo.Style["display"] = "block";
                        //P_logo.Attributes["src"] = item["cover"].ToString();
                        //title_finder.InnerText = item["title"].ToString().Length > 50 ? item["title"].ToString().Substring(0, 48) + "..." : item["title"].ToString();
                        //description_finder.InnerText = item["subtitle"].ToString().Length > 50 ? item["subtitle"].ToString().Substring(0, 48) + "..." : item["subtitle"].ToString();

                        //Основные поля
                        //S_title.Value = item["title"].ToString();
                        //S_description.Value = item["subtitle"].ToString();
                        //S_image.Value = item["cover"].ToString();
                    }
                }
                else
                {
                    //create.Visible = true;
                    //save.Visible = false;
                    //TitleType.InnerText = "Новая карта";
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                Session["role"] = 0;
                Response.Redirect("~/Permission.aspx");
            }
        }
        else Response.Redirect("~/Login.aspx");
    }

    /// <summary>
    /// Page Refresh
    /// </summary>
    private void _pageRefresh()
    {
        //create.Attributes.Add("disabled", "disabled");

        DataSet dataSet = Core._getAllMap();
        
        P_maplist.Controls.Clear();

        if (dataSet.Tables.Count > 0)
        {
            foreach (DataRow item in dataSet.Tables[0].Rows)
            {
                Controls_listitem l_item = (Controls_listitem)Page.LoadControl("../Controls/listitem.ascx");
                l_item.Link = "/pages/maps.aspx#";
                l_item.Head = item["title"].ToString();
                l_item.Decription = item["subtitle"].ToString() == string.Empty ? "Без описания" : item["subtitle"].ToString();
                l_item.IDent = Convert.ToInt32(item["id"].ToString());
                l_item.Cover = item["cover"].ToString();
                P_maplist.Controls.Add(l_item);
            }
        }
        else
        {
            P_maplist.InnerText = "Список карт пуст!";
        }
    }

    /// <summary>
    /// Create Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void create_Click(object sender, EventArgs e)
    {
        Core.Map map = new Core.Map();
        //map.Url = address.Value;
        //map.Name = S_title.Value;
        //map.description = S_description.Value;
        //map.img = S_image.Value;
        
        Core.CreateMap(map, Core.WriteType.create);

        //address.Value = string.Empty;
        //S_title.Value = string.Empty;
        //S_description.Value = string.Empty;
        //S_image.Value = string.Empty;

        _pageRefresh();
    }

    /// <summary>
    /// Save Click
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void save_Click(object sender, EventArgs e)
    {
        Core.Map map = new Core.Map();
        map.id = Convert.ToInt32(Request.Params["edit"]);
        //map.Url = address.Value;
        //map.Name = S_title.Value;
        //map.description = S_description.Value;
        //map.img = S_image.Value;
        Core.CreateMap(map, Core.WriteType.edit);
    }
}
#endregion