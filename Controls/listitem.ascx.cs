using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_listitem : System.Web.UI.UserControl
{
    private int Ident = -1;
    public int IDent
    {
        get { return Ident; }
        set { Ident = value; }
    }

    private string link = string.Empty;
    public string Link
    {
        get { return link; }
        set { link = value; }
    }

    private string head = string.Empty;
    public string Head
    {
        get { return head; }
        set { head = value; }
    }

    private string decription = string.Empty;
    public string Decription
    {
        get { return decription; }
        set { decription = value; }
    }

    private string topic = string.Empty;
    public string Topic
    {
        get { return topic; }
        set { topic = value; }
    }

    public string ColorBorder = "border-left: 3px #0067b0 solid !important;";
    private int countdocs = 0;
    public int CountDocs
    {
        get { return countdocs; }
        set { countdocs = value; }
    }

    private string cover = "/images/images.jpg";
    public string Cover
    {
        get { return cover; }
        set { cover = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        P_item.HRef = link;
        P_head.InnerHtml = head + (topic != string.Empty ? "&nbsp;-&nbsp;<small style = \"color: #999;\">" + topic + "&nbsp;&nbsp;" + (countdocs != 0 ? "<span style = \"color: green\">[ Документов - " + countdocs + " ]</span>" : "") + "</small>" : string.Empty);
        P_decription.InnerText = decription;

        delete.Attributes.Add("data-element", Ident.ToString());
        edit.Attributes.Add("data-element", Ident.ToString());

        if (countdocs != 0)
        {
            ColorBorder = "border-left: 3px green solid !important;";
        }
    }
}