using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_classification_item : System.Web.UI.UserControl
{
    public TimeSpan t
    {
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

    public int state
    {
        get { return _state; }
        set { _state = value; }
    }
    private int _state = 5;

    public string _time = "00:00:00";
    public string _state_ = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        _time = (t.Hours < 10 ? string.Format("0{0}", t.Hours) : t.Hours.ToString()) + ":" + (t.Minutes < 10 ? string.Format("0{0}", t.Minutes) : t.Minutes.ToString()) + ":" + (t.Seconds < 10 ? string.Format("0{0}", t.Seconds) : t.Seconds.ToString());
        switch(_state)
        {
            default:
                {
                    _state_ = "Завершено";
                    break;
                }
            case 1008:
                {
                    _state_ = "Завершено с ошибкой";
                    break;
                }
            case 5:
                {
                    _state_ = "Завершено";
                    break;
                }
            case 8:
                {
                    _state_ = "Вычисления на R";
                    break;
                }
            case 3:
                {
                    _state_ = "Начало выполнения операции";
                    break;
                }
            case 2008:
                {
                    _state_ = "Вычисления вероятности";
                    break;
                }
        }
    }
}