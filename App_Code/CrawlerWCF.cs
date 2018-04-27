using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.RegularExpressions;
using System.Net;
using System.Data.SqlClient;
using System.Data;

// ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя класса "CrawlerWCF" в коде, системе контроля версий и файле конфигурации.
public class CrawlerWCF : ICrawlerWCF
{
    public byte[] GetContent(string Url, int mapid, bool a_replace)
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

        //ищем значение для тега src
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

                if (al.IndexOf("//") == -1)
                {
                    innerhtml = re.Replace(innerhtml, delegate(Match m)
                    { return myUri.GetLeftPart(UriPartial.Authority) + al; });

                    content = content.Replace(buf_img, innerhtml);
                }

            }
        }
        while (img != -1);

        //меняем src у скриптов
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
                    innerhtml = re.Replace(innerhtml, delegate(Match m)
                    { return myUri.GetLeftPart(UriPartial.Authority) + al; });

                    content = content.Replace(buf_img, innerhtml);
                }

            }
        }
        while (img != -1);

        //Меняем все ссылки у тега а
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

        content = Regex.Replace(content, "<!--(.|\n)*?-->", string.Empty);

        content = content.Replace("@import \"/", "@import \"" + myUri.GetLeftPart(UriPartial.Authority) + "/");
        
        //Раскоментить если необходим танец с бубном ♪♫

        //content = content.Replace("></script>", "><bscript>");
        //content = Regex.Replace(content, @"<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>", string.Empty);
        //content = content.Replace("><bscript>", "></script>");

        //Непонятные скрипты - ТЕСТЫ (УДАЛИТЬ ПО НЕНАДОБНОСТИ)
        content = Regex.Replace(content, @"<iframe(.|\n)*?>", string.Empty);

        byte[] bytes = new byte[content.Length * sizeof(char)];
        System.Buffer.BlockCopy(content.ToCharArray(), 0, bytes, 0, bytes.Length);

        string constring = System.Configuration.ConfigurationManager.AppSettings["connectionSQL"];
        
        SqlConnection con = new SqlConnection(constring);

            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "SELECT ID FROM BUILDS WHERE area_id = " + mapid;
            int result =  Convert.ToInt32(cmd.ExecuteScalar());
            if (result != 0)
            {
                cmd = new SqlCommand("UPDATE BUILDS SET CONTENT =  @content WHERE area_id = @mapid", con);
	            cmd.Parameters.Add(new SqlParameter("@mapid",mapid));
                cmd.Parameters.Add(new SqlParameter("@content", SqlDbType.VarBinary)).Value = bytes;
                cmd.ExecuteNonQuery();
            }
            else
            {
                cmd = new SqlCommand("INSERT INTO BUILDS (area_id, content) VALUES (@mapid, @content)", con);
                cmd.Parameters.Add(new SqlParameter("@mapid", mapid));
                cmd.Parameters.Add(new SqlParameter("@content", SqlDbType.VarBinary)).Value = bytes;
                cmd.ExecuteNonQuery();
            }
            con.Close();


       
        return bytes;
    }

    public string Lemmas(string Url, string[] pr)
    {
        string result_ = "";

        try
        {
            Crawler crw = new Crawler();
            if (crw._validateurl(Url))
            {
                Task<string> content = crw._getWebContentAsync(Url);
                string _text = crw._parceHtml(content.Result);
                int max = 0;
                Dictionary<string, int> _list = crw._getContentArray(_text, out max);

                var sorted =
                      (from item
                       in _list
                       orderby item.Value
                       descending
                       select item);

                string result = string.Empty; //"<table class=\"table table-striped\"><thead><tr><th>Исходное слово</th><th>Нормальная форма</th><th>Повторений</th></tr></thead><tbody>";
                Parallel.ForEach(sorted, value =>
                {
                    try
                    {
                        if (value.Key.Trim() != string.Empty)
                        {
                            string[] val = value.Key.Trim().Split(new Char[] { '§' });
                            if (val.Length > 0)
                            {
                                if (val[0].Trim() != string.Empty)
                                {
                                    if (val[1].IndexOf("|") != -1)
                                        val[1] = val[1].Split(new Char[] { '|' })[0];

                                    if (!pr.Contains(val[1].Trim().ToUpper()))
                                    {
                                        double size_div = max != 0 ? (value.Value * 100) / max : 0;
                                        result += "<tr><td>" + val[0] + "</td><td>" + val[1] + "</td><td><div style = 'width: " + (size_div) + "%; height: 8px; background-color: #aaa; float: left; margin-top:3px; margin-right: 4px;'>&nbsp</div><div style = 'float: left; font-size: 8pt;'>" + value.Value.ToString() + "</div></td></tr>";
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                });
                //result += "</tbody></table>";
                result_ = result;
            }
            else
            {
                result_ = "No valid url - " + Url;
            }
        }
        catch (Exception err)
        {
            result_ = err.Message;
        }

        return result_;
    }
}
