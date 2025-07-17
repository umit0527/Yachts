using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;
using System.Net.Mail;
using System.Net;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Yachts.FrontEnd
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCountryList();
                BindModelList();
            }
        }

        private void BindCountryList()  // "國家"下拉式選單
        {
            DBHelper db = new DBHelper();
            string sql = @"select Id, Name 
                           from Country 
                           order by Name asc";

            DataTable dt = db.SearchDB(sql);

            if (dt.Rows.Count > 0)
            {
                CountryList.DataSource = dt;
                CountryList.DataTextField = "Name";   // 顯示名稱
                CountryList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
                CountryList.DataBind();

                // 加入提示選項
                CountryList.Items.Insert(0, new ListItem("請選擇國家", ""));
            }
        }

        private void BindModelList()  // "船型"下拉式選單
        {
            DBHelper db = new DBHelper();
            string sql = @"select Id, (RTRIM(Name) + ' '+CONVERT(varchar, Number)) as DisplayName
                           from Model
                           order by Name desc, Number";

            DataTable dt = db.SearchDB(sql);

            if (dt.Rows.Count > 0)
            {
                BrochureList.DataSource = dt;
                BrochureList.DataTextField = "DisplayName";   // 顯示名稱
                BrochureList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
                BrochureList.DataBind();

                // 加入提示選項
                BrochureList.Items.Insert(0, new ListItem("請選擇型號", ""));
            }
        }

        // 驗證電子郵件格式的方法 (配合前端 TextMode="Email" 使用)
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // 雙重驗證：使用 MailAddress 類別來驗證格式
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void SendEmail(string userName, string userEmail, string formContent, string phone,
                               string country, string brochure)
        {
            try
            {
                // 第一封：發送給使用者的感謝信
                MailMessage userMail = new MailMessage();
                userMail.From = new MailAddress("umit0527@gmail.com", "Yachts Customer Service");
                userMail.To.Add(new MailAddress(userEmail));
                userMail.Subject = "感謝您的諮詢 - Yachts";

                userMail.Body = $"親愛的 {userName} 您好，\r\n\r\n" +
                               $"感謝您透過 Yachts 網站與我們聯繫！\r\n\r\n" +
                               $"我們已收到您的諮詢，相關資訊如下：\r\n" +
                               $"• 聯絡電話：{phone}\r\n" +
                               $"• 所在國家：{country}\r\n" +
                               $"• 感興趣的船型：{brochure}\r\n" +
                               $"• 您的留言：{formContent}\r\n\r\n" +
                               $"我們的專業團隊將在 24 小時內與您聯繫。\r\n\r\n" +
                               $"此致\r\n" +
                               $"Yachts 客服團隊\r\n" +
                               $"電話：+886(7)641 2422";

                userMail.IsBodyHtml = false;
                userMail.BodyEncoding = System.Text.Encoding.UTF8;
                userMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // 第二封：發送給管理員的通知信
                MailMessage adminMail = new MailMessage();
                adminMail.From = new MailAddress("umit0527@gmail.com", "Yachts Website System");
                adminMail.To.Add("umit0527@gmail.com");
                adminMail.Subject = $"[新客戶諮詢] {userName} - {DateTime.Now:yyyy/MM/dd HH:mm}";

                adminMail.Body = $"=== 新的客戶諮詢 ===\r\n\r\n" +
                                $"時間：{DateTime.Now:yyyy年MM月dd日 HH:mm:ss}\r\n" +
                                $"姓名：{userName}\r\n" +
                                $"電子郵件：{userEmail}\r\n" +
                                $"電話：{phone}\r\n" +
                                $"國家：{country}\r\n" +
                                $"感興趣的船型：{brochure}\r\n" +
                                $"留言內容：\r\n{formContent}\r\n\r\n" +
                                $"請儘快與客戶聯繫。\r\n\r\n" +
                                $"--- 系統自動發送 ---";

                adminMail.IsBodyHtml = false;
                adminMail.BodyEncoding = System.Text.Encoding.UTF8;
                adminMail.SubjectEncoding = System.Text.Encoding.UTF8;

                // 設定 SMTP
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("umit0527@gmail.com", "fmpu pjch ytaf heec");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Timeout = 30000;

                // 記錄發送前的資訊
                Response.Write($"<script>console.log('準備發送使用者郵件到: {userEmail}');</script>");

                // 發送給使用者
                smtp.Send(userMail);
                Response.Write($"<script>console.log('使用者郵件發送成功');</script>");

                // 等待一小段時間，避免發送太快
                System.Threading.Thread.Sleep(1000);

                // 發送給管理員
                Response.Write($"<script>console.log('準備發送管理員通知郵件');</script>");
                smtp.Send(adminMail);
                Response.Write($"<script>console.log('管理員郵件發送成功');</script>");

                // 清理資源
                userMail.Dispose();
                adminMail.Dispose();
                smtp.Dispose();
            }
            catch (SmtpException smtpEx)
            {
                string errorMsg = $"SMTP錯誤: {smtpEx.Message}";
                Response.Write($"<script>console.error('{errorMsg}');</script>");
                throw;
            }
            catch (Exception ex)
            {
                string errorMsg = $"發送郵件時發生錯誤: {ex.Message}";
                Response.Write($"<script>console.error('{errorMsg}');</script>");
                throw;
            }
        }

        protected void Submit_Click(object sender, ImageClickEventArgs e)  //送出按鈕
        {
            string userName = TextName.Text.Trim();
            string userEmail = TextEmail.Text.Trim();
            string formContent = TextComments.Text.Trim();
            string phone = TextPhone.Text.Trim();
            string countryId = CountryList.SelectedValue;
            string countryName = CountryList.SelectedItem.Text;

            string brochureId = BrochureList.SelectedValue;
            string brochureName = BrochureList.SelectedItem.Text;

            // 除錯：顯示所有欄位值
            Response.Write($"<script>console.log('姓名: [{userName}]');</script>");
            Response.Write($"<script>console.log('信箱: [{userEmail}]');</script>");
            Response.Write($"<script>console.log('電話: [{phone}]');</script>");
            Response.Write($"<script>console.log('國家: [{countryName}]');</script>");
            Response.Write($"<script>console.log('船型: [{brochureName}]');</script>");
            Response.Write($"<script>console.log('內容: [{formContent}]');</script>");

            // 修正驗證邏輯 - 檢查必填欄位是否都有填寫
            bool isValid = true;
            string errorMessage = "";

            // 驗證姓名
            if (string.IsNullOrWhiteSpace(userName))
            {
                isValid = false;
                errorMessage += "請填寫姓名\\n";
            }

            // 驗證電子郵件
            if (string.IsNullOrWhiteSpace(userEmail))
            {
                isValid = false;
                errorMessage += "請填寫電子郵件\\n";
            }
            else if (!IsValidEmail(userEmail))
            {
                isValid = false;
                errorMessage += "電子郵件格式不正確\\n";
            }

            // 驗證電話
            if (string.IsNullOrWhiteSpace(phone))
            {
                isValid = false;
                errorMessage += "請填寫電話\\n";
            }

            // 驗證國家 - 修改驗證方式
            if (CountryList.SelectedIndex <= 0) // 使用 SelectedIndex 檢查是否選擇了有效選項
            {
                isValid = false;
                errorMessage += "請選擇國家\\n";
            }

            // 驗證船型 - 修改驗證方式
            if (BrochureList.SelectedIndex <= 0) // 使用 SelectedIndex 檢查是否選擇了有效選項
            {
                isValid = false;
                errorMessage += "請選擇船型\\n";
            }

            // 驗證內容
            if (string.IsNullOrWhiteSpace(formContent))
            {
                isValid = false;
                errorMessage += "請填寫留言內容\\n";
            }

            Response.Write($"<script>console.log('驗證結果: {isValid}');</script>");
            Response.Write($"<script>console.log('錯誤訊息: {errorMessage}');</script>");

            if (isValid)
            {
                try
                {
                    //寫進資料庫
                    try
                    {
                        // 嘗試寫入資料庫
                        SaveContactMessage(userName, userEmail, phone, countryId, brochureId, formContent);
                    }
                    catch (Exception ex)
                    {
                        // 顯示資料庫錯誤訊息在瀏覽器 console
                        Response.Write($"<script>console.error('資料庫錯誤: {ex.Message}');</script>");
                        throw; // 繼續往上拋錯，以便 alert 顯示
                    }

                    // 寄出信件
                    SendEmail(userName, userEmail, formContent, phone, countryName, brochureName);
                    Response.Write("<script>alert('成功送出！'); window.location='Contact-F.aspx';</script>");
                    Response.End();
                }
                catch (Exception ex)
                {
                    // 最終 catch：顯示錯誤 alert
                    Response.Write($"<script>console.error('發送郵件異常: {ex.Message}');</script>");
                    string errorMsg = ex.Message.Replace("'", "").Replace("\"", "").Replace("\r", "").Replace("\n", "\\n");
                    Response.Write($"<script>alert('送出失敗，錯誤訊息：{errorMsg}');</script>");
                }
            }
            else
            {
                Response.Write("<script>alert('" + errorMessage + "');</script>");
            }   
        }

        private void SaveContactMessage(string userName, string userEmail, string phone,
                                string country, string brochure, string formContent)  //寫進資料庫
        {
            DBHelper db = new DBHelper();

            string sql = @"INSERT INTO Contact
                   (Name, Email, Phone, CountryId, BrochureId, Comments ,SendedAt)
                   VALUES (@Name, @Email, @Phone, @CountryId, @BrochureId, @Comments, @SendedAt)";

            var param = new Dictionary<string, object> {
                { "@Name", userName },
                { "@Email", userEmail},
                { "@Phone", phone},
                { "@CountryId", country},
                { "@BrochureId", brochure},
                { "@Comments", formContent},
                { "@SendedAt",DateTime.Now}
            };

            db.ExecuteNonQuery(sql, param);
        }
    }
}