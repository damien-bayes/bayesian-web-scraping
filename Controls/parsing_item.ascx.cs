using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_parsing_item : System.Web.UI.UserControl
{
    public TimeSpan t {
        get { return _t; }
        set { _t = value; } 
    }

    private TimeSpan _t;

    public string area_name
    {
        get { return _area_name; }
        set { _area_name = value; }
    }
    private string _area_name = string.Empty;

    public int area_id
    {
        get { return _area_id; }
        set { _area_id = value; }
    }
    private int _area_id = 0;

    public int state_pr
    {
        get { return _State; }
        set { _State = value; }
    }
    private int _State = 0;
    public string _state_ = "Парсинг завершен";

    public bool file
    {
        get { return _file; }
        set { _file = value; }
    }
    private bool _file = false;

    public string _time = "00:00:00";
    public string _file_ = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        _time = (t.Hours < 10 ? string.Format("0{0}", t.Hours) : t.Hours.ToString()) + ":" + (t.Minutes < 10 ? string.Format("0{0}", t.Minutes) : t.Minutes.ToString()) + ":" + (t.Seconds < 10 ? string.Format("0{0}", t.Seconds) : t.Seconds.ToString());
        _file_ = _file == true ? "<a target=\"_blank\" href=\"corpuses" + area_id + ".html\" class=\"place-right icon-eye-2\" style = \"font-size: 12pt; cursor: pointer; color: #2e92cf; margin-top: 3px;\"></a>" : string.Empty;
        if (_State == 2009)
            _state_ = "Задача сбора ссылок завершена";
    }
}