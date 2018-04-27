using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ContentPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url_s = string.Empty;
        if (Request.Params["id"] != null)
        {
            url_s = Request.Params["id"].ToString();
        }

        bool a_r = true;
        if (Request.Params["replace"] != null)
        {
            a_r = false;
        }

        if (url_s != string.Empty)
        {

            byte[] content_byte = Core.GetContent(url_s, true);

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            char[] chars = new char[content_byte.Length / sizeof(char)];
            System.Buffer.BlockCopy(content_byte, 0, chars, 0, content_byte.Length);
            string content = new string(chars);
            content += "<script type=\"text/javascript\" src=\"http://10.10.9.132:777/js/jquery-1.10.2.js\" ></script>" +
                        "<script type=\"text/javascript\" src=\"http://10.10.9.132:777/js/bootstrap.js\" ></script>" +
                        "<script type=\"text/javascript\" src=\"http://10.10.9.132:777/js/contentpage.js\" ></script>";

            content += "<div id=\"informer-top\" style=\"border-top: 1px dashed #333333; position:absolute; width:0px; height:1px; top:0px; left:0px;\"></div>" +
                        "<div id=\"informer-bottom\" style=\"border-top: 1px dashed #333333; position:absolute; width:0px; height:1px; top:0px; left:0px;\"></div>" +

                        "<div id=\"informer-left\" style=\"border-left: 1px dashed #333333; position:absolute; width:1px; height:0px; top:0px; left:0px;\"></div>" +
                        "<div id=\"informer-right\" style=\"border-right: 1px dashed #333333; position:absolute; width:1px; height:0px; top:0px; left:0px;\"></div>" +
                        "<div id=\"tagName\" class=\"tooltip exr\" style=\"position:absolute; z-index:9999999; visibility:hidden;\"><div class=\"tooltip-arrow\"></div>" +
                            "<div class=\"tooltip-inner exr\" style=\"background-color: #2C3742; padding:5px; border-radius: 3px; box-shadow: 0 0 20px rgba(0,0,0,0.5); line-height:18px;\">" +
                                "<span style=\"border-width: 1px; border-color: #666666; margin-right:5px; display: none; border-right-style: solid; padding-right: 3px;\" id=\"restart\" class=\"exr\"><img id=\"resetimage\" class=\"exr\" src=\"../images/none_active_arrow_out.png\" style=\"float:left;\" /></span>" +
                                "<span id=\"tagnameElement\" style=\"color: white\" class=\"exr\">div</span>" +
                                "<span id=\"tagnameElementId\" style=\"color: #ED4DFF;\" class=\"exr\">pagenator</span>" +
                                "<span id=\"tagnameElementClass\" style=\"color: #4CC3FF; margin-left:2px;\" class=\"exr\">pagenator</span></div>" +
                            "<img src=\"http://10.10.9.132:777/images/dark_blue_arrow_example1.png\" style=\"position:relative; top:-5px; left: 10px;\"  class=\"exr\"/>" +
                        "</div>";

            Response.Write(content);
        }
    }
}