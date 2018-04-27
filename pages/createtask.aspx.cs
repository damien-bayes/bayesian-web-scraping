using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_createTask : System.Web.UI.Page
{
    private Guid guid = new Guid("ef768f8e-777c-4238-a24d-9b125bda6d0a");
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
                P_day.Value = DateTime.Now.ToString("dd.MM.yyyy");
                P_hour.Value = DateTime.Now.ToString("HH");
                P_minute.Value = DateTime.Now.ToString("mm");

                P_day_div.Attributes["data-date"] = DateTime.Now.ToString("yyyy-MM-dd");

                DataSet maps = Core._getAllMap();
                DataSet areas = Core._getAllAreasForTasks();

                foreach (DataRow item in maps.Tables[0].Rows)
                {
                    ListItem ls = new ListItem();
                    ls.Value = item["id"].ToString();
                    ls.Text = item["title"].ToString();
                    P_maps.Items.Add(ls);
                }

                foreach (DataRow item in areas.Tables[0].Rows)
                {
                    ListItem ls = new ListItem();
                    ls.Value = item["id"].ToString();
                    ls.Text = item["title"].ToString();
                    P_areas.Items.Add(ls);
                }

                for (int i = 1; i < 31; i++)
                {
                    ListItem ls = new ListItem();
                    ls.Value = i.ToString();
                    ls.Text = i.ToString();
                    days.Items.Add(ls);
                }
            }

            _getAllTasks();

            if (Request.Params["id"] != null)
            {

            }
        }
    }

    private void _getAllTasks()
    {
        DataSet ds = Core._getAllTasks();
        tasks.InnerHtml = "";
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            string state = "<span data-element = '" + item["id"].ToString() + "' class = 'icon-remove place-right removetask' style = 'color: red; cursor: pointer;'></span>";

            if (Convert.ToInt32(item["state"]) == 6)
                state += "<span val = '" + item["id"].ToString() + "' class = 'icon-play-2 place-right' style = 'color: green; cursor: pointer;'>&nbsp;&nbsp;</span>";
            else
                state += "<span val = '" + item["id"].ToString() + "' class = 'icon-pause-2 place-right' style = 'color: grey; cursor: pointer;'>&nbsp;&nbsp;</span>";

            string day = string.Empty;
            switch (item["interval"].ToString())
            {
                case "0":
                    {
                        day = "Каждый день";
                        break;
                    }
                case "7":
                    {

                        day = "Каждую неделю (" + _getDayName(Convert.ToInt32(item["day"])) + ")";
                        break;
                    }
                case "30":
                    {

                        day = "Каждое " + item["day"].ToString() + " число месяца";
                        break;
                    }
                case "1":
                    {

                        day = "Выполнить один раз";
                        break;
                    }
            }

            if (Convert.ToBoolean(item["removeresults"]) == true)
                day += "  *";

            Controls_task_item tsk = (Controls_task_item)Page.LoadControl("../Controls/task_item.ascx");
            tsk.style_ = (Convert.ToInt32(item["state"]) == 6 ? "border-color: grey !important;" : "");
            tsk.id_item = item["id"].ToString();
            tsk.countAll = Core._getEventsByTaskId(Convert.ToInt32(item["id"])).Tables[0].Rows.Count.ToString();
            tsk.state = state;
            tsk.title = item["title"].ToString();
            tsk.day = day;
            tsk.time = item["time"].ToString();
            tasks.Controls.Add(tsk);
        }
    }

    protected void P_maps_SelectedIndexChanged(object sender, EventArgs e)
    {
        P_areas.Items.Clear();

        DataSet ds = Core._getAllAreasForTasks(Convert.ToInt32((sender as DropDownList).SelectedValue));
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            ListItem ls = new ListItem();
            ls.Value = item["id"].ToString();
            ls.Text = item["title"].ToString();
            P_areas.Items.Add(ls);
        }
    }

    protected void create_Click(object sender, EventArgs e)
    {
        Core.TaskItem ts = new Core.TaskItem();

        ts.time = P_hour.Value + ":" + P_minute.Value + ":00";
        ts.state = 3;
        ts.area_id = Convert.ToInt32(P_areas.SelectedValue);
        ts.removeresult = removeresult.Checked;

        switch (interval.SelectedValue)
        {
            case "1":
                {
                    ts.interval = 0;
                    ts.day = -1;
                    break;
                }
            case "7":
                {
                    ts.interval = 7;
                    ts.day = Convert.ToInt32(dayofweek.SelectedValue);
                    break;
                }
            case "30":
                {
                    ts.interval = 30;
                    ts.day = Convert.ToInt32(days.SelectedValue);
                    break;
                }
            case "0":
                {
                    ts.interval = 1;
                    ts.day = 0;
                    break;
                }
        }


        Core.CreateTask(ts);
        Response.Redirect("~/pages/createtask.aspx");
    }

    //Выбираем период
    protected void interval_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ds = sender as DropDownList;
        
        switch (ds.SelectedValue)
        {
            case "1":
                {
                    namepr.InnerText = "";
                    panel2.Style["display"] = "none";
                    panel3.Style["display"] = "none";
                    panel1.Style["display"] = "none";
                    break;
                }
            case "7":
                {
                    namepr.InnerText = "День недели";
                    panel2.Style["display"] = "block";
                    panel3.Style["display"] = "none";
                    panel1.Style["display"] = "none";
                    break;
                }
            case "30":
                {
                    namepr.InnerText = "Выбрать день";
                    panel3.Style["display"] = "block";
                    panel2.Style["display"] = "none";
                    panel1.Style["display"] = "none";
                    break;
                }
            case "0":
                {
                    namepr.InnerText = "Выбрать дату";
                    panel3.Style["display"] = "none";
                    panel2.Style["display"] = "none";
                    panel1.Style["display"] = "block";
                    break;
                }
        }
    }

    protected void now_Click(object sender, EventArgs e)
    {
        Core.TaskItem ts = new Core.TaskItem();

        string Tnow = DateTime.Now.ToString("HH:mm:ss");

        ts.time = Tnow;
        ts.state = 1;
        ts.area_id = Convert.ToInt32(P_areas.SelectedValue);
        ts.interval = 0;
        ts.day = -1;


        Core.CreateTask(ts);
        Response.Redirect("~/pages/createtask.aspx");
    }

    private string _getDayName(int day)
    {
        string result = string.Empty;

        switch(day)
        {
            case 1:
                {
                    result = "ПН";
                    break;
                }
            case 2:
                {
                    result = "ВТ";
                    break;
                }
            case 3:
                {
                    result = "СР";
                    break;
                }
            case 4:
                {
                    result = "ЧТ";
                    break;
                }
            case 5:
                {
                    result = "ПТ";
                    break;
                }
            case 6:
                {
                    result = "СБ";
                    break;
                }
            case 7:
                {
                    result = "ВС";
                    break;
                }
        }

        return result;
    }
}