using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;


namespace Yachts.BackEnd
{
    public partial class Register : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }       
        private byte[] CreateSalt()  //Argon2 加密：加鹽
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);
            return buffer;
        }
        
        private byte[] HashPassword(string password, byte[] salt)  //雜湊
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            //底下這些數字會影響運算時間，而且驗證時要用一樣的值
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // 4 核心就設成 8
            argon2.Iterations = 4; // 迭代運算次數
            argon2.MemorySize = 1024 * 1024; // 1 GB

            return argon2.GetBytes(16);
        }
        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string name = Name.Text;
            string account2 = Account.Text;
            string chkpassword = CheckPassword.Text;
            string password2 = Password.Text;

            // 基本欄位檢查
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(account2) ||
                string.IsNullOrEmpty(password2) || string.IsNullOrEmpty(chkpassword))
            {
                Response.Write("<script>alert('請填寫所有欄位');</script>");
                return;
            }

            if (password2 != chkpassword)
            {
                Response.Write("<script>alert('密碼與確認密碼不一致');</script>");
                return;
            }

            bool haveSameAccount = false;

            string connection = WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString;

            // 第一次查詢，確認是否有重複帳號
            using (SqlConnection conn = new SqlConnection(connection))
            {
                string sql = @"SELECT Id, Name FROM Administrator WHERE Account = @Account";
                SqlCommand commandCheck = new SqlCommand(sql, conn);
                commandCheck.Parameters.AddWithValue("@Account", account2);  // 使用 account2 即使用者輸入的帳號

                conn.Open();
                using (SqlDataReader reader = commandCheck.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        haveSameAccount = true;
                        LabelAdd.Visible = true; // 帳號重複通知
                    }
                }
            }

            // 若帳號未重複才進行插入
            if (!haveSameAccount)
            {
                // Hash 密碼與產生 salt
                var salt = CreateSalt();
                string saltStr = Convert.ToBase64String(salt);
                var hash = HashPassword(password2, salt);
                string hashPassword = Convert.ToBase64String(hash);

                using (SqlConnection conn = new SqlConnection(connection))
                {
                    string sqlAdd = @"INSERT INTO Administrator (Account, Password, salt, Name, CreatedAt, ImgPath)
                              VALUES (@Account, @Password, @salt, @Name, @CreatedAt, @ImgPath)";
                    SqlCommand commandAdd = new SqlCommand(sqlAdd, conn);
                    string ImgPath = "bruce-mars.jpg";  //預設的大頭照

                    commandAdd.Parameters.AddWithValue("@Account", account2);
                    commandAdd.Parameters.AddWithValue("@Password", hashPassword);
                    commandAdd.Parameters.AddWithValue("@salt", saltStr);
                    commandAdd.Parameters.AddWithValue("@Name", name);
                    commandAdd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    commandAdd.Parameters.AddWithValue("@ImgPath", ImgPath);

                    conn.Open();
                    commandAdd.ExecuteNonQuery();
                }

                Response.Write("<script>alert('註冊成功'); window.location='Login.aspx';</script>");
            }
            else
            {
                Response.Write("<script>alert('帳號已存在，註冊失敗');</script>");
            }
        }
        protected void btnSignin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}