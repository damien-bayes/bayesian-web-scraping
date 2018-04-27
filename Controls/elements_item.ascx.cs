using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_elements_item : System.Web.UI.UserControl
{
    private string nameelement = "Html тег";
    public string nameElement
    {
        get { return nameelement; }
        set { nameelement = value; }
    }


    private string src_ = "../screenshots/CSS-Code-Snippets.jpg";
    public string Src_
    {
        get { return src_; }
        set { src_ = value; }
    }

    private string description = "Описание отсутвует";
    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    private string id_ = "Описание отсутвует";
    public string ID_
    {
        get { return id_; }
        set { id_ = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }
}