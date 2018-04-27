using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_dropdown : System.Web.UI.UserControl
{
    private List<item_dropdown> list_ = new List<item_dropdown>();
    public List<item_dropdown> List 
    { 
        get { return list_; }
        set { list_ = value; } 
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        /*bool selected = false;
            if (Request.Params["map"] != null)
            {
                selected = true;
                int val_id = 0;
                bool parce =  int.TryParse(Request.Params["map"], out val_id);
                if (parce)
                    _getAreas(val_id);
            }
            int index = 1;
            MMMM.InnerHtml = string.Empty;
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (selected == false)
                {
                    if (index == 1)
                    {
                        fontName.Value = item["title"].ToString();
                        int val_id = 0;
                        bool parce = int.TryParse(item["id"].ToString(), out val_id);
                        if (parce)
                            _getAreas(val_id);
                    }
                }
                else
                {
                    if (item["id"].ToString() == Request.Params["map"].ToString())
                        fontName.Value = item["title"].ToString();
                }
                MMMM.InnerHtml += "<div class=\"item_hover\" style=\"padding:5px; padding-left: 20px;\" attr-key = \"map\" attr-value=\"" + item["id"].ToString() + "\">" + item["title"].ToString() + "</div>";
                index++;
            }*/
    }

    public class item_dropdown
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}