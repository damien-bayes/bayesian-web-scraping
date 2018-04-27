using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_parsing_progress : System.Web.UI.UserControl
{
    public int progress { 
        get { return _progress; }
        set { _progress = value; }
    }
    private int _progress = 0;

    public string area_name
    {
        get { return _area_name; }
        set { _area_name = value; }
    }
    private string _area_name = string.Empty;

    public string url_now
    {
        get { return _url_now; }
        set { _url_now = value; }
    }
    private string _url_now = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}