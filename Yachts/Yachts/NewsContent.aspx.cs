using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts
{
    public partial class NewsContent : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindNewsContent();
                BindCategory();
                BindDownloads();

                string newsId = Request.QueryString["Id"];
                Session["CategoryId"] = Request.QueryString["CategoryId"];

                // 預設導向第一個種類的消息列表
                if (string.IsNullOrEmpty(newsId))
                {
                    // 從資料庫查目前存在的第一筆資料
                    string sql = @"SELECT TOP 1 Id FROM NewsCategory ORDER BY Id";
                    DataTable dt = db.SearchDB(sql);

                    if (dt.Rows.Count > 0)
                    {
                        string defaultId = dt.Rows[0]["Id"].ToString();
                        Response.Redirect("News.aspx?CategoryId=" + defaultId);
                        return;
                    }
                }
            }
        }
        private void BindNewsContent()  //顯示內容的Repeater
        {
            string categoryId = Request.QueryString["Id"];

            if (!string.IsNullOrEmpty(categoryId))
            {
                string sql = @"select n.Title, n.CreatedAt , n.Id, n.UpdatedAt, n.content ,
                                      nc.Name as CategoryName
                               from News n
                               join NewsCategory nc on nc.Id=n.CategoryId
                               where n.Id=@Id
                              ";

                var param = new Dictionary<string, object> { { "@Id", categoryId } };

                DataTable dt = db.SearchDB(sql, param);
                rptNewsContent.DataSource = dt;
                rptNewsContent.DataBind();

                if (dt.Rows.Count > 0)
                {
                    string title = dt.Rows[0]["Title"].ToString();
                    string categoryName = dt.Rows[0]["CategoryName"].ToString();

                    Label1.Text = categoryName;
                    Label2.Text = categoryName;
                }
            }
        }
        private void BindCategory()  //顯示 種類
        {
            string sql = @"select Id, Name 
                           from NewsCategory
                          ";

            DataTable dt = db.SearchDB(sql);
            rptNewsCategory.DataSource = dt;
            rptNewsCategory.DataBind();
        }
        private void BindDownloads()  //顯示 "檔案下載"
        {
            string Id = Request.QueryString["Id"];
            if (!string.IsNullOrEmpty(Id))
            {
                string sql = @"select *
                               from NewsDownloads nd
                               join News n on n.Id=nd.NewsId
                               where n.Id=@Id
                              ";
                var param = new Dictionary<string, object> { { "@Id", Id } };
                DataTable dt = db.SearchDB(sql, param);
                if (dt.Rows.Count > 0)
                {
                    rptDownloads.DataSource = dt;
                    rptDownloads.DataBind();
                }
            }
        }
        protected void lnkDownload_Click(object sender, EventArgs e)  //點擊 "檔案下載"
        {
            LinkButton btn = (LinkButton)sender;
            string id = btn.CommandArgument;

            // 從資料庫根據 fileId 取得檔案資訊
            string sql = "SELECT * FROM NewsDownloads WHERE Id = @Id";

            var param = new Dictionary<string, object> { { "@Id", id } };

            DataTable dt = db.SearchDB(sql, param);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                string filePath = Server.MapPath("~/Uploads/" + row["FilePath"].ToString());
                string fileName = Path.GetFileName(filePath); // 從完整路徑中取得檔名（含副檔名）

                // 如果檔案存在
                if (File.Exists(filePath))
                {
                    Response.Clear();
                    Response.ContentType = "application/octet-stream";  // 告訴瀏覽器檔案的類型
                    //設定檔案下載的回應，讓瀏覽器用指定的檔名下載。
                    Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8) + "\"");
                    Response.TransmitFile(filePath);  //把伺服器上的實體檔案「直接傳送」給使用者下載或瀏覽。
                    Response.End();
                }
                else
                {
                    Response.Write("<script>alert('下載失敗！')</script>");
                }
            }
        }
    }
}