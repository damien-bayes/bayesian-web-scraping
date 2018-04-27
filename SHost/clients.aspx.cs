using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SHost_clients : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");

        _getallClients();
    }

    private void _getallClients()
    {
        tbodyclients.InnerHtml = string.Empty;
        int index = 1;

        Dictionary<string, int> infoClientTasks = Core._getCountTasksForClients();

        foreach (KeyValuePair<SHost.ClientStruct, bool> item in Core._getClients())
        {
            int count = 0;
            if (infoClientTasks.ContainsKey(item.Key.name))
                count = infoClientTasks[item.Key.name];

            string devicetype = "icon-monitor-2", deviceOS = "icon-windows";
            switch(item.Key.type)
            {
                case SHost.DeviceType.PC:
                    {
                        break;
                    }
                case SHost.DeviceType.Android:
                    {
                        deviceOS = "icon-android";
                        devicetype = "icon-phone-2";
                        break;
                    }
                case SHost.DeviceType.WindowsPhone:
                    {
                        devicetype = "icon-phone-2";
                        break;
                    }
                case SHost.DeviceType.IOS:
                    {
                        deviceOS = "iconapple";
                        devicetype = "icon-phone-2";
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            tbodyclients.InnerHtml += @"<tr style = 'color:" + (item.Key.internet == false ? "grey" : "black") + @"'>
                                         <td><span style = 'font-size:14pt;' class = '" + devicetype + @"' ></span></td>
                                         <td><span style = 'font-size:14pt;' class = '" + deviceOS + @"' ></span></td>
                                         <td><span id='item_" + index + "' onclick = 'javascript:runscript(this);'>" + item.Key.name + "&nbsp;-&nbsp;" + count + @"</span></td>
                                         <td>" + item.Key.processorcount + @" шт.</td>
                                         <td>" + item.Key.memorysize + @" Mb</td>
                                         <td style = 'text-align:center;'>" + (item.Key.internet == true ? "<span style = 'color: green; font-size:14pt;' class = 'icon-globe' ></span>" : "<span style = 'color: red; font-size:14pt;' class = 'icon-globe' ></span>") + @"</td>
                                         <td>" + item.Key.IEVersion + @"</td>
                                         <td>" + (item.Key.VideoController.Length > 0 ? item.Key.VideoController[0] : "Неопределенно") + @"</td>  
                                         <td>" + item.Key.ProcessorLoadPercentage + @"</td>                                              
                                         <td>" + (item.Value == true ? "Свободен" : "<strong>Занят</strong>") + @"</td>
                                        </tr>";
            index++;
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        _getallClients();
    }
}