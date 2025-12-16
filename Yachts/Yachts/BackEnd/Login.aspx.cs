using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Yachts.BackEnd
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            //account2去接輸入的帳號
            string account2 = Account.Text;
            //password2去接輸入的密碼
            string password2 = Password.Text;

            //建立資料庫連線
            string connection = WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connection))
            {
                //建立指令本身
                string sql = @"select Id, Name 
                               from [User] 
                               where Account = @Account AND Password = @Password";
                //建立指令物件，將連線與指令綁定
                SqlCommand command = new SqlCommand(sql, conn);
                //核對輸入的帳號與密碼，user資料庫是否有符合的值
                command.Parameters.AddWithValue("@Account", account2);
                command.Parameters.AddWithValue("@Password", password2);

                //開啟連線
                conn.Open();
                //用 SqlDataReader 從上到下依照執行的指令，逐筆撈取資料
                SqlDataReader reader = command.ExecuteReader();
                //如果資料獲取完整
                if (reader.Read())
                {
                    //用戶登入成功，Session["userid"]=user表的id，讓其他頁面可以使用，例如新增回覆時需要
                    Session["userid"] = (int)reader["Id"];
                    Session["Name"] = reader["Name"].ToString();
                    Response.Write("<script>alert('登入成功'); window.location='/BackEnd/Index.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('帳號或密碼錯誤');</script>");
                }
            }
        }
    }
}