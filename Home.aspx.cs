using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Home : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        ((MasterPage)this.Master).HeaderTitle = "Панель управления";
        ((MasterPage)this.Master).HeaderDescription = "Создание шаблонов, указание маршрутов и целей поиска, установление расписания для выполнения задач и многое другое.";

        DataSet meta = Core._getMetaBlock(1);

        DataSet maps = Core._getAllMap();
        DataSet areas_ = Core._getAllAreas();
        int tasks = Core._getTasksToDay();

        //P_tasks.InnerText = tasks.ToString();
        //P_countmaps.InnerText = maps.Tables[0].Rows.Count.ToString();
        //areas.InnerText = areas_.Tables[0].Rows.Count.ToString();

        _getSystemInfo();
        
    }

    protected void TimerAjax_Tick(object sender, EventArgs e)
    {
        _getSystemInfo();
    }

    private void _getSystemInfo()
    {
        List<Core.EventsParsing> events_p = Core._getEventsParsing();
        
        tasks_panel.Controls.Clear();
        
        foreach (Core.EventsParsing item in events_p)
        {
            if (item.eventtype == 1)
            {
                if (item.countNow < item.countAsMax)
                {
                    Controls_parsing_progress pr = (Controls_parsing_progress)Page.LoadControl("~/Controls/parsing_progress.ascx");
                    pr.progress = item.progres;
                    pr.area_name = item.areaname;
                    pr.url_now = item.urlnow.Length > 50 ? item.urlnow.Substring(0, 50) + "..." : item.urlnow;
                    tasks_panel.Controls.Add(pr);
                }
                else
                {
                    DateTime start = item.timestart;
                    
                    DateTime end = item.timeend;
                    if ((end.Hour - start.Hour) < 0)
                        end = end.AddHours(12);
                    TimeSpan t = end - start;

                    Controls_parsing_item pr = (Controls_parsing_item)Page.LoadControl("~/Controls/parsing_item.ascx");
                    pr.area_id = item.area_id;
                    pr.area_name = item.areaname;
                    pr.t = t;
                    
                    pr.state_pr = item.type;
                    tasks_panel.Controls.Add(pr);
                }
            }
            else if (item.eventtype == 2)
            {
                DateTime start = item.timestart;
                DateTime end = item.timeend;
                TimeSpan t = end - start;

                Controls_hi_item pr = (Controls_hi_item)Page.LoadControl("~/Controls/hi_item.ascx");
                pr.AreaIdentity = item.area_id;
                pr.AreaName = item.areaname;
                pr.IsTimeSpan = t;
                pr.State = item.type;
                tasks_panel.Controls.Add(pr);
            }
            else if (item.eventtype == 3)
            {
                DateTime start = item.timestart;
                DateTime end = item.timeend;
                TimeSpan t = end - start;

                Controls_classification_item pr = (Controls_classification_item)Page.LoadControl("~/Controls/classification_item.ascx");
                pr.area_id = item.area_id;
                pr.area_name = item.areaname;
                pr.t = t;
                pr.state = item.type;
                tasks_panel.Controls.Add(pr);
            }
        }
    }
}