using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Сводное описание для SQL
/// </summary>
public static class SQL
{
	public static DataSet _getDataAsDT(string connection, string qwery)
    {
        DataSet ds = new DataSet();

        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = connection;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = qwery;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(ds);
            }
        }

        return ds;
    }

    public static void exectQuery(string connection, string qwery)
    {
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = connection;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = qwery;
                cmd.ExecuteNonQuery();
            }
        }
    }

    public static double _getIdFromExectQuery(string connection, string qwery, string tablename)
    {
        double id = 0;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = connection;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = qwery + "; SELECT @@IDENTITY FROM " + tablename;
                id = (double)cmd.ExecuteScalar();
            }
        }

        return id;
    }

    public static double _getId(string connection, string qwery)
    {
        double id = 0;
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = connection;
            conn.Open();
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = qwery;
                id = (double)cmd.ExecuteScalar();
            }
        }

        return id;
    }
}