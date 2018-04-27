using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

[ScriptService]
public partial class Bin_Ajax_Awesomium : System.Web.UI.Page
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

            byte[] content_byte = Core.client.GetContent(url_s, Convert.ToInt32(Request.Params["mapid"]), a_r);

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            char[] chars = new char[content_byte.Length / sizeof(char)];
            System.Buffer.BlockCopy(content_byte, 0, chars, 0, content_byte.Length);
            string content = new string(chars);
            Response.Write(content);
        }
    }


    [WebMethod()]
    public static void UploadImage(string imageData)
    {
        FileStream fs = new FileStream("E:\\image.png", FileMode.Create);
        BinaryWriter bw = new BinaryWriter(fs);

        byte[] data = Convert.FromBase64String(imageData);

        bw.Write(data);
        bw.Close();
    }
}