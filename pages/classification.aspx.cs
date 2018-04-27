using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

public partial class pages_classification : System.Web.UI.Page
{
    private Guid guid = new Guid("ef768f8e-777c-4238-a24d-9b125bda6d0b");
    public string count_checkbox1 = "0";
    public string count_checkbox2 = "0";
    public string buf_folder = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Login.aspx");
        if (Core._getPageAccess(Convert.ToInt32(Session["role"]), guid) == false)
        {
            grid.InnerHtml = "У Вас нет прав для просмотра данной страницы";
        }

        if (!IsPostBack)
        {
            DataSet areas = Core._getAllAreasForTasks();
            foreach (DataRow item in areas.Tables[0].Rows)
            {
                int countDocus = Core._getCountDocsForAreas(Convert.ToInt32(item["id"]));
                ListItem ls = new ListItem();
                ls.Value = item["id"].ToString();
                ls.Text = (item["title"].ToString().Length > 34 ? item["title"].ToString().Substring(0, 34) + "..." : item["title"].ToString()) + (countDocus != 0 ? " <span style = \"color: green\">[ " + countDocus + " ]</span>" : "");

                CheckBoxList1.Items.Add(ls);

                ListItem ls1 = new ListItem();
                ls1.Value = item["id"].ToString();
                ls1.Text = (item["title"].ToString().Length > 34 ? item["title"].ToString().Substring(0, 34) + "..." : item["title"].ToString()) + (countDocus != 0 ? " <span style = \"color: green\">[ " + countDocus + " ]</span>" : "");

                CheckBoxList2.Items.Add(ls1);

                DataSet topics = Core._getAllTopics();
                topic_s.Items.Clear();
                foreach (DataRow item_topics in topics.Tables[0].Rows)
                {
                    ListItem ls_topic = new ListItem();
                    if (Convert.ToInt32(item_topics["id"]) != 8)
                    {
                        ls_topic.Enabled = false;
                    }
                    ls_topic.Value = item_topics["id"].ToString();
                    ls_topic.Text = item_topics["topic"].ToString();
                    topic_s.Items.Add(ls_topic);
                }

            }
        }
        count_checkbox1 = CheckBoxList1.Items.Count.ToString();
        count_checkbox2 = CheckBoxList2.Items.Count.ToString();

        

        Chart1.Visible = false;

        Process[] pr = Process.GetProcessesByName("Classification");
        if (pr.Length != 0)
        {
            warning.Style["visibility"] = "visible";
            warning.Style["height"] = "auto";
        }


        getRangingResultsEx();
        statistic.Visible = true;
        TimerAjax.Enabled = false;
    }
    protected void now_Click(object sender, EventArgs e)
    {

        List<int> vector1 = new List<int>();

        List<ListItem> selected = CheckBoxList1.Items.Cast<ListItem>()
                                .Where(li => li.Selected)
                                .ToList();
        for (int i = 0; i < selected.Count; i++)
        {
            vector1.Add(Convert.ToInt32(selected[i].Value));
        }

        List<int> vector2 = new List<int>();

        selected = CheckBoxList2.Items.Cast<ListItem>()
                                .Where(li => li.Selected)
                                .ToList();
        for (int i = 0; i < selected.Count; i++)
        {
            vector2.Add(Convert.ToInt32(selected[i].Value));
        }

        int topic = Convert.ToInt32(topic_s.SelectedValue);

        Core._classification(vector1, vector2, topic);

    }
    protected void TimerAjax_Tick(object sender, EventArgs e)
    {
        //getRangingResults();
        getRangingResultsEx();
    }
    private void getRangingResultsEx()
    {
        DataSet analitic = Core._getAnalyticRaging(0);

        List<string> names = new List<string>();
        List<double> values = new List<double>();
        names.Add("PRECISION");
        names.Add("RECALL");
        names.Add("FSCORE");

        foreach (DataRow item in analitic.Tables[0].Rows)
        {
            values.Add(Convert.ToDouble(item["precision"]));
            values.Add(Convert.ToDouble(item["recall"]));
            values.Add(Convert.ToDouble(item["fscore"]));
            break;
        }

        Chart1.Series["Default"].Points.DataBindXY(names, values);
        Chart1.Series["Default"].Points[0].Color = Color.RoyalBlue;
        Chart1.Series["Default"].Points[1].Color = Color.OrangeRed;
        Chart1.Series["Default"].Points[2].Color = Color.DarkMagenta;
        Chart1.Series["Default"].ChartType = SeriesChartType.Column;
        Chart1.Series["Default"]["PieLabelStyle"] = "Disabled";
        Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
        for (int i = 0; i < values.Count; i++)
        {
            Chart1.Series["Default"].Points[i].Label = values[i].ToString();
        }
        Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.Gray;
        Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle = new LabelStyle() { Font = new Font("Verdana", 7.5f) };
        Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.Gray;
        Chart1.ChartAreas["ChartArea1"].AxisY.LabelStyle = new LabelStyle() { Font = new Font("Verdana", 7.5f) };

        Chart1.ChartAreas["ChartArea1"].AxisX2.MajorGrid.LineColor = Color.Gray;
        Chart1.ChartAreas["ChartArea1"].AxisY2.MajorGrid.LineColor = Color.Gray;
        Chart1.Legends[0].Enabled = false;

        Chart1.Visible = true;

        DataSet ds = Core._getResultRaging(8);
        his.Controls.Clear();
        foreach (DataRow item in ds.Tables[0].Rows)
        {
            string title = item["title"].ToString() == string.Empty ? item["url"].ToString() : item["title"].ToString();
            string url = title.Length > 70 ? title.Substring(0, 70) + "..." : title;
            string tr_string = "<tr class=\"odd\"><td class=\" \"><a style = \"white-space: nowrap\" target = \"_blank\" href = \"" + item["url"].ToString() + "\">" + url + "</a></td><td>" + item["topic"].ToString() + "</td><td>" + item["prob"].ToString() + "</td></tr>";
            his.InnerHtml += tr_string;
        }
    }
}

public class grafic_classification
{
    public double SVM_PRECISION { get; set; }
    public double SVM_RECALL { get; set; }
    public double SVM_FSCORE { get; set; }
}