using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_calculate_hi : System.Web.UI.Page
{
    private Guid guid = new Guid("ef768f8e-777c-4238-a24d-9b125bda6d0a");
    public string count_checkbox = "0";
    public bool yes_event = true;
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
            if (!IsPostBack)
            {
                DataSet areas = Core._getAllAreasForTasksNoFilter();
                int index = 1;
                foreach (DataRow item in areas.Tables[0].Rows)
                {
                    int countDocus = Core._getCountDocsForAreas(Convert.ToInt32(item["id"]));
                    ListItem ls = new ListItem();
                    ls.Value = item["id"].ToString();
                    ls.Text = (item["title"].ToString().Length > 34 ? item["title"].ToString().Substring(0, 34) + "..." : item["title"].ToString());
                    
                    P_areas.Items.Add(ls);

                    if (index != 1)
                    {
                        ListItem ls_check = new ListItem();
                        ls_check.Value = item["id"].ToString();
                        ls_check.Text = (item["title"].ToString().Length > 34 ? item["title"].ToString().Substring(0, 34) + "..." : item["title"].ToString()) + (countDocus != 0 ? " <span style = \"color: green\">[ " + countDocus + " ]</span>" : "");
                        CheckBoxList1.Items.Add(ls_check);
                    }
                    else
                    {
                        int countClass1 = Core._getCountDocsForAreas(Convert.ToInt32(item["id"]));
                        countclass1.InnerText = "Выбранно документов: " + countClass1;
                    }

                    index++;
                }
            }
        }
        count_checkbox = CheckBoxList1.Items.Count.ToString();

        DataSet ds = Core.GetAllTaskHI();
        string
            trs = string.Empty;

        foreach (DataRow item in ds.Tables[0].Rows)
        {
            trs += "<tr class=\"odd\"><td class=\"sorting_1\">" + item["id"].ToString() + "</td><td class=\" \">" + item["title"].ToString() + "</td><td class=\" \">" + item["url"].ToString() + "</td><td style = \"white-space: nowrap\"><span area = \"" + item["id"] + "\"  onclick = \"show_hi(this)\" style=\"cursor:pointer; color: #16499a\" class = \"icon-list\" ></span>&nbsp;&nbsp;<span style = \"color:#eeeeee\">|</span>&nbsp;&nbsp<span area = \"" + item["id"] + "\"  onclick = \"show_cloud(this)\" style=\"cursor:pointer; color: #16499a\" class = \"icon-cloud\" ></span></td></tr>";
        }

        his.InnerHtml = trs;
    }
    protected void P_areas_SelectedIndexChanged(object sender, EventArgs e)
    {
        CheckBoxList1.Items.Clear();
        DataSet areas = Core._getAllAreasForTasksNoFilter();
        foreach (DataRow item in areas.Tables[0].Rows)
        {
            if (P_areas.SelectedValue != item["id"].ToString())
            {
                int countDocus = Core._getCountDocsForAreas(Convert.ToInt32(item["id"]));
                ListItem ls = new ListItem();
                ls.Value = item["id"].ToString();
                ls.Text = (item["title"].ToString().Length > 34 ? item["title"].ToString().Substring(0, 34) + "..." : item["title"].ToString()) + (countDocus != 0 ? " <span style = \"color: green\">[ " + countDocus + " ]</span>" : "");

                CheckBoxList1.Items.Add(ls);
            }
            else
            {
                int countClass1 = Core._getCountDocsForAreas(Convert.ToInt32(item["id"]));
                countclass1.InnerText = "Выбранно документов: " + countClass1;
            }
        }
        count_checkbox = CheckBoxList1.Items.Count.ToString();
    }
    protected void now_Click(object sender, EventArgs e)
    {
        if (yes_event == true)
        {
            int area = Convert.ToInt32(P_areas.SelectedValue);
            List<ListItem> selected = CheckBoxList1.Items.Cast<ListItem>()
                                    .Where(li => li.Selected)
                                    .ToList();
            int[] areas = new int[selected.Count];
            for (int i = 0; i < selected.Count; i++)
            {
                areas[i] = Convert.ToInt32(selected[i].Value);
            }

            Core._calculateXi(area, areas, 0);
            yes_event = false;
            now.Enabled = false;

            Response.Redirect("calculate_hi.aspx");
        }
    }
    protected void TimerAjax_Tick(object sender, EventArgs e)
    {
        DataSet ds = Core.GetAllTaskHI();
        string
            trs = string.Empty;

        foreach (DataRow item in ds.Tables[0].Rows)
        {
            trs += "<tr class=\"odd result_hi\"><td class=\"sorting_1\">" + item["id"].ToString() + "</td><td class=\" \">" + item["title"].ToString() + "</td><td class=\" \">" + item["url"].ToString() + "</td><td style = \"white-space: nowrap\"><span area = \"" + item["id"] + "\"  onclick = \"show_hi(this)\" style=\"cursor:pointer; color:  #16499a\" class = \"icon-list\" ></span>&nbsp;&nbsp;<span style = \"color:#eeeeee\">|</span>&nbsp;&nbsp<span area = \"" + item["id"] + "\"  onclick = \"show_cloud(this)\" style=\"cursor:pointer; color: #16499a\" class = \"icon-cloud\" ></span></td></tr>";
        }

        his.InnerHtml = trs;
    }
}