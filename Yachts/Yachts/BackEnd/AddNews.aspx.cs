using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class AddNews : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategoryList();
                
                //限制上傳檔案為圖片
                FUCoverPath.Attributes["accept"] = "image/*";
                
            }
        }
        private void BindCategoryList()  // "種類"下拉式選單
        {
            string sql = @"select Id, Name 
                           from NewsCategory 
                           order by Name asc";

            DataTable dt = db.SearchDB(sql);

            if (dt.Rows.Count > 0)
            {
                CategoryList.DataSource = dt;
                CategoryList.DataTextField = "Name";   // 顯示名稱
                CategoryList.DataValueField = "Id";  //抓取對應 ID 以便寫進資料庫
                CategoryList.DataBind();

                // 加入提示選項
                CategoryList.Items.Insert(0, new ListItem("請選擇種類", ""));
            }
        }
        private void InsertDownloadsFile(string newsId)  //新增 檔案下載
        {
            var uploadFiles = FUDownloadsFile.PostedFiles;

            //如果有上傳檔案
            if (uploadFiles != null)
            {
                foreach (HttpPostedFile file in uploadFiles)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string extension = Path.GetExtension(file.FileName);  //取得副檔名
                        string fileName = DateTime.Now.Ticks.ToString() + extension;
                        string uploadsFolder = Server.MapPath("~/Uploads/"); // 取得實體路徑
                        Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在         
                        string fullPath = Path.Combine(uploadsFolder, fileName);  //組合成完整檔案路徑

                        // 實際儲存檔案到 uploads 資料夾
                        file.SaveAs(fullPath);

                        string sql = @"INSERT INTO NewsDownloads (NewsId, FilePath,  CreatedAt)
                           VALUES  (@NewsId, @FilePath, @CreatedAt)
                          ";

                        var param = new Dictionary<string, object>()
            {
                { "@NewsId", newsId },
                { "@FilePath", fileName ?? "" },
                { "@CreatedAt",DateTime.Now}
            };
                        db.ExecuteNonQuery(sql, param);
                    }
                }
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string content = CKEditor1.Text.Trim();
            string categoryList = CategoryList.SelectedValue;
            string title = txtTitle.Text.Trim();
            HttpFileCollection uploadedFiles = Request.Files;
            bool sticky = chbSticky.Checked;

            // 驗證是否有上傳圖片
            if (uploadedFiles.Count == 0 || uploadedFiles[0].ContentLength == 0)
            {
                Response.Write("<script>alert('請上傳封面');</script>");
                return;
            }
            else if (string.IsNullOrWhiteSpace(categoryList) || string.IsNullOrWhiteSpace(title) ||
                     string.IsNullOrWhiteSpace(content))
            {
                Response.Write("<script>alert('請填寫所有欄位');</script>");
                return;
            }

            // 處理圖片
            HttpPostedFile file = uploadedFiles[0]; // 只取第一張圖片
            string extension = Path.GetExtension(file.FileName).ToLower();

            if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif"))
            {
                Response.Write("<script>alert('圖片格式僅限 JPG、JPEG、PNG、GIF');</script>");
                return;
            }

            // 儲存圖片
            string saveFileName = DateTime.Now.Ticks.ToString() + extension;
            string savePath = Server.MapPath("~/Uploads/Photos/") + saveFileName;
            file.SaveAs(savePath);

            // 插入新聞相簿資料（含封面路徑）
            string sqlAlbum = @"INSERT INTO News (CreatedAt, CategoryId, Title, CoverPath, content ,Sticky)
                                VALUES ( @CreatedAt, @CategoryId, @Title, @CoverPath, @content ,@Sticky);             
                                SELECT SCOPE_IDENTITY();";

            var albumParams = new Dictionary<string, object>()
    {
        { "@CreatedAt", DateTime.Now },
        { "@CategoryId", categoryList },
        { "@Title", title },
        { "@CoverPath", saveFileName },
        { "@content", content },
        { "@Sticky",sticky}
    };

            object resultAlbum = db.ExecuteScalar(sqlAlbum, albumParams);

            string newsId = resultAlbum?.ToString();

            if (!string.IsNullOrEmpty(newsId))
            {
                InsertDownloadsFile(newsId);

                Response.Write("<script>alert('新增成功！'); window.location='News-B.aspx';</script>");
            }
            else
            {
                Response.Write("<script>alert('新增失敗，請稍後再試！');</script>");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("News-B.aspx");
        }
    }
}