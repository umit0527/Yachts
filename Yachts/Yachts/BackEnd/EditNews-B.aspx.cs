using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yachts.Helper;

namespace Yachts.BackEnd
{
    public partial class EditNnews_B : System.Web.UI.Page
    {
        DBHelper db = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //用 viewstate 存 NewsId ，讓檔案刪除時可以各自刪除
                ViewState["NewsId"] = Request.QueryString["Id"]; 

                if (Request.QueryString["Id"] == null || !int.TryParse(Request.QueryString["Id"], out int newsDownloadsId))
                {
                    Response.Redirect("News-B.aspx");
                    return;
                }

                LoadNewsData();

                if (ViewState["NewsId"] != null)  // 從 ViewState 取得 newsId
                {
                    int newsId = Convert.ToInt32(ViewState["NewsId"]);
                    LoadDownloadFiles(newsId);
                }

                //限制上傳檔案為圖片
                FileUpload1.Attributes["accept"] = "image/*";
            }
        }
        private void LoadNewsData()
        {
            if (Request.QueryString["Id"] == null)
            {
                return;
            }
            else
            {
                int newsId = int.Parse(Request.QueryString["Id"]);

                string sql = @"select n.CategoryId,Title, n.Content ,n.UpdatedAt ,n.CoverPath ,n.Sticky,
                                      nd.FilePath, nd.NewsId
                               from News n
                               left join NewsDownloads nd on nd.NewsId=n.Id
                               where n.Id = @Id";

                var param = new Dictionary<string, object> { { "@Id", newsId } };
                DataTable dt = db.SearchDB(sql, param);

                if (dt.Rows.Count > 0)
                {
                    int categoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]);
                    string content = dt.Rows[0]["Content"].ToString();
                    string coverPath = dt.Rows[0]["CoverPath"].ToString();
                    string title = dt.Rows[0]["Title"].ToString();
                    bool sticky = Convert.ToBoolean(dt.Rows[0]["Sticky"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["Sticky"]));
                    string filePath = dt.Rows[0]["FilePath"].ToString();

                    txtTitle.Text = title;  //標題
                    imgCover.ImageUrl = "~/Uploads/Photos/" + coverPath;  //封面
                    CategoryList.SelectedValue = categoryId.ToString();
                    chbSticky.Checked = sticky; //置頂狀態
                    CKEditor1.Text = content;  //內容
                    BindCategoryList(); // 所有種類
                    LoadDownloadFiles(newsId);  //呼叫 檔案下載的repeater
                }
            }
        } //顯示 主要內容
        private void LoadDownloadFiles(int newsId, List<int> excludeIds = null)  //顯示 檔案下載
        {
            string sql = @"SELECT Id,FilePath FROM NewsDownloads WHERE NewsId = @NewsId";
            var param = new Dictionary<string, object> { { "@NewsId", newsId } };
            DataTable dt = db.SearchDB(sql, param);

            if (excludeIds != null && excludeIds.Any())
            {
                foreach (int id in excludeIds)
                {
                    var rows = dt.Select("Id = " + id);
                    foreach (var row in rows)
                    {
                        dt.Rows.Remove(row);
                    }
                }
                dt.AcceptChanges();
            }

            rptEditFiles.DataSource = dt;
            rptEditFiles.DataBind();
        }
        private void InsertFile(string newsId)  //上傳 "檔案下載" 的核心
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
                        string fileName = DateTime.UtcNow.Ticks.ToString() + extension;
                        string uploadsFolder = Server.MapPath("~/Uploads/"); // 取得實體路徑
                        Directory.CreateDirectory(uploadsFolder); // 確保資料夾存在         
                        string fullPath = Path.Combine(uploadsFolder, fileName);  //組合成完整檔案路徑

                        // 實際儲存檔案到 uploads 資料夾
                        file.SaveAs(fullPath);

                        string sql = @"INSERT INTO NewsDownloads (NewsId, FilePath,  UpdatedAt)
                                       VALUES  (@NewsId, @FilePath, @UpdatedAt)
                                      ";

                        var param = new Dictionary<string, object>()
            {
                { "@NewsId", newsId },
                { "@FilePath", fileName ?? "" },
                { "@UpdatedAt",DateTime.Now}
            };
                        db.ExecuteNonQuery(sql, param);
                    }
                }
            }
        }
        protected void btnDeleteFile(object source, RepeaterCommandEventArgs e)  //刪除 "檔案下載" 的核心
        {
            if (e.CommandName == "DeleteFile")
            {
                int id = Convert.ToInt32(e.CommandArgument);

                // 加到待刪除清單
                List<int> deleteList = ViewState["FilesToDelete"] as List<int> ?? new List<int>();
                if (!deleteList.Contains(id))
                {
                    deleteList.Add(id);
                }
                ViewState["FilesToDelete"] = deleteList;

                // 隱藏該刪除的檔案（重新綁定排除）
                int newsId = Convert.ToInt32(ViewState["NewsId"]);
                // 重新綁定顯示，排除該檔案
                LoadDownloadFiles(newsId, deleteList);
            }
        }
        protected void rptEditFiles_ItemCommand(object source, RepeaterCommandEventArgs e)  //
        {
            if (e.CommandName == "DeleteFile")
            {
                btnDeleteFile(source, e);
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int newsId = int.Parse(Request.QueryString["Id"]);
            string content = CKEditor1.Text.Trim();
            // 把 HTML tag 移除，只留下純文字，用來檢查「送出時是否真的為空，還是保有html標籤」
            string plainText = Regex.Replace(content, "<.*?>", "").Trim();
            string categoryList = CategoryList.SelectedValue;
            string title = txtTitle.Text.Trim();
            string newCoverPath = "";
            HttpFileCollection uploadedFiles = Request.Files;
            bool sticky = chbSticky.Checked;

            if (Request.QueryString["Id"] != null)
            {
                // 驗證是否有上傳圖片、選擇種類、填寫標題與內容
                if (string.IsNullOrWhiteSpace(categoryList) || string.IsNullOrWhiteSpace(title) ||
                    string.IsNullOrWhiteSpace(plainText))
                {
                    Response.Write("<script>alert('必填欄位填寫不完整');</script>");
                    return;
                }

                // 取得原本的封面路徑
                string getCoverSql = "SELECT CoverPath FROM News WHERE Id = @Id";
                var paramCover = new Dictionary<string, object> { { "@Id", newsId } };
                DataTable dtCover = db.SearchDB(getCoverSql, paramCover);

                if (dtCover.Rows.Count > 0)
                {
                    newCoverPath = dtCover.Rows[0]["CoverPath"].ToString();
                }

                // 如果有上傳新圖片，則覆蓋原圖
                if (uploadedFiles.Count > 0 && uploadedFiles[0].ContentLength > 0)
                {
                    HttpPostedFile file = uploadedFiles[0];
                    string extension = Path.GetExtension(file.FileName).ToLower();

                    if (!(extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif"))
                    {
                        Response.Write("<script>alert('圖片格式僅限 JPG、JPEG、PNG、GIF');</script>");
                        return;
                    }

                    // 儲存新圖片
                    string saveFileName = DateTime.Now.Ticks.ToString() + extension;
                    string savePath = Server.MapPath("~/Uploads/Photos/") + saveFileName;
                    file.SaveAs(savePath);

                    newCoverPath = saveFileName; // 使用新圖片
                }

                //處理刪除檔案
                if (ViewState["FilesToDelete"] is List<int> deleteFileList)
                {
                    foreach (int id in deleteFileList)
                    {
                        //找出要刪除的那個檔案名稱
                        string sqlFilePath = "SELECT FilePath FROM NewsDownloads WHERE Id = @Id";
                        var param = new Dictionary<string, object> { { "@Id", id } };
                        object resultDeletFile = db.ExecuteScalar(sqlFilePath, param);
                        if (resultDeletFile != null)
                        {
                            string fileName = resultDeletFile.ToString();
                            string filePath = Server.MapPath(fileName);

                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }

                        }
                        string sqlDeletFile = "DELETE FROM NewsDownloads WHERE Id = @Id";  //找出要刪除的那個檔案Id
                        db.ExecuteNonQuery(sqlDeletFile, param);
                    }
                }
                // 清除 ViewState
                ViewState["FilesToDelete"] = null;
                InsertFile(newsId.ToString());

                // 更新新聞資料（含封面路徑）
                string sql = @"update News set 
                                              Content=@Content  , 
                                              CategoryId=@CategoryId ,
                                              Title=@Title,
                                              CoverPath=@CoverPath,
                                              UpdatedAt=@UpdatedAt,
                                              Sticky=@Sticky
                                              where Id=@Id
                              ";

                var Params = new Dictionary<string, object>()
    {
        { "@Content", content },
        { "@CategoryId", categoryList },
        { "@Title", title },
        { "@CoverPath", newCoverPath  },
        { "@UpdatedAt", DateTime.Now },
        {"@Sticky", sticky},
        { "@Id", newsId }
    };

                int result = db.ExecuteNonQuery(sql, Params);

                if (result > 0)
                {
                    Response.Write("<script>alert('更新成功！'); window.location='News-B.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('更新失敗，請稍後再試！');</script>");
                }
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("News-B.aspx");
        }
        private int editId
        {
            get => ViewState["EditId"] == null ? -1 : (int)ViewState["EditId"];
            set => ViewState["EditId"] = value;
        }
    }
}