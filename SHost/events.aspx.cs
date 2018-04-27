using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SHost_events : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        _getAllEvents();
    }

    private void _getAllEvents()
    {
        DataSet events = Core._getEvents();
        content_events.InnerHtml = string.Empty;

        foreach (DataRow item in events.Tables[0].Rows)
        {
            content_events.InnerHtml += @"<tr>
                                        <td>" + item["id"].ToString() + @"</td>
                                        <td>" + item["task_id"].ToString() + @"</td>
                                        <td>" + item["STATE_T"].ToString() + @"</td>
                                        <td>" + item["start"].ToString() + @"</td>
                                        <td>" + item["agents"].ToString() + @"</td>
                                        <td>" + item["end"].ToString() + @"</td>
                                        </tr>";
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        _getAllEvents();
    }
}