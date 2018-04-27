#region directives
using HtmlAgilityPack;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using WatiN.Core;
using WatiN.Core.UtilityClasses;
using System.Configuration;
#endregion

#region core script
public static class Core
{
    public class Map
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string img { get; set; }
        public string description { get; set; }
        public string Url { get; set; }
        public int topic { get; set; }
    }

    public class TaskItem
    {
        public int area_id { get; set; }
        public int state { get; set; }
        public int day { get; set; }
        public string time { get; set; }
        public int interval { get; set; }
        public bool removeresult { get; set; }
    }

    public class EventsParsing
    {
        public int type { get; set; }
        public int eventtype { get; set; }
        public string areaname { get; set; }
        public int area_id { get; set; }
        public int progres { get; set; }
        public int countAsMax { get; set; }
        public int countNow { get; set; }
        public DateTime timestart { get; set; }
        public DateTime timeend { get; set; }
        public string urlnow { get; set; }
    }

    public enum WriteType
    {
        create,
        edit
    }

    #region global variables
    static string _connectionString = ConfigurationManager.AppSettings["sqlConnectionString"];
    #endregion

    #region properties
    public static string ConnectionString
    {
        get { return _connectionString; }
        set { _connectionString = value; }
    }
    #endregion

    #region Переменные
        public static CrwClient.CrawlerWCFClient client = new CrwClient.CrawlerWCFClient();

        public static SHost.SHostClient server;

    #endregion

    #region ПОЛУЧЕНИЕ КОНТЕНТА СТРАНИЦЫ

        public static byte[] GetContent(string Url, bool a_replace)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(Url);
            WebRequest.DefaultWebProxy = null;
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            string charSet = response.CharacterSet;
            Encoding encoding;
            if (String.IsNullOrEmpty(charSet))
                encoding = Encoding.Default;
            else
                encoding = Encoding.GetEncoding(charSet);
            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), encoding);

            string content = sr.ReadToEnd().Trim();

            Uri myUri = new Uri(Url, UriKind.Absolute);
            content = content.Replace("href=\"/", "href=\"" + myUri.GetLeftPart(UriPartial.Authority) + "/");
            content = content.Replace("href=\"../", "href=\"" + myUri.GetLeftPart(UriPartial.Authority) + "/");

            //TAG SRC

            int start_p = 0;
            int img = 0;
            do
            {
                img = content.IndexOf("<img", start_p);
                if (img != -1)
                {
                    start_p = img + 1;
                    string innerhtml = content.Substring(img, content.IndexOf(">", start_p) - img) + ">";
                    string buf_img = innerhtml;
                    Regex re = new Regex(
                            @"(?<=<img .*?src\s*=\s*"")[^""]+(?="".*?>)");
                    MatchCollection mc = re.Matches(innerhtml);

                    string al = string.Empty;
                    foreach (Match m in mc)
                        al += m.Value;

                    if (al.IndexOf("http") == -1)
                    {
                        if (al.IndexOf("//") != 0)
                        {
                            innerhtml = re.Replace(innerhtml, delegate(Match m)
                            { return myUri.GetLeftPart(UriPartial.Authority) + al; });

                            content = content.Replace(buf_img, innerhtml);
                        }
                    }

                }
            }
            while (img != -1);

            //TAG SCRIPT

            start_p = 0;
            img = 0;
            do
            {
                img = content.IndexOf("<script", start_p);
                if (img != -1)
                {
                    start_p = img + 1;
                    string innerhtml = content.Substring(img, content.IndexOf(">", start_p) - img) + ">";
                    string buf_img = innerhtml;
                    Regex re = new Regex(
                            @"(?<=<script .*?src\s*=\s*"")[^""]+(?="".*?>)");
                    MatchCollection mc = re.Matches(innerhtml);

                    string al = string.Empty;
                    foreach (Match m in mc)
                        al += m.Value;

                    if (al.IndexOf("http") == -1)
                    {
                        if (al.IndexOf("//") != 0)
                        {
                            innerhtml = re.Replace(innerhtml, delegate(Match m)
                            { return myUri.GetLeftPart(UriPartial.Authority) + al; });

                            content = content.Replace(buf_img, innerhtml);
                        }
                    }

                }
            }
            while (img != -1);

            //TAG A
            
            start_p = 0;
            img = 0;
            if (a_replace == true)
            {
                do
                {
                    img = content.IndexOf("<a ", start_p);
                    if (img != -1)
                    {
                        start_p = img + 1;
                        string innerhtml = content.Substring(img, content.IndexOf("a>", start_p) - img) + "a>";
                        string buf_img = innerhtml;
                        Regex re = new Regex(
                                @"(?<=<a .*?href\s*=\s*"")[^""]+(?="".*?>)");
                        MatchCollection mc = re.Matches(innerhtml);
                        innerhtml = re.Replace(innerhtml, delegate(Match m)
                        { return "\" onclick = \"javascript: return false;\""; });

                        content = content.Replace(buf_img, innerhtml);

                    }
                }
                while (img != -1);
            }

            content = content.Replace("@import \"/", "@import \"" + myUri.GetLeftPart(UriPartial.Authority) + "/");

            //Непонятные скрипты - ТЕСТЫ (УДАЛИТЬ ПО НЕНАДОБНОСТИ)
            content = Regex.Replace(content, @"<iframe(.|\n)*?>", string.Empty);

            byte[] bytes = new byte[content.Length * sizeof(char)];
            System.Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
        
    #endregion

    #region Чисто для сайта
    public static bool ValidateUser(string userName, string passWord)
    {
        SqlConnection conn;
        SqlCommand cmd;
        string lookupPassword = null;

        if ((null == userName) || (0 == userName.Length) || (userName.Length > 15))
        {
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of userName failed.");
            return false;
        }

        if ((null == passWord) || (0 == passWord.Length) || (passWord.Length > 25))
        {
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Input validation of passWord failed.");
            return false;
        }

        try
        {
            conn = new SqlConnection(ConnectionString);
            conn.Open();

            cmd = new SqlCommand("getUserPasswordByLogin", conn);
            cmd.Parameters.Add("@userName", SqlDbType.VarChar, 25);
            cmd.Parameters["@userName"].Value = userName;
            cmd.CommandType = CommandType.StoredProcedure;

            lookupPassword = (string)cmd.ExecuteScalar();

            cmd.Dispose();
            conn.Dispose();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.WriteLine("[ValidateUser] Exception " + ex.Message);
        }

        if (null == lookupPassword)
        {
            return false;
        }

        return (0 == string.Compare(lookupPassword, passWord, false));

    }

    public static DataSet _getMetaBlock(int role)
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT ln.position, bl.* FROM meta.lines as ln
                                    LEFT JOIN meta.block as bl
                                    ON bl.id = ln.block 
                                    ORDER BY ln.position";
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
            }
            conn.Close();
        } 

        return ds;
    }

    public static bool _getPageAccess(int role, Guid page)
    {
        bool result = false;
        try
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = @"SELECT pr.enable FROM meta.pagesroles as pr
                                    LEFT JOIN meta.pages as p
                                    ON p.id = pr.page
                                    WHERE pr.role = " + role.ToString() + "AND p.guid = '" + page.ToString() + "'";
                    result = (bool)cmd.ExecuteScalar();
                }
                conn.Close();
            }
        }
        catch { }

        return result;
    }

    public static int _getRole(string Login)
    {
        int result = 0;

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = @"SELECT ur.role FROM userroles as ur
                                    LEFT JOIN users as u
                                    ON ur.[user] = u.id
                                    WHERE u.login = '" + Login + "'";
                result = (int)cmd.ExecuteScalar();
            }
            conn.Close();
        }

        return result;
    }

    public static string SaveFile(byte[] data)
    {
        Guid guid = Guid.NewGuid();
        string urlfile = guid.ToString() + ".jpg";
        var fs = new BinaryWriter(new FileStream(@"C:\\tmp\\" + guid + ".jpg", FileMode.Append, FileAccess.Write));
        fs.Write(data);
        fs.Close();
        return urlfile;
    }

    public static int CreateMap(Map map, WriteType type)
    {
        int id = 0;
        
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (type == WriteType.create)
                    {
                        cmd.CommandText = "createMap";
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = map.Name;
                        cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = map.description;
                        cmd.Parameters.Add("@img", SqlDbType.NVarChar).Value = map.img;
                        cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = map.Url;
                        id = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    else
                    {
                        cmd.CommandText = "editMap";
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = map.id;
                        cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = map.Name;
                        cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = map.description;
                        cmd.Parameters.Add("@img", SqlDbType.NVarChar).Value = map.img;
                        cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = map.Url;
                        cmd.ExecuteNonQuery();

                        id = map.id;
                    }
                    
                }
                conn.Close();
            }
        

        return id;
    }

    public static int CreateSubCategory(int mapid, Map map, WriteType type)
    {
        int id = 0;

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                if (type == WriteType.create)
                {
                    cmd.CommandText = "createArea";
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = map.Name;
                    cmd.Parameters.Add("@mapid", SqlDbType.Int).Value = mapid;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = map.description;
                    cmd.Parameters.Add("@img", SqlDbType.NVarChar).Value = map.img;
                    cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = map.Url;
                    cmd.Parameters.Add("@topic", SqlDbType.Int).Value = map.topic;

                    id = Convert.ToInt32(cmd.ExecuteScalar());
                }
                else
                {
                    cmd.CommandText = "editArea";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = map.id;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = map.Name;
                    cmd.Parameters.Add("@mapid", SqlDbType.Int).Value = mapid;
                    cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = map.description;
                    cmd.Parameters.Add("@img", SqlDbType.NVarChar).Value = map.img;
                    cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = map.Url;
                    cmd.Parameters.Add("@topic", SqlDbType.Int).Value = map.topic;
                    cmd.ExecuteNonQuery();
                    id = map.id;
                }
            }
            conn.Close();
        }

        return id;
    }

    public static int CreateTask(TaskItem task)
    {
        int id = 0;

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "createTask";
                cmd.Parameters.Add("areaid", SqlDbType.Int).Value = task.area_id;
                cmd.Parameters.Add("state", SqlDbType.Int).Value = task.state;
                cmd.Parameters.Add("day", SqlDbType.Int).Value = task.day;
                cmd.Parameters.Add("time", SqlDbType.NVarChar).Value = task.time;
                cmd.Parameters.Add("interval", SqlDbType.Int).Value = task.interval;
                cmd.Parameters.Add("active", SqlDbType.Bit).Value = 1;
                cmd.Parameters.Add("removeresults", SqlDbType.Bit).Value = (task.removeresult == true ? 1 : 0);

                id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            conn.Close();
        }

        return id;
    }

    public static bool DeleteMap(int map_id)
    {
        bool result = false;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString    ;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "deleteMap";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = map_id;
                Convert.ToInt32(cmd.ExecuteScalar());
                result = true;
            }
            conn.Close();
        }
        return result;
    }

    public static bool DeleteArea(int area_id)
    {
        bool result = false;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "deleteArea";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = area_id;
                Convert.ToInt32(cmd.ExecuteScalar());
                result = true;
            }
            conn.Close();
        }
        return result;
    }

    public static bool DeleteTask(int task_id)
    {
        bool result = false;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "deleteTask";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = task_id;
                Convert.ToInt32(cmd.ExecuteScalar());
                result = true;
            }
            conn.Close();
        }
        return result;
    }

    public static bool DeleteSubMap(int area_id)
    {
        bool result = false;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "deleteArea";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = area_id;
                Convert.ToInt32(cmd.ExecuteScalar());
                result = true;
            }
            conn.Close();
        }
        return result;
    }

    public static bool CheckMap(Map map, out string query)
    {
        bool write = false;
        query = "";
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "checkMap";
                cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = map.Name;
                cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = map.description;
                cmd.Parameters.Add("@img", SqlDbType.NVarChar).Value = map.img;
                cmd.Parameters.Add("@url", SqlDbType.NVarChar).Value = map.Url;

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    write = true;

            }
            conn.Close();
        } 

        return write;
    }

    public static bool CheckArea(Map map, int map_id)
    {
        bool write = false;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("SELECT COUNT(id) FROM Areas WHERE {0} = {5} AND {1} = '{6}' AND {2} = '{7}' and {3} = '{8}' AND {4} = '{9}'", "map_id", "title", "subtitle", "image", "url", map_id, map.Name, map.description, map.img, map.Url);

                if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    write = true;

            }
            conn.Close();
        }

        return write;
    }

    public static DataSet _getAllMap()
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Maps";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            conn.Close();
        } 
        return ds;
    }

    public static DataSet _getAllAreas(int mapId)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT A.*, T.topic FROM Areas as A
                                    LEFT JOIN dbo.topics as T
                                    ON T.id = A.topic_id 
                                    WHERE map_id = " + mapId + " ORDER BY A.id DESC";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            conn.Close();
        }
        return ds;
    }

    public static DataSet _getAllAreas()
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Areas ORDER BY id DESC";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            conn.Close();
        }
        return ds;
    }

    public static DataSet _getAllAreasForTasks()
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT * FROM(
                                    SELECT A.*, I.id as I, R.id AS R FROM Areas as A
                                    LEFT JOIN dbo.intentions as I
                                    ON I.area_id = A.id
                                    LEFT JOIN dbo.routes as R
                                    ON R.area_id = A.id) as RES WHERE (NOT RES.I is NULL) AND (NOT RES.R is NULL) ORDER BY RES.id DESC";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            conn.Close();
        }
        return ds;
    }

    public static DataSet _getAllAreasForTasksNoFilter()
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT * FROM(
                                    SELECT A.*, I.id as I, R.id AS R FROM Areas as A
                                    LEFT JOIN dbo.intentions as I
                                    ON I.area_id = A.id
                                    LEFT JOIN dbo.routes as R
                                    ON R.area_id = A.id) as RES ORDER BY RES.id DESC";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            conn.Close();
        }
        return ds;
    }

    public static DataSet _getAllAreasForTasks(int map_id)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT * FROM(
                                    SELECT A.*, I.id as I, R.id AS R FROM Areas as A
                                    LEFT JOIN dbo.intentions as I
                                    ON I.area_id = A.id
                                    LEFT JOIN dbo.routes as R
                                    ON R.area_id = A.id WHERE A.map_id = " + map_id + @") as RES WHERE (NOT RES.I is NULL) AND (NOT RES.R is NULL) ORDER BY RES.id DESC";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

            }
            conn.Close();
        }
        return ds;
    }

    public static int _getCountDocsForAreas(int area_id)
    {
        int result = 0;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT COUNT(ID) FROM [CWsystem].[dbo].[links] where area_id = " + area_id;
                result = (int)cmd.ExecuteScalar();
            }
            conn.Close();
        }
        return result;
    }

    public static Dictionary<string,string> _getAreaRowById(int ID)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Areas WHERE id = " + ID;
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    result["title"] = read["title"].ToString();
                    result["subtitle"] = read["subtitle"].ToString();
                    result["image"] = read["image"].ToString();
                    break;
                }
            }
            conn.Close();
        }

        return result;
    }

    public static string _getMapFromId(int mapId)
    {
        string result = string.Empty;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT url FROM Maps WHERE id = " + mapId;
                result = (string)cmd.ExecuteScalar();
            }
            conn.Close();
        }

        return result;
    }

    public static DataSet _getMapFromId(int mapId, bool all)
    {
        DataSet result = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Maps WHERE id = " + mapId;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(result);
            }
            conn.Close();
        }

        return result;
    }

    public static DataSet _getAreaNameFromId(int areaId)
    {
        DataSet result = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Areas WHERE id = " + areaId;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(result);
            }
            conn.Close();
        }

        return result;
    }

    public static DataSet _getPathForArea(int area_id)
    {
        DataSet result = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM routes WHERE area_id = " + area_id;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(result);
            }
            conn.Close();
        }

        return result;
    }

    public static Dictionary<int,string> _getTopics()
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM topics";
                SqlDataReader read = cmd.ExecuteReader();
                while (read.Read())
                {
                    result.Add(Convert.ToInt32(read["id"]), read["topic"].ToString());
                }
            }
            conn.Close();
        }
        return result;
    } 

    public static void _setRef(string reference, string name, string value)
    {
        Dictionary<int, string> result = new Dictionary<int, string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "INSERT INTO " + reference+ " (" + name + ") VALUES ('" + value + "')";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }
    } 

    public static int _getTasksToDay()
    {
        int result = 0;
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT COUNT(id) FROM Tasks WHERE Active = 1 AND (Interval = 0 OR Day = " + DateTime.Now.Day + ")";
            result = (int)cmd.ExecuteScalar();
            con.Close();
        }

        return result;
    }

    public static string[] _getTaskInfo(int id)
    {
        string[] result = new string[5];
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = @"SELECT a.*, t.active, t.time FROM [CWsystem].[dbo].[tasks] as t

                                LEFT JOIN [CWsystem].[dbo].[areas] as a
                                ON a.id = t.area_id

                                WHERE t.id=" + id;

            SqlDataReader r = cmd.ExecuteReader();
            while (r.Read())
            {
                result[0] = r["image"].ToString().IndexOf("images.jpg") == -1 ? r["image"].ToString() : "http://10.10.9.132:777/" + r["image"].ToString();
                result[1] = r["title"].ToString();
                result[2] = r["subtitle"].ToString() == string.Empty ? "Данных нет" : r["subtitle"].ToString();
                result[3] = r["active"].ToString();
                result[4] = r["time"].ToString();
            }
            con.Close();
        }

        return result;
    }

    public static DataSet _getLogs()
    {
        DataSet ds = new DataSet();

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT TOP 7 * FROM Logs ORDER BY ID DESC";
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(ds);
            con.Close();
        }

        return ds;
    }

    public static DataSet _getEvents()
    {
        DataSet ds = new DataSet();

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT ev.*, ts_s.state as STATE_T FROM Events as ev LEFT JOIN taskstate as ts_s ON ts_s.id = ev.state ORDER BY ID DESC";
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(ds);
            con.Close();
        }

        return ds;
    }

    public static DataSet _getEventsByTaskId(int task_id)
    {
        DataSet ds = new DataSet();

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Events WHERE task_id = " + task_id;
            SqlDataAdapter ad = new SqlDataAdapter(cmd);
            ad.Fill(ds);
            con.Close();
        }

        return ds;
    }

    public static int saveNewIntention(int areaid, string xpath, string imageurl)
    {
        int result = 0;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "createIntention";
                cmd.Parameters.Add("area_id", SqlDbType.Int).Value = areaid;
                cmd.Parameters.Add("tp", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("xpath", SqlDbType.NVarChar).Value = xpath;
                cmd.Parameters.Add("imageurl", SqlDbType.NVarChar).Value = imageurl;
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            conn.Close();
        }

        return result;
    }

    public static int saveNewHref(int areaid, string attr, string imageurl)
    {
        int result = 0;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "createIntention";
                cmd.Parameters.Add("area_id", SqlDbType.Int).Value = areaid;
                cmd.Parameters.Add("tp", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("xpath", SqlDbType.NVarChar).Value = attr;
                cmd.Parameters.Add("imageurl", SqlDbType.NVarChar).Value = imageurl;
                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            conn.Close();
        }

        return result;
    }

    public static int deleteRowInIntetion(int id)
    {
        int result = 0;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = string.Format("DELETE FROM Intentions WHERE id = " + id);
                result = cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        return result;
    }

    public static string deleteRowInIntetionByGUID(string idimage)
    {
        string result = string.Empty;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                
                cmd.Connection = conn;
                cmd.CommandText = "SELECT imageurl FROM Intentions WHERE id = '" + idimage + "'";
                result = (string)cmd.ExecuteScalar();
            }
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "DELETE FROM Intentions WHERE id = '" + idimage + "'";
                cmd.ExecuteNonQuery();
            }
            conn.Close();
        }

        return result;
    }

    public static DataSet _getIntentionsArea(int areaid)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Intentions Where area_id = " + areaid;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
            }
            conn.Close();
        }
        return ds;
    }

    public static DataSet _getAllTasks()
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT t.*, a.title, st.state as name FROM Tasks as t LEFT JOIN Areas as a ON a.id = t.area_id LEFT JOIN TaskState as st ON st.id = t.state";
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
            }
            conn.Close();
        }

        return ds;
    }

    public static string[] _getStopWords()
    {
        string[] result = new string[0];
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM StopWords";
                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);

                int count = ds.Tables[0].Rows.Count;
                int index = 0;
                result = new string[count];

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    result[index] = item["word"].ToString().Trim().ToUpper();
                    index++;
                }
            }
            conn.Close();
        }

        return result;
    }

    public static DataSet QueryInBase(string table)
    {
        DataSet ds = new DataSet();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM " + table;
                
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);

            }
            conn.Close();
        }

        return ds;
    }

    public static void _setStopWordsInSQL(string[] words)
    {
        using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["connectionSQL"]))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            foreach (string item in words)
            {
                cmd.CommandText = "INSERT INTO stopwords (word) VALUES ('" + item.ToUpper().Trim() + "')";
                cmd.ExecuteNonQuery();
            }


            con.Close();
        }
    }

    public static void _setTaskState(int task_id, int state)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE TASKS SET state = " + state + ", active = " + (state == 3 ? "1" : "0") + " WHERE id = " + task_id;
            cmd.ExecuteNonQuery();
        }
    }

    public static List<string> DumpHRefs(string inputString)
    {
        List<string> result = new List<string>();
        Match m;
        string HRefPattern = "href\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";

        try
        {
            m = Regex.Match(inputString, HRefPattern,
                            RegexOptions.IgnoreCase | RegexOptions.Compiled,
                            TimeSpan.FromSeconds(1));
            while (m.Success)
            {
                result.Add(m.Groups[1].Value);
                m = m.NextMatch();
            }
        }
        catch (RegexMatchTimeoutException)
        {
           
        }

        return result;
    }

    public static int _getStep(string a, string b, out string url)
    {
        int result = 0;

        char[] _a = a.ToCharArray();
        char[] _b = b.ToCharArray();
        List<char> r1 = new List<char>();
        List<char> r2 = new List<char>();

        bool find = false;
        int startporition = 0;
        int len = 0;

        for (int i = 0; i < _a.Length; i++)
        {
            if (_a[i] != _b[i])
            {
                r1.Add(_a[i]);
                r2.Add(_b[i]);

                if (startporition == 0)
                {
                    startporition = i;
                    len++;
                }

                find = true;
            }
            else
            {
                if (find == true)
                {
                    int _buf = -1;
                    if (int.TryParse(_a[i].ToString(), out _buf))
                    {
                        r1.Add(_a[i]);
                        r2.Add(_a[i]);
                        len++;
                    }
                }
            }
        }

        string _w1 = string.Empty;
        string _w2 = string.Empty;

        foreach (char item in r1)
        {
            _w1 += item.ToString();
        }

        foreach (char item in r2)
        {
            _w2 += item.ToString();
        }

        int _t1 = 0;
        int _t2 = 0;

        int.TryParse(_w1, out _t1);
        int.TryParse(_w2, out _t2);

        int min = Math.Min(_t1, _t2);

        if (min == _t1)
            result = _t2 - _t1;
        else
            result = _t1 - _t2;

        string url1 = a.Substring(0, startporition);
        string url2 = a.Substring(startporition + len);
        url = url1 + "{step}" + url2;

        return result;
    }

    public static void saveNewPath(int areaid, 
        int routestype, 
        int start, int end, 
        string url, string pattern, 
        int step, 
        string element, 
        bool innerLinks)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT id FROM routes WHERE area_id = " + areaid;
            int res = 0;

            try
            {
                res = (int)cmd.ExecuteScalar();
            }
            catch { }

            if (res != 0)
            {
                cmd.CommandText = "UPDATE routes SET routestype = " + routestype + ", start = " + start + ", [end] = " + end + ", url = '" + url + "', step = " + step + ", element = '" + element + "', innerLinks = " + (innerLinks == false ? 0 : 1) + " WHERE area_id = " + areaid;
                cmd.ExecuteNonQuery();
            }
            else
            {
                cmd.CommandText = @"INSERT INTO routes (area_id,routestype,start, [end], url, step, element, innerLinks) 
                                    VALUES (" + areaid + ", " + routestype + ", " + start + ", " + end + ", '" + url + "', " + step + ",'" + element + "'," + (innerLinks == false ? 0 : 1) + ")";
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }

    public static Dictionary<string,int> _getCountTasksForClients()
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT COUNT(DISTINCT url) as CountUrl, Computer FROM results GROUP BY computer";
            SqlDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                result.Add(dr["Computer"].ToString(), Convert.ToInt32(dr["CountUrl"].ToString()));
            }
            con.Close();
        }

        return result;
    }

    public static void ChangeTimeInTask(int task, string value)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE tasks SET time = '" + value + "' WHERE id = " + task;
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }

    /// <summary>
    /// Gets username from database
    /// </summary>
    /// <param name="userIdentityName">Context.User.Identity.Name</param>
    /// <returns></returns>
    public static DataSet GetProfile(string userIdentityName)
    {
        DataSet dataSetResult = new DataSet();

        using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
        {
            if (sqlConnection != null && sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();

                if (sqlConnection.State == ConnectionState.Open)
                {
                    SqlCommand sqlCommand = new SqlCommand()
                    {
                        Connection = sqlConnection,
                        CommandType = CommandType.Text,
                        CommandText = "SELECT pr.* FROM users as u LEFT JOIN profile as pr ON pr.[user] = u.id WHERE u.login = '" + userIdentityName + "'" // ?
                    };

                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    sqlDataAdapter.Fill(dataSetResult);
                }
            }
        }

        return dataSetResult;
    }

    public static void saveProfile(string login, string avatar)
    {
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "UPDATE [profile] SET avatar = '" + avatar + "' WHERE [user] = (SELECT id FROM users WHERE login = '" + login + "')";
            int res = cmd.ExecuteNonQuery();
            if (res == 0)
            {
                cmd.CommandText = "INSERT INTO [profile] ([user],avatar) VALUES  ((SELECT id FROM users WHERE login = '" + login + "'), '" + avatar + "')";
                cmd.ExecuteNonQuery();
            }
            con.Close();
        }
    }

    public static List<EventsParsing> _getEventsParsing()
    {
        List<EventsParsing> list = new List<EventsParsing>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            using (SqlCommand cmd = new SqlCommand())
            {
                conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT top 11

                                    ev_table.id,
                                    ev_table.event_type,
                                    ev_table.[state],
                                    ev_table.start,
                                    ev_table.[end],
                                    ev_table.agents,
                                    a.title,
                                    a.id as area_id,
                                    r.start as st_start,
                                    r.[end] as st_end,
                                    r.step

                                    FROM dbo.events as ev_table 

                                    left join dbo.tasks as ts_table
                                    on ev_table.task_id = ts_table.id

                                    left join dbo.areas as a
                                    on a.id = ts_table.area_id

                                    left join dbo.routes as r
                                    on r.area_id = ts_table.area_id

                                    order by ev_table.id DESC";

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    
                    EventsParsing ev = new EventsParsing();

                    if (Convert.ToInt32(item["event_type"]) == 1)
                    {
                        ev.areaname = item["title"].ToString().Length > 40 ? item["title"].ToString().Substring(0, 40) + "..." : item["title"].ToString();
                        ev.area_id = Convert.ToInt32(item["area_id"]);
                        ev.countAsMax = Convert.ToInt32(item["st_end"].ToString());

                        if (Convert.ToInt32(item["state"].ToString()) == 3)
                        {
                            _getCountCorpuses(Convert.ToInt32(item["area_id"].ToString()), ref ev);
                        }
                        else
                        {
                            ev.progres = 100;
                            ev.countNow = Convert.ToInt32(item["st_end"].ToString());
                            ev.urlnow = string.Empty;
                        }

                        ev.timestart = Convert.ToDateTime(item["start"].ToString());
                        try
                        {
                            ev.timeend = Convert.ToDateTime(item["end"].ToString());
                        }
                        catch { ev.timeend = DateTime.Now; }
                        ev.type = Convert.ToInt32(item["state"].ToString());
                    }
                    else if (Convert.ToInt32(item["event_type"]) == 2)
                    {
                        ev.areaname = "Вычисление Хи - квадрата";
                        ev.timestart = Convert.ToDateTime(item["start"].ToString());
                        try
                        {
                            ev.timeend = Convert.ToDateTime(item["end"].ToString());
                        }
                        catch { ev.timeend = DateTime.Now; }
                        ev.type = Convert.ToInt32(item["state"].ToString());
                    }
                    else if (Convert.ToInt32(item["event_type"]) == 3)
                    {
                        ev.areaname = "Классификация";
                        ev.timestart = Convert.ToDateTime(item["start"].ToString());
                        try
                        {
                            ev.timeend = Convert.ToDateTime(item["end"].ToString());
                        }
                        catch { ev.timeend = DateTime.Now; }
                        ev.type = Convert.ToInt32(item["state"].ToString());
                    }

                    ev.eventtype = Convert.ToInt32(item["event_type"]);
                    list.Add(ev);
                }
            }
            conn.Close();
        }
        return list;
    }

    private static void _getCountCorpuses(int area, ref EventsParsing ev)
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = ConnectionString;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;

                cmd.CommandText = @"SELECT SUM(R.Now_) as N, SUM(R.All_) as A, (SUM(R.All_) - SUM(R.Now_)) as RZ FROM (
                                    SELECT COUNT(DISTINCT url) as Now_, 0 as All_ FROM dbo.results Where area_id = " + area.ToString() + @"
                                    UNION ALL
                                    SELECT 0 as Now_, COUNT(id) as All_ FROM dbo.Links WHERE Area_id = " + area.ToString() + @") as R";
                DataSet math = new DataSet();
                SqlDataAdapter ad_math = new SqlDataAdapter(cmd);
                ad_math.Fill(math);

                int all = 0;
                int now = 0;
                int rs = 0;

                foreach (DataRow item in math.Tables[0].Rows)
                {
                    all = Convert.ToInt32(item["A"]);
                    now = Convert.ToInt32(item["N"]);
                    rs = Convert.ToInt32(item["RZ"]);
                }

                cmd.CommandText = @"SELECT * FROM (
		                                    SELECT 
			                                    COUNT(DISTINCT [url]) as count_c,
			                                    MAX([create_date]) as maxdate
		                                      FROM [dbo].[corpus] where area_id = " + area.ToString() + @") as table1

                                    LEFT JOIN [dbo].[corpus] as c
                                    ON c.area_id = " + area.ToString() + @" AND c.create_date = table1.maxdate";

                DataSet ds = new DataSet();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    ev.countAsMax = all;
                    if (all == 0)
                        ev.progres = 0;
                    else
                        ev.progres = (now * 100) / all;
                    ev.countNow = now; 
                    ev.urlnow = item["url"].ToString();
                    break;
                }
            }
            conn.Close();
            conn.Dispose();
        }
    }

    public static DataSet _getAllRaging()
    {
        DataSet ds = new DataSet();

        return ds;
    }

    public static DataSet _getAnalyticRaging(int id)
    {
        DataSet ds = SQL._getDataAsDT(ConnectionString, @"SELECT SUM(A.[precision]) /2 as precision, SUM(A.recall)/2 as recall, SUM(A.fscore)/2 as fscore FROM dbo.ranginganalytics as A");
        return ds;
    }

    public static DataSet _getResultRaging(int id)
    {
        DataSet ds = SQL._getDataAsDT(ConnectionString, 
                                      @"SELECT L.url, L.title, Ranging.prob ,CASE Ranging.class WHEN 1 THEN 'Ислам' ELSE 'Не ислам' END as topic
                                      FROM [CWsystem].[dbo].[rangingresults] as Ranging
                                      LEFT JOIN [CWsystem].[dbo].links as L
                                      ON L.id = Ranging.[link]
                                      WHERE Ranging.ranging_id = (SELECT id FROM [CWsystem].[dbo].[ranginglist] WHERE topic_id = " + id + ")");
        return ds;
    }

    public static DataSet _getAllTopics()
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection(ConnectionString))
        {
            conn.ConnectionString = ConnectionString;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = @"SELECT * FROM dbo.topics";
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
            }
        }

        return ds;
    }

    public static DataSet _getCloudRaging(int id)
    {
        DataSet ds = new DataSet();

        return ds;
    }

    public static bool renderPagePart(string text, string outputPath, Rectangle crop)
    {
        WebBrowser wb = new WebBrowser();
        wb.ScrollBarsEnabled = false;
        wb.ScriptErrorsSuppressed = true;
        wb.DocumentText = text;
        //wb.Document.Encoding = "UTF-8";
        while (wb.ReadyState != WebBrowserReadyState.Complete)
        {
            Application.DoEvents();
        }
        wb.Width = wb.Document.Body.ScrollRectangle.Width;
        wb.Height = wb.Document.Body.ScrollRectangle.Height;
        if (wb.Height > 16)
        {
            using (Bitmap bitmap = new Bitmap(wb.Width, wb.Height))
            {
                wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Width));
                wb.Dispose();
                Rectangle rect = new Rectangle(crop.Left, crop.Top, wb.Width > 250 ? 250 : wb.Width, wb.Height > 250 ? 250 : wb.Height);

                Bitmap cropped = bitmap.Clone(rect, bitmap.PixelFormat);

                //cropped = Contrast(cropped, 50);
                cropped.Save(outputPath, ImageFormat.Png);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    public static void GetSnapshotUsingWatiN(string name, string url, string location)
    {
        using (IE ie = new IE(url))
        {
            ie.BringToFront();
            ie.WaitForComplete();
            string fName = location + "/" + name + ".png";

            using (FileStream fStr = new FileStream(fName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                CaptureWebPage cwp = new CaptureWebPage(ie.DomContainer);
                cwp.CaptureWebPageToFile(fStr, CaptureWebPage.ImageCodecs.Png, false, false, 100, 100);
                fStr.Close();
            }
        }
    }

    #endregion

    #region Работа с клиентами сервера SHost 

        public static Dictionary<SHost.ClientStruct, bool> _getClients()
        {
            Dictionary<SHost.ClientStruct, bool> client = new Dictionary<SHost.ClientStruct, bool>();
            try
            {
                server = new SHost.SHostClient(new System.ServiceModel.InstanceContext(new CallbackHandler()));
                client = server.s_GetAllClients();
                server.Close();
            }
            catch { }

            return client;
        }

        public static void RunScript(string clientName, string script)
        {
            Dictionary<SHost.ClientStruct, bool> client = new Dictionary<SHost.ClientStruct, bool>();
            try
            {
                /*server = new SHost.SHostClient(new System.ServiceModel.InstanceContext(new CallbackHandler()));
                server.s_RunScript(clientName, script);
                server.Close();*/
            }
            catch { }
        }

    #endregion

    #region ХИ - квадрат
        public static Dictionary<SHost.ClientStruct, bool> _calculateXi(int area_id, int[] areas, double limit)
        {
            Dictionary<SHost.ClientStruct, bool> client = new Dictionary<SHost.ClientStruct, bool>();
                server = new SHost.SHostClient(new System.ServiceModel.InstanceContext(new CallbackHandler()));
                
                SHost.HItask hi = new SHost.HItask();
                hi.aread_id = area_id;
                hi.aread_ids = areas;
                hi.limit = 0;

                server.s_CalculateHI_InSQL(hi);

                server.Close();


            return client;
        }

        public static DataSet GetAllTaskHI()
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM (SELECT DISTINCT(area_id) FROM dbo.hi) as H
                                        LEFT JOIN dbo.areas as a
                                        ON a.id = H.area_id";

                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(ds);
                }
                conn.Close();
                conn.Dispose();
            }

            return ds;
        }

        public static DataSet GetHIresult(int area_id)
        {
            DataSet ds = new DataSet();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT TOP 200
                                        t.term,
                                        H.A,
                                        H.B,
                                        H.C,
                                        H.D,
                                        H.HI,
                                        H.MI,
                                        H.IG
                                        FROM dbo.hi as H
                                        LEFT JOIN dbo.terms as t
                                        ON t.id = H.term
                                        WHERE area_id = " + area_id + @" AND H.A > H.B AND H.B < H.D
                                        ORDER BY HI DESC";  //And H.a > H.c

                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    ad.Fill(ds);
                }
                conn.Close();
                conn.Dispose();
            }

            return ds;
        }


    #endregion

    #region Классификация

        public static void _classification(List<int> vector1, List<int> vector2, int class1)
        {
            Dictionary<SHost.ClientStruct, bool> client = new Dictionary<SHost.ClientStruct, bool>();
            server = new SHost.SHostClient(new System.ServiceModel.InstanceContext(new CallbackHandler()));

            server.s_Сlassification(vector1.ToArray(), vector2.ToArray(), class1);
            server.Close();
        }

        public static string GetUriFromId(int id)
        {
            string result = string.Empty;
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                using (SqlCommand cmd = new SqlCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT url FROM dbo.links WHERE id = " + id;
                    result = cmd.ExecuteScalar().ToString();
                }
                conn.Close();
            }

            return result;
        }

        public static void CreateOrUpdateRanging(string name, int[] class1, int[] class2, DateTime date)
        {

        }

    #endregion

    #region Облако тегов

        public static Dictionary<string, int> GetWordsForCloud(int id)
        {
            Dictionary<string, int> ds = new Dictionary<string, int>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConnectionString;
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = @"SELECT * FROM (
                                        SELECT T.term, COUNT(R.id) as C
                                        FROM dbo.results as R 
                                        LEFT JOIN dbo.terms as T
                                        ON T.id = R.result
                                        WHERE 
                                        R.area_id = " + id + @" AND R.result IN (SELECT ChI.term FROM dbo.hi AS ChI where area_id = " + id + @" AND HI > 6.6 AND A  > B AND B < D)
                                        GROUP BY T.term
                                        )as I
                                        WHERE I.C > 150
                                        ORDER BY C DESC";
                    SqlDataReader read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        ds.Add(read["term"].ToString(), Convert.ToInt32(read["C"]));
                    }
                    read.Close();

                    if (ds.Count == 0 )
                    {
                        cmd.CommandText = @"SELECT * FROM (
                                        SELECT T.term, COUNT(R.id) as C
                                        FROM dbo.results as R 
                                        LEFT JOIN dbo.terms as T
                                        ON T.id = R.result
                                        WHERE 
                                        R.area_id = " + id + @" AND R.result IN (SELECT ChI.term FROM dbo.hi AS ChI where area_id = " + id + @" AND A  > B AND B < D)
                                        GROUP BY T.term
                                        )as I
                                        ORDER BY C DESC";
                        read = cmd.ExecuteReader();
                        while (read.Read())
                        {
                            ds.Add(read["term"].ToString(), Convert.ToInt32(read["C"]));
                        }

                        read.Close();
                    }
                }
            }

            return ds;
        }

    #endregion
}

