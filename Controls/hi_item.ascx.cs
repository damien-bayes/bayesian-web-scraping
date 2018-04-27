#region directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region controls_hi_item script
public partial class Controls_hi_item : System.Web.UI.UserControl
{
    #region global variables
    TimeSpan _timeSpan;
    string _areaName;
    int _areaIdentity = 0;
    int _state = 5;

    public string _timeOutput = "00:00:00", _statusOutput = string.Empty;
    #endregion

    #region properties
    public TimeSpan IsTimeSpan
    {
        get { return _timeSpan; }
        set { _timeSpan = value; }
    }

    public string AreaName
    {
        get { return _areaName; }
        set { _areaName = value; }
    }

    public int AreaIdentity
    {
        get { return _areaIdentity; }
        set { _areaIdentity = value; }
    }

    public int State
    {
        get { return _state; }
        set { _state = value; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        _timeOutput = (IsTimeSpan.Hours < 10 ? 
            string.Format("0{0}", IsTimeSpan.Hours) : 
            IsTimeSpan.Hours.ToString()) + ":" + (IsTimeSpan.Minutes < 10 ? 
            string.Format("0{0}", IsTimeSpan.Minutes) : 
            IsTimeSpan.Minutes.ToString()) + ":" + (IsTimeSpan.Seconds < 10 ? 
            string.Format("0{0}", IsTimeSpan.Seconds) : 
            IsTimeSpan.Seconds.ToString());

        switch(State)
        {
            default:
                {
                    _statusOutput = "Завершено";
                    break;
                }
            case 3:
                {
                    _statusOutput = "Вычисление по заданному алгоритму";
                    break;
                }
            case 5:
                {
                    _statusOutput = "Завершено";
                    break;
                }
            case 7:
                {
                    _statusOutput = "Запись результата в базу данных";
                    break;
                }
        }
    }
}
#endregion