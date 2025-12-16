using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Yachts.Helper
{
    public class DBHelper
    {
        //建立連線物件
        SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString);

        //建立指令物件
        SqlCommand command = new SqlCommand();

        public void OpenDB()  //開啟連線
        {
            //如果連項狀態不為開啟，即連線狀態為關閉時
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
        }
        public DataTable SearchDB(string sql, Dictionary<string, object> dictionary = null)  //查詢
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (dictionary != null)
                    {
                        //歷遍 dictionary 每一行
                        foreach (var item in dictionary)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
        public object ExecuteScalar(string sql, Dictionary<string, object> dictionary = null)  //寫入資料庫
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }
        public int ExecuteNonQuery(string sql, Dictionary<string, object> dictionary = null)  //取得 ID 用
        {
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    if (dictionary != null)
                    {
                        foreach (var item in dictionary)
                        {
                            command.Parameters.AddWithValue(item.Key, item.Value);
                        }
                    }

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}