public class CallbackHandler : SHost.ISHostCallback
{
    public void Matrix(int[] a, int[] b)
    {

    }

    public void ClientLeave(string name)
    {
    }


    public void SetClientGuid(Guid id)
    {
    }

    public void CrawlerJoin(SHost.TaskClient[] task, string[] xpaths, Guid TaskForServer, bool last)
    {
        
    }

    public void RunScript(string script)
    {

    }

    public void ExecuteText(string name)
    {
        throw new NotImplementedException();
    }
}

public class Crawler
{
    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

    public async Task<string> _getWebContentAsync(string url)
    {
        HttpClient client = new HttpClient();
        Task<string> getStringTask = client.GetStringAsync(url);
        string urlContents = await getStringTask;
        client.Dispose();
        return urlContents;
    }

    public bool _validateurl(string url)
    {
        Uri res;
        return Uri.TryCreate(url, UriKind.Absolute, out res) && res.Scheme == Uri.UriSchemeHttp;
    }

    public string _parceHtml(string content)
    {
        doc.OptionWriteEmptyNodes = true;
        doc.LoadHtml(content);

        string result = doc.DocumentNode.SelectSingleNode("//body").InnerText;
        string path = System.Configuration.ConfigurationManager.AppSettings["mystempath"];

        //lemmas
        Guid id = Guid.NewGuid();
        StreamWriter sw = new StreamWriter(@"E:\" + id + ".txt", true, Encoding.GetEncoding(1251));
        sw.Write(result);
        sw.Close();

        Guid idout = Guid.NewGuid();
        Process mystem = new Process();
        mystem.StartInfo.FileName = path + "mystem.exe";
        mystem.StartInfo.Arguments = @"-c E:\" + id + @".txt E:\" + idout + ".txt";
        mystem.StartInfo.UseShellExecute = false;
        mystem.StartInfo.RedirectStandardInput = true;
        mystem.StartInfo.RedirectStandardOutput = true;

        String outputText = " ";
        mystem.Start();
        StreamWriter mystemStreamWriter = mystem.StandardInput;
        StreamReader mystemStreamReader = mystem.StandardOutput;
        string bs = mystemStreamReader.ReadToEnd();
        mystemStreamWriter.Write(bs);
        mystemStreamWriter.Close();
        outputText += mystemStreamReader.ReadToEnd() + " ";
        mystem.WaitForExit();
        mystem.Close();

        result = File.ReadAllText(@"E:\" + idout + ".txt", Encoding.GetEncoding(1251));

        string[] tags = new string[] { "&nbsp", "&time", "&copy", "	", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "-", "+", @"\", "_", "=", "<", ">", ",", ".", "?", ":", ";" };

        foreach (string tag in tags)
        {
            result = result.Replace(tag, "");
        };

        File.Delete(@"E:\" + id + ".txt");
        File.Delete(@"E:\" + idout + ".txt");

        return result;
    }

    public Dictionary<string,int> _getContentArray(string text, out int max)
    {
        max = 1;
        int buf_max = 1;
        Dictionary<string, int> _list = new Dictionary<string, int>();
        string[] _intext = text.Split(new Char[] { ' ' });
        Parallel.ForEach(_intext, value =>
        {
            string rs = Regex.Replace(value, @"[A-Za-z\d-.?!)(,:/]+", "");
            if (Regex.IsMatch(rs, @"[А-Яа-яёЁ]+") && Regex.Replace(rs, @"[А-Яа-яёЁ]+", "").Trim() != string.Empty)
            {
                rs = rs.Replace("{", "§");
                rs = rs.Split(new Char[] { '}' })[0];
                if (!_list.ContainsKey(rs.Trim().ToLower()))
                    _list[rs.Trim().ToLower()] = 1;
                else
                {
                    _list[rs.Trim().ToLower()] = _list[rs.Trim().ToLower()] + 1;
                    if (_list[rs.Trim().ToLower()] > buf_max)
                        buf_max = _list[rs.Trim().ToLower()];
                }

                
            }
        });

        max = buf_max;

        return _list;
    }
}
#endregion