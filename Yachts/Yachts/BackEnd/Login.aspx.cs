using Konscious.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class Login : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Cookies["RememberMe"] != null)
            {
                string rememberedAccount = Request.Cookies["RememberMe"]["Account"];
                Account.Text = rememberedAccount;
                chkRememberMe.Checked = true;
            }
        }
        
        private byte[] HashPassword(string password, byte[] salt)  // Argon2 驗證加密密碼，處理加鹽的密碼功能
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));

            //底下這些數字會影響運算時間，而且驗證時要用一樣的值
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // 4 核心就設成 8
            argon2.Iterations = 4; //迭代運算次數
            argon2.MemorySize = 1024 * 1024; // 1 GB

            return argon2.GetBytes(16);
        }

        private bool VerifyHash(string password, byte[] salt, byte[] hash)  //驗證
        {
            var newHash = HashPassword(password, salt);
            return hash.SequenceEqual(newHash); // LINEQ
        }
        
        private void SetAuthenTicket(string userData, string userId)  //設定驗證票
        {
            //宣告一個驗證票 //需額外引入 using System.Web.Security;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userId, DateTime.Now, DateTime.Now.AddHours(3), false, userData);
            //加密驗證票
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);
            //建立 Cookie
            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //將 Cookie 寫入回應
            Response.Cookies.Add(authenticationCookie);
        } 
        protected void btnSignin_Click(object sender, EventArgs e)
        {
            string account2 = Account.Text;
            string password2 = Password.Text;

            string connection = WebConfigurationManager.ConnectionStrings["YachtsConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connection))
            {
                string sql = @"SELECT Id, Name, Account, Password, salt 
                       FROM Administrator 
                       WHERE Account = @Account";
                SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@Account", account2);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    string hashStr = dataTable.Rows[0]["Password"].ToString();
                    string saltStr = dataTable.Rows[0]["Salt"].ToString();

                    if (string.IsNullOrWhiteSpace(hashStr) || string.IsNullOrWhiteSpace(saltStr))
                    {
                        Label4.Text = "密碼錯誤，登入失敗！";
                        Label4.Visible = true;
                        return;
                    }
                    try
                    {
                        byte[] hash = Convert.FromBase64String(hashStr);
                        byte[] salt = Convert.FromBase64String(saltStr);

                        bool success = VerifyHash(password2, salt, hash);

                        if (success)
                        {
                            // 登入成功，寫入 Session
                            Session["AdminiId"] = dataTable.Rows[0]["Id"];
                            Session["Name"] = dataTable.Rows[0]["Name"].ToString();

                            string userData = dataTable.Rows[0]["Account"].ToString() + ";" +
                                              dataTable.Rows[0]["Name"].ToString();

                            SetAuthenTicket(userData, account2);

                            // Cookie：記住我
                            if (chkRememberMe.Checked)
                            {
                                HttpCookie cookie = new HttpCookie("RememberMe");
                                cookie["Account"] = account2;
                                cookie.Expires = DateTime.Now.AddDays(7);
                                Response.Cookies.Add(cookie);
                            }
                            else
                            {
                                if (Request.Cookies["RememberMe"] != null)
                                {
                                    HttpCookie cookie = new HttpCookie("RememberMe");
                                    cookie.Expires = DateTime.Now.AddDays(-1);
                                    Response.Cookies.Add(cookie);
                                }
                            }

                            Response.Redirect("Yachts-B.aspx");
                        }
                        else
                        {
                            Label4.Text = "密碼錯誤，登入失敗！";
                            Label4.Visible = true;
                        }
                    }
                    catch (FormatException)
                    {
                        Label4.Text = "密碼錯誤，登入失敗！";
                        Label4.Visible = true;
                    }
                }
                else
                {
                    Label4.Text = "帳號錯誤，登入失敗！";
                    Label4.Visible = true;
                }
            }
        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}