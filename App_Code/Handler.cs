using System;
using System.Web;
using HtmlAgilityPack;
using System.Text;
using System.Web.SessionState;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebsitesScreenshot;
using System.Xml;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using GlobalJetStrikeParsing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using ACAWebThumbLib;

public class Handler : IHttpHandler, IReadOnlySessionState
{
    /// <summary>
    /// ДЛЯ РАБОТЫ С AJAX ФУНКЦИЯМИ
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest(HttpContext context)
    {

        if (context.Request.Params["type"] != null)
        {
            switch(context.Request.Params["type"])
            {
                default:
                    break;
                
                case "info":
                {
                    #region ПОЛУЧИТЬ ИНФОРМАЦИЮ О САЙТЕ

                    context.Response.ContentType = "application/json";
                    HtmlWeb html = new HtmlWeb();
                    HtmlAgilityPack.HtmlDocument doc = html.Load(context.Request.Params["info"]);
                    try
                    {
                        int encode = doc.DeclaredEncoding.WindowsCodePage;
                        if (encode == 1251)
                        {
                            html.OverrideEncoding = Encoding.GetEncoding("windows-1251");
                            doc = html.Load(context.Request.Params["info"]);
                        }
                    }
                    catch { }
                    string title = "";
                    string logo = "";
                    string description = "";

                    string document = doc.DocumentNode.InnerHtml;
                    if (document.IndexOf("<title>") != -1)
                    {
                        int start = document.IndexOf("<title>");
                        title = document.Substring(start);
                        title = title.Substring(0, title.IndexOf("</")).Replace("<title>", "");
                    }

                    var images = doc.DocumentNode.SelectNodes("//img");
                    if (images != null)
                    {
                        foreach (HtmlNode item in images)
                        {
                            if (item.Attributes[@"src"].Value.ToLower().IndexOf("logo") != -1)
                            {
                                logo = item.Attributes[@"src"].Value;
                                break;
                            }
                        }
                    }

                    var meta = doc.DocumentNode.SelectNodes("//meta");
                    if (meta != null)
                    {
                        foreach (HtmlNode item in meta)
                        {
                            try
                            {
                                if (item.Attributes[@"name"].Value.ToLower() == "keywords")
                                {
                                    description = item.Attributes[@"content"].Value;
                                    break;
                                }
                            }
                            catch { }
                        }
                    }
                    context.Response.Write("{\"title\":\"" + title + "\", \"logo\" : \"" + logo + "\", \"desc\" : \"" + description + "\"}");
                    break;

                    #endregion
                }
                case "xpath":
                {
                    #region ПОЛУЧИТЬ XPATH ЭЛЕМЕНТА

                    string url = context.Request.Params["url"];
                        if (!string.IsNullOrEmpty(url))
                        {
                            context.Response.ContentType = "text/plain";
                            byte[] content_byte = Core.client.GetContent(url,2, true);
                            char[] chars = new char[content_byte.Length / sizeof(char)];
                            System.Buffer.BlockCopy(content_byte, 0, chars, 0, content_byte.Length);
                            string content = new string(chars);
                            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                            html.LoadHtml(content);
                            var aTags = html.DocumentNode.SelectSingleNode(context.Request.Params["xpath"]);
                            if (aTags != null)
                            {
                                context.Response.Write(aTags.InnerText);
                            }
                            else
                            {
                                context.Response.Write("Not find from xpath : " + context.Request.Params["xpath"]);
                            }
                        }
                        break;

                    #endregion
                }
                case "file":
                {
                    #region ФОТО ПОЛЬЗОВАТЕЛЯ

                    context.Response.ContentType = "text/plain";
                        try
                        {
                            if (context.Request.Files.Count > 0)
                            {

                                string path = context.Server.MapPath("~/images/UserImage/");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);

                                var file = context.Request.Files[0];
                                if (file.FileName.ToUpper().IndexOf("JPG") > 0)
                                {
                                    string filename = Path.Combine(path, file.FileName);
                                    file.SaveAs(filename);
                                    var ser = new System.Web.Script.Serialization.JavaScriptSerializer();
                                    var result_ = new { name = @"\images\UserImage\" + file.FileName };
                                    context.Response.Write(ser.Serialize(result_));
                                }
                                else
                                {
                                    context.Response.Write("");
                                }
                            }
                            else
                            {
                                context.Response.Write("{\"url\":\"no\"}");
                            }
                        }
                        catch(Exception err) { context.Response.Write("{\"url\":\"" + err + "\"}"); }

                        break;

                    #endregion
                }
                case "lemmas":
                {
                    #region ЛЕММАТИЗАЦИЯ

                    string url = context.Request.Params["url_"];
                        if (Core.client.State != System.ServiceModel.CommunicationState.Opened)
                            Core.client.Open();
                        context.Response.Write(Core.client.Lemmas(url, Core._getStopWords()));

                        GC.Collect(0, GCCollectionMode.Forced);
                        GC.WaitForPendingFinalizers();

                        break;

                    #endregion
                }
                case "screenshot":
                {
                    #region ИЗОБРАЖЕНИЕ ОБЛАСТИ САЙТА

                    string path = string.Empty;
                        context.Response.ContentType = "text/plain";

                        string constring = System.Configuration.ConfigurationManager.AppSettings["connectionSQL"];
                        SqlConnection con = new SqlConnection(constring);
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "SELECT CONTENT FROM BUILDS WHERE area_id = " + context.Request.Params["mapid"];
                        byte[] result = (byte[])(cmd.ExecuteScalar());
                        string content = string.Empty;
                        if (result.Length != 0)
                        {
                            char[] chars = new char[result.Length / sizeof(char)];
                            System.Buffer.BlockCopy(result, 0, chars, 0, result.Length);
                            content = new string(chars);
                            content = Regex.Replace(content, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", string.Empty);
                            content = Regex.Replace(content, @"<!--(.|\n)*?-->", string.Empty);
                            //JetStrikeParsing jstr = new JetStrikeParsing();

                            HtmlAgilityPack.HtmlDocument dc = new HtmlAgilityPack.HtmlDocument();
                            dc.LoadHtml(content);
                            var tags = dc.DocumentNode.SelectSingleNode(context.Request.Params["xpath"]);
                            try
                            {
                                if (tags != null)
                                {
                                    Guid name = Guid.NewGuid();

                                    string body = string.Empty;
                                    int bodyS = content.IndexOf("body");
                                    body = content.Substring(0, bodyS) + "body><div>" + tags.InnerHtml + "</div></body></html>";
                                    
                                    /*body = Regex.Replace(body, @"<meta(.|\n)*?/>", string.Empty);
                                    body = Regex.Replace(body, @"<\s*" + "link rel=\"(stylesheet)\".*?>", string.Empty);
                                    body = Regex.Replace(body, @"<!DOCTYPE(.|\n)*?>", string.Empty);*/

                                    /*string body = jstr.ReplaceBodyElement(content, tags.InnerHtml);*/

                                    try
                                    {

                                        /*using (StreamWriter writer = new StreamWriter(System.Configuration.ConfigurationManager.AppSettings["myscreenpath"] + name.ToString() + ".html"))
                                        {
                                            writer.Write(body);
                                        }

                                        var t = new Thread(() => Core.GetSnapshotUsingWatiN(
                                            name.ToString(), 
                                            System.Configuration.ConfigurationManager.AppSettings["siteaddress"] + "/screenshots/" + name.ToString() + ".html", 
                                            System.Configuration.ConfigurationManager.AppSettings["myscreenpath"]));
                                        t.SetApartmentState(ApartmentState.STA);
                                        t.Start();
                                        t.Join();*/

                                        Rectangle crop = new Rectangle(0, 0, 0, 0);
                                        var t = new Thread(() => Core.renderPagePart(body, System.Configuration.ConfigurationManager.AppSettings["myscreenpath"] + name.ToString() + ".png", new Rectangle(0, 0, 0, 0)));
                                        t.SetApartmentState(ApartmentState.STA);
                                        t.Start();
                                        t.Join();

                                        /*ACAWebThumbLib.ThumbMakerClass cl = new ThumbMakerClass();
                                        cl.SetHTMLSource(body);
                                        cl.StartSnap();
                                        cl.SaveImage(System.Configuration.ConfigurationManager.AppSettings["myscreenpath"] + name.ToString() + ".png");*/

                                        /*System.Drawing.Image img = HtmlRenderer.HtmlRender.RenderToImage(body, 600);
                                        img.Save(System.Configuration.ConfigurationManager.AppSettings["myscreenpath"] + name.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);*/
                                        
                                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["myscreenpath"] + name.ToString() + ".png"))
                                            path = name.ToString() + ".png";
                                        else
                                            path = "CSS-Code-Snippets.jpg";
                                    }
                                    catch
                                    {
                                        path = "CSS-Code-Snippets.jpg";
                                    }

                                    int saveinbase = Core.saveNewIntention(Convert.ToInt32(context.Request.Params["mapid"]), context.Request.Params["xpath"], path);
                                    context.Response.Write("" + saveinbase.ToString() + ":" + path);
                                }
                                else
                                {
                                    context.Response.Write("errorxpath");
                                    //context.Response.Write(content);
                                }
                            }
                            catch { context.Response.Write("errorxpath"); }
                        }
                        else
                        {
                            context.Response.Write("{\"url\":\"no\"}");
                        }
                        con.Close();
                        break;

                    #endregion
                }
                case "savehref":
                {
                    #region СОХРАНИТЬ АДДРЕСС САЙТА

                    context.Response.ContentType = "text/plain";
                        string href = context.Request.Params["hrf"];

                        if (href.Length != 0)
                        {
                            int saveinbase = Core.saveNewHref(Convert.ToInt32(context.Request.Params["mapid"]), href, "CSS-Code-Snippets.jpg");
                            context.Response.Write("" + saveinbase.ToString() + ":CSS-Code-Snippets.jpg");
                        }
                        else
                        {
                            context.Response.Write("{\"url\":\"no\"}");
                        }
                        break;

                    #endregion
                }
                case "pgntr":
                {
                    #region ПАДЖИНАТОР

                    context.Response.ContentType = "application/json";
                        string constring = System.Configuration.ConfigurationManager.AppSettings["connectionSQL"];

                        int step = 1;
                        string url = string.Empty;

                        SqlConnection con = new SqlConnection(constring);
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "SELECT CONTENT FROM BUILDS WHERE area_id = " + context.Request.Params["mapid"];
                        byte[] result = (byte[])(cmd.ExecuteScalar());
                        string content = string.Empty;
                        if (result.Length != 0)
                        {
                            char[] chars = new char[result.Length / sizeof(char)];
                            System.Buffer.BlockCopy(result, 0, chars, 0, result.Length);
                            content = new string(chars);

                            HtmlAgilityPack.HtmlDocument dc = new HtmlAgilityPack.HtmlDocument();
                            dc.LoadHtml(content);
                            try
                            {
                                HtmlNodeCollection tags = dc.DocumentNode.SelectNodes(context.Request.Params["xpath"]);
                                if (tags.Count > 0)
                                {
                                    List<string> hrefs = Core.DumpHRefs(tags[0].InnerHtml);

                                    if (hrefs.Count > 2)
                                    {
                                        step = Core._getStep(hrefs[1], hrefs[2], out url);
                                    }
                                    else if (hrefs.Count == 2)
                                    {
                                        step = Core._getStep(hrefs[0], hrefs[1], out url);
                                    }
                                }
                            }
                            catch { }
                        }
                        context.Response.Write("{\"step\":\"" + step + "\", \"url\":\"" + url + "\"}");
                        break;

                    #endregion
                }
                case "ajax":
                {
                    #region AJAX ПАДЖИНАТОР

                    context.Response.ContentType = "application/json";
                        string constring = System.Configuration.ConfigurationManager.AppSettings["connectionSQL"];

                        string text = string.Empty;
                        string element = string.Empty;

                        SqlConnection con = new SqlConnection(constring);
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "SELECT CONTENT FROM BUILDS WHERE area_id = " + context.Request.Params["mapid"];
                        byte[] result = (byte[])(cmd.ExecuteScalar());
                        string content = string.Empty;
                        if (result.Length != 0)
                        {
                            char[] chars = new char[result.Length / sizeof(char)];
                            System.Buffer.BlockCopy(result, 0, chars, 0, result.Length);
                            content = new string(chars);

                            HtmlAgilityPack.HtmlDocument dc = new HtmlAgilityPack.HtmlDocument();
                            dc.LoadHtml(content);
                            HtmlNodeCollection tags = dc.DocumentNode.SelectNodes(context.Request.Params["xpath"]);
                            try
                            {
                                if (tags.Count > 0)
                                {
                                    foreach (HtmlNode item in tags)
                                    {
                                        text += item.InnerText;
                                    }
                                    element = tags[0].OriginalName;
                                }
                            }
                            catch { }
                        }
                        context.Response.Write("{\"text\":\"" + text + "\", \"element\":\"" + element + "\"}");
                        break;

                    #endregion
                }
                case "reference":
                {
                    #region ЗАБЫЛ ))

                    context.Response.ContentType = "application/json";
                        string constring = System.Configuration.ConfigurationManager.AppSettings["connectionSQL"];

                        SqlConnection con = new SqlConnection(constring);
                        con.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = con;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = "INSERT INTO " + context.Request.Params["name"] + " (" + context.Request.Params["column"] + ") VALUES ('" + context.Request.Params["value"] + "')";
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        context.Response.Write("{\"text\":\"write\", \"element\":\"new\"}");
                        break;

                    #endregion
                }
                case "changetaskstate":
                {
                    #region ИЗМЕНИТЬ СОСТОЯНИЕ ЗАДАЧИ

                    if (!string.IsNullOrEmpty(context.Request.Params["id"].ToString()))
                    {
                        Core._setTaskState(Convert.ToInt32(context.Request.Params["id"]), Convert.ToInt32(context.Request.Params["state"]));
                    }
                    break;

                    #endregion
                }
                case "removeintent":
                {
                    #region УДАЛИТЬ ЭЛЕМЕНТА ОБЛАСТИ

                    context.Response.ContentType = "text/plain";
                    string result = Core.deleteRowInIntetionByGUID(context.Request.Params["id"]);
                    if (result != string.Empty)
                    {
                        if (result != "CSS-Code-Snippets.jpg")
                            File.Delete(System.Configuration.ConfigurationManager.AppSettings["myscreenpath"] + result);
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }
                case "deletemap":
                {
                    #region УДАЛИТЬ КАРТУ

                    context.Response.ContentType = "text/plain";
                    if (context.Request.Params["id"] != null)
                    {
                        Core.DeleteMap(Convert.ToInt32(context.Request.Params["id"]));
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }
                case "deletearea":
                {
                    #region УДАЛИТЬ ОБЛАСТЬ

                    context.Response.ContentType = "text/plain";
                    if (context.Request.Params["id"] != null)
                    {
                        Core.DeleteArea(Convert.ToInt32(context.Request.Params["id"]));
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }
                case "deletetask":
                {
                    #region УДАЛИТЬ ЗАДАЧУ

                    context.Response.ContentType = "text/plain";
                    if (context.Request.Params["id"] != null)
                    {
                        Core.DeleteTask(Convert.ToInt32(context.Request.Params["id"]));
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }
                case "changetime":
                {
                    #region ИЗМЕНИТЬ ВРЕМЯ ЗАДАЧИ

                    context.Response.ContentType = "text/plain";
                    if (context.Request.Params["id"] != null)
                    {
                        if (context.Request.Params["value"] != null)
                        {
                            Core.ChangeTimeInTask(Convert.ToInt32(context.Request.Params["id"]), context.Request.Params["value"]);
                        }
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }
                case "saveavatar":
                {
                    #region СОХРАНИТЬ АВАТАРА ПОЛЬЗОВАТЕЛЯ

                    context.Response.ContentType = "text/plain";
                    if (context.Request.Params["name"] != null)
                    {
                        if (context.Request.Params["url"] != null)
                        {
                            Core.saveProfile(context.Request.Params["name"], context.Request.Params["url"].ToString());
                        }
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }
                case "runscript":
                {
                    #region ЗАПУСТИТЬ СКРИПТ

                    context.Response.ContentType = "text/plain";
                    if (context.Request.Params["clientName"] != null)
                    {
                        Core.RunScript(context.Request.Params["clientName"].ToString(), context.Request.Params["script"].ToString());
                    }
                    context.Response.Write("OK");
                    break;

                    #endregion
                }

            }

        }

        if (context.Request.Params["create"] != null)
        {
            switch (context.Request.Params["instanse"])
            {
                case "map":
                    {
                        context.Response.ContentType = "application/json";
                       
                        Core.Map map = new Core.Map();
                        map.Name = context.Request.Params["name"];
                        map.description = context.Request.Params["description"];
                        map.img = context.Request.Params["img_"];
                        map.Url = context.Request.Params["url_"].Replace("Handler.ashx","");
                        string query = "";
                        if (Core.CheckMap(map,out query) == false)
                        {
                            int mapId = Core.CreateMap(map, Core.WriteType.create);
                            context.Response.Write("{\"identity\":\"" + mapId + "\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"identity\":\"write\"}");
                        }
                        break;
                    }
                case "category":
                    {
                        context.Response.ContentType = "application/json";

                        Core.Map map = new Core.Map();
                        map.Name = context.Request.Params["name"];
                        map.description = context.Request.Params["description"];
                        map.img = context.Request.Params["img_"];
                        map.Url = context.Request.Params["url_"].Replace("Handler.ashx", "");

                        int map_id = Convert.ToInt32(context.Request.Params["map_id"]);
                        if (Core.CheckArea(map, map_id) == false)
                        {
                            int areaId = Core.CreateSubCategory(map_id, map, Core.WriteType.create);
                            context.Response.Write("{\"identity\":\"" + areaId + "\"}");
                        }
                        else
                        {
                            context.Response.Write("{\"identity\":\"write\"}");
                        }
                        break;
                    }
            }
        }

        if (context.Request.Params["delete"] != null)
        {
            switch (context.Request.Params["instanse"])
            {
                case "submap":
                    {
                        context.Response.ContentType = "application/json";
                        bool result = Core.DeleteSubMap(Convert.ToInt32(context.Request.Params["id"]));
                        context.Response.Write("{\"identity\":\"ok\"}");
                        break;
                    }
            }
        }

        if (context.Request.Params["clients"] != null)
        {
            switch (context.Request.Params["type"])
            {
                case "clients":
                    {
                        string result = string.Empty;
                        Dictionary<SHost.ClientStruct, bool> clients = Core._getClients();
                        foreach (KeyValuePair<SHost.ClientStruct, bool> item in clients)
                        {
                            result+= @"<tr>
                                         <td><span class = 'glyphicon glyphicon-cog' ></span></td>
                                         <td>" + item.Key.name + @"</td>
                                         <td>" + item.Key.processorcount + @" шт.</td>
                                         <td>" + item.Key.memorysize + @" Mb</td>
                                         <td>" + (item.Key.internet == true ? "<span style = 'color: green;' class = 'glyphicon glyphicon-globe' ></span>" : "<span style = 'color: red;' class = 'glyphicon glyphicon-globe' ></span>") + @"</td>
                                         <td>" + item.Key.IEVersion + @"</td>
                                         <td>" + (item.Key.VideoController.Length > 0 ? item.Key.VideoController[0] : "Нету") + @"</td>  
                                         <td>" + item.Key.ProcessorLoadPercentage + @"</td>                                              
                                         <td>" + (item.Value == true ? "Свободен" : "Занят") + @"</td>
                                        </tr>";
                        }

                        context.Response.Write(result);
                        break;
                    }
            }
        }

        #region Функции для работы с Android

        if (context.Request.Params["android"] != null)
        {
            switch (context.Request.Params["command"])
            {
                case "clients":
                    {
                        context.Response.ContentType = "application/json";
                        string result = string.Empty;
                        Dictionary<SHost.ClientStruct, bool> clients = Core._getClients();
                        if (clients.Count != 0)
                            result += "[";

                        foreach (KeyValuePair<SHost.ClientStruct, bool> item in clients)
                        {
                            if (result != "[")
                            {
                                result+=",";
                            }
                            result += "{\"name\":\"" + item.Key.name + "\",\"state\":\"" + (item.Value == true ? "Свободен" : "Занят") + "\",\"prcount\":\"" + item.Key.processorcount + "шт\",\"msize\":\"" + item.Key.memorysize + " mb\"}";
                        }
                        if (result != string.Empty)
                            result += "]";
                        context.Response.Write("{\"art\":" + result + "}");
                        break;
                    }
                case "tasks":
                    {
                        context.Response.ContentType = "application/json";
                        string result = string.Empty;
                        DataSet ds = Core._getAllTasks();
                        if (ds.Tables[0].Rows.Count != 0)
                            result += "[";

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            if (result != "[")
                            {
                                result += ",";
                            }
                            result += "{\"id\":\"" + item["id"].ToString() + "\",\"name\":\"" + item["title"].ToString() + "\",\"time\":\"" + item["time"].ToString() + "\"}";
                        }
                        if (result != string.Empty)
                            result += "]";
                        context.Response.Write("{\"art\":" + result + "}");
                        break;
                    }
                case "taskinfo":
                    {
                        if (context.Request.Params["id"] != null)
                        {
                            context.Response.ContentType = "application/json";
                            string result = string.Empty;
                            string[] ds = Core._getTaskInfo(Convert.ToInt32(context.Request.Params["id"]));
                            if (ds.Length != 0)
                                result += "[";

                            result += "{";
                            result += "\"image\":\"" + ds[0] + "\",";
                            result += "\"title\":\"" + ds[1] + "\",";
                            result += "\"subtitle\":\"" + ds[2] + "\",";
                            result += "\"state\":\"" + ds[3] + "\",";
                            result += "\"time\":\"" + ds[4] + "\"";
                            result += "}";

                            if (result != string.Empty)
                                result += "]";
                            context.Response.Write("{\"art\":" + result + "}");
                        }
                        break;
                    }
                case "taskstate":
                    {
                        if (context.Request.Params["id"] != null)
                        {
                            context.Response.ContentType = "application/json";
                            string result = string.Empty;
                            Core._setTaskState(Convert.ToInt32(context.Request.Params["id"]), Convert.ToInt32(context.Request.Params["state"]));
                                result += "[";
                            if (result != string.Empty)
                                result += "]";
                            context.Response.Write("{\"art\":" + result + "}");
                        }
                        break;
                    }
                case "taskmaps":
                    {
                        context.Response.ContentType = "application/json";
                        string result = string.Empty;
                        DataSet ds = Core._getAllMap();
                        if (ds.Tables[0].Rows.Count != 0)
                            result += "[";
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            if (result != "[")
                            {
                                result += ",";
                            }
                            result += "{\"id\":\"" + item["id"].ToString() + "\",\"item\":\"" + item["title"].ToString() + "\"}";
                        }


                        if (result != string.Empty)
                            result += "]";
                        result = "{\"maps\":" + result + "}";
                        context.Response.Write(result);
                        break;
                    }
                case "taskareas":
                    {
                        context.Response.ContentType = "application/json";
                        string result = string.Empty;
                        DataSet ds = Core._getAllAreas();
                        if (ds.Tables[0].Rows.Count != 0)
                            result += "[";
                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            if (result != "[")
                            {
                                result += ",";
                            }
                            result += "{\"id\":\"" + item["id"].ToString() + "\",\"item\":\"" + item["title"].ToString() + "\"}";
                        }


                        if (result != string.Empty)
                            result += "]";
                        result = "{\"areas\":" + result + "}";
                        context.Response.Write(result);
                        break;
                    }
            }
        }
        
        #endregion
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